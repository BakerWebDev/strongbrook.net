﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Exigo.OData;
using Exigo.WebService;
using System.Net;
using System.IO;
using System.Xml;

public partial class ShoppingCheckoutReview : Page, IPostBackEventHandler
{
    #region Page Load
    protected void Page_Load(object sender, EventArgs e)
    {
        cmdPlaceOrder.Text = Resources.Shopping.PlaceYourOrder;

        Shopping.PropertyBag.ReferredByEndOfCheckout = true;
        Shopping.PropertyBag.Save();


        // If there are no items in the cart, redirect them to the cart page.
        if (Shopping.Cart.Items.Count == 0)
        {
            Response.Redirect(Shopping.GetStepUrl(ShoppingStep.Cart));
            return;
        }


        if (!IsPostBack)
        {
            PopulateAvailableShippingMethods_Load();

            PopulatePropertyBagValues_Load();
        }
    }
    #endregion

    #region Properties
    public ShoppingCartManager Shopping
    {
        get
        {
            if (_shopping == null)
            {
                _shopping = new ShoppingCartManager();
            }
            return _shopping;
        }
    }
    private ShoppingCartManager _shopping;

    public List<ShoppingCartItemViewModel> CartItems
    {
        get
        {
            if (_cartItems == null)
            {
                var response = ExigoApiContext.CreateWebServiceContext().GetItems(new GetItemsRequest
                {
                    WarehouseID = Shopping.Configuration.WarehouseID,
                    PriceType = Shopping.Configuration.PriceTypeID,
                    CurrencyCode = Shopping.Configuration.CurrencyCode,
                    LanguageID = Shopping.Configuration.LanguageID,
                    RestrictToWarehouse = true,
                    ReturnLongDetail = false,

                    ItemCodes = Shopping.Cart.Items.Where(i => i.Type == ShoppingCartItemType.Default).Select(i => i.ItemCode).Distinct().ToArray(),
                });


                _cartItems = new List<ShoppingCartItemViewModel>();
                foreach (var itemResponse in response.Items)
                {
                    var itemViewModel = new ShoppingCartItemViewModel(itemResponse, Shopping.Cart, ShoppingCartItemType.Default);
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
            if (_calculatedOrder == null)
            {
                _calculatedOrder = ExigoApiContext.CreateWebServiceContext().CalculateOrder(new CalculateOrderRequest
                {
                    WarehouseID = Shopping.Configuration.WarehouseID,
                    PriceType = Shopping.Configuration.PriceTypeID,
                    CurrencyCode = Shopping.Configuration.CurrencyCode,
                    ReturnShipMethods = true,
                    ShipMethodID = Shopping.PropertyBag.ShipMethodID,
                    Details = Shopping.Cart.Items.Select(i => new OrderDetailRequest
                    {
                        ItemCode = i.ItemCode,
                        Quantity = i.Quantity,
                        ParentItemCode = i.ParentItemCode
                    }).ToArray(),

                    City = Shopping.PropertyBag.ShippingCity,
                    State = Shopping.PropertyBag.ShippingState,
                    Zip = Shopping.PropertyBag.ShippingZip,
                    Country = Shopping.PropertyBag.ShippingCountry
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
            if (!string.IsNullOrEmpty(_newCreditCardPaymentToken))
            {
                var paymentApi = new ExigoPaymentApi();
                _newCreditCardPaymentToken = paymentApi.FetchCreditCardToken
                (
                    Shopping.PropertyBag.CreditCardNameOnCard,
                    Shopping.PropertyBag.CreditCardExpirationDate.Month,
                    Shopping.PropertyBag.CreditCardExpirationDate.Year
                );
            }
            return _newCreditCardPaymentToken;
        }
    }
    private string _newCreditCardPaymentToken;

    private int NewOrderID;
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
        Shopping.Cart.Items.Where(c => c.Type == ShoppingCartItemType.Default).ToList().ForEach(item =>
        {
            details.Add(new OrderDetailRequest
            {
                ItemCode        = item.ItemCode,
                Quantity        = item.Quantity,
                ParentItemCode  = item.ParentItemCode                    
            });
        });

        return details.ToArray();
    }
    #endregion

    #region Exigo API Requests
    // Creating an order
    private CreateOrderRequest Request_CreateOrder()
    {
        CreateOrderRequest request = new CreateOrderRequest();

        request.CustomerID = Identity.Current.CustomerID;

        request.CurrencyCode = Shopping.Configuration.CurrencyCode;
        request.WarehouseID = Shopping.Configuration.WarehouseID;
        request.PriceType = Shopping.Configuration.PriceTypeID;
        request.ShipMethodID = Shopping.PropertyBag.ShipMethodID;
        request.OrderDate = DateTime.Now;
        request.OrderStatus = OrderStatusType.Incomplete;
        request.OrderType = Exigo.WebService.OrderType.APIOrder;

        request.FirstName = Shopping.PropertyBag.ShippingFirstName;
        request.LastName = Shopping.PropertyBag.ShippingLastName;
        request.Address1 = Shopping.PropertyBag.ShippingAddress1;
        request.Address2 = Shopping.PropertyBag.ShippingAddress2;
        request.City = Shopping.PropertyBag.ShippingCity;
        request.State = Shopping.PropertyBag.ShippingState;
        request.Zip = Shopping.PropertyBag.ShippingZip;
        request.Country = Shopping.PropertyBag.ShippingCountry;
        request.Phone = Shopping.PropertyBag.Phone;
        request.Email = Shopping.PropertyBag.Email;

        request.Details = GetOrderDetails();

        return request;
    }


    // Handling credit cards
    private SetAccountCreditCardTokenRequest Request_SaveNewCreditCardToAccount()
    {
        SetAccountCreditCardTokenRequest request = new SetAccountCreditCardTokenRequest();

        request.CustomerID = Identity.Current.CustomerID;

        request.CreditCardToken = NewCreditCardPaymentToken;
        request.CreditCardAccountType = AccountCreditCardType.Secondary;
        request.ExpirationMonth = Shopping.PropertyBag.CreditCardExpirationDate.Month;
        request.ExpirationYear = Shopping.PropertyBag.CreditCardExpirationDate.Year;

        request.BillingName = Shopping.PropertyBag.CreditCardNameOnCard;
        request.BillingAddress = Shopping.PropertyBag.CreditCardBillingAddress;
        request.BillingCity = Shopping.PropertyBag.CreditCardBillingCity;
        request.BillingState = Shopping.PropertyBag.CreditCardBillingState;
        request.BillingZip = Shopping.PropertyBag.CreditCardBillingZip;
        request.BillingCountry = Shopping.PropertyBag.CreditCardBillingCountry;

        return request;
    }
    private ChargeCreditCardTokenRequest Request_ChargeNewCreditCard()
    {
        ChargeCreditCardTokenRequest request = new ChargeCreditCardTokenRequest();

        request.CreditCardToken = NewCreditCardPaymentToken;

        request.BillingName = Shopping.PropertyBag.CreditCardNameOnCard;
        request.BillingAddress = Shopping.PropertyBag.CreditCardBillingAddress;
        request.BillingCity = Shopping.PropertyBag.CreditCardBillingCity;
        request.BillingState = Shopping.PropertyBag.CreditCardBillingState;
        request.BillingZip = Shopping.PropertyBag.CreditCardBillingZip;
        request.BillingCountry = Shopping.PropertyBag.CreditCardBillingCountry;

        return request;
    }
    private ChargeCreditCardTokenOnFileRequest Request_ChargeCreditCardOnFile(AccountCreditCardType creditCardType)
    {
        ChargeCreditCardTokenOnFileRequest request = new ChargeCreditCardTokenOnFileRequest();

        request.CreditCardAccountType = creditCardType;

        return request;
    }


    // Handling bank accounts
    private SetAccountCheckingRequest Request_SaveNewBankAccountToAccount()
    {
        SetAccountCheckingRequest request = new SetAccountCheckingRequest();

        request.CustomerID = Identity.Current.CustomerID;

        request.BankName = Shopping.PropertyBag.BankAccountBankName;
        request.NameOnAccount = Shopping.PropertyBag.BankAccountNameOnAccount;
        request.BankAccountType = Shopping.PropertyBag.BankAccountAccountType;
        request.BankAccountNumber = Shopping.PropertyBag.BankAccountAccountNumber;
        request.BankRoutingNumber = Shopping.PropertyBag.BankAccountRoutingNumber;

        request.BillingAddress = Shopping.PropertyBag.BankAccountBankAddress;
        request.BillingCity = Shopping.PropertyBag.BankAccountBankCity;
        request.BillingState = Shopping.PropertyBag.BankAccountBankState;
        request.BillingZip = Shopping.PropertyBag.BankAccountBankZip;
        request.BillingCountry = Shopping.PropertyBag.BankAccountBankCountry;

        return request;
    }
    private DebitBankAccountRequest Request_ChargeNewBankAccount()
    {
        DebitBankAccountRequest request = new DebitBankAccountRequest();

        request.BankName = Shopping.PropertyBag.BankAccountBankName;
        request.NameOnAccount = Shopping.PropertyBag.BankAccountNameOnAccount;
        request.BankAccountType = Shopping.PropertyBag.BankAccountAccountType;
        request.BankAccountNumber = Shopping.PropertyBag.BankAccountAccountNumber;
        request.BankRoutingNumber = Shopping.PropertyBag.BankAccountRoutingNumber;

        request.BillingAddress = Shopping.PropertyBag.BankAccountBankAddress;
        request.BillingCity = Shopping.PropertyBag.BankAccountBankCity;
        request.BillingState = Shopping.PropertyBag.BankAccountBankState;
        request.BillingZip = Shopping.PropertyBag.BankAccountBankZip;
        request.BillingCountry = Shopping.PropertyBag.BankAccountBankCountry;

        return request;
    }
    private DebitBankAccountOnFileRequest Request_ChargeBankAccountOnFile()
    {
        DebitBankAccountOnFileRequest request = new DebitBankAccountOnFileRequest();

        return request;
    }

    #region Obsolete Methods
    [Obsolete("This method saves the credit card WITHOUT using tokenization, which is NOT PCI-Compliant. This method should not be used unless otherwise informed by Exigo.")]
    private SetAccountCreditCardRequest Request_SaveCreditCard_Legacy()
    {
        // DEVELOPER NOTE: This method saves the credit card WITHOUT using tokenization, 
        // which is NOT PCI-Compliant. This method should not be used unless otherwise informed
        // by Exigo.


        var request = new SetAccountCreditCardRequest();

        request.CreditCardAccountType       = AccountCreditCardType.Secondary;
        request.BillingName                 = Shopping.PropertyBag.CreditCardNameOnCard;
        request.CreditCardNumber            = Shopping.PropertyBag.CreditCardNumber;
        request.ExpirationMonth             = Shopping.PropertyBag.CreditCardExpirationDate.Month;
        request.ExpirationYear              = Shopping.PropertyBag.CreditCardExpirationDate.Year;
        request.CvcCode                     = Shopping.PropertyBag.CreditCardCvc;

        request.BillingAddress              = Shopping.PropertyBag.CreditCardBillingAddress;
        request.BillingCity                 = Shopping.PropertyBag.CreditCardBillingCity;
        request.BillingState                = Shopping.PropertyBag.CreditCardBillingState;
        request.BillingZip                  = Shopping.PropertyBag.CreditCardBillingZip;
        request.BillingCountry              = Shopping.PropertyBag.CreditCardBillingCountry;

        return request;
    }

    [Obsolete("This method charges the credit card WITHOUT using tokenization, which is NOT PCI-Compliant. This method should not be used unless otherwise informed by Exigo.")]
    private ChargeCreditCardRequest Request_ChargeCreditCard_Legacy()
    {
        // DEVELOPER NOTE: This method charges the credit card WITHOUT using tokenization, 
        // which is NOT PCI-Compliant. This method should not be used unless otherwise informed
        // by Exigo.


        var request = new ChargeCreditCardRequest();

        request.BillingName                 = Shopping.PropertyBag.CreditCardNameOnCard;
        request.CreditCardNumber            = Shopping.PropertyBag.CreditCardNumber;
        request.ExpirationMonth             = Shopping.PropertyBag.CreditCardExpirationDate.Month;
        request.ExpirationYear              = Shopping.PropertyBag.CreditCardExpirationDate.Year;
        request.CvcCode                     = Shopping.PropertyBag.CreditCardCvc;

        request.BillingAddress              = Shopping.PropertyBag.CreditCardBillingAddress;
        request.BillingCity                 = Shopping.PropertyBag.CreditCardBillingCity;
        request.BillingState                = Shopping.PropertyBag.CreditCardBillingState;
        request.BillingZip                  = Shopping.PropertyBag.CreditCardBillingZip;
        request.BillingCountry              = Shopping.PropertyBag.CreditCardBillingCountry;

        return request;
    }
    #endregion
    #endregion

    #region Exigo API Transaction Requests
    private TransactionalRequest Request_OrderTransaction()
    {
        TransactionalRequest request = new TransactionalRequest();
        List<ApiRequest> details = new List<ApiRequest>();


        // Add the request to create an order
        details.Add(Request_CreateOrder());


        // Add the requests that handle the payment method for the order
        switch (Shopping.PropertyBag.PaymentType)
        {
            case ShoppingCartPropertyBag.PaymentMethodType.NewCreditCard:
                if (!IsTestCreditCard(Shopping.PropertyBag.CreditCardNumber))
                {
                    details.Add(Request_SaveNewCreditCardToAccount());
                    details.Add(Request_ChargeNewCreditCard());
                }
                break;

            case ShoppingCartPropertyBag.PaymentMethodType.PrimaryCreditCard:
                details.Add(Request_ChargeCreditCardOnFile(AccountCreditCardType.Primary));
                break;

            case ShoppingCartPropertyBag.PaymentMethodType.SecondaryCreditCard:
                details.Add(Request_ChargeCreditCardOnFile(AccountCreditCardType.Secondary));
                break;

            case ShoppingCartPropertyBag.PaymentMethodType.NewBankAccount:
                if (!IsTestBankAccountAccountNumber(Shopping.PropertyBag.BankAccountAccountNumber))
                {
                    details.Add(Request_SaveNewBankAccountToAccount());
                    details.Add(Request_ChargeNewBankAccount());
                }
                break;
                
            case ShoppingCartPropertyBag.PaymentMethodType.BankAccountOnFile:
                details.Add(Request_ChargeBankAccountOnFile());
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
        var orderResponse = ExigoApiContext.CreateWebServiceContext().ProcessTransaction(Request_OrderTransaction());


        // If successful, parse each APIResponse object and grab the necessary variables.
        if (orderResponse.Result.Status == ResultStatus.Success)
        {
            foreach (var apiResponse in orderResponse.TransactionResponses)
            {
                if (apiResponse is CreateOrderResponse) NewOrderID = ((CreateOrderResponse)apiResponse).OrderID;
            }
            CreateLitmosUserID();
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
                .Where(c => Shopping.Configuration.AvailableShipMethods.Contains(c.ShipMethodID))
                .OrderBy(s => s.ShippingAmount))
            {
                ListItem newListItem = new ListItem
                {
                    Text = shipMethod.Description + " (" + shipMethod.ShippingAmount.ToString("C") + ")",
                    Value = shipMethod.ShipMethodID.ToString()
                };


                if (Shopping.PropertyBag.ShipMethodID == shipMethod.ShipMethodID)
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
                var defaultShippingMethodID = CalculatedOrder.ShipMethods.Where(s => s.ShippingAmount > 0M).OrderBy(s => s.ShippingAmount).FirstOrDefault();
                if (defaultShippingMethodID != null)
                {
                    rdoShipMethod.Items.FindByValue(defaultShippingMethodID.ShipMethodID.ToString()).Selected = true;
                }
            }
        }
    }
    #endregion

    #region View Models
    public class ShoppingCartItemViewModel
    {
        public ShoppingCartItemViewModel(ItemResponse itemResponse, ShoppingCart cart, ShoppingCartItemType type)
        {
            this.ItemCode = itemResponse.ItemCode;
            this.Description = itemResponse.Description;
            this.PriceEach = itemResponse.Price;
            this.BV = itemResponse.BusinessVolume;
            this.CV = itemResponse.CommissionableVolume;
            this.Image = GlobalUtilities.GetProductImagePath(itemResponse.TinyPicture);

            var quantity = cart.Items
                        .Where(i => i.ItemCode == itemResponse.ItemCode && i.Type == type)
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
        public ShoppingCartItemType Type { get; set; }
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
            Shopping.Reset();


            // Redirect to the final page.
            Response.Redirect(Shopping.GetStepUrl(ShoppingStep.Complete) + "?orderid=" + NewOrderID);
        }
        catch (Exception ex)
        {
            ApplicationErrors.ErrorMessage += ex.Message;
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
                Shopping.PropertyBag.ShipMethodID = Convert.ToInt32(rdoShipMethod.SelectedValue);
                Shopping.PropertyBag.Save();

                Response.Redirect(Request.Url.AbsoluteUri);
                break;

            case "ChangeShippingAddress":
                Shopping.PropertyBag.ReferredByEndOfCheckout = true;
                Shopping.PropertyBag.Save();
                Response.Redirect(Shopping.GetStepUrl(ShoppingStep.ShippingAddress));
                break;

            case "ChangePayment":
                Shopping.PropertyBag.ReferredByEndOfCheckout = true;
                Shopping.PropertyBag.Save();
                Response.Redirect(Shopping.GetStepUrl(ShoppingStep.Payment));
                break;

            case "ChangeItems":
                Shopping.PropertyBag.ReferredByEndOfCheckout = true;
                Shopping.PropertyBag.Save();
                Response.Redirect(Shopping.GetStepUrl(ShoppingStep.Cart));
                break;


            default:
                throw new Exception("RaisePostBackEvent argument '" + args[0] + "' is not defined.");
        }
    }
    #endregion

    #region SEP Integration (Litmos)
    //SEP Integration
    public string SepUserCreateWebServiceUrl { get; set; }
    public string SepWebServiceClientId { get; set; }
    public string SepWebServiceReferrerUrl { get; set; }

    public int CustomerID = Identity.Current.CustomerID;
    public string FirstName = Identity.Current.FirstName;
    public string LastName = Identity.Current.LastName;
    public string Username = Identity.Current.Website.LoginName;
    public string Password = Identity.Current.Website.Password;
    public string DaytimePhone = Identity.Current.ContactInfo.Phone;
    public string email = Identity.Current.ContactInfo.Email;
    public int CustTy = Identity.Current.CustomerTypeID;
    public string LitmosID { get; set; }

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
    #endregion

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
    #endregion

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

    #endregion
}
