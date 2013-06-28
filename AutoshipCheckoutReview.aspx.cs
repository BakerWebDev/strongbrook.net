using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Exigo.OData;
using Exigo.WebService;
using System.Security.Cryptography;
using System.IO;
using System.Net;
using System.Xml;

public partial class AutoshipCheckoutReview : Page, IPostBackEventHandler
{
    #region Page Load
    protected void Page_Load(object sender, EventArgs e)
    {
        #region is this needed now that I changed "Autoship.PropertyBag.CreditCardNumber.Length == 0" to "Autoship.PropertyBag.CreditCardNumber == null"
        PopulatePropertyBagValues_Load(); // Put this here to see if it would fix the CC null issue.

        Autoship.PropertyBag.ReferredByEndOfCheckout = false; // Put this here to see if it would fix the CC null issue.

        if(IsPostBack) // Put this here to see if it would fix the CC null issue.  Need to make sure putting this here didn't mess with anything else.
        {
            Autoship.PropertyBag.ReferredByEndOfCheckout = true;
            Autoship.PropertyBag.Save();        
        }
        #endregion is this needed now that I changed "Autoship.PropertyBag.CreditCardNumber.Length == 0" to "Autoship.PropertyBag.CreditCardNumber == null"

        //#region Or is this needed?
        //Autoship.PropertyBag.ReferredByEndOfCheckout = true;
        //Autoship.PropertyBag.Save();        
        //#endregion Or is this needed?


        cmdPlaceOrder.Text = Resources.Shopping.SaveYourAutoship;

        // If there are no items in the cart, redirect them to the cart page.
        if (Autoship.Cart.Items.Count == 0)
        {
            Response.Redirect(Autoship.GetStepUrl(AutoshipManagerStep.Cart));
            return;
        }


        // If there is no credit card number on the billing account, redirect to the Billing step.
        //if (Autoship.PropertyBag.CreditCardNumber.Length == 0)
        if (Autoship.PropertyBag.CreditCardNumber == null) // The above didn't seem to work when the autoship was being created for the first time, so I changed it to this.
        {
            Response.Redirect(Autoship.GetStepUrl(AutoshipManagerStep.Payment));
        }


        if (!IsPostBack)
        {
            if(CalculatedOrder.ShipMethods[0].ShippingAmount == 0M)
            {
                PopulateAvailableShippingMethods_Load2();
            }
            else
            {
                PopulateAvailableShippingMethods_Load();
            }

            //PopulatePropertyBagValues_Load();
        }
    }
    #endregion

    #region Properties
    public AutoshipCartManager Autoship
    {
        get
        {
            if (_autoship == null)
            {
                _autoship = new AutoshipCartManager();
            }
            return _autoship;
        }
    }
    private AutoshipCartManager _autoship;

    public List<ShoppingCartItemViewModel> CartItems
    {
        get
        {
            if (_cartItems == null)
            {
                var response = ExigoApiContext.CreateWebServiceContext().GetItems(new GetItemsRequest
                {
                    WarehouseID = Autoship.Configuration.WarehouseID,
                    PriceType = Autoship.Configuration.PriceTypeID,
                    CurrencyCode = Autoship.Configuration.CurrencyCode,
                    LanguageID = Autoship.Configuration.LanguageID,
                    RestrictToWarehouse = true,
                    ReturnLongDetail = false,

                    ItemCodes = Autoship.Cart.Items.Select(i => i.ItemCode).Distinct().ToArray(),
                });


                _cartItems = new List<ShoppingCartItemViewModel>();
                foreach (var itemResponse in response.Items)
                {
                    var itemViewModel = new ShoppingCartItemViewModel(itemResponse, Autoship.Cart);
                    _cartItems.Add(itemViewModel);

                }
            }
            return _cartItems;
        }
    }
    private List<ShoppingCartItemViewModel> _cartItems;

    public CalculateOrderResponse CalculatedOrder
    {
        get
        {

            if(Autoship.PropertyBag.ShipMethodID == 0)Autoship.PropertyBag.ShipMethodID = 2;
            Autoship.PropertyBag.Save();

            if (_calculatedOrder == null)
            {
                _calculatedOrder = ExigoApiContext.CreateWebServiceContext().CalculateOrder(new CalculateOrderRequest
                {
                    WarehouseID = Autoship.Configuration.WarehouseID,
                    PriceType = Autoship.Configuration.PriceTypeID,
                    CurrencyCode = Autoship.Configuration.CurrencyCode,
                    ReturnShipMethods = true,
                    ShipMethodID = Autoship.PropertyBag.ShipMethodID,
                    Details = Autoship.Cart.Items.Select(i => new OrderDetailRequest
                    {
                        ItemCode = i.ItemCode,
                        Quantity = i.Quantity,
                        ParentItemCode = i.ParentItemCode
                    }).ToArray(),

                    City = Autoship.PropertyBag.ShippingCity,
                    State = Autoship.PropertyBag.ShippingState,
                    Zip = Autoship.PropertyBag.ShippingZip,
                    Country = Autoship.PropertyBag.ShippingCountry
                });
            }
            return _calculatedOrder;
        }
    }
    private CalculateOrderResponse _calculatedOrder;

