using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Net.Mail;
using Exigo.WebService;
using System.Windows.Forms;
using System.ComponentModel;
using System.Web.Services;

public partial class GPRform : System.Web.UI.Page
{
    #region Page Load
    protected void Page_Load(object sender, EventArgs e)
    {
        drdlTimeZone.Attributes.Add("onchange", "Save();" + Page.ClientScript.GetPostBackEventReference(drdlTimeZone, null).ToString());
        if (!IsPostBack)
        {
            //
            drdlTimeZone.Attributes.Add("onchange", "getdata()");
            //drdlTimeZone.Attributes.Add("onchange", "update()");

            //drdcity.Attributes.Add("onchange", "getdata2()");
        }
        if (IsPostBack)
        {

            SetCurrentUser();
            Click_Submit();
        }
        PopulateAvailabilityFields();
        PopulateNetWorthFields();
    }

    #endregion Page Load

    #region Exigo API requests
    private CreateCustomerLeadRequest Request_CreateCustomerLead()
    {
        var NotesInLongForm = new StringBuilder();
        NotesInLongForm.AppendFormat(@"
            <table>
                <tr>
                    <td>
                        Comments: {0}
                    </td>
                </tr>
                <tr>
                    <td>
                        This Lead came from the GPR form of: {1}
                    </td>
                </tr>
            </table>
        "
        , Comments
        , LastName
        );

        CreateCustomerLeadRequest request = new CreateCustomerLeadRequest();
        try
        {
            request.CustomerID = CurrentUser_ID;
            request.FirstName = FirstName;
            request.LastName = LastName;
            request.Phone = Phone1;
            request.Phone2 = Phone2;
            request.Email = Email;
            //request.Address1 = LikelyAvailable;
            request.Address2 = TimeZone;
            request.Fax = NetWorth;
            request.Notes = NotesInLongForm.ToString();
            request.BirthDate = DateTime.Now;
            request.Zip = "1";

            CreateCustomerLeadResponse response = ExigoApiContext.CreateWebServiceContext().CreateCustomerLead(request); // api.WebService.CreateCustomerLead(request);

        }
        catch
        {
            isValid = false;
        }

        return request;
    }
    private CreateCustomerLeadRequest Request_CreateCustomerLeadForCorporate()
    {
        var NotesInLongForm = new StringBuilder();
        NotesInLongForm.AppendFormat(@"
            <table>
                <tr>
                    <td>
                        Comments: {0}
                    </td>
                </tr>
                <tr>
                    <td>
                        This is GPR test number: {1}
                    </td>
                </tr>
            </table>
        "
        , Comments
        , LastName
        );

        CreateCustomerLeadRequest request = new CreateCustomerLeadRequest();
        try
        {
            request.CustomerID = 24100; // Any time a GPR Lead is created there will also be one created for the corporate account 24100.
            request.FirstName = FirstName;
            request.LastName = LastName;
            request.Phone = Phone1;
            request.Phone2 = Phone2;
            request.Email = Email;
            //request.Address1 = LikelyAvailable;
            request.Address2 = TimeZone;
            request.Notes = NotesInLongForm.ToString();
            request.BirthDate = DateTime.Now;
            request.Zip = "1";

            isValid = true;

        }
        catch
        {
            isValid = false;
        }

        return request;
    }
    private CreateOrderRequest Request_PlaceGPRRorder()
    {
        var NotesInLongForm = new StringBuilder();
        NotesInLongForm.AppendFormat(@"
            <table>
                <tr>
                    <td>
                        Comments: {0}
                    </td>
                </tr>
            </table>
        "
        , Comments
        );

        CreateOrderRequest request = new CreateOrderRequest();
        try
        {
            // Variables that are required by the API
            request.CustomerID = CurrentUser_ID;
            request.CurrencyCode = "usd";
            request.OrderStatus = OrderStatusType.Shipped;
            request.OrderType = OrderType.APIOrder;
            request.PriceType = GlobalSettings.Shopping.DefaultPriceTypeID;
            request.WarehouseID = 4;
            request.ShipMethodID = 15;
            request.City = "Orem";
            request.State = "UT";
            request.Country = "US";

            // Add Personal Information
            request.FirstName = FirstName;
            request.LastName = LastName;
            request.Phone = Phone1;
            request.Email = Email;
            //request.Address1 = LikelyAvailable;
            request.Address2 = TimeZone;

            // Using the Other fields for misc data
            request.Other16 = AppointmentDate;
            request.Other17 = NetWorth;

            // Add Notes and the Date of the Order
            request.Notes = NotesInLongForm.ToString();
            request.OrderDate = DateTime.Now;

            //Add Details
            List<OrderDetailRequest> details = new List<OrderDetailRequest>();

            OrderDetailRequest detail1 = new OrderDetailRequest();
            detail1.ItemCode = "GPRR";
            detail1.Quantity = 1;
            details.Add(detail1);

            //Now attach the list to the request
            request.Details = details.ToArray();

            isValid = true;
        }
        catch
        {
            isValid = false;
        }

        //Send Request to Server and Get Response
        var context = ExigoApiContext.CreateWebServiceContext();
        CreateOrderResponse res = context.CreateOrder(request);

        Response.Write(res.OrderID);

        return request;
    }
    #endregion

