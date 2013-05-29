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
        CalendarExtender1.StartDate = DateTime.Today.AddDays(1);
        CalendarExtender1.EndDate = DateTime.Today.AddDays(7);

        if (!IsPostBack)
        {
            string timeZoneSelection = Request.Form["timeZone"];
            if (timeZoneSelection == null)
            {
                PopulateAvailabilityFields();
                PopulateNetWorthFields();
            }

            string timeFrameSelection = Request.Form["timeFrame"];
            if (timeFrameSelection != null)
            {
                PopulateAppointmentTimeFields();
            }

            CreateACookie();

        }
        if (IsPostBack)
        {
            SetCurrentUserUsingOData();
            SetCurrentUserUsingWebService();
            Click_Submit();
        }

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
                <tr>
                    <td>
                        Requested date of appointment: {2}
                    </td>
                </tr>
                <tr>
                    <td>
                        Requested time of appointment: {3}
                    </td>
                </tr>
            </table>
        "
        , Comments
        , LastName
        , AppointmentDate
        , "AppointmentTime"
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
            request.Address1 = LikelyAvailable;
            request.Address2 = TimeZone;
            request.Fax = NetWorth;
            request.Notes = NotesInLongForm.ToString();
            request.BirthDate = DateTime.Now;
            request.Zip = "1";

            CreateCustomerLeadResponse response = ExigoApiContext.CreateWebServiceContext().CreateCustomerLead(request);

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
                <tr>
                    <td>
                        Requested date of appointment: {2}
                    </td>
                </tr>
                <tr>
                    <td>
                        Requested time of appointment: {3}
                    </td>
                </tr>
            </table>
        "
        , Comments
        , LastName
        , AppointmentDate
        , AppointmentTimeSelectedByTheProspect
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
            request.Address1 = LikelyAvailable;
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
            request.City = Phone1;
            request.State = "UT";
            request.Country = "US";

            // Add Personal Information
            request.FirstName = FirstName;
            request.LastName = LastName;
            request.Phone = Phone1;
            request.Email = Email;
            request.Address1 = LikelyAvailable;
            request.Address2 = TimeZone;

            // Using the Other fields for misc data
            request.Other16 = AppointmentDate;
            request.Other17 = NetWorth;
            request.Other18 = AppointmentTimeInCorporateTimeZone;
            request.Other19 = Email;

            // Add Notes and the Date of the Order
            request.Notes = Comments; // NotesInLongForm.ToString();
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

        orderID = res.OrderID;

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
                SendEmailToProspect();
                SendEmailToCorporate();
                SendEmailToProspectsUpline();
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
    public void SetCurrentUserUsingOData()
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
    public void SetCurrentUserUsingWebService()
    {
        try
        {
            //var tasks = new List<Task>();
            var customerID = Convert.ToInt32(Request.QueryString["ID"]);

            var data = ExigoApiContext.CreateWebServiceContext().GetCustomerSite(new GetCustomerSiteRequest
            {
                CustomerID = customerID
            });

            this.CurrentUser_WebAlias = data.WebAlias;
        }
        catch
        {
            Response.Write("An invalid customer ID has been supplied. <br />");
        }
    }
    private void PopulateAvailabilityFields()
    {

        firstAvailableTime.Items.Clear();
        firstAvailableTime.Items.Add(new ListItem(""));
        firstAvailableTime.Items.Add(new ListItem("Morning"));
        firstAvailableTime.Items.Add(new ListItem("Afternoon"));
        firstAvailableTime.Items.Add(new ListItem("Evening"));

        ddlTimeZone.Items.Clear();
        ddlTimeZone.Items.Add(new ListItem(""));
        ddlTimeZone.Items.Add(new ListItem("Hawaii Time"));
        ddlTimeZone.Items.Add(new ListItem("Pacific Time"));
        ddlTimeZone.Items.Add(new ListItem("Mountain Time"));
        ddlTimeZone.Items.Add(new ListItem("Central Time"));
        ddlTimeZone.Items.Add(new ListItem("Eastern Time"));

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
    private void PopulateAppointmentTimeFields()
    {
        #region Populate Dropdown
        Response.Expires = -1;
        Response.ContentType = "text/plain";

        #region Properties
        string timeZone = Request.Form["timeZone"];
        string timeFrameSelection = Request.Form["timeFrame"];

        string oMin = "</option>" + " " + "<option>";

        string closedSunday = "Closed Sunday";

        #region Hawaii Hours
        string from_6AM_to_1PM = "<option>" + "" + oMin + "from 6:00AM to 6:30AM" + oMin + "from 6:30AM to 7:00AM" + oMin + "from 7:00AM to 7:30AM" + oMin + "from 7:30AM to 8:00AM" + oMin + "from 8:00AM to 8:30AM" + oMin + "from 8:30AM to 9:00AM" + oMin + "from 9:00AM to 9:30AM" + oMin + "from 9:30AM to 10:00AM" + oMin + "from 10:00AM to 10:30AM" + oMin + "from 10:30AM to 11:00AM" + oMin + "from 11:00AM to 11:30AM" + oMin + "from 11:30AM to 12:00PM" + oMin + "from 12:00PM to 12:30PM" + oMin + "from 12:30PM to 1:00PM" + "</option>";
        string from_6AM_to_8AM = "<option>" + "" + oMin + "from 6:00AM to 6:30AM" + oMin + "from 6:30AM to 7:00AM" + oMin + "from 7:00AM to 7:30AM" + oMin + "from 7:30AM to 8:00AM" + "</option>";
        string from_noon_to_4PM = "<option>" + "from 12:00PM to 12:30PM" + oMin + "from 12:30PM to 1:00PM" + oMin + "from 1:00PM to 1:30PM" + oMin + "from 1:30PM to 2:00PM" + oMin + "from 2:00PM to 2:30PM" + oMin + "from 2:30PM to 3:00PM" + oMin + "from 3:00PM to 3:30PM" + oMin + "from 3:30PM to 4:00PM" + "</option>";
        string from_6AM_to_9AM = "<option>" + "" + oMin + "from 6:00AM to 6:30AM" + oMin + "from 6:30AM to 7:00AM" + oMin + "from 7:00AM to 7:30AM" + oMin + "from 7:30AM to 8:00AM" + oMin + "from 8:00AM to 8:30AM" + oMin + "from 8:30AM to 9:00AM" + "</option>";
        string from_9AM_to_5PM = "<option>" + "" + oMin + "from 9:00AM to 9:30AM" + oMin + "from 9:30AM to 10:00AM" + oMin + "from 10:00AM to 10:30AM" + oMin + "from 10:30AM to 11:00AM" + oMin + "from 11:00AM to 11:30AM" + oMin + "from 11:30AM to 12:00PM" + oMin + "from 12:00PM to 12:30PM" + oMin + "from 12:30PM to 1:00PM" + oMin + "from 1:00PM to 1:30PM" + oMin + "from 1:30PM to 2:00PM" + oMin + "from 2:00PM to 2:30PM" + oMin + "from 2:30PM to 3:00PM" + oMin + "from 3:00PM to 3:30PM" + oMin + "from 3:30PM to 4:00PM" + oMin + "from 4:00PM to 4:30PM" + oMin + "from 4:30PM to 5:00PM" + "</option>";
        #endregion Hawaii Hours

        #region Pacific Hours
        string from_9AM_to_4PM = "<option>" + "" + oMin + "from 9:00AM to 9:30AM" + oMin + "from 9:30AM to 10:00AM" + oMin + "from 10:00AM to 10:30AM" + oMin + "from 10:30AM to 11:00AM" + oMin + "from 11:00AM to 11:30AM" + oMin + "from 11:30AM to 12:00PM" + oMin + "from 12:00PM to 12:30PM" + oMin + "from 12:30PM to 1:00PM" + oMin + "from 1:00PM to 1:30PM" + oMin + "from 1:30PM to 2:00PM" + oMin + "from 2:00PM to 2:30PM" + oMin + "from 2:30PM to 3:00PM" + oMin + "from 3:00PM to 3:30PM" + oMin + "from 3:30PM to 4:00PM" + "</option>";
        string from_9AM_to_11AM = "<option>" + "" + oMin + "from 9:00AM to 9:30AM" + oMin + "from 9:30AM to 10:00AM" + oMin + "from 10:00AM to 10:30AM" + oMin + "from 10:30AM to 11:00AM" + oMin + "</option>";
        string from_3PM_to_7PM = "<option>" + "from 3:00PM to 3:30PM" + oMin + "from 3:30PM to 4:00PM" + oMin + "from 4:00PM to 4:30PM" + oMin + "from 4:30PM to 5:00PM" + oMin + "from 5:00PM to 5:30PM" + oMin + "from 5:30PM to 6:00PM" + oMin + "from 6:00PM to 6:30PM" + oMin + "from 6:30PM to 7:00PM" + "</option>";
        string from_9AM_to_noon = "<option>" + "" + oMin + "from 9:00AM to 9:30AM" + oMin + "from 9:30AM to 10:00AM" + oMin + "from 10:00AM to 10:30AM" + oMin + "from 10:30AM to 11:00AM" + oMin + "from 11:00AM to 11:30AM" + oMin + "from 11:30AM to 12:00PM" + "</option>";
        string from_12PM_to_8PM = "<option>" + "" + oMin + "from 12:00PM to 12:30PM" + oMin + "from 12:30PM to 1:00PM" + oMin + "from 1:00PM to 1:30PM" + oMin + "from 1:30PM to 2:00PM" + oMin + "from 2:00PM to 2:30PM" + oMin + "from 2:30PM to 3:00PM" + oMin + "from 3:00PM to 3:30PM" + oMin + "from 3:30PM to 4:00PM" + oMin + "from 4:00PM to 4:30PM" + oMin + "from 4:30PM to 5:00PM" + oMin + "from 5:00PM to 5:30PM" + oMin + "from 5:30PM to 6:00PM" + oMin + "from 6:00PM to 6:30PM" + oMin + "from 6:30PM to 7:00PM" + oMin + "from 7:00PM to 7:30PM" + oMin + "from 7:30PM to 8:00PM" + "</option>";
        #endregion Pacific Hours

        #region Mountain Hours
        string from_10AM_to_5PM = "<option>" + "" + oMin + "from 10:00AM to 10:30AM" + oMin + "from 10:30AM to 11:00AM" + oMin + "from 11:00AM to 11:30AM" + oMin + "from 11:30AM to 12:00PM" + oMin + "from 12:00PM to 12:30PM" + oMin + "from 12:30PM to 1:00PM" + oMin + "from 1:00PM to 1:30PM" + oMin + "from 1:30PM to 2:00PM" + oMin + "from 2:00PM to 2:30PM" + oMin + "from 2:30PM to 3:00PM" + oMin + "from 3:00PM to 3:30PM" + oMin + "from 3:30PM to 4:00PM" + oMin + "from 4:00PM to 4:30PM" + oMin + "from 4:30PM to 5:00PM" + "</option>";
        string from_10AM_to_noon = "<option>" + "" + oMin + "from 10:00AM to 10:30AM" + oMin + "from 10:30AM to 11:00AM" + oMin + "from 11:00AM to 11:30AM" + oMin + "from 11:30AM to 12:00PM" + "</option>";
        string from_4PM_to_8PM = "<option>" + "from 4:00PM to 4:30PM" + oMin + "from 4:30PM to 5:00PM" + oMin + "from 5:00PM to 5:30PM" + oMin + "from 5:30PM to 6:00PM" + oMin + "from 6:00PM to 6:30PM" + oMin + "from 6:30PM to 7:00PM" + oMin + "from 7:00PM to 7:30PM" + oMin + "from 7:30PM to 8:00PM" + "</option>";
        string from_10AM_to_1PM = "<option>" + "" + oMin + "from 10:00AM to 10:30AM" + oMin + "from 10:30AM to 11:00AM" + oMin + "from 11:00AM to 11:30AM" + oMin + "from 11:30AM to 12:00PM" + oMin + "from 12:00AM to 12:30AM" + oMin + "from 12:30AM to 1:00PM" + "</option>";
        string from_1PM_to_9PM = "<option>" + "" + oMin + "from 1:00PM to 1:30PM" + oMin + "from 1:30PM to 2:00PM" + oMin + "from 2:00PM to 2:30PM" + oMin + "from 2:30PM to 3:00PM" + oMin + "from 3:00PM to 3:30PM" + oMin + "from 3:30PM to 4:00PM" + oMin + "from 4:00PM to 4:30PM" + oMin + "from 4:30PM to 5:00PM" + oMin + "from 5:00PM to 5:30PM" + oMin + "from 5:30PM to 6:00PM" + oMin + "from 6:00PM to 6:30PM" + oMin + "from 6:30PM to 7:00PM" + oMin + "from 7:00PM to 7:30PM" + oMin + "from 7:30PM to 8:00PM" + oMin + "from 8:00PM to 8:30PM" + oMin + "from 8:30PM to 9:00PM" + "</option>";
        #endregion Mountain Hours

        #region Central Hours
        string from_11AM_to_6PM = "<option>" + "" + oMin + "from 11:00AM to 11:30AM" + oMin + "from 11:30AM to 12:00PM" + oMin + "from 12:00PM to 12:30PM" + oMin + "from 12:30PM to 1:00PM" + oMin + "from 1:00PM to 1:30PM" + oMin + "from 1:30PM to 2:00PM" + oMin + "from 2:00PM to 2:30PM" + oMin + "from 2:30PM to 3:00PM" + oMin + "from 3:00PM to 3:30PM" + oMin + "from 3:30PM to 4:00PM" + oMin + "from 4:00PM to 4:30PM" + oMin + "from 4:30PM to 5:00PM" + oMin + "from 5:00PM to 5:30PM" + oMin + "from 5:30PM to 6:00PM" + "</option>";
        string from_11AM_to_1PM = "<option>" + "" + oMin + "from 11:00AM to 11:30AM" + oMin + "from 11:30AM to 12:00PM" + oMin + "from 12:00AM to 12:30AM" + oMin + "from 12:30AM to 1:00PM" + "</option>";
        string from_5PM_to_9PM = "<option>" + "from 5:00PM to 5:30PM" + oMin + "from 5:30PM to 6:00PM" + oMin + "from 6:00PM to 6:30PM" + oMin + "from 6:30PM to 7:00PM" + oMin + "from 7:00PM to 7:30PM" + oMin + "from 7:30PM to 8:00PM" + oMin + "from 8:30PM to 8:00PM" + oMin + "from 8:00PM to 8:30PM" + oMin + "from 8:30PM to 9:00PM" + "</option>";
        string from_11AM_to_2PM = "<option>" + "" + oMin + "from 11:00AM to 11:30AM" + oMin + "from 11:30AM to 12:00PM" + oMin + "from 12:00AM to 12:30AM" + oMin + "from 12:30AM to 1:00PM" + oMin + "from 1:00PM to 1:30PM" + oMin + "from 1:30PM to 2:00PM" + "</option>";
        string from_2PM_to_10PM = "<option>" + "" + oMin + "from 2:00PM to 2:30PM" + oMin + "from 2:30PM to 3:00PM" + oMin + "from 3:00PM to 3:30PM" + oMin + "from 3:30PM to 4:00PM" + oMin + "from 4:00PM to 4:30PM" + oMin + "from 4:30PM to 5:00PM" + oMin + "from 5:00PM to 5:30PM" + oMin + "from 5:30PM to 6:00PM" + oMin + "from 6:00PM to 6:30PM" + oMin + "from 6:30PM to 7:00PM" + oMin + "from 7:00PM to 7:30PM" + oMin + "from 7:30PM to 8:00PM" + oMin + "from 8:00PM to 8:30PM" + oMin + "from 8:30PM to 9:00PM" + oMin + "from 9:00PM to 9:30PM" + oMin + "from 9:30PM to 10:00PM" + "</option>";
        #endregion Central Hours

        #region Eastern Hours
        string from_12PM_to_7PM = "<option>" + "" + oMin + "from 12:00PM to 12:30PM" + oMin + "from 12:30PM to 1:00PM" + oMin + "from 1:00PM to 1:30PM" + oMin + "from 1:30PM to 2:00PM" + oMin + "from 2:00PM to 2:30PM" + oMin + "from 2:30PM to 3:00PM" + oMin + "from 3:00PM to 3:30PM" + oMin + "from 3:30PM to 4:00PM" + oMin + "from 4:00PM to 4:30PM" + oMin + "from 4:30PM to 5:00PM" + oMin + "from 5:00PM to 5:30PM" + oMin + "from 5:30PM to 6:00PM" + oMin + "from 6:00PM to 6:30PM" + oMin + "from 6:30PM to 7:00PM" + "</option>";
        string from_12PM_to_2PM = "<option>" + "" + oMin + "from 12:00PM to 12:30PM" + oMin + "from 12:30PM to 1:00PM" + oMin + "from 1:00PM to 1:30PM" + oMin + "from 1:30PM to 2:00PM" + "</option>";
        string from_6PM_to_10PM = "<option>" + "from 6:00PM to 6:30PM" + oMin + "from 6:30PM to 7:00PM" + oMin + "from 7:00PM to 7:30PM" + oMin + "from 7:30PM to 8:00PM" + oMin + "from 8:00PM to 8:30PM" + oMin + "from 8:30PM to 9:00PM" + oMin + "from 9:00PM to 9:30PM" + oMin + "from 9:30PM to 10:00PM";
        string from_12PM_to_3PM = "<option>" + "" + oMin + "from 12:00PM to 12:30PM" + oMin + "from 12:30PM to 1:00PM" + oMin + "from 1:00PM to 1:30PM" + oMin + "from 1:30PM to 2:00PM" + oMin + "from 2:00PM to 2:30PM" + oMin + "from 2:30PM to 3:00PM" + "</option>";
        string from_3PM_to_11PM = "<option>" + "" + oMin + "from 3:00PM to 3:30PM" + oMin + "from 3:30PM to 4:00PM" + oMin + "from 4:00PM to 4:30PM" + oMin + "from 4:30PM to 5:00PM" + oMin + "from 5:00PM to 5:30PM" + oMin + "from 5:30PM to 6:00PM" + oMin + "from 6:00PM to 6:30PM" + oMin + "from 6:30PM to 7:00PM" + oMin + "from 7:00PM to 7:30PM" + oMin + "from 7:30PM to 8:00PM" + oMin + "from 8:00PM to 8:30PM" + oMin + "from 8:30PM to 9:00PM" + oMin + "from 9:00PM to 9:30PM" + oMin + "from 9:30PM to 10:00PM" + oMin + "from 10:00PM to 10:30PM" + oMin + "from 10:30PM to 11:00PM" + "</option>";
        #endregion Eastern Hours

        #endregion Properties

        #region Method for switching Time Zones
        switch (timeZone)
        {
            #region Hawaii Times Original
            //case "Hawaii Time":
            //    switch (timeFrameSelection)
            //    {
            //        case "Sunday":
            //            Response.Write("<option>" + closedSunday + "</option>");
            //            break;
            //        case "Monday":
            //            Response.Write("<option>" + select + oMin + f7Ato730A + oMin + f730Ato8A + oMin + f8Ato830A + oMin + f830Ato9A + oMin + f9Ato930A + oMin + f930Ato10AM + oMin + f10Ato1030A + oMin + f1030Ato11A + oMin + f11Ato1130A + oMin + f1130Ato12P + oMin + f12Pto1230P + oMin + f1230Pto1P + oMin + f1Pto130P + oMin + f130Pto2P + oMin + f2Pto230P + oMin + f230Pto3P + oMin + f3Pto330P + oMin + f330Pto4P + oMin + f4Pto430P + oMin + f430Pto5P + oMin + f5Pto530P + oMin + f530Pto6P + oMin + f6Pto630P + oMin + f630Pto7P + "</option>");
            //            break;
            //        case "Tuesday":
            //            Response.Write("<option>" + select + oMin + f7Ato730A + oMin + f730Ato8A + oMin + f8Ato830A + oMin + f830Ato9A + oMin + f9Ato930A + oMin + f930Ato10AM + oMin + f10Ato1030A + oMin + f1030Ato11A + oMin + f11Ato1130A + oMin + f1130Ato12P + oMin + f12Pto1230P + oMin + f1230Pto1P + oMin + f1Pto130P + oMin + f130Pto2P + oMin + f2Pto230P + oMin + f230Pto3P + oMin + f3Pto330P + oMin + f330Pto4P + oMin + f4Pto430P + oMin + f430Pto5P + oMin + f5Pto530P + oMin + f530Pto6P + oMin + f6Pto630P + oMin + f630Pto7P + "</option>");
            //            break;
            //        case "Wednesday":
            //            Response.Write("<option>" + select + oMin + f7Ato730A + oMin + f730Ato8A + oMin + f8Ato830A + oMin + f830Ato9A + oMin + f9Ato930A + oMin + f930Ato10AM + oMin + f10Ato1030A + oMin + f1030Ato11A + oMin + f11Ato1130A + oMin + f1130Ato12P + oMin + f12Pto1230P + oMin + f1230Pto1P + oMin + f1Pto130P + oMin + f130Pto2P + oMin + f2Pto230P + oMin + f230Pto3P + oMin + f3Pto330P + oMin + f330Pto4P + oMin + f4Pto430P + oMin + f430Pto5P + oMin + f5Pto530P + oMin + f530Pto6P + oMin + f6Pto630P + oMin + f630Pto7P + "</option>");
            //            break;
            //        case "Thursday":
            //            Response.Write("<option>" + select + oMin + f7Ato730A + oMin + f730Ato8A + oMin + f8Ato830A + oMin + f830Ato9A + oMin + f9Ato930A + oMin + f930Ato10AM + oMin + f10Ato1030A + oMin + f1030Ato11A + oMin + f11Ato1130A + oMin + f1130Ato12P + oMin + f12Pto1230P + oMin + f1230Pto1P + oMin + f1Pto130P + oMin + f130Pto2P + oMin + f2Pto230P + oMin + f230Pto3P + oMin + f3Pto330P + oMin + f330Pto4P + oMin + f4Pto430P + oMin + f430Pto5P + oMin + f5Pto530P + oMin + f530Pto6P + oMin + f6Pto630P + oMin + f630Pto7P + "</option>");
            //            break;
            //        case "Friday":
            //            Response.Write("<option>" + select + oMin + f7Ato730A + oMin + f730Ato8A + oMin + f8Ato830A + oMin + f830Ato9A + oMin + f9Ato930A + oMin + f930Ato10AM + oMin + f10Ato1030A + oMin + f1030Ato11A + oMin + f11Ato1130A + oMin + f1130Ato12P + oMin + f12Pto1230P + oMin + f1230Pto1P + oMin + f1Pto130P + oMin + f130Pto2P + oMin + f2Pto230P + oMin + f230Pto3P + oMin + f3Pto330P + oMin + f330Pto4P + oMin + f4Pto430P + oMin + f430Pto5P + oMin + f5Pto530P + oMin + f530Pto6P + oMin + f6Pto630P + oMin + f630Pto7P + "</option>");
            //            break;
            //        case "Saturday":
            //            Response.Write("<option>" + closedSaturday + "</option>");
            //            break;
            //    }
            //    break;
            #endregion Hawaii Times Original

            #region Hawaii Times
            case "Hawaii Time":
                switch (timeFrameSelection)
                {
                    case "Sunday":
                        Response.Write("<option>" + closedSunday + "</option>");
                        break;
                    case "Monday":
                        Response.Write(from_6AM_to_1PM);
                        break;
                    case "Tuesday":
                        Response.Write(from_9AM_to_5PM);
                        break;
                    case "Wednesday":
                        Response.Write(from_9AM_to_5PM);
                        break;
                    case "Thursday":
                        Response.Write(from_9AM_to_5PM);
                        break;
                    case "Friday":
                        Response.Write(from_6AM_to_1PM);
                        break;
                    case "Saturday":
                        Response.Write(from_6AM_to_9AM);
                        break;
                }
                break;
            #endregion Hawaii Times

            #region Pacific Times
            case "Pacific Time":
                switch (timeFrameSelection)
                {
                    case "Sunday":
                        Response.Write("<option>" + closedSunday + "</option>");
                        break;
                    case "Monday":
                        Response.Write(from_9AM_to_4PM);
                        break;
                    case "Tuesday":
                        Response.Write(from_12PM_to_8PM);
                        break;
                    case "Wednesday":
                        Response.Write(from_12PM_to_8PM);
                        break;
                    case "Thursday":
                        Response.Write(from_12PM_to_8PM);
                        break;
                    case "Friday":
                        Response.Write(from_9AM_to_4PM);
                        break;
                    case "Saturday":
                        Response.Write(from_9AM_to_noon);
                        break;
                }
                break;
            #endregion Pacific Times

            #region Mountain Times
            case "Mountain Time":
                switch (timeFrameSelection)
                {
                    case "Sunday":
                        Response.Write("<option>" + closedSunday + "</option>");
                        break;
                    case "Monday":
                        Response.Write(from_10AM_to_5PM);
                        break;
                    case "Tuesday":
                        Response.Write(from_1PM_to_9PM);
                        break;
                    case "Wednesday":
                        Response.Write(from_1PM_to_9PM);
                        break;
                    case "Thursday":
                        Response.Write(from_1PM_to_9PM);
                        break;
                    case "Friday":
                        Response.Write(from_10AM_to_5PM);
                        break;
                    case "Saturday":
                        Response.Write(from_10AM_to_1PM);
                        break;
                }
                break;
            #endregion Pacific Times

            #region Central Times
            case "Central Time":
                switch (timeFrameSelection)
                {
                    case "Sunday":
                        Response.Write("<option>" + closedSunday + "</option>");
                        break;
                    case "Monday":
                        Response.Write(from_11AM_to_6PM);
                        break;
                    case "Tuesday":
                        Response.Write(from_2PM_to_10PM);
                        break;
                    case "Wednesday":
                        Response.Write(from_2PM_to_10PM);
                        break;
                    case "Thursday":
                        Response.Write(from_2PM_to_10PM);
                        break;
                    case "Friday":
                        Response.Write(from_11AM_to_6PM);
                        break;
                    case "Saturday":
                        Response.Write(from_11AM_to_2PM);
                        break;
                }
                break;
            #endregion Pacific Times

            #region Eastern Times
            case "Eastern Time":
                switch (timeFrameSelection)
                {
                    case "Sunday":
                        Response.Write("<option>" + closedSunday + "</option>");
                        break;
                    case "Monday":
                        Response.Write(from_12PM_to_7PM);
                        break;
                    case "Tuesday":
                        Response.Write(from_3PM_to_11PM);
                        break;
                    case "Wednesday":
                        Response.Write(from_3PM_to_11PM);
                        break;
                    case "Thursday":
                        Response.Write(from_3PM_to_11PM);
                        break;
                    case "Friday":
                        Response.Write(from_12PM_to_7PM);
                        break;
                    case "Saturday":
                        Response.Write(from_12PM_to_3PM);
                        break;
                }
                break;
            #endregion Eastern Times
        }
        #endregion Method for switching Time Zones

        Response.End();

        #endregion
    }

    public void CreateACookie()
    {
        try
        {
            HttpCookie userCookie = new HttpCookie("userCookie");
            userCookie.Expires = DateTime.Now.AddDays(1);

            string timeFrame = Request.Form["timeFrameSelected"];
            if (timeFrame != null)
            {
                userCookie.Values.Add("AppointmentTime", timeFrame);

                Response.Cookies.Add(userCookie);
            }
        }
        catch
        {
            // ErrorString = "Your request could not be completed.  If you continue to receive this error, please contact support";
        }
    }
    public string theCookie
    {
        get
        {
            string appointmentTimeFromCookie = Request.Cookies["userCookie"].Values["AppointmentTime"];

            return appointmentTimeFromCookie;
        }
    }

    #endregion Helpers

    #region Properties
    public int orderID { get; set; }
    public int CurrentUser_ID { get; set; }
    public string CurrentUser_FirstName { get; set; }
    public string CurrentUser_LastName { get; set; }
    public string CurrentUser_Email { get; set; }
    public string CurrentUser_Phone { get; set; }
    public string CurrentUser_WebAlias { get; set; }

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

    private string NetWorth
    {
        get { return netWorth.SelectedValue; }
        set { netWorth.SelectedValue = value; }
    }

    private string TimeZone
    {
        get { return ddlTimeZone.SelectedValue; }
        set { ddlTimeZone.SelectedValue = value; }
    }
    private string AppointmentDate
    {
        get { return Date1.Text; }
        set { Date1.Text = value; }
    }
    public string cookee;
    public string AppointmentTimeSelectedByTheProspect
    {
        get
        {
            string cookee = "";
            if (RadioButtonSchedule.Checked)
            {
                cookee = theCookie;
            }
            else
            {
                cookee = "";
            }
            return cookee;
        }
    }
    public string AppointmentTimeInCorporateTimeZone
    {
        get
        {
            #region Properties
            string timeZone = Request.Form["timeZone"];
            string timeFrameSelection = Request.Form["timeFrame"];
            #endregion Properties

            string theTimeAdjustedForTheCallCenter = "";
            if (RadioButtonSchedule.Checked)
            {
                #region Method for switching Time Zones
                switch (TimeZone)
                {
                    #region Hawaii Times
                    case "Hawaii Time":
                        switch (theCookie)
                        {
                            case "Closed Sundays":
                                theTimeAdjustedForTheCallCenter = "Closed Sundays";
                                break;
                            case "from 6:00AM to 6:30AM":
                                theTimeAdjustedForTheCallCenter = "from 10:00AM to 10:30AM";
                                break;
                            case "from 6:30AM to 7:00AM":
                                theTimeAdjustedForTheCallCenter = "from 10:30AM to 11:00AM";
                                break;
                            case "from 7:00AM to 7:30AM":
                                theTimeAdjustedForTheCallCenter = "from 11:00AM to 11:30AM";
                                break;
                            case "from 7:30AM to 8:00AM":
                                theTimeAdjustedForTheCallCenter = "from 11:30AM to 12:00PM";
                                break;
                            case "from 8:00AM to 8:30AM":
                                theTimeAdjustedForTheCallCenter = "from 12:00PM to 12:30PM";
                                break;
                            case "from 8:30AM to 9:00AM":
                                theTimeAdjustedForTheCallCenter = "from 12:30PM to 1:00PM";
                                break;
                            case "from 9:00AM to 9:30AM":
                                theTimeAdjustedForTheCallCenter = "from 1:00PM to 1:30PM";
                                break;
                            case "from 9:30AM to 10:00AM":
                                theTimeAdjustedForTheCallCenter = "from 1:30PM to 2:00PM";
                                break;
                            case "from 10:00AM to 10:30AM":
                                theTimeAdjustedForTheCallCenter = "from 2:00PM to 2:30PM";
                                break;
                            case "from 10:30AM to 11:00AM":
                                theTimeAdjustedForTheCallCenter = "from 2:30PM to 3:00PM";
                                break;
                            case "from 11:00AM to 11:30AM":
                                theTimeAdjustedForTheCallCenter = "from 3:00PM to 3:30PM";
                                break;
                            case "from 11:30AM to 12:00PM":
                                theTimeAdjustedForTheCallCenter = "from 3:30PM to 4:00PM";
                                break;
                            case "from 12:00PM to 12:30PM":
                                theTimeAdjustedForTheCallCenter = "from 4:00PM to 4:30PM";
                                break;
                            case "from 12:30PM to 1:00PM":
                                theTimeAdjustedForTheCallCenter = "from 4:30PM to 5:00PM";
                                break;
                        }
                        break;
                    #endregion Hawaii Times

                    #region Pacific Times
                    case "Pacific Time":
                        switch (theCookie)
                        {
                            case "Closed Sundays":
                                theTimeAdjustedForTheCallCenter = "Closed Sundays";
                                break;
                            case "from 6:00AM to 6:30AM":
                                theTimeAdjustedForTheCallCenter = "from 7:00AM to 7:30AM";
                                break;
                            case "from 6:30AM to 7:00AM":
                                theTimeAdjustedForTheCallCenter = "from 7:30AM to 8:00AM";
                                break;
                            case "from 7:00AM to 7:30AM":
                                theTimeAdjustedForTheCallCenter = "from 8:00AM to 8:30AM";
                                break;
                            case "from 7:30AM to 8:00AM":
                                theTimeAdjustedForTheCallCenter = "from 8:30AM to 9:00AM";
                                break;
                            case "from 8:00AM to 8:30AM":
                                theTimeAdjustedForTheCallCenter = "from 9:00AM to 9:30AM";
                                break;
                            case "from 8:30AM to 9:00AM":
                                theTimeAdjustedForTheCallCenter = "from 9:30AM to 10:00AM";
                                break;
                            case "from 9:00AM to 9:30AM":
                                theTimeAdjustedForTheCallCenter = "from 10:00AM to 10:30AM";
                                break;
                            case "from 9:30AM to 10:00AM":
                                theTimeAdjustedForTheCallCenter = "from 10:30AM to 11:00AM";
                                break;
                            case "from 10:00AM to 10:30AM":
                                theTimeAdjustedForTheCallCenter = "from 11:00AM to 11:30AM";
                                break;
                            case "from 10:30AM to 11:00AM":
                                theTimeAdjustedForTheCallCenter = "from 11:30AM to 12:00PM";
                                break;
                            case "from 11:00AM to 11:30AM":
                                theTimeAdjustedForTheCallCenter = "from 12:00PM to 12:30PM";
                                break;
                            case "from 11:30AM to 12:00PM":
                                theTimeAdjustedForTheCallCenter = "from 12:30PM to 1:00PM";
                                break;
                            case "from 12:00PM to 12:30PM":
                                theTimeAdjustedForTheCallCenter = "from 1:00PM to 1:30PM";
                                break;
                            case "from 12:30PM to 1:00PM":
                                theTimeAdjustedForTheCallCenter = "from 1:30PM to 2:00PM";
                                break;
                            case "from 1:00PM to 1:30PM":
                                theTimeAdjustedForTheCallCenter = "from 2:00PM to 2:30PM";
                                break;
                            case "from 1:30PM to 2:00PM":
                                theTimeAdjustedForTheCallCenter = "from 2:30PM to 3:00PM";
                                break;
                            case "from 2:00PM to 2:30PM":
                                theTimeAdjustedForTheCallCenter = "from 3:00PM to 3:30PM";
                                break;
                            case "from 2:30PM to 3:00PM":
                                theTimeAdjustedForTheCallCenter = "from 3:30PM to 4:00PM";
                                break;
                            case "from 3:00PM to 3:30PM":
                                theTimeAdjustedForTheCallCenter = "from 4:00PM to 4:30PM";
                                break;
                            case "from 3:30PM to 4:00PM":
                                theTimeAdjustedForTheCallCenter = "from 4:30PM to 5:00PM";
                                break;
                            case "from 4:00PM to 4:30PM":
                                theTimeAdjustedForTheCallCenter = "from 5:00PM to 5:30PM";
                                break;
                            case "from 4:30PM to 5:00PM":
                                theTimeAdjustedForTheCallCenter = "from 5:30PM to 6:00PM";
                                break;
                            case "from 5:00PM to 5:30PM":
                                theTimeAdjustedForTheCallCenter = "from 6:00PM to 6:30PM";
                                break;
                            case "from 5:30PM to 6:00PM":
                                theTimeAdjustedForTheCallCenter = "from 6:30PM to 7:00PM";
                                break;
                            case "from 6:00PM to 6:30PM":
                                theTimeAdjustedForTheCallCenter = "from 7:00PM to 7:30PM";
                                break;
                            case "from 6:30PM to 7:00PM":
                                theTimeAdjustedForTheCallCenter = "from 7:30PM to 8:00PM";
                                break;
                            case "from 7:00PM to 7:30PM":
                                theTimeAdjustedForTheCallCenter = "from 8:00PM to 8:30PM";
                                break;
                            case "from 7:30PM to 8:00PM":
                                theTimeAdjustedForTheCallCenter = "from 8:30PM to 9:00PM";
                                break;
                            case "from 8:00PM to 8:30PM":
                                theTimeAdjustedForTheCallCenter = "from 9:00PM to 9:30PM";
                                break;
                            case "from 8:30PM to 9:00PM":
                                theTimeAdjustedForTheCallCenter = "from 9:30PM to 10:00PM";
                                break;
                            case "from 9:00PM to 9:30PM":
                                theTimeAdjustedForTheCallCenter = "from 10:00PM to 10:30PM";
                                break;
                            case "from 9:30PM to 10:00PM":
                                theTimeAdjustedForTheCallCenter = "from 10:30PM to 11:00PM";
                                break;
                        }
                        break;
                    #endregion Pacific Times

                    #region Mountain Times
                    case "Mountain Time":
                        switch (theCookie)
                        {
                            case "Closed Sundays":
                                theTimeAdjustedForTheCallCenter = "Closed Sundays";
                                break;
                            case "from 6:00AM to 6:30AM":
                                theTimeAdjustedForTheCallCenter = "from 6:00AM to 6:30AM";
                                break;
                            case "from 6:30AM to 7:00AM":
                                theTimeAdjustedForTheCallCenter = "from 6:30AM to 7:00AM";
                                break;
                            case "from 7:00AM to 7:30AM":
                                theTimeAdjustedForTheCallCenter = "from 7:00AM to 7:30AM";
                                break;
                            case "from 7:30AM to 8:00AM":
                                theTimeAdjustedForTheCallCenter = "from 7:30AM to 8:00AM";
                                break;
                            case "from 8:00AM to 8:30AM":
                                theTimeAdjustedForTheCallCenter = "from 8:00AM to 8:30AM";
                                break;
                            case "from 8:30AM to 9:00AM":
                                theTimeAdjustedForTheCallCenter = "from 8:30AM to 9:00AM";
                                break;
                            case "from 9:00AM to 9:30AM":
                                theTimeAdjustedForTheCallCenter = "from 9:00AM to 9:30AM";
                                break;
                            case "from 9:30AM to 10:00AM":
                                theTimeAdjustedForTheCallCenter = "from 9:30AM to 10:00AM";
                                break;
                            case "from 10:00AM to 10:30AM":
                                theTimeAdjustedForTheCallCenter = "from 10:00AM to 10:30AM";
                                break;
                            case "from 10:30AM to 11:00AM":
                                theTimeAdjustedForTheCallCenter = "from 10:30AM to 11:00AM";
                                break;
                            case "from 11:00AM to 11:30AM":
                                theTimeAdjustedForTheCallCenter = "from 11:00AM to 11:30AM";
                                break;
                            case "from 11:30AM to 12:00PM":
                                theTimeAdjustedForTheCallCenter = "from 11:30AM to 12:00PM";
                                break;
                            case "from 12:00PM to 12:30PM":
                                theTimeAdjustedForTheCallCenter = "from 12:00PM to 12:30PM";
                                break;
                            case "from 12:30PM to 1:00PM":
                                theTimeAdjustedForTheCallCenter = "from 12:30PM to 1:00PM";
                                break;
                            case "from 1:00PM to 1:30PM":
                                theTimeAdjustedForTheCallCenter = "from 1:00PM to 1:30PM";
                                break;
                            case "from 1:30PM to 2:00PM":
                                theTimeAdjustedForTheCallCenter = "from 1:30PM to 2:00PM";
                                break;
                            case "from 2:00PM to 2:30PM":
                                theTimeAdjustedForTheCallCenter = "from 2:00PM to 2:30PM";
                                break;
                            case "from 2:30PM to 3:00PM":
                                theTimeAdjustedForTheCallCenter = "from 2:30PM to 3:00PM";
                                break;
                            case "from 3:00PM to 3:30PM":
                                theTimeAdjustedForTheCallCenter = "from 3:00PM to 3:30PM";
                                break;
                            case "from 3:30PM to 4:00PM":
                                theTimeAdjustedForTheCallCenter = "from 3:30PM to 4:00PM";
                                break;
                            case "from 4:00PM to 4:30PM":
                                theTimeAdjustedForTheCallCenter = "from 4:00PM to 4:30PM";
                                break;
                            case "from 4:30PM to 5:00PM":
                                theTimeAdjustedForTheCallCenter = "from 4:30PM to 5:00PM";
                                break;
                            case "from 5:00PM to 5:30PM":
                                theTimeAdjustedForTheCallCenter = "from 5:00PM to 5:30PM";
                                break;
                            case "from 5:30PM to 6:00PM":
                                theTimeAdjustedForTheCallCenter = "from 5:30PM to 6:00PM";
                                break;
                            case "from 6:00PM to 6:30PM":
                                theTimeAdjustedForTheCallCenter = "from 6:00PM to 6:30PM";
                                break;
                            case "from 6:30PM to 7:00PM":
                                theTimeAdjustedForTheCallCenter = "from 6:30PM to 7:00PM";
                                break;
                            case "from 7:00PM to 7:30PM":
                                theTimeAdjustedForTheCallCenter = "from 7:00PM to 7:30PM";
                                break;
                            case "from 7:30PM to 8:00PM":
                                theTimeAdjustedForTheCallCenter = "from 7:30PM to 8:00PM";
                                break;
                            case "from 8:00PM to 8:30PM":
                                theTimeAdjustedForTheCallCenter = "from 8:00PM to 8:30PM";
                                break;
                            case "from 8:30PM to 9:00PM":
                                theTimeAdjustedForTheCallCenter = "from 8:30PM to 9:00PM";
                                break;
                            case "from 9:00PM to 9:30PM":
                                theTimeAdjustedForTheCallCenter = "from 9:00PM to 9:30PM";
                                break;
                            case "from 9:30PM to 10:00PM":
                                theTimeAdjustedForTheCallCenter = "from 9:30PM to 10:00PM";
                                break;
                        }
                        break;
                    #endregion Mountain Times

                    #region Central Times
                    case "Central Time":
                        switch (theCookie)
                        {
                            case "Closed Sundays":
                                theTimeAdjustedForTheCallCenter = "Closed Sundays";
                                break;
                            case "from 6:00AM to 6:30AM":
                                theTimeAdjustedForTheCallCenter = "from 5:00AM to 5:30AM";
                                break;
                            case "from 6:30AM to 7:00AM":
                                theTimeAdjustedForTheCallCenter = "from 5:30AM to 6:00AM";
                                break;
                            case "from 7:00AM to 7:30AM":
                                theTimeAdjustedForTheCallCenter = "from 6:00AM to 6:30AM";
                                break;
                            case "from 7:30AM to 8:00AM":
                                theTimeAdjustedForTheCallCenter = "from 6:30AM to 7:00AM";
                                break;
                            case "from 8:00AM to 8:30AM":
                                theTimeAdjustedForTheCallCenter = "from 7:00AM to 7:30AM";
                                break;
                            case "from 8:30AM to 9:00AM":
                                theTimeAdjustedForTheCallCenter = "from 7:30AM to 8:00AM";
                                break;
                            case "from 9:00AM to 9:30AM":
                                theTimeAdjustedForTheCallCenter = "from 8:00AM to 8:30AM";
                                break;
                            case "from 9:30AM to 10:00AM":
                                theTimeAdjustedForTheCallCenter = "from 8:30AM to 9:00AM";
                                break;
                            case "from 10:00AM to 10:30AM":
                                theTimeAdjustedForTheCallCenter = "from 9:00AM to 9:30AM";
                                break;
                            case "from 10:30AM to 11:00AM":
                                theTimeAdjustedForTheCallCenter = "from 9:30AM to 10:00AM";
                                break;
                            case "from 11:00AM to 11:30AM":
                                theTimeAdjustedForTheCallCenter = "from 10:00AM to 10:30AM";
                                break;
                            case "from 11:30AM to 12:00PM":
                                theTimeAdjustedForTheCallCenter = "from 10:30AM to 11:00AM";
                                break;
                            case "from 12:00PM to 12:30PM":
                                theTimeAdjustedForTheCallCenter = "from 11:00AM to 11:30AM";
                                break;
                            case "from 12:30PM to 1:00PM":
                                theTimeAdjustedForTheCallCenter = "from 11:30AM to 12:00PM";
                                break;
                            case "from 1:00PM to 1:30PM":
                                theTimeAdjustedForTheCallCenter = "from 12:00PM to 12:30PM";
                                break;
                            case "from 1:30PM to 2:00PM":
                                theTimeAdjustedForTheCallCenter = "from 12:30PM to 1:00PM";
                                break;
                            case "from 2:00PM to 2:30PM":
                                theTimeAdjustedForTheCallCenter = "from 1:00PM to 1:30PM";
                                break;
                            case "from 2:30PM to 3:00PM":
                                theTimeAdjustedForTheCallCenter = "from 1:30PM to 2:00PM";
                                break;
                            case "from 3:00PM to 3:30PM":
                                theTimeAdjustedForTheCallCenter = "from 2:00PM to 2:30PM";
                                break;
                            case "from 3:30PM to 4:00PM":
                                theTimeAdjustedForTheCallCenter = "from 2:30PM to 3:00PM";
                                break;
                            case "from 4:00PM to 4:30PM":
                                theTimeAdjustedForTheCallCenter = "from 3:00PM to 3:30PM";
                                break;
                            case "from 4:30PM to 5:00PM":
                                theTimeAdjustedForTheCallCenter = "from 3:30PM to 4:00PM";
                                break;
                            case "from 5:00PM to 5:30PM":
                                theTimeAdjustedForTheCallCenter = "from 4:00PM to 4:30PM";
                                break;
                            case "from 5:30PM to 6:00PM":
                                theTimeAdjustedForTheCallCenter = "from 4:30PM to 5:00PM";
                                break;
                            case "from 6:00PM to 6:30PM":
                                theTimeAdjustedForTheCallCenter = "from 5:00PM to 5:30PM";
                                break;
                            case "from 6:30PM to 7:00PM":
                                theTimeAdjustedForTheCallCenter = "from 5:30PM to 6:00PM";
                                break;
                            case "from 7:00PM to 7:30PM":
                                theTimeAdjustedForTheCallCenter = "from 6:00PM to 6:30PM";
                                break;
                            case "from 7:30PM to 8:00PM":
                                theTimeAdjustedForTheCallCenter = "from 6:30PM to 7:00PM";
                                break;
                            case "from 8:00PM to 8:30PM":
                                theTimeAdjustedForTheCallCenter = "from 7:00PM to 7:30PM";
                                break;
                            case "from 8:30PM to 9:00PM":
                                theTimeAdjustedForTheCallCenter = "from 7:30PM to 8:00PM";
                                break;
                            case "from 9:00PM to 9:30PM":
                                theTimeAdjustedForTheCallCenter = "from 8:00PM to 8:30PM";
                                break;
                            case "from 9:30PM to 10:00PM":
                                theTimeAdjustedForTheCallCenter = "from 8:30PM to 9:00PM";
                                break;
                        }
                        break;
                    #endregion Central Times

                    #region Eastern Times
                    case "Eastern Time":
                        switch (theCookie)
                        {
                            case "Closed Sundays":
                                theTimeAdjustedForTheCallCenter = "Closed Sundays";
                                break;
                            case "from 6:00AM to 6:30AM":
                                theTimeAdjustedForTheCallCenter = "from 4:00AM to 4:30AM";
                                break;
                            case "from 6:30AM to 7:00AM":
                                theTimeAdjustedForTheCallCenter = "from 4:30AM to 5:00AM";
                                break;
                            case "from 7:00AM to 7:30AM":
                                theTimeAdjustedForTheCallCenter = "from 5:00AM to 5:30AM";
                                break;
                            case "from 7:30AM to 8:00AM":
                                theTimeAdjustedForTheCallCenter = "from 5:30AM to 6:00AM";
                                break;
                            case "from 8:00AM to 8:30AM":
                                theTimeAdjustedForTheCallCenter = "from 6:00AM to 6:30AM";
                                break;
                            case "from 8:30AM to 9:00AM":
                                theTimeAdjustedForTheCallCenter = "from 6:30AM to 7:00AM";
                                break;
                            case "from 9:00AM to 9:30AM":
                                theTimeAdjustedForTheCallCenter = "from 7:00AM to 7:30AM";
                                break;
                            case "from 9:30AM to 10:00AM":
                                theTimeAdjustedForTheCallCenter = "from 7:30AM to 8:00AM";
                                break;
                            case "from 10:00AM to 10:30AM":
                                theTimeAdjustedForTheCallCenter = "from 8:00AM to 8:30AM";
                                break;
                            case "from 10:30AM to 11:00AM":
                                theTimeAdjustedForTheCallCenter = "from 8:30AM to 9:00AM";
                                break;
                            case "from 11:00AM to 11:30AM":
                                theTimeAdjustedForTheCallCenter = "from 9:00AM to 9:30AM";
                                break;
                            case "from 11:30AM to 12:00PM":
                                theTimeAdjustedForTheCallCenter = "from 9:30AM to 10:00AM";
                                break;
                            case "from 12:00PM to 12:30PM":
                                theTimeAdjustedForTheCallCenter = "from 10:00AM to 10:30AM";
                                break;
                            case "from 12:30PM to 1:00PM":
                                theTimeAdjustedForTheCallCenter = "from 10:30AM to 11:00AM";
                                break;
                            case "from 1:00PM to 1:30PM":
                                theTimeAdjustedForTheCallCenter = "from 11:00AM to 11:30AM";
                                break;
                            case "from 1:30PM to 2:00PM":
                                theTimeAdjustedForTheCallCenter = "from 11:30AM to 12:00PM";
                                break;
                            case "from 2:00PM to 2:30PM":
                                theTimeAdjustedForTheCallCenter = "from 12:00PM to 12:30PM";
                                break;
                            case "from 2:30PM to 3:00PM":
                                theTimeAdjustedForTheCallCenter = "from 12:30PM to 1:00PM";
                                break;
                            case "from 3:00PM to 3:30PM":
                                theTimeAdjustedForTheCallCenter = "from 1:00PM to 1:30PM";
                                break;
                            case "from 3:30PM to 4:00PM":
                                theTimeAdjustedForTheCallCenter = "from 1:30PM to 2:00PM";
                                break;
                            case "from 4:00PM to 4:30PM":
                                theTimeAdjustedForTheCallCenter = "from 2:00PM to 2:30PM";
                                break;
                            case "from 4:30PM to 5:00PM":
                                theTimeAdjustedForTheCallCenter = "from 2:30PM to 3:00PM";
                                break;
                            case "from 5:00PM to 5:30PM":
                                theTimeAdjustedForTheCallCenter = "from 3:00PM to 3:30PM";
                                break;
                            case "from 5:30PM to 6:00PM":
                                theTimeAdjustedForTheCallCenter = "from 3:30PM to 4:00PM";
                                break;
                            case "from 6:00PM to 6:30PM":
                                theTimeAdjustedForTheCallCenter = "from 4:00PM to 4:30PM";
                                break;
                            case "from 6:30PM to 7:00PM":
                                theTimeAdjustedForTheCallCenter = "from 4:30PM to 5:00PM";
                                break;
                            case "from 7:00PM to 7:30PM":
                                theTimeAdjustedForTheCallCenter = "from 5:00PM to 5:30PM";
                                break;
                            case "from 7:30PM to 8:00PM":
                                theTimeAdjustedForTheCallCenter = "from 5:30PM to 6:00PM";
                                break;
                            case "from 8:00PM to 8:30PM":
                                theTimeAdjustedForTheCallCenter = "from 6:00PM to 6:30PM";
                                break;
                            case "from 8:30PM to 9:00PM":
                                theTimeAdjustedForTheCallCenter = "from 6:30PM to 7:00PM";
                                break;
                            case "from 9:00PM to 9:30PM":
                                theTimeAdjustedForTheCallCenter = "from 7:00PM to 7:30PM";
                                break;
                            case "from 9:30PM to 10:00PM":
                                theTimeAdjustedForTheCallCenter = "from 7:30PM to 8:00PM";
                                break;
                        }
                        break;
                    #endregion Eastern Times
                }
                #endregion Method for switching Time Zones
            }
            return theTimeAdjustedForTheCallCenter;
        }
    }

    public string _likelyAvailable;
    public string LikelyAvailable
    {
        get
        {
            var _likelyAvailable = firstAvailableTime.SelectedValue;
            if (_likelyAvailable != "" && RadioButtonRequest.Checked)
            {
                return _likelyAvailable;
            }
            else
            {
                _likelyAvailable = "";
            }

            return _likelyAvailable;
        }
    }

    private string Comments
    {
        get { return txtComments.InnerText; }
        set { txtComments.InnerText = value; }
    }

    public bool isValid { get; set; }
    public bool emailSent { get; set; }
    #endregion Properties

    #region Email Senders
    #region Render
    private string appointmentType
    {
        get
        {
            string button = "";
            if(RadioButtonSchedule.Checked) button = "schedule";
            if(RadioButtonRequest.Checked) button = "request";
            return button;
        }
            
    }
    public string TimeFrameTypeParagraph { get; set; }
    public string TimeFrameTypeForProspect
    {
        get
        {
            switch (appointmentType)
            {
                case "schedule": var paragraphOne = new StringBuilder();
                    #region Paragraph_1
                    paragraphOne.AppendFormat(@"
                    <h1>Congratulations {0}!</h1>
                    <p>
                        By requesting your customized Game Plan Report you've taken your first step to creating positive cash-flow for life!
                    </p>
                    <p>
                        A Game Plan Counselor will be contacting you at your selected appointment time. He or she will spend a few minutes asking you questions that will 
                        be used to generate your customized Game Plan. He or she will then email you your game plan options; or if you are an ideal candidate for the program, 
                        will schedule an appointment for you to meet with a Game Plan Specialist from the executive team to go over your custom Game Plan Report.
                    </p>
                    <p>
                        <strong>The date and time you requested to be contacted for your Game Plan Report is:</strong><br />
                        {5}<br />
                        {6}<br />
                        {7}<br />
                        <strong>Make sure to mark your calendar for this conversation and be sure to call us back if you miss our call.</strong>
                        <br />
                    </p>
                    <p>
                        If anything comes up and you need to reschedule your appointment or would like to get a Game Plan sooner, please contact Strongbrook at 801-204-9117.
                    </p>
                    <p>
                        In the meantime, feel free to visit <strong>http://{8}.strongbrook.com</strong> for more information: 
                        On this site you will be able to download a free digital copy of our 233 page hard cover book, <i>The Strait Path To Real Estate Wealth.</i> Simply enter the code, “FREE”.
                        You will also be able to access reports of our most recent completed real estate transactions and learn what people all over the country are saying about Strongbrook.
                    </p>
                    <p>
                        We look forward to sharing how the addition of Strongbrook's program can help build your wealth and turbo-charge your retirement cash-flow through investment grade rental real estate! 
                    </p>
                    <p>
                    <strong>To Your Success,                        </strong>
                    <br />  The Strongbrook Team
                    </p>
                    <br />
                    <strong><u>The information you provided</u>     </strong>
                    <p>     Name: {0} {1}                  </p>
                    <p>     Main Phone: {2}                         </p>
                    <p>     Secondary Phone: {3}                    </p>
                    <p>     Email Address: {4}                      </p>
                    <p>     Date Requested if any: {5}              </p>
                    <p>     Time Requested if any: {6}              </p>
                    <p>     Your Time Zone: {7}                     </p>
                    ", FirstName // 0
                     , LastName // 1
                     , Phone1 // 2
                     , Phone2 // 3
                     , Email // 4
                     , AppointmentDate // 5
                     , AppointmentTimeSelectedByTheProspect // 6
                     , TimeZone // 7
                     , CurrentUser_WebAlias // 8
                     );
                    #endregion Paragraph_1
                    TimeFrameTypeParagraph = paragraphOne.ToString();
                    break;
                case "request": var paragraphTwo = new StringBuilder();
                    #region Paragraph_2
                        paragraphTwo.AppendFormat(@"
                        <h1>Congratulations {0}!</h1>
                        <p>
                            By requesting your customized Game Plan Report you've taken your first step to creating positive cash-flow for life!
                        </p>
                        <p>
                            A Game Plan Counselor will be contacting you at your selected appointment time. He or she will spend a few minutes asking you questions that will 
                            be used to generate your customized Game Plan. He or she will then email you your game plan options; or if you are an ideal candidate for the program, 
                            will schedule an appointment for you to meet with a Game Plan Specialist from the executive team to go over your custom Game Plan Report.
                        </p>
                        <p>
                            <strong>The time of day selected for the appointment is:</strong><br />
                            Any {5} in the {6} time zone.<br />
                            <strong>Make sure to mark your calendar for this conversation and be sure to call us back if you miss our call.</strong>
                            <br />
                        </p>
                        <p>
                            If anything comes up and you need to reschedule your appointment or would like to get a Game Plan sooner, please contact Strongbrook at 801-204-9117.
                        </p>
                        <p>
                            In the meantime, feel free to visit <strong>http://{7}.strongbrook.com</strong> for more information: 
                            On this site you will be able to download a free digital copy of our 233 page hard cover book, <i>The Strait Path To Real Estate Wealth.</i> Simply enter the code, “FREE”.
                            You will also be able to access reports of our most recent completed real estate transactions and learn what people all over the country are saying about Strongbrook.
                        </p>
                        <p>
                            We look forward to sharing how the addition of Strongbrook's program can help build your wealth and turbo-charge your retirement cash-flow through investment grade rental real estate! 
                        </p>
                        <p>
                        <strong>To Your Success,                        </strong>
                        <br />  The Strongbrook Team
                        </p>
                        <br />
                        <strong><u>The information you provided</u>     </strong>
                        <p>     Name: {0} {1}                  </p>
                        <p>     Main Phone: {2}                         </p>
                        <p>     Secondary Phone: {3}                    </p>
                        <p>     Email Address: {4}                      </p>
                        <p>     Likely Available: {5}                   </p>
                        <p>     Your Time Zone: {6}                     </p>
                        ", FirstName // 0
                         , LastName // 1
                         , Phone1 // 2
                         , Phone2 // 3
                         , Email // 4
                         , LikelyAvailable // 5
                         , TimeZone // 6
                         , CurrentUser_WebAlias // 7
                         );
                        #endregion Paragraph_2
                    TimeFrameTypeParagraph = paragraphTwo.ToString();
                    break;
            }
            return TimeFrameTypeParagraph;
        }
    }
    public string TimeFrameTypeForUpline
    {
        get
        {
            switch (appointmentType)
            {
                case "schedule": var paragraphOne = new StringBuilder();
                    #region Paragraph_1
                    paragraphOne.AppendFormat(@"
                    <h1>Congratulations, {0} {1} has just requested a Game Plan!</h1>
                    <p>
                        <strong>Requested Contact Time (If the prospect requested a specific date and time to be contacted.)</strong><br />
                        {2}<br />
                        {3}<br />
                        {4}<br />
                        <strong>If possible, you may want to do a follow up call with them to see how it went.</strong>
                    </p>
                    <p>
                    <strong>To Your Success,                        </strong>
                    <br />  The Strongbrook Team
                    </p>
                    <br />
                    <br />
                    <p>
                    <strong><u>The prospect's information</u>       </strong>
                    <p>     Prospect Name: {0} {1}                  </p>
                    <p>     Main Phone: {5}                         </p>
                    <p>     Secondary Phone: {6}                    </p>
                    <p>     Email Address: {7}                      </p>
                    <p>     Prospects Time Zone: {4}                </p>
                    ", FirstName // 0
                     , LastName // 1
                     , AppointmentDate // 2
                     , AppointmentTimeSelectedByTheProspect // 3
                     , TimeZone // 4
                     , Phone1   // 5
                     , Phone2   // 6
                     , Email    // 7
                     );
                    #endregion Paragraph_1
                    TimeFrameTypeParagraph = paragraphOne.ToString();
                    break;
                case "request": var paragraphTwo = new StringBuilder();
                    #region Paragraph_2
                        paragraphTwo.AppendFormat(@"
                        <h1>Congratulations, {0} {1} has just requested a Game Plan!</h1>
                        <p>
                            <strong>The time of day selected for the appointment is:</strong><br />
                            Any {2} in the {3} time zone.<br />
                            <strong>If possible, you may want to do a follow up call with them in a few days to see how it went.</strong>
                        </p>
                        <p>
                        <strong>To Your Success,                        </strong>
                        <br />  The Strongbrook Team
                        </p>
                        <br />
                        <br />
                        <p>
                        <strong><u>The prospect's information</u>       </strong>
                        <p>     Prospect Name: {0} {1}                  </p>
                        <p>     Main Phone: {4}                         </p>
                        <p>     Secondary Phone: {5}                    </p>
                        <p>     Email Address: {6}                      </p>
                        <p>     Prospects Time Zone: {3}                </p>
                        ", FirstName // 0
                         , LastName // 1
                         , LikelyAvailable // 2
                         , TimeZone // 3
                         , Phone1 // 4
                         , Phone2 // 5
                         , Email // 6
                         );
                        #endregion Paragraph_2
                    TimeFrameTypeParagraph = paragraphTwo.ToString();
                    break;
            }
            return TimeFrameTypeParagraph;
        }
    }
    public string TimeFrameTypeForCorporate
    {
        get
        {
            switch (appointmentType)
            {
                case "schedule": var paragraphOne = new StringBuilder();
                    #region Paragraph_1
                    paragraphOne.AppendFormat(@"
                    <h1>    New Game Plan Request for: {1} {2}      </h1>
                    <br />
                    <p>     Order ID: {0}                          </p>
                    <p>     Prospect Name: {1} {2}                  </p>
                    <p>     Main Phone: {3}                         </p>
                    <p>     Secondary Phone: {4}                    </p>
                    <p>     Email Address: {5}                      </p>
                    <p>     Date Requested if any: {6}              </p>
                    <p>     Time Requested if any: {7}              </p>
                    <p>     Prospects Time Zone: {8}                </p>
                    <p>     Estimated Net Worth: {9}                </p>
                    <p><u>  Comments:                               </u>  
                    <br />  {10}
                    <br />
                    <p><u>  Enroller Information:                   </u>
                    <br />  {11}
                    <br />  {12}
                    <br />  {13}
                    </p>
                    ", orderID // 0
                     , FirstName // 1
                     , LastName // 2
                     , Phone1 // 3
                     , Phone2 // 4
                     , Email // 5
                     , AppointmentDate // 6
                     , AppointmentTimeInCorporateTimeZone // 7
                     , TimeZone // 8
                     , NetWorth // 9
                     , Comments  // 10
                     , CurrentUser_FirstName + " " + CurrentUser_LastName // 11
                     , CurrentUser_Email // 12
                     , CurrentUser_Phone  // 13
                     );
                    #endregion Paragraph_1
                    TimeFrameTypeParagraph = paragraphOne.ToString();
                    break;
                case "request": var paragraphTwo = new StringBuilder();
                    #region Paragraph_2
                        paragraphTwo.AppendFormat(@"
                        <h1>    New Game Plan Request for: {1} {2}      </h1>
                        <br />
                        <p>     Order ID: {0}                          </p>
                        <p>     Prospect Name: {1} {2}                  </p>
                        <p>     Main Phone: {3}                         </p>
                        <p>     Secondary Phone: {4}                    </p>
                        <p>     Email Address: {5}                      </p>
                        <p>     Likely Available: {6}                   </p>
                        <p>     Prospects Time Zone: {7}                </p>
                        <p>     Estimated Net Worth: {8}                </p>
                        <p><u>  Comments:                               </u>  
                        <br />  {9}
                        <br />
                        <p><u>  Enroller Information:                   </u>
                        <br />  {10}
                        <br />  {11}
                        <br />  {12}
                        </p>
                        ", orderID // 0
                         , FirstName // 1
                         , LastName // 2
                         , Phone1 // 3
                         , Phone2 // 4
                         , Email // 5
                         , LikelyAvailable // 6
                         , TimeZone // 7
                         , NetWorth // 8
                         , Comments  // 9
                         , CurrentUser_FirstName + " " + CurrentUser_LastName // 10
                         , CurrentUser_Email // 11
                         , CurrentUser_Phone  // 12
                         );
                        #endregion Paragraph_2
                    TimeFrameTypeParagraph = paragraphTwo.ToString();
                    break;
            }
            return TimeFrameTypeParagraph;
        }
    }

    #endregion Render
    private bool SendEmailToProspect()
    {
        emailSent = false;

        #region Email Header Properties
        //First Create the Address Info
        MailAddress from = new MailAddress("support@strongbrookdirect.com", "No Reply");
        MailAddress to = new MailAddress(Email, FirstName + " " + LastName);
        //MailAddress cc = new MailAddress("Chris.Ferguson@strongbrook.com");
        //MailAddress bcc = new MailAddress("Tyler.Bennett@strongbrook.com");

        //Construct the email - just simple text email
        MailMessage message = new MailMessage(from, to);
        //message.CC.Add(cc);
        //message.Bcc.Add(bcc);
        //message.Bcc.Add("aaronbaker315@me.com");
        message.Bcc.Add("paul.janson@strongbrook.com");
        message.Subject = string.Format("New Game Plan requested for {0} {1}", FirstName, LastName);
        message.IsBodyHtml = true;
        #endregion Email Header Properties

        #region Email Message Body
        message.Body = TimeFrameTypeForProspect;
        #endregion Email Message Body

        //#region Main Mail server connection properties
        ////Email SMTP Settings
        //Int16 port = 25;
        //SmtpClient client = new SmtpClient("smtpout.secureserver.net", port);

        //// Use these properties for a un-secure SMTP connection. ie. the strongbrookdirect.com email server.
        //client.UseDefaultCredentials = false;

        //client.Credentials = new System.Net.NetworkCredential("support@strongbrookdirect.com", "Reic2012");
        //#endregion Main Mail server connection properties

        #region Secondary Mail server connection properties. Use this as a backup if necessary!
        Int16 port = 25;
        SmtpClient client = new SmtpClient("smtp.gmail.com", port);

        // Use these properties for a secure SMTP connection.
        client.UseDefaultCredentials = true;
        client.EnableSsl = true;

        client.Credentials = new System.Net.NetworkCredential("aaron@bakerwebdev.com", "sting123");
        #endregion Secondary Mail server connection properties. Use this as a backup if necessary!

        #region Attempt to send the message
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
        #endregion Attempt to send the message

        return emailSent;
    }
    private bool SendEmailToProspectsUpline()
    {
        emailSent = false;

        #region Email Header Properties
        //First Create the Address Info
        MailAddress from = new MailAddress("support@strongbrookdirect.com", "No Reply");
        MailAddress to = new MailAddress(CurrentUser_Email, CurrentUser_FirstName + " " + CurrentUser_LastName);

        //Construct the email - just simple text email
        MailMessage message = new MailMessage(from, to);
        //message.Bcc.Add("aaronbaker315@me.com");
        message.Bcc.Add("paul.janson@strongbrook.com");
        message.Subject = string.Format("New Game Plan requested for {0} {1}", FirstName, LastName);
        message.IsBodyHtml = true;
        #endregion Email Header Properties

        #region Email Message Body
        message.Body = TimeFrameTypeForUpline;
        #endregion Email Message Body

        //#region Main Mail server connection properties
        ////Email SMTP Settings
        //Int16 port = 25;
        //SmtpClient client = new SmtpClient("smtpout.secureserver.net", port);

        //// Use these properties for a un-secure SMTP connection. ie. the strongbrookdirect.com email server.
        //client.UseDefaultCredentials = false;

        //client.Credentials = new System.Net.NetworkCredential("support@strongbrookdirect.com", "Reic2012");
        //#endregion Main Mail server connection properties

        #region Secondary Mail server connection properties. Use this as a backup if necessary!
        Int16 port = 25;
        SmtpClient client = new SmtpClient("smtp.gmail.com", port);

        // Use these properties for a secure SMTP connection.
        client.UseDefaultCredentials = true;
        client.EnableSsl = true;

        client.Credentials = new System.Net.NetworkCredential("aaron@bakerwebdev.com", "sting123");
        #endregion Secondary Mail server connection properties. Use this as a backup if necessary!

        #region Attempt to send the message
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
        #endregion Attempt to send the message

        return emailSent;
    }
    private bool SendEmailToCorporate()
    {
        emailSent = false;

        #region Email Header Properties
        //First Create the Address Info
        MailAddress from = new MailAddress("support@strongbrookdirect.com", "No Reply");
        MailAddress to = new MailAddress("GamePlanRequest@strongbrook.com", "GPR Group");
        MailAddress cc = new MailAddress("Chris.Ferguson@strongbrook.com");
        MailAddress bcc = new MailAddress("Tyler.Bennett@strongbrook.com");

        //Construct the email - just simple text email
        MailMessage message = new MailMessage(from, to);
        message.CC.Add(cc);
        message.Bcc.Add(bcc);
        message.Bcc.Add("aaronbaker315@me.com");
        message.Bcc.Add("paul.janson@strongbrook.com");
        message.Subject = string.Format("New Game Plan requested for {0} {1}", FirstName, LastName);
        message.IsBodyHtml = true;
        #endregion Email Header Properties

        #region Email Message Body
        message.Body = TimeFrameTypeForCorporate;
        #endregion Email Message Body

        #region Main Mail server connection properties
        //Email SMTP Settings
        Int16 port = 25;
        SmtpClient client = new SmtpClient("smtpout.secureserver.net", port);

        // Use these properties for a un-secure SMTP connection. ie. the strongbrookdirect.com email server.
        client.UseDefaultCredentials = false;

        client.Credentials = new System.Net.NetworkCredential("support@strongbrookdirect.com", "Reic2012");
        #endregion Main Mail server connection properties

        //#region Secondary Mail server connection properties. Use this as a backup if necessary!
        //Int16 port = 25;
        //SmtpClient client = new SmtpClient("smtp.gmail.com", port);

        //// Use these properties for a secure SMTP connection.
        //client.UseDefaultCredentials = true;
        //client.EnableSsl = true;

        //client.Credentials = new System.Net.NetworkCredential("aaron@bakerwebdev.com", "sting123");
        //#endregion Secondary Mail server connection properties. Use this as a backup if necessary!

        #region Attempt to send the message
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
        #endregion Attempt to send the message

        return emailSent;
    }
    #endregion Email Senders

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