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

        #region Properties
        string timeZone = Request.Form["timeZone"];
        string timeFrameSelection = Request.Form["timeFrame"];

        string oMin = "</option>" + " " + "<option>";

        string closedSunday     = "Closed Sunday";

        #region Hawaii Hour
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
}