    #region Do Stuff
    public void SaveDataToExigo()
    {
        // Any time a GPR request Lead is created there will also be one created for the corporate account 24100 first.
        Request_CreateCustomerLeadForCorporate();
        if (isValid)
        {
            Request_PlaceGPRRorder();
            Request_CreateCustomerLead();
            try
            {
                SendEmail();
                if (emailSent)
                {
                    Response.Redirect("GamePlanSubmissionThankYou.aspx");
                }
            }
            catch (Exception ex)
            {
                Response.Write("There was an error when attempting to send the email" + "<br /><br />" + ex);
            }
        }
        else
        {
            HtmlTextWriter writer = new HtmlTextWriter(Response.Output);
            writer.Write("We're sorrry, your request could not be completed.  If this problem persists, please contact customer support ");
        }
    }
    #endregion Do Stuff

    #region Helpers
    public void SetCurrentUser()
    {
        try
        {
            var context = ExigoApiContext.CreateODataContext();
            var customer = (from c in context.Customers
                            where c.CustomerID == Convert.ToInt32(Request.QueryString["ID"])
                            select new
                            {
                                c.CustomerID,
                                c.Email,
                                c.Phone,
                                c.FirstName,
                                c.LastName
                            }).SingleOrDefault();

            CurrentUser_ID = customer.CustomerID;
            CurrentUser_FirstName = customer.FirstName;
            CurrentUser_LastName = customer.LastName;
            CurrentUser_Email = customer.Email;
            CurrentUser_Phone = customer.Phone;
        }
        catch
        {
            Response.Write("An invalid customer ID has been supplied. <br />");
        }
    }
    private void PopulateAvailabilityFields()
    {

        daysAvailable.Items.Clear();



        daysAvailable.Items.Add(new ListItem("Any Day of the week"));
        daysAvailable.Items.Add(new ListItem("Monday"));
        daysAvailable.Items.Add(new ListItem("Tuesday"));
        //daysAvailable.Items.Add(new ListItem("3pm - 5pm"));
        //daysAvailable.Items.Add(new ListItem("5pm - 7pm"));

        drdlTimeZone.Items.Clear();
        drdlTimeZone.Items.Add(new ListItem("Select Your Time Zone"));
        drdlTimeZone.Items.Add(new ListItem("Pacific Time"));
        drdlTimeZone.Items.Add(new ListItem("Mountain Time"));
        drdlTimeZone.Items.Add(new ListItem("Central Time"));
        drdlTimeZone.Items.Add(new ListItem("Eastern Time"));
        drdlTimeZone.Items.Add(new ListItem("Hawaii Time"));

    }
    private void PopulateNetWorthFields()
    {
        netWorth.Items.Clear();

        netWorth.Items.Add(new ListItem("Estimated Net Worth (optional)"));
        netWorth.Items.Add(new ListItem("$0 - $49,999"));
        netWorth.Items.Add(new ListItem("$50,000 - $99,999"));
        netWorth.Items.Add(new ListItem("$100,000 - $249,999"));
        netWorth.Items.Add(new ListItem("$250,000 - $499,999"));
        netWorth.Items.Add(new ListItem("$500,000 - $1,000,000"));
        netWorth.Items.Add(new ListItem("$1,000,000+"));
        netWorth.Items.Add(new ListItem("Don't Know"));
    }

    #endregion

    #region Properties
    public int CurrentUser_ID { get; set; }
    public string CurrentUser_FirstName { get; set; }
    public string CurrentUser_LastName { get; set; }
    public string CurrentUser_Email { get; set; }
    public string CurrentUser_Phone { get; set; }