    private string NewCreditCardPaymentToken
    {
        get
        {
            //if (!string.IsNullOrEmpty(_newCreditCardPaymentToken))
            if (string.IsNullOrEmpty(_newCreditCardPaymentToken))
            {
                var paymentApi = new ExigoPaymentApi();
                _newCreditCardPaymentToken = paymentApi.FetchCreditCardToken
                (
                    Autoship.PropertyBag.CreditCardNumber,
                    Autoship.PropertyBag.CreditCardExpirationDate.Month,
                    Autoship.PropertyBag.CreditCardExpirationDate.Year
                );
            }
            return _newCreditCardPaymentToken;
        }
    }
    private string _newCreditCardPaymentToken;

    private int NewAutoOrderID;
    #endregion

    #region Load Data
    public void PopulatePropertyBagValues_Load()
    {
    }
    #endregion

    #region Order Details
    private OrderDetailRequest[] GetOrderDetails()
    {
        List<OrderDetailRequest> details = new List<OrderDetailRequest>();

        // Add the items from the shopping cart
        Autoship.Cart.Items.ForEach(item =>
        {
            details.Add(new OrderDetailRequest
            {
                ItemCode = item.ItemCode,
                Quantity = item.Quantity,
                ParentItemCode = item.ParentItemCode
            });
        });

        return details.ToArray();
    }
    #endregion

    #region Exigo API Requests
    // Creating an order
    private CreateAutoOrderRequest Request_SaveAutoship()
    {
        CreateAutoOrderRequest request = new CreateAutoOrderRequest();

        if (Autoship.PropertyBag.ExistingAutoshipID > 0)
        {
            request.ExistingAutoOrderID = Autoship.PropertyBag.ExistingAutoshipID;
            request.OverwriteExistingAutoOrder = true;
        }

        if(Autoship.PropertyBag.ShipMethodID == 0)Autoship.PropertyBag.ShipMethodID = 8;
        Autoship.PropertyBag.Save();

        request.CustomerID = Identity.Current.CustomerID;
        request.CurrencyCode = Autoship.Configuration.CurrencyCode;
        request.WarehouseID = Autoship.Configuration.WarehouseID;
        request.PriceType = Autoship.Configuration.PriceTypeID;
        request.ShipMethodID = Autoship.PropertyBag.ShipMethodID;
        request.Frequency = Autoship.PropertyBag.Frequency;
        request.StartDate = Autoship.PropertyBag.StartDate;
        request.Description = Autoship.PropertyBag.AutoshipDescription;

        switch(Autoship.PropertyBag.PaymentType)
        {
            case AutoshipCartPropertyBag.PaymentMethodType.PrimaryCreditCard: request.PaymentType = Exigo.WebService.AutoOrderPaymentType.PrimaryCreditCard; break;
            case AutoshipCartPropertyBag.PaymentMethodType.SecondaryCreditCard: request.PaymentType = Exigo.WebService.AutoOrderPaymentType.SecondaryCreditCard; break;
            case AutoshipCartPropertyBag.PaymentMethodType.NewCreditCard: 
                if (Autoship.PropertyBag.CreditCardType == AccountCreditCardType.Primary) request.PaymentType = Exigo.WebService.AutoOrderPaymentType.PrimaryCreditCard;
                if (Autoship.PropertyBag.CreditCardType == AccountCreditCardType.Secondary) request.PaymentType = Exigo.WebService.AutoOrderPaymentType.SecondaryCreditCard; 
                break;

            case AutoshipCartPropertyBag.PaymentMethodType.BankAccountOnFile:
            case AutoshipCartPropertyBag.PaymentMethodType.NewBankAccount: request.PaymentType = Exigo.WebService.AutoOrderPaymentType.CheckingAccount; break;

            default: throw new Exception(Autoship.PropertyBag.PaymentType.ToString() + " is an unsupported payment type for autoships at this time.");
        }

        request.FirstName = Autoship.PropertyBag.ShippingFirstName;
        request.LastName = Autoship.PropertyBag.ShippingLastName;
        request.Address1 = Autoship.PropertyBag.ShippingAddress1;
        request.Address2 = Autoship.PropertyBag.ShippingAddress2;
        request.City = Autoship.PropertyBag.ShippingCity;
        request.State = Autoship.PropertyBag.ShippingState;
        request.Zip = Autoship.PropertyBag.ShippingZip;
        request.Country = Autoship.PropertyBag.ShippingCountry;
        request.Phone = Autoship.PropertyBag.Phone;
        request.Email = Autoship.PropertyBag.Email;

        request.Details = GetOrderDetails();

        return request;
    }


    // Handling credit cards
    private SetAccountCreditCardTokenRequest Request_SaveNewCreditCardToAccount(AccountCreditCardType creditCardType)
    {
        SetAccountCreditCardTokenRequest request = new SetAccountCreditCardTokenRequest();

        request.CustomerID = Identity.Current.CustomerID;

        request.CreditCardToken = NewCreditCardPaymentToken;
        request.CreditCardAccountType = creditCardType;
        request.ExpirationMonth = Autoship.PropertyBag.CreditCardExpirationDate.Month;
        request.ExpirationYear = Autoship.PropertyBag.CreditCardExpirationDate.Year;

        request.BillingName = Autoship.PropertyBag.CreditCardNameOnCard;
        request.BillingAddress = Autoship.PropertyBag.CreditCardBillingAddress;
        request.BillingCity = Autoship.PropertyBag.CreditCardBillingCity;
        request.BillingState = Autoship.PropertyBag.CreditCardBillingState;
        request.BillingZip = Autoship.PropertyBag.CreditCardBillingZip;
        request.BillingCountry = Autoship.PropertyBag.CreditCardBillingCountry;

        return request;
    }


