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

            SetCurrentUser();
            Click_Submit();

        }

    }
    #endregion Page Load

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
        , AppointmentTime
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
            request.City = "Orem";
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
                //SendEmail();
                SendEmailToCallCenter();
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

        firstAvailableTime.Items.Clear();
        firstAvailableTime.Items.Add(new ListItem("Most Likely Time Available"));
        firstAvailableTime.Items.Add(new ListItem("Morning"));
        firstAvailableTime.Items.Add(new ListItem("Afternoon"));
        firstAvailableTime.Items.Add(new ListItem("Evening"));

        drdlTimeZone.Items.Clear();
        drdlTimeZone.Items.Add(new ListItem("Select Your Time Zone"));
        drdlTimeZone.Items.Add(new ListItem("Hawaii Time"));
        drdlTimeZone.Items.Add(new ListItem("Pacific Time"));
        drdlTimeZone.Items.Add(new ListItem("Mountain Time"));
        drdlTimeZone.Items.Add(new ListItem("Central Time"));
        drdlTimeZone.Items.Add(new ListItem("Eastern Time"));

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
        string from_6AM_to_1PM = "<option>" + "Select a Time" + oMin + "from 6:00AM to 6:30AM" + oMin + "from 6:30AM to 7:00AM" + oMin + "from 7:00AM to 7:30AM" + oMin + "from 7:30AM to 8:00AM" + oMin + "from 8:00AM to 8:30AM" + oMin + "from 8:30AM to 9:00AM" + oMin + "from 9:00AM to 9:30AM" + oMin + "from 9:30AM to 10:00AM" + oMin + "from 10:00AM to 10:30AM" + oMin + "from 10:30AM to 11:00AM" + oMin + "from 11:00AM to 11:30AM" + oMin + "from 11:30AM to 12:00PM" + oMin + "from 12:00PM to 12:30PM" + oMin + "from 12:30PM to 1:00PM" + "</option>";
        string from_6AM_to_8AM = "<option>" + "Select a Time" + oMin + "from 6:00AM to 6:30AM" + oMin + "from 6:30AM to 7:00AM" + oMin + "from 7:00AM to 7:30AM" + oMin + "from 7:30AM to 8:00AM" + "</option>";
        string from_noon_to_4PM = "<option>" + "from 12:00PM to 12:30PM" + oMin + "from 12:30PM to 1:00PM" + oMin + "from 1:00PM to 1:30PM" + oMin + "from 1:30PM to 2:00PM" + oMin + "from 2:00PM to 2:30PM" + oMin + "from 2:30PM to 3:00PM" + oMin + "from 3:00PM to 3:30PM" + oMin + "from 3:30PM to 4:00PM" + "</option>";
        string from_6AM_to_9AM = "<option>" + "Select a Time" + oMin + "from 6:00AM to 6:30AM" + oMin + "from 6:30AM to 7:00AM" + oMin + "from 7:00AM to 7:30AM" + oMin + "from 7:30AM to 8:00AM" + oMin + "from 8:00AM to 8:30AM" + oMin + "from 8:30AM to 9:00AM" + "</option>";
        #endregion Hawaii Hours

        #region Pacific Hours
        string from_9AM_to_4PM = "<option>" + "Select a Time" + oMin + "from 9:00AM to 9:30AM" + oMin + "from 9:30AM to 10:00AM" + oMin + "from 10:00AM to 10:30AM" + oMin + "from 10:30AM to 11:00AM" + oMin + "from 11:00AM to 11:30AM" + oMin + "from 11:30AM to 12:00PM" + oMin + "from 12:00PM to 12:30PM" + oMin + "from 12:30PM to 1:00PM" + oMin + "from 1:00PM to 1:30PM" + oMin + "from 1:30PM to 2:00PM" + oMin + "from 2:00PM to 2:30PM" + oMin + "from 2:30PM to 3:00PM" + oMin + "from 3:00PM to 3:30PM" + oMin + "from 3:30PM to 4:00PM" + "</option>";
        string from_9AM_to_11AM = "<option>" + "Select a Time" + oMin + "from 9:00AM to 9:30AM" + oMin + "from 9:30AM to 10:00AM" + oMin + "from 10:00AM to 10:30AM" + oMin + "from 10:30AM to 11:00AM" + oMin + "</option>";
        string from_3PM_to_7PM = "<option>" + "from 3:00PM to 3:30PM" + oMin + "from 3:30PM to 4:00PM" + oMin + "from 4:00PM to 4:30PM" + oMin + "from 4:30PM to 5:00PM" + oMin + "from 5:00PM to 5:30PM" + oMin + "from 5:30PM to 6:00PM" + oMin + "from 6:00PM to 6:30PM" + oMin + "from 6:30PM to 7:00PM" + "</option>";
        string from_9AM_to_noon = "<option>" + "Select a Time" + oMin + "from 9:00AM to 9:30AM" + oMin + "from 9:30AM to 10:00AM" + oMin + "from 10:00AM to 10:30AM" + oMin + "from 10:30AM to 11:00AM" + oMin + "from 11:00AM to 11:30AM" + oMin + "from 11:30AM to 12:00PM" + "</option>";
        #endregion Pacific Hours

        #region Mountain Hours
        string from_10AM_to_5PM = "<option>" + "Select a Time" + oMin + "from 10:00AM to 10:30AM" + oMin + "from 10:30AM to 11:00AM" + oMin + "from 11:00AM to 11:30AM" + oMin + "from 11:30AM to 12:00PM" + oMin + "from 12:00PM to 12:30PM" + oMin + "from 12:30PM to 1:00PM" + oMin + "from 1:00PM to 1:30PM" + oMin + "from 1:30PM to 2:00PM" + oMin + "from 2:00PM to 2:30PM" + oMin + "from 2:30PM to 3:00PM" + oMin + "from 3:00PM to 3:30PM" + oMin + "from 3:30PM to 4:00PM" + oMin + "from 4:00PM to 4:30PM" + oMin + "from 4:30PM to 5:00PM" + "</option>";
        string from_10AM_to_noon = "<option>" + "Select a Time" + oMin + "from 10:00AM to 10:30AM" + oMin + "from 10:30AM to 11:00AM" + oMin + "from 11:00AM to 11:30AM" + oMin + "from 11:30AM to 12:00PM" + "</option>";
        string from_4PM_to_8PM = "<option>" + "from 4:00PM to 4:30PM" + oMin + "from 4:30PM to 5:00PM" + oMin + "from 5:00PM to 5:30PM" + oMin + "from 5:30PM to 6:00PM" + oMin + "from 6:00PM to 6:30PM" + oMin + "from 6:30PM to 7:00PM" + oMin + "from 7:00PM to 7:30PM" + oMin + "from 7:30PM to 8:00PM" + "</option>";
        string from_10AM_to_1PM = "<option>" + "Select a Time" + oMin + "from 10:00AM to 10:30AM" + oMin + "from 10:30AM to 11:00AM" + oMin + "from 11:00AM to 11:30AM" + oMin + "from 11:30AM to 12:00PM" + oMin + "from 12:00AM to 12:30AM" + oMin + "from 12:30AM to 1:00PM" + "</option>";
        #endregion Mountain Hours

        #region Central Hours
        string from_11AM_to_6PM = "<option>" + "Select a Time" + oMin + "from 11:00AM to 11:30AM" + oMin + "from 11:30AM to 12:00PM" + oMin + "from 12:00PM to 12:30PM" + oMin + "from 12:30PM to 1:00PM" + oMin + "from 1:00PM to 1:30PM" + oMin + "from 1:30PM to 2:00PM" + oMin + "from 2:00PM to 2:30PM" + oMin + "from 2:30PM to 3:00PM" + oMin + "from 3:00PM to 3:30PM" + oMin + "from 3:30PM to 4:00PM" + oMin + "from 4:00PM to 4:30PM" + oMin + "from 4:30PM to 5:00PM" + oMin + "from 5:00PM to 5:30PM" + oMin + "from 5:30PM to 6:00PM" + "</option>";
        string from_11AM_to_1PM = "<option>" + "Select a Time" + oMin + "from 11:00AM to 11:30AM" + oMin + "from 11:30AM to 12:00PM" + oMin + "from 12:00AM to 12:30AM" + oMin + "from 12:30AM to 1:00PM" + "</option>";
        string from_5PM_to_9PM = "<option>" + "from 5:00PM to 5:30PM" + oMin + "from 5:30PM to 6:00PM" + oMin + "from 6:00PM to 6:30PM" + oMin + "from 6:30PM to 7:00PM" + oMin + "from 7:00PM to 7:30PM" + oMin + "from 7:30PM to 8:00PM" + oMin + "from 8:30PM to 8:00PM" + oMin + "from 8:00PM to 8:30PM" + oMin + "from 8:30PM to 9:00PM" + "</option>";
        string from_11AM_to_2PM = "<option>" + "Select a Time" + oMin + "from 11:00AM to 11:30AM" + oMin + "from 11:30AM to 12:00PM" + oMin + "from 12:00AM to 12:30AM" + oMin + "from 12:30AM to 1:00PM" + oMin + "from 1:00PM to 1:30PM" + oMin + "from 1:30PM to 2:00PM" + "</option>";
        #endregion Central Hours

        #region Eastern Hours
        string from_12PM_to_7PM = "<option>" + "Select a Time" + oMin + "from 12:00PM to 12:30PM" + oMin + "from 12:30PM to 1:00PM" + oMin + "from 1:00PM to 1:30PM" + oMin + "from 1:30PM to 2:00PM" + oMin + "from 2:00PM to 2:30PM" + oMin + "from 2:30PM to 3:00PM" + oMin + "from 3:00PM to 3:30PM" + oMin + "from 3:30PM to 4:00PM" + oMin + "from 4:00PM to 4:30PM" + oMin + "from 4:30PM to 5:00PM" + oMin + "from 5:00PM to 5:30PM" + oMin + "from 5:30PM to 6:00PM" + oMin + "from 6:00PM to 6:30PM" + oMin + "from 6:30PM to 7:00PM" + "</option>";
        string from_12PM_to_2PM = "<option>" + "Select a Time" + oMin + "from 12:00PM to 12:30PM" + oMin + "from 12:30PM to 1:00PM" + oMin + "from 1:00PM to 1:30PM" + oMin + "from 1:30PM to 2:00PM" + "</option>";
        string from_6PM_to_10PM = "<option>" + "from 6:00PM to 6:30PM" + oMin + "from 6:30PM to 7:00PM" + oMin + "from 7:00PM to 7:30PM" + oMin + "from 7:30PM to 8:00PM" + oMin + "from 8:00PM to 8:30PM" + oMin + "from 8:30PM to 9:00PM" + oMin + "from 9:00PM to 9:30PM" + oMin + "from 9:30PM to 10:00PM";
        string from_12AM_to_3PM = "<option>" + "Select a Time" + oMin + "from 12:00PM to 12:30PM" + oMin + "from 12:30PM to 1:00PM" + oMin + "from 1:00PM to 1:30PM" + oMin + "from 1:30PM to 2:00PM" + oMin + "from 2:00PM to 2:30PM" + oMin + "from 2:30PM to 3:00PM" + "</option>";
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
                        Response.Write(from_6AM_to_8AM + oMin + from_noon_to_4PM);
                        break;
                    case "Wednesday":
                        Response.Write(from_6AM_to_8AM + oMin + from_noon_to_4PM);
                        break;
                    case "Thursday":
                        Response.Write(from_6AM_to_8AM + oMin + from_noon_to_4PM);
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
                        Response.Write(from_9AM_to_11AM + oMin + from_3PM_to_7PM);
                        break;
                    case "Wednesday":
                        Response.Write(from_9AM_to_11AM + oMin + from_3PM_to_7PM);
                        break;
                    case "Thursday":
                        Response.Write(from_9AM_to_11AM + oMin + from_3PM_to_7PM);
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
                        Response.Write(from_10AM_to_noon + oMin + from_4PM_to_8PM);
                        break;
                    case "Wednesday":
                        Response.Write(from_10AM_to_noon + oMin + from_4PM_to_8PM);
                        break;
                    case "Thursday":
                        Response.Write(from_10AM_to_noon + oMin + from_4PM_to_8PM);
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
                        Response.Write(from_11AM_to_1PM + oMin + from_5PM_to_9PM);
                        break;
                    case "Wednesday":
                        Response.Write(from_11AM_to_1PM + oMin + from_5PM_to_9PM);
                        break;
                    case "Thursday":
                        Response.Write(from_11AM_to_1PM + oMin + from_5PM_to_9PM);
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
                        Response.Write(from_12PM_to_2PM + oMin + from_6PM_to_10PM);
                        break;
                    case "Wednesday":
                        Response.Write(from_12PM_to_2PM + oMin + from_6PM_to_10PM);
                        break;
                    case "Thursday":
                        Response.Write(from_12PM_to_2PM + oMin + from_6PM_to_10PM);
                        break;
                    case "Friday":
                        Response.Write(from_12PM_to_7PM);
                        break;
                    case "Saturday":
                        Response.Write(from_12AM_to_3PM);
                        break;
                }
                break;
            #endregion Eastern Times
        }
        #endregion Method for switching Time Zones

        Response.End();

        #endregion
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

    public string NetWorth
    {
        get { return netWorth.SelectedValue; }
        set { netWorth.SelectedValue = value; }
    }

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




    //public string asdf
    //{
    //    get
    //    {
    //        return txtSelectedTimeFrame.Text;
    //    }
    //    set
    //    {
    //        txtSelectedTimeFrame.Text = value;
    //    }
    //}


    public string AppointmentTime
    {
        get
        {
            return theCookie;
        }
    }



    public string AdjustedAppointmentTime // <--- This is where I stopped yesterday.
    {
        get
        {
            string asdf = "";
            if (TimeZone == "Hawaii" && theCookie == "from 6:00AM to 6:30AM")
            {
                asdf = "from 10:00AM to 10:30AM";

            }
            return asdf;
        }
    }














    public string LikelyAvailable
    {
        get { return firstAvailableTime.SelectedValue; }
        set { firstAvailableTime.SelectedValue = value; }
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
         , LikelyAvailable // 5
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

        #region Main Mail server connection properties
        SmtpClient client = new SmtpClient("smtpout.secureserver.net", port);

        // Use these properties for a un-secure SMTP connection. ie. the strongbrookdirect.com email server.
        client.UseDefaultCredentials = false;

        client.Credentials = new System.Net.NetworkCredential("support@strongbrookdirect.com", "Reic2012");
        #endregion Main Mail server connection properties

        #region Secondary Mail server connection properties. Use this as a backup if necessary!
        //SmtpClient client = new SmtpClient("smtp.gmail.com", port);

        //// Use these properties for a secure SMTP connection.
        //client.UseDefaultCredentials = true;
        //client.EnableSsl = true;

        //client.Credentials = new System.Net.NetworkCredential("aaron@bakerwebdev.com", "sting123");
        #endregion Secondary Mail server connection properties. Use this as a backup if necessary!

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



    private bool SendEmailToCallCenter()
    {
        //string timeFrame = Request.Form["timeFrameSelected"];

        emailSent = false;

        ////First Create the Address Info
        MailAddress from = new MailAddress("support@strongbrookdirect.com", "No Reply");
        //MailAddress to = new MailAddress("GamePlanRequest@strongbrook.com", "GPR Group");
        //MailAddress cc = new MailAddress("Chris.Ferguson@strongbrook.com");
        //MailAddress bcc = new MailAddress("aaron.baker@strongbrook.com");

        MailAddress to = new MailAddress("aaron.baker@strongbrook.com");


        ////Construct the email - just simple text email
        MailMessage message = new MailMessage(from, to);
        //message.CC.Add(cc);
        //message.Bcc.Add(bcc);
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
Time Requested if any: {8}
Estimated Net Worth: {9}

Comments: 
{10}


Enroller Information:
{11}
{12}
{13}
        ", FirstName // 0
         , LastName // 1
         , Phone1 // 2
         , Phone2 // 3
         , Email // 4
         , LikelyAvailable // 5
         , TimeZone // 6
         , AppointmentDate // 7
         , AdjustedAppointmentTime // 8
         , NetWorth // 9
         , Comments  // 10
         , CurrentUser_FirstName + " " + CurrentUser_LastName // 11
         , CurrentUser_Email // 12
         , CurrentUser_Phone  // 13
         );
        #endregion Email Message Body

        //Email SMTP Settings
        Int16 port = 25;

        #region Main Mail server connection properties
        SmtpClient client = new SmtpClient("smtpout.secureserver.net", port);

        // Use these properties for a un-secure SMTP connection. ie. the strongbrookdirect.com email server.
        client.UseDefaultCredentials = false;

        client.Credentials = new System.Net.NetworkCredential("support@strongbrookdirect.com", "Reic2012");
        #endregion Main Mail server connection properties

        #region Secondary Mail server connection properties. Use this as a backup if necessary!
        //SmtpClient client = new SmtpClient("smtp.gmail.com", port);

        //// Use these properties for a secure SMTP connection.
        //client.UseDefaultCredentials = true;
        //client.EnableSsl = true;

        //client.Credentials = new System.Net.NetworkCredential("aaron@bakerwebdev.com", "sting123");
        #endregion Secondary Mail server connection properties. Use this as a backup if necessary!

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