    private string FirstName
    {
        get { return txtFirstName.Text; }
        set { txtFirstName.Text = value; }
    }
    private string LastName
    {
        get { return txtLastName.Text; }
        set { txtLastName.Text = value; }
    }
    private string Phone1
    {
        get { return txtPhone1.Text; }
        set { txtPhone1.Text = value; }
    }
    private string Phone2
    {
        get { return txtPhone2.Text; }
        set { txtPhone2.Text = value; }
    }
    private string Email
    {
        get { return txtEmail.Text; }
        set { txtEmail.Text = value; }
    }

    //public string LikelyAvailable
    //{
    //    get { return daysAvailable.SelectedValue; }
    //    set { daysAvailable.SelectedValue = value; }
    //}
    public string TimeZone
    {
        get { return drdlTimeZone.SelectedValue; }
        set { drdlTimeZone.SelectedValue = value; }
    }
    public string AppointmentDate
    {
        get { return Date1.Text; }
        set { Date1.Text = value; }
    }

    public string NetWorth
    {
        get { return netWorth.SelectedValue; }
        set { netWorth.SelectedValue = value; }
    }

    private string Comments
    {
        get { return txtComments.InnerText; }
        set { txtComments.InnerText = value; }
    }

    public bool isValid { get; set; }
    public bool emailSent { get; set; }
    #endregion

    #region Email Sender
    private bool SendEmail()
    {
        emailSent = false;

        //First Create the Address Info
        MailAddress from = new MailAddress("support@strongbrookdirect.com", "No Reply");
        MailAddress to = new MailAddress("GamePlanRequest@strongbrook.com", "GPR Group");
        MailAddress cc = new MailAddress(Email);
        MailAddress bcc = new MailAddress(CurrentUser_Email, CurrentUser_FirstName + " " + CurrentUser_LastName);

        //Construct the email - just simple text email
        MailMessage message = new MailMessage(from, to);
        message.CC.Add(cc);
        message.Bcc.Add("Chris.Ferguson@strongbrook.com");
        message.Bcc.Add("Tyler.Bennett@strongbrook.com");
        message.Bcc.Add(bcc);
        message.Subject = string.Format("New Game Plan requested for {0} {1}", FirstName, LastName);
        #region Email Message Body
        message.Body = string.Format(@"
A request for a Game Plan Report has been submitted for the following individual:

Name: {0} {1}
Main Phone: {2}
Secondary Phone: {3}
Email Address: {4}
Likely Available: {5}
TimeZone: {6}
Date Requested if any: {7}
Estimated Net Worth: {8}

Comments: 
{9}


Enroller Information:
{10}
{11}
{12}
        ", FirstName // 0
         , LastName // 1
         , Phone1 // 2
         , Phone2 // 3
         , Email // 4
         , "LikelyAvailable" // 5
         , TimeZone // 6
         , AppointmentDate // 7
         , NetWorth // 8
         , Comments  // 9
         , CurrentUser_FirstName + " " + CurrentUser_LastName // 10
         , CurrentUser_Email // 11
         , CurrentUser_Phone  // 12
         );
        #endregion Email Message Body

        //Email SMTP Settings
        Int16 port = 25;
        SmtpClient client = new SmtpClient("smtp.gmail.com", port); // ("smtpout.secureserver.net", port);

        //// Use these properties for a un-secure SMTP connection.
        //client.UseDefaultCredentials = false;

        // Use these properties for a secure SMTP connection.
        client.UseDefaultCredentials = true;
        client.EnableSsl = true;

        client.Credentials = new System.Net.NetworkCredential("aaron@bakerwebdev.com", "sting123"); // ("support@strongbrookdirect.com", "Reic2012");

        try
        {
            client.Send(message);
            emailSent = true;
        }
        catch (Exception ex)
        {
            emailSent = false;
            HtmlTextWriter writer = new HtmlTextWriter(Response.Output);
            Response.Write(ex);
            writer.Write("We're sorry, your request could not be completed.  If this problem persists, please contact customer support " + ex.ToString());
        }

        return emailSent;
    }
    #endregion

    #region Button
    protected void Click_Submit()
    {
        try
        {
            SaveDataToExigo();
        }
        catch (Exception ex)
        {
            HtmlTextWriter writer = new HtmlTextWriter(Response.Output);
            writer.Write("We're sorry, your request could not be completed.  If this problem persists, please contact customer support " + ex.ToString());
        }
    }
    #endregion

    #region Error Handling
    public string Message
    {
        get
        {
            return _message;
        }
        set
        {
            _message += value;
            //ShowMessage.Value = "True";
        }
    }
    private string _message;

    private void ClearMessage()
    {
        Message = string.Empty;
        //ShowMessage.Value = "";
    }

    #endregion



}