    // Handling bank accounts
    private SetAccountCheckingRequest Request_SaveNewBankAccountToAccount()
    {
        SetAccountCheckingRequest request = new SetAccountCheckingRequest();

        request.CustomerID = Identity.Current.CustomerID;

        request.BankName = Autoship.PropertyBag.BankAccountBankName;
        request.NameOnAccount = Autoship.PropertyBag.BankAccountNameOnAccount;
        request.BankAccountType = Autoship.PropertyBag.BankAccountAccountType;
        request.BankAccountNumber = Autoship.PropertyBag.BankAccountAccountNumber;
        request.BankRoutingNumber = Autoship.PropertyBag.BankAccountRoutingNumber;

        request.BillingAddress = Autoship.PropertyBag.BankAccountBankAddress;
        request.BillingCity = Autoship.PropertyBag.BankAccountBankCity;
        request.BillingState = Autoship.PropertyBag.BankAccountBankState;
        request.BillingZip = Autoship.PropertyBag.BankAccountBankZip;
        request.BillingCountry = Autoship.PropertyBag.BankAccountBankCountry;

        return request;
    }
    #endregion

    #region Exigo API Transaction Requests
    private TransactionalRequest Request_AutoshipTransaction()
    {
        TransactionalRequest request = new TransactionalRequest();
        List<ApiRequest> details = new List<ApiRequest>();


        // Add the request to create an order
        details.Add(Request_SaveAutoship());


        // Add the requests that handle the payment method for the order
        switch (Autoship.PropertyBag.PaymentType)
        {
            case AutoshipCartPropertyBag.PaymentMethodType.NewCreditCard:
                details.Add(Request_SaveNewCreditCardToAccount(Autoship.PropertyBag.CreditCardType));
                break;

            case AutoshipCartPropertyBag.PaymentMethodType.NewBankAccount:
                details.Add(Request_SaveNewBankAccountToAccount());
                break;
        }


        request.TransactionRequests = details.ToArray();
        return request;
    }
    #endregion

    #region Submit Order
    private void SubmitOrderToExigoAPI()
    {
        // Call the Exigo API to process the order transaction and all the requests contained inside.
        var orderResponse = ExigoApiContext.CreateWebServiceContext().ProcessTransaction(Request_AutoshipTransaction());


        // If successful, parse each APIResponse object and grab the necessary variables.
        if (orderResponse.Result.Status == ResultStatus.Success)
        {
            foreach (var apiResponse in orderResponse.TransactionResponses)
            {
                if (apiResponse is CreateAutoOrderResponse) NewAutoOrderID = ((CreateAutoOrderResponse)apiResponse).AutoOrderID;
            }
            #region Create Litmos User
            if (IsPurchasingTheStrongbrookAcademySubscription)
            {
                CreateLitmosUserID();
                SendRestCall();
                CreateWealthClubMonthlySubscription();
                Request_UpdateCustomer();

                string response = AddLitmosTeamForCoaching();
                if (response != "Success")
                {
                    ErrorString += "We could not create your Academy Account";
                }
            }
            #endregion Create Litmos User
        }
    }
    #endregion

    #region Validation Methods
    // Validates the credit card against a Luhn algorithm to ensure the card is valid. This method will return true if the card number is identified as a test credit card number.
    private bool IsCreditCardValid(string cardNumber)
    {
        if (IsTestCreditCard(cardNumber)) return true;

        const string allowed = "0123456789";
        int i;

        StringBuilder cleanNumber = new StringBuilder();
        for (i = 0; i < cardNumber.Length; i++)
        {
            if (allowed.IndexOf(cardNumber.Substring(i, 1)) >= 0)
                cleanNumber.Append(cardNumber.Substring(i, 1));
        }
        if (cleanNumber.Length < 13 || cleanNumber.Length > 16)
            return false;

        for (i = cleanNumber.Length + 1; i <= 16; i++)
            cleanNumber.Insert(0, "0");

        int multiplier, digit, sum, total = 0;
        string number = cleanNumber.ToString();

        for (i = 1; i <= 16; i++)
        {
            multiplier = 1 + (i % 2);
            digit = int.Parse(number.Substring(i - 1, 1));
            sum = digit * multiplier;
            if (sum > 9)
                sum -= 9;
            total += sum;
        }

        return (total % 10 == 0);
    }

    // Return a boolean that reflects whether the credit card equals "9696" or not.
    private bool IsTestCreditCard(string cardNumber)
    {
        return (cardNumber == "9696");
    }

    // Return a boolean that reflects whether the credit card equals "9696" or not.
    private bool IsTestBankAccountAccountNumber(string bankAccountAccountNumber)
    {
        return (bankAccountAccountNumber == "9696");
    }
    #endregion

