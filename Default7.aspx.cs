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

        string from_700AM_to_730AM      = "7:00 AM to 7:30 AM";
        string from_730AM_to_800AM      = "7:30 AM to 8:00 AM";
        string from_800AM_to_830AM      = "8:00 AM to 8:30 AM";
        string from_830AM_to_900AM      = "8:30 AM to 9:00 AM";
        string from_900AM_to_930AM      = "9:00 AM to 9:30 AM";
        string from_930AM_to_1000AM     = "9:30 AM to 10:00 AM";
        string from_1000AM_to_1030AM    = "10:00 AM to 10:30 AM";
        string from_1030AM_to_1100AM    = "10:30 AM to 11:00 AM";
        string from_1100AM_to_1130AM    = "11:00 AM to 11:30 AM";
        string from_1130AM_to_1200PM    = "11:30 AM to 12:00 PM";
        string from_1200PM_to_1230PM    = "12:00 PM to 12:30 PM";
        string from_1230AM_to_100PM     = "12:30 PM to 1:00 PM";
        string from_100PM_to_130PM      = "1:00 PM to 1:30 PM";
        string from_130PM_to_200PM      = "1:30 PM to 2:00 PM";
        string from_200PM_to_230PM      = "2:00 PM to 2:30 PM";
        string from_230PM_to_300PM      = "2:30 PM to 3:00 PM";
        string from_300PM_to_330PM      = "3:00 PM to 3:30 PM";
        string from_330PM_to_400PM      = "3:30 PM to 4:00 PM";
        string from_400PM_to_430PM      = "4:00 PM to 4:30 PM";
        string from_430PM_to_500PM      = "4:30 PM to 5:00 PM";
        string from_500PM_to_530PM      = "5:00 PM to 5:30 PM";
        string from_530PM_to_600PM      = "5:30 PM to 6:00 PM";
        string from_600PM_to_630PM      = "6:00 PM to 6:30 PM";
        string from_630PM_to_700PM      = "6:30 PM to 7:00 PM";

        string closedSunday     = "Closed Sunday";
        string closedSaturday   = "Closed Saturday";

        //string Hawaii_1 = "7 to 8" + " " + Request.Form["timeZone"];
        //string Hawaii_2 = "11 to 12" + " " + Request.Form["timeZone"];
        //string Hawaii_3 = "12 to 1" + " " + Request.Form["timeZone"];
        //string Hawaii_6 = "3 to 4" + " " + Request.Form["timeZone"];

        //string Pacific_1 = "9:00 AM to 9:30 AM"     + " " + Request.Form["timeZone"];
        //string Pacific_2 = "9:30 AM to 10:00 AM"    + " " + Request.Form["timeZone"];
        //string Pacific_3 = "10:00 AM to 10:30 AM"   + " " + Request.Form["timeZone"];
        //string Pacific_4 = "10:30 AM to 11:00 AM"   + " " + Request.Form["timeZone"];
        //string Pacific_5 = "11:00 AM to 11:30 AM"   + " " + Request.Form["timeZone"];
        //string Pacific_6 = "11:30 AM to 12:00 PM"   + " " + Request.Form["timeZone"];
        //string Pacific_7 = "12:00 PM to 12:30 PM"   + " " + Request.Form["timeZone"];
        //string Pacific_8 = "12:30 PM to 1:00 PM"    + " " + Request.Form["timeZone"];
        //string Pacific_9 = "1:00 PM to 1:30 PM"     + " " + Request.Form["timeZone"];
        //string Pacific_10 = "1:30 PM to 2:00 PM"    + " " + Request.Form["timeZone"];
        //string Pacific_11 = "2:00 PM to 2:30 PM"    + " " + Request.Form["timeZone"];
        //string Pacific_12 = "2:30 PM to 3:00 PM"    + " " + Request.Form["timeZone"];
        //string Pacific_13 = "3:00 PM to 3:30 PM"    + " " + Request.Form["timeZone"];
        //string Pacific_14 = "3:30 PM to 4:00 PM"    + " " + Request.Form["timeZone"];
        //string Pacific_15 = "4:00 PM to 4:30 PM"    + " " + Request.Form["timeZone"];
        //string Pacific_16 = "4:30 PM to 5:00 PM"    + " " + Request.Form["timeZone"];

        //string Mountain_1 = "10 to 11" + " " + Request.Form["timeZone"];
        //string Mountain_2 = "11 to 12" + " " + Request.Form["timeZone"];
        //string Mountain_3 = "12 to 1" + " " + Request.Form["timeZone"];

        //string Eastern_1 = "12 to 1" + " " + Request.Form["timeZone"];
        //string Eastern_2 = "1 to 2" + " " + Request.Form["timeZone"];
        //string Eastern_3 = "2 to 3" + " " + Request.Form["timeZone"];
        //string Eastern_4 = "3 to 4" + " " + Request.Form["timeZone"];
        //string Eastern_5 = "4 to 5" + " " + Request.Form["timeZone"];
        //string Eastern_6 = "5 to 6" + " " + Request.Form["timeZone"];


        switch (timeZone)
        { 
            case "Hawaii Time":
                switch (timeFrameSelection)
                {
                    case "Sunday":
                        Response.Write("<option>" + select + "</option>" + "<option>" + Hawaii_1 + "</option>" + "<option>" + Hawaii_2 + "</option>" + "<option>" + Hawaii_3 + "</option>");
                        break;
                    case "Monday":
                        Response.Write("<option>" + select + "</option>" + "<option>" + Hawaii_1 + "</option>" + "<option>" + Hawaii_2 + "</option>" + "<option>" + Hawaii_3 + "</option>");
                        break;
                    case "Tuesday":
                        Response.Write("<option>" + select + "</option>" + "<option>" + Hawaii_6 + "</option>" + "<option>" + Hawaii_6 + "</option>" + "<option>" + Hawaii_6 + "</option>");
                        break;
                }
            break;
        }

        Response.End();
    
        #endregion
    }

}