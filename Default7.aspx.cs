using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Default7 : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        #region Populate Dropdown 
        Response.Expires = -1;
        Response.ContentType = "text/plain";
        string timeZone = Request.Form["timeZone"];




        string timeFrameSelection = Request.Form["timeFrame"];

        string select = "Select a Time";
        string isWorking = "It's working";

        string Hawaii_1 = "7 to 8" + " " + Request.Form["timeZone"];
        string Hawaii_2 = "11 to 12" + " " + Request.Form["timeZone"];
        string Hawaii_3 = "12 to 1" + " " + Request.Form["timeZone"];
        string Hawaii_6 = "3 to 4" + " " + Request.Form["timeZone"];

        string Pacific_1 = "9:00 AM to 9:30 AM" + " " + Request.Form["timeZone"];
        string Pacific_2 = "9:30 AM to 10:00 AM" + " " + Request.Form["timeZone"];
        string Pacific_3 = "10:00 AM to 10:30 AM" + " " + Request.Form["timeZone"];
        string Pacific_4 = "10:30 AM to 11:00 AM" + " " + Request.Form["timeZone"];
        string Pacific_5 = "11:00 AM to 11:30 AM" + " " + Request.Form["timeZone"];
        string Pacific_6 = "11:30 AM to 12:00 PM" + " " + Request.Form["timeZone"];
        string Pacific_7 = "12:00 PM to 12:30 PM" + " " + Request.Form["timeZone"];
        string Pacific_8 = "12:30 PM to 1:00 PM" + " " + Request.Form["timeZone"];
        string Pacific_9 = "1:00 PM to 1:30 PM" + " " + Request.Form["timeZone"];
        string Pacific_10 = "1:30 PM to 2:00 PM" + " " + Request.Form["timeZone"];
        string Pacific_11 = "2:00 PM to 2:30 PM" + " " + Request.Form["timeZone"];
        string Pacific_12 = "2:30 PM to 3:00 PM" + " " + Request.Form["timeZone"];
        string Pacific_13 = "3:00 PM to 3:30 PM" + " " + Request.Form["timeZone"];
        string Pacific_14 = "3:30 PM to 4:00 PM" + " " + Request.Form["timeZone"];
        string Pacific_15 = "4:00 PM to 4:30 PM" + " " + Request.Form["timeZone"];
        string Pacific_16 = "4:30 PM to 5:00 PM" + " " + Request.Form["timeZone"];

        string Mountain_1 = "10 to 11" + " " + Request.Form["timeZone"];
        string Mountain_2 = "11 to 12" + " " + Request.Form["timeZone"];
        string Mountain_3 = "12 to 1" + " " + Request.Form["timeZone"];

        string Eastern_1 = "12 to 1" + " " + Request.Form["timeZone"];
        string Eastern_2 = "1 to 2" + " " + Request.Form["timeZone"];
        string Eastern_3 = "2 to 3" + " " + Request.Form["timeZone"];
        string Eastern_4 = "3 to 4" + " " + Request.Form["timeZone"];
        string Eastern_5 = "4 to 5" + " " + Request.Form["timeZone"];
        string Eastern_6 = "5 to 6" + " " + Request.Form["timeZone"];

        if (timeZone == "Hawaii Time" && timeFrameSelection == "Monday")
        {
            Response.Write("<option>" + select + "</option>" + "<option>" + Hawaii_1 + "</option>" + "<option>" + Hawaii_2 + "</option>" + "<option>" + Hawaii_3 + "</option>");
        }
        else if (timeZone == "Hawaii Time" && timeFrameSelection == "Tuesday")
        {
            Response.Write("<option>" + isWorking + "</option>");
        }
        else if (timeZone == "Pacific Time")
        {
            Response.Write("<option>" + select + "</option>" + "<option>" + Pacific_1 + "</option>" + "<option>" + Pacific_2 + "</option>" + "<option>" + Pacific_3 + "</option>" + "<option>" + Pacific_4 + "</option>" + "<option>" + Pacific_5 + "</option>" + "<option>" + Pacific_6 + "</option>" + "</option>" + "<option>" + Pacific_7 + "</option>" + "<option>" + Pacific_8 + "</option>" + "<option>" + Pacific_9 + "</option>" + "<option>" + Pacific_10 + "</option>" + "<option>" + Pacific_11 + "</option>" + "<option>" + Pacific_12 + "</option>" + "<option>" + Pacific_13 + "</option>" + "<option>" + Pacific_14 + "</option>");
        }
        else if (timeZone == "Eastern Time")
        {
            Response.Write("<option>" + select + "</option>" + "<option>" + Eastern_1 + "</option>" + "<option>" + Eastern_2 + "</option>" + "<option>" + Eastern_3 + "</option>" + "<option>" + Eastern_4 + "</option>" + "<option>" + Eastern_5 + "</option>" + "<option>" + Eastern_6 + "</option>");
        }
        else if (timeZone == "Mountain Time")
        {
            Response.Write("<option>" + select + "</option>" + "<option>" + Mountain_1 + "</option>" + "<option>" + Mountain_2 + "</option>" + "<option>" + Mountain_3 + "</option>");
        }
        else
        {
            Response.Write("<option>" + select + "</option>" + "<option>" + Mountain_1 + "</option>" + "<option>" + Mountain_2 + "</option>" + "<option>" + Mountain_3 + "</option>");
        }

        Response.End();
    
        #endregion
    }

}