    #region Populate Data
    public void PopulateAvailableShippingMethods_Load()
    {
        // Populate the available ship methods if we have any.
        if (CalculatedOrder.ShipMethods.Length == 0)
        {
            // Do nothing
        }
        else
        {
            rdoShipMethod.Items.Clear();


            foreach (var shipMethod in CalculatedOrder.ShipMethods
                .Where(c => Autoship.Configuration.AvailableShipMethods.Contains(c.ShipMethodID))
                .OrderBy(s => s.ShippingAmount))
            {
                ListItem newListItem = new ListItem
                {
                    Text = shipMethod.Description + " (" + shipMethod.ShippingAmount.ToString("C") + ")",
                    Value = shipMethod.ShipMethodID.ToString()
                };


                if (Autoship.PropertyBag.ShipMethodID == shipMethod.ShipMethodID)
                {
                    // De-select any previously selected items
                    rdoShipMethod.Items.Cast<ListItem>().Where(i => i.Selected == true).ToList().ForEach(i => i.Selected = false);

                    // Select our new list item
                    newListItem.Selected = true;
                }


                rdoShipMethod.Items.Add(newListItem);
            }


            // Do one final check to see if any ship methods in the radio list are currently selected, and if not, select the least expensive option that isn't $0.00
            var selectedShippingMethodItems = rdoShipMethod.Items.Cast<ListItem>().Where(i => i.Selected == true).ToList();
            if (selectedShippingMethodItems.Count == 0)
            {
                if(OrderShouldShip)
                {
                    var defaultShippingMethodID = CalculatedOrder.ShipMethods.Where(s => s.ShippingAmount > 0M && s.ShipMethodID < 4 && s.ShipMethodID > 8).OrderBy(s => s.ShippingAmount).FirstOrDefault();
                    if (defaultShippingMethodID != null)
                    {
                        rdoShipMethod.Items.FindByValue(defaultShippingMethodID.ShipMethodID.ToString()).Selected = true;
                    }                
                }
                else
                {
                    var defaultShippingMethodID = CalculatedOrder.ShipMethods.Where(s => s.ShipMethodID == 8).OrderBy(s => s.ShippingAmount).FirstOrDefault();
                    if (defaultShippingMethodID != null)
                    {
                        rdoShipMethod.Items.FindByValue(defaultShippingMethodID.ShipMethodID.ToString()).Selected = true;
                    }                   
                }

            }
        }
    }
    public void PopulateAvailableShippingMethods_Load2()
    {
        // Populate the available ship methods if we have any.
        if (CalculatedOrder.ShipMethods.Length == 0)
        {
            // Do nothing
        }
        else
        {
            rdoShipMethod2.Items.Clear();


            foreach (var shipMethod in CalculatedOrder.ShipMethods
                .Where(c => Autoship.Configuration2.AvailableShipMethods.Contains(c.ShipMethodID))
                .OrderBy(s => s.ShippingAmount))
            {
                ListItem newListItem = new ListItem
                {
                    Text = shipMethod.Description + " (" + shipMethod.ShippingAmount.ToString("C") + ")",
                      Value = shipMethod.ShipMethodID.ToString()                  
                };

                if (Autoship.PropertyBag.ShipMethodID == shipMethod.ShipMethodID)
                {
                    // De-select any previously selected items
                    rdoShipMethod2.Items.Cast<ListItem>().Where(i => i.Selected == true).ToList().ForEach(i => i.Selected = false);

                    // Select our new list item
                    newListItem.Selected = true;
                }

                rdoShipMethod2.Items.Add(newListItem);
            }


            // Do one final check to see if any ship methods in the radio list are currently selected, and if not, select the least expensive option that is $0.00
            var selectedShippingMethodItems = rdoShipMethod2.Items.Cast<ListItem>().Where(i => i.Selected == true).ToList();
            if (selectedShippingMethodItems.Count == 0)
            {
                if(OrderShouldShip)
                {
                    var defaultShippingMethodID = CalculatedOrder.ShipMethods.Where(s => s.ShippingAmount > 0M && s.ShipMethodID < 4 && s.ShipMethodID > 8).OrderBy(s => s.ShippingAmount).FirstOrDefault();
                    if (defaultShippingMethodID != null)
                    {
                        rdoShipMethod2.Items.FindByValue(defaultShippingMethodID.ShipMethodID.ToString()).Selected = true;
                    }                
                }
                else
                {
                    var defaultShippingMethodID = CalculatedOrder.ShipMethods.FirstOrDefault();
                    if (defaultShippingMethodID != null)
                    {
                        rdoShipMethod2.Items.FindByValue(defaultShippingMethodID.ShipMethodID.ToString()).Selected = true;
                    }                   
                }

            }
        }
    }
    #endregion

    #region View Models
    public class ShoppingCartItemViewModel
    {
        public ShoppingCartItemViewModel(ItemResponse itemResponse, ShoppingCart cart)
        {
            this.ItemCode = itemResponse.ItemCode;
            this.Description = itemResponse.Description;
            this.PriceEach = itemResponse.Price;
            this.BV = itemResponse.BusinessVolume;
            this.CV = itemResponse.CommissionableVolume;
            this.Image = GlobalUtilities.GetProductImagePath(itemResponse.TinyPicture);

            var quantity = cart.Items
                        .Where(i => i.ItemCode == itemResponse.ItemCode)
                        .Select(i => i.Quantity).FirstOrDefault();

            this.Quantity = quantity;
            this.PriceTotal = itemResponse.Price * quantity;
        }

        public string ItemCode { get; set; }
        public string Description { get; set; }
        public decimal PriceEach { get; set; }
        public decimal PriceTotal { get; set; }
        public decimal BV { get; set; }
        public decimal CV { get; set; }
        public string Image { get; set; }
        public decimal Quantity { get; set; }
    }
    #endregion

    #region Event Handlers
    public void PlaceOrder_Click(object sender, EventArgs e)
    {
        try
        {
            // Submit our order to the Exigo API for order creation and payment processing.
            SubmitOrderToExigoAPI();


            // Reset the cart now that we're done with it.
            Autoship.Reset();


            // Redirect to the final page.
            Response.Redirect(Autoship.GetStepUrl(AutoshipManagerStep.Complete) + "?orderid=" + NewAutoOrderID);
        }
        catch (Exception ex)
        {
            Response.Write(ex.Message);
            if(ex.Message.Equals("The remote server returned an error: (409) Conflict."))
            {
                Response.Write("<br />The email on your account is already on file with Strongbrook Academy.<br />You most likely already have a Strongbrook Academy account.");
            }
            if(ex.Message.Equals("The remote server returned an error: (400) Bad Request."))
            {
                Response.Write("<br />Please double check your email address.<br />You must have a valid email address on file with Strongbrook.");
            }




            
            ErrorString += ex.Message;
        }
    }

    public void ChangeShippingMethod_SelectedIndexChanged(object sender, EventArgs e)
    {
        RaisePostBackEvent("ChangeShippingMethod");
    }
    #endregion

    #region IPostBackEventHandlers
    public void RaisePostBackEvent(string eventArgument)
    {
        string[] args = eventArgument.Split('|');

        switch (args[0])
        {
            case "ChangeShippingMethod":
                Autoship.PropertyBag.ShipMethodID = Convert.ToInt32(rdoShipMethod.SelectedValue);
                Autoship.PropertyBag.ShipMethodID = Convert.ToInt32(rdoShipMethod2.SelectedValue);
                Autoship.PropertyBag.Save();

                Response.Redirect(Request.Url.AbsoluteUri);
                break;

            case "ChangeShippingAddress":
                Autoship.PropertyBag.ReferredByEndOfCheckout = true;
                Autoship.PropertyBag.Save();
                Response.Redirect(Autoship.GetStepUrl(AutoshipManagerStep.ShippingAddress));
                break;

            case "ChangePayment":
                Autoship.PropertyBag.ReferredByEndOfCheckout = true;
                Autoship.PropertyBag.Save();
                Response.Redirect(Autoship.GetStepUrl(AutoshipManagerStep.Payment));
                break;

            case "ChangeItems":
                Autoship.PropertyBag.ReferredByEndOfCheckout = true;
                Autoship.PropertyBag.Save();
                Response.Redirect(Autoship.GetStepUrl(AutoshipManagerStep.Cart));
                break;

            case "ChangeDetails":
                Autoship.PropertyBag.ReferredByEndOfCheckout = true;
                Autoship.PropertyBag.Save();
                Response.Redirect(Autoship.GetStepUrl(AutoshipManagerStep.Details));
                break;


            default:
                throw new Exception("RaisePostBackEvent argument '" + args[0] + "' is not defined.");
        }
    }
    #endregion

    #region SEP Integration (Litmos)

    #region Properties required by Litmos
    public string SepUserCreateWebServiceUrl = "http://service.netgenondemand.com/Service/UserAccountAPI.aspx";
    public string SepWebServiceClientId = "01840765-8685-41c7-93c6-c614c0e70790---2f91efb3-e6ff-48b5-b5b5-c48da3d55468";
    public string SepWebServiceReferrerUrl = "http://www.strongbrookenroll.com";
    public int CustomerID = Identity.Current.CustomerID;
    public string FirstName = Identity.Current.FirstName;
    public string LastName = Identity.Current.LastName;
    public string Username = Identity.Current.Website.LoginName;
    public string DaytimePhone = Identity.Current.ContactInfo.Phone;
    public string email = Identity.Current.ContactInfo.Email;
    public int CustTy = Identity.Current.CustomerTypeID;
    public string LitmosID { get; set; }
    #region Get the Password from the cookie
    public string PasswordCookie = "Password";
    public string SavedLoginPassword { get; set; }
    private string Password
    {
        get
        {
            // Gets the password from the cookie
            if (Request.Cookies[PasswordCookie] != null)
            {
                string cookiePasswdEncrypted = Request.Cookies[PasswordCookie].Value;
                string cookiePasswdDecrypted = Decrypt(cookiePasswdEncrypted, "theKey").Split('|')[0];
                SavedLoginPassword = cookiePasswdDecrypted;
            }
            return SavedLoginPassword;
        }
    }

    #endregion Get the password from the cookie
    public bool oneOfTheItemsIsForStrongbrookAcademy;
    public bool IsPurchasingTheStrongbrookAcademySubscription
    {
        get
        {
            Autoship.Cart.Items.ForEach(item =>
            {
                if (item.ItemCode == "3120" || item.ItemCode == "3130") oneOfTheItemsIsForStrongbrookAcademy = true;
                else { oneOfTheItemsIsForStrongbrookAcademy = false; }
            });

            if(oneOfTheItemsIsForStrongbrookAcademy) return true;
            else { return false; }
        }
    }

    #endregion Properties required by Litmos

    #region XML
    private static byte[] GenerateLitmostXML(string userName, string firstName, string lastName, string email, string phone)
    {
        MemoryStream stream = new MemoryStream();

        XmlTextWriter writer = new XmlTextWriter(stream, Encoding.UTF8);
        writer.Formatting = Formatting.Indented;
        writer.WriteStartDocument();
        writer.WriteStartElement("User");
        writer.WriteStartElement("UserName");
        writer.WriteString(userName);
        writer.WriteEndElement();
        writer.WriteStartElement("FirstName");
        writer.WriteString(firstName);
        writer.WriteEndElement();
        writer.WriteStartElement("LastName");
        writer.WriteString(lastName);
        writer.WriteEndElement();
        writer.WriteStartElement("Email");
        writer.WriteString(email);
        writer.WriteEndElement();
        writer.WriteStartElement("DisableMessages");
        writer.WriteString("false");
        writer.WriteEndElement();
        writer.WriteStartElement("Active");
        writer.WriteString("true");
        writer.WriteEndElement();
        writer.WriteStartElement("PhoneWork");
        writer.WriteString(phone);
        writer.WriteEndElement();
        writer.WriteStartElement("SkipFirstLogin");
        writer.WriteString("false");
        writer.WriteEndElement();
        writer.WriteEndElement();
        writer.WriteEndDocument();
        writer.Flush();
        writer.Close();

        return stream.ToArray();
    }
    #endregion XML

    #region REST calls
    private string apiKey
    {
        get { return "558755F0-2546-48CE-925C-18681D4D5909"; }
    }
    private string source
    {
        get { return "StrongBrook"; }
    }
    //private string teamID = "-G3OQ3zKHoc1";
    //private string courseID = "14054-A";
    public void SendRestCall()
    {
        //Set Litmos URI
        Uri uri = new Uri("https://api.litmos.com/v1.svc");

        //Create XML for USER creation
        byte[] dataByte = GenerateLitmostXML(email, FirstName, LastName, email, DaytimePhone);

        //Send XML to API for creation
        HttpWebRequest req = WebRequest.Create(uri + "/users?apikey=" + apiKey + "&source=" + source) as HttpWebRequest;
        req.Method = "POST";

        req.ContentType = "text/xml";
        req.KeepAlive = false;
        req.Timeout = 400000;
        req.ContentLength = dataByte.Length;

        Stream POSTstream = req.GetRequestStream();
        POSTstream.Write(dataByte, 0, dataByte.Length);
        string userID;

        HttpWebResponse res = req.GetResponse() as HttpWebResponse;

        //if not successful, send error
        if (res.StatusCode == HttpStatusCode.Created)
        {
            string xmlString = new StreamReader(res.GetResponseStream(), Encoding.UTF8).ReadToEnd();
            using (XmlReader reader = XmlReader.Create(new StringReader(xmlString)))
            {
                reader.ReadToFollowing("Id");
                userID = reader.ReadElementContentAsString();
                LitmosID = userID;
            }

            dataByte = AddLitmosTeam(userID, email, FirstName, LastName); SendLitmosTeamCall(uri, dataByte);

        }
        else
        {
            //ErrorString += "Litmos was not created successfully";
        }
    }
    public void SendLitmosTeamCall(Uri uri, byte[] dataByte)
    {
        HttpWebRequest req = WebRequest.Create("https://api.litmos.com/v1.svc/teams/-G3OQ3zKHoc1/users?apikey=" + apiKey + "&source=" + source) as HttpWebRequest;
        //HttpWebRequest req = WebRequest.Create(uri + "/teams/" + teamID + "/users?apikey=" + apiKey + "&source=" + source) as HttpWebRequest;
        req.Method = "POST";

        req.ContentType = "text/xml";
        req.KeepAlive = false;
        req.Timeout = 400000;
        req.ContentLength = dataByte.Length;

        Stream POSTstream = req.GetRequestStream();
        POSTstream.Write(dataByte, 0, dataByte.Length);

        HttpWebResponse res = req.GetResponse() as HttpWebResponse;

        if (res.StatusCode != HttpStatusCode.Created)
        {
            //ErrorString += "Could not add to Team";
        }
    }
    public void SendLitmosCourseCall(Uri uri, byte[] dataByte, string userID)
    {
        HttpWebRequest req = WebRequest.Create(uri + "/users/" + userID + "/courses?apikey=" + apiKey + "&source=" + source) as HttpWebRequest;
        req.Method = "POST";

        req.ContentType = "text/xml";
        req.KeepAlive = false;
        req.Timeout = 400000;
        req.ContentLength = dataByte.Length;

        Stream POSTstream = req.GetRequestStream();
        POSTstream.Write(dataByte, 0, dataByte.Length);

        HttpWebResponse res = req.GetResponse() as HttpWebResponse;

        if (res.StatusCode != HttpStatusCode.Created)
        {
            //ErrorString += "Could not add course";
        }
    }
    public static byte[] AddLitmosCourse(string courseID)
    {
        MemoryStream stream = new MemoryStream();

        XmlTextWriter writer = new XmlTextWriter(stream, Encoding.UTF8);
        writer.Formatting = Formatting.Indented;
        writer.WriteStartDocument();
        writer.WriteStartElement("Courses");
        writer.WriteStartElement("Course");
        writer.WriteStartElement("Id");
        writer.WriteString(courseID);
        writer.WriteEndElement();
        writer.WriteEndElement();
        writer.WriteEndElement();
        writer.WriteEndDocument();
        writer.Flush();
        writer.Close();

        return stream.ToArray();
    }
    public static byte[] AddLitmosTeam(string userID, string userName, string FirstName, string LastName)
    {
        MemoryStream stream = new MemoryStream();

        XmlTextWriter writer = new XmlTextWriter(stream, Encoding.UTF8);
        writer.Formatting = Formatting.Indented;
        writer.WriteStartDocument();
        writer.WriteStartElement("Users");
        writer.WriteStartElement("User");
        writer.WriteStartElement("Id");
        writer.WriteString(userID);
        writer.WriteEndElement();
        writer.WriteStartElement("UserName");
        writer.WriteString(userName);
        writer.WriteEndElement();
        writer.WriteStartElement("FirstName");
        writer.WriteString(FirstName);
        writer.WriteEndElement();
        writer.WriteStartElement("LastName");
        writer.WriteString(LastName);
        writer.WriteEndElement();
        writer.WriteEndElement();
        writer.WriteEndElement();
        writer.WriteEndDocument();
        writer.Flush();
        writer.Close();

        return stream.ToArray();
    }
    public string CreateLitmosAccountForCoaching()
    {
        LitmosUser user = new LitmosUser()
        {
            UserName = email,
            FirstName = FirstName,
            LastName = LastName,
            FullName = FirstName + " " + LastName,
            Email = email,
            DisableMessages = "false",
            PhoneWork = DaytimePhone,
            SkipFirstLogin = "false",
            ExigoCustomerID = CustomerID
        };
        LitmosAccounts litmos = new LitmosAccounts();

        string response = litmos.Create_LitmosUser("SM_6IOz57-g1", user);

        return response;
    }
    public string AddLitmosTeamForCoaching()
    {
        LitmosUser user = new LitmosUser
        {
            UserID = LitmosID
        };

        LitmosAccounts litmos = new LitmosAccounts();
        string response = litmos.Assign_Team(user, "SM_6IOz57-g1");

        return response;
    }
    public void GetCourses(object sender, EventArgs e)
    {
        Uri uri = new Uri("https://api.litmos.com/v1.svc");

        HttpWebRequest req = WebRequest.Create(uri + "/Courses?apikey=" + apiKey + "&source=" + source) as HttpWebRequest;
        req.Method = "GET";

        HttpWebResponse res = req.GetResponse() as HttpWebResponse;

        if (res.StatusCode != HttpStatusCode.Created)
        {
            string xmlString = new StreamReader(res.GetResponseStream(), Encoding.UTF8).ReadToEnd();
            using (XmlReader reader = XmlReader.Create(new StringReader(xmlString)))
            {
                reader.ReadToFollowing("Id");
                string userID = reader.ReadElementContentAsString();
            }
        }
    }

    #endregion REST calls

    #region Create the Autoship Order
    private CreateAutoOrderRequest CreateWealthClubMonthlySubscription()
    {
        CreateAutoOrderRequest request = new CreateAutoOrderRequest();

        request.CustomerID = Identity.Current.CustomerID;

        request.CurrencyCode = Autoship.Configuration.CurrencyCode;
        request.WarehouseID = Autoship.Configuration.WarehouseID;
        request.PriceType = Autoship.Configuration.PriceTypeID;
        request.ShipMethodID = Autoship.PropertyBag.ShipMethodID;

        request.ProcessType = Exigo.WebService.AutoOrderProcessType.AlwaysProcess;
        request.PaymentType = Exigo.WebService.AutoOrderPaymentType.PrimaryCreditCard;
        request.Frequency = Exigo.WebService.FrequencyType.Monthly;
        request.StartDate = DateTime.Now.AddDays(30);

        request.FirstName = Autoship.PropertyBag.ShippingFirstName;
        request.LastName = Autoship.PropertyBag.ShippingLastName;
        request.Address1 = Autoship.PropertyBag.ShippingAddress1;
        request.Address2 = Autoship.PropertyBag.ShippingAddress2;
        request.City = Autoship.PropertyBag.ShippingCity;
        request.State = Autoship.PropertyBag.ShippingState;
        request.Zip = Autoship.PropertyBag.ShippingZip;
        request.Country = Autoship.PropertyBag.ShippingCountry;
        request.Phone = Autoship.PropertyBag.Phone;
        request.Email = Autoship.PropertyBag.Email;
        request.Details = SilverPackageDetails();

        return request;
    }
    OrderDetailRequest[] SilverPackageDetails()
    {
        List<OrderDetailRequest> details = new List<OrderDetailRequest>();
        details.Add(new OrderDetailRequest
        {
            ItemCode = "3120",
            Quantity = 1
        });
        return details.ToArray();
    }

    #endregion Create the Autoship Order



    #region Update the customer with the new Litmos User ID
    protected void CreateLitmosUserID()
    {
        WebResponse result = null;
        try
        {
            string SEPUrl = string.Format(SepUserCreateWebServiceUrl +
                                          "?Action=Create" +
                                          "&ClientID=" + SepWebServiceClientId +
                                          "&ExternalId=" + CustomerID +
                                          "&FirstName=" + FirstName +
                                          "&LastName=" + LastName +
                                          "&Username=" + Username +
                                          "&Password=" + Password +
                                          "&Phone=" + DaytimePhone +
                                          "&Email=" + email +
                                          "&Verbose=false" +
                                          "&ProductId=" + CustTy);

            WebRequest req = WebRequest.Create(SEPUrl);
            req.Timeout = 270000; //give it 4.5 mins (right under script timeout)
            req.Method = "POST";


            // Set our variables
            var h = (HttpWebRequest)req;
            h.Referer = SepWebServiceReferrerUrl;

            Stream requestStream = null;
            req.ContentLength = 0;

            result = req.GetResponse();
            Stream receiveStream = result.GetResponseStream();
            Encoding encode = Encoding.GetEncoding("utf-8");
            var sr = new StreamReader(receiveStream, encode);

            string response = sr.ReadToEnd();

            if (requestStream != null) requestStream.Close();
            sr.Close();
        }
        catch (Exception ex)
        {
            //ErrorString += "An unexpected error has occurred while processing your request.<br />" + ex.Message;
        }
    }
    public void Request_UpdateCustomer()
    {
        UpdateCustomerRequest req = new UpdateCustomerRequest()
        {
            CustomerID = CustomerID,
            Field4 = LitmosID
        };

        UpdateCustomerResponse res = ExigoApiContext.CreateWebServiceContext().UpdateCustomer(req);
    }
    #endregion Update the customer with the new Litmos User ID

    #region Decrypt the cookie data
    string Decrypt(string coded, string key)
    {
        RijndaelManaged cryptProvider = new RijndaelManaged();
        cryptProvider.KeySize = 256;
        cryptProvider.BlockSize = 256;
        cryptProvider.Mode = CipherMode.CBC;
        SHA256Managed hashSHA256 = new SHA256Managed();
        cryptProvider.Key = hashSHA256.ComputeHash(ASCIIEncoding.ASCII.GetBytes(key));
        string iv = "signup";
        cryptProvider.IV = hashSHA256.ComputeHash(ASCIIEncoding.ASCII.GetBytes(iv));
        byte[] cipherTextByteArray = Convert.FromBase64String(coded);
        MemoryStream ms = new MemoryStream();
        CryptoStream cs = new CryptoStream(ms, cryptProvider.CreateDecryptor(), CryptoStreamMode.Write);
        cs.Write(cipherTextByteArray, 0, cipherTextByteArray.Length);
        cs.FlushFinalBlock();
        cs.Close();
        byte[] byt = ms.ToArray();
        return Encoding.ASCII.GetString(byt);
    }

    #endregion Decrypt the cookie data

    #endregion




    #region temp fix

    protected List<ItemResponse> ExigoItems
    {
        get
        {
            if (allItemsList == null)
            {
                // Get the items from Exigo
                GetItemsRequest req = new GetItemsRequest();
                req.WarehouseID = Autoship.Configuration.WarehouseID;
                req.CurrencyCode = Autoship.Configuration.CurrencyCode;
                req.PriceType = Autoship.Configuration.PriceTypeID;
                req.LanguageID = Autoship.Configuration.LanguageID;
                req.WebID = Autoship.Configuration.WebID;
                req.WebCategoryID = Autoship.Configuration.WebCategoryID;
                req.RestrictToWarehouse = true;
                req.ReturnLongDetail = false;
                GetItemsResponse res = ExigoApiContext.CreateWebServiceContext().GetItems(req);

                var itemList = res.Items.ToList();

                allItemsList = itemList;
            }

            return allItemsList;
        }
    }
    private List<ItemResponse> allItemsList;


    public string[] ProductsToShip
    {
        get
        {
            List<string> orders = new List<string>();

            foreach (var item in ExigoItems.Where(itm => itm.IsVirtual == false))
            {
                orders.Add(item.ItemCode);
            }
            return orders.ToArray();
        }
    }

    public List<string> productList = new List<string>();


    public bool OrderShouldShip
    {
        get
        {
            bool isVirtual = false;
            bool ShipOrder = false;

            int i = 0;
            while (i < ProductsToShip.Length)
            {
                if (productList.Contains(ProductsToShip[i]))
                {
                    ShipOrder = true;
                }
                i++;
            }
            if (ShipOrder)
            {
                isVirtual = false;
            }
            else
            {
                isVirtual = true;
            }

            return ShipOrder;
        }
    }

    #endregion temp fix



    public string ErrorString { get; set; }


}
