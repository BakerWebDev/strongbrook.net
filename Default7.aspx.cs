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

        string select = "Select a Time";

        string f5Ato530A = "5:00 AM to 5:30 AM";
        string f530Ato6A = "5:30 AM to 6:00 AM";
        string f6Ato630A = "6:00 AM to 6:30 AM";
        string f630Ato7A = "6:30 AM to 7:00 AM";
        string f7Ato730A = "7:00 AM to 7:30 AM";
        string f730Ato8A = "7:30 AM to 8:00 AM";
        string f8Ato830A = "8:00 AM to 8:30 AM";
        string f830Ato9A = "8:30 AM to 9:00 AM";
        string f9Ato930A = "9:00 AM to 9:30 AM";
        string f930Ato10AM = "9:30 AM to 10:00 AM";
        string f10Ato1030A = "10:00 AM to 10:30 AM";
        string f1030Ato11A = "10:30 AM to 11:00 AM";
        string f11Ato1130A = "11:00 AM to 11:30 AM";
        string f1130Ato12P = "11:30 AM to 12:00 PM";
        string f12Pto1230P = "12:00 PM to 12:30 PM";
        string f1230Pto1P = "12:30 PM to 1:00 PM";
        string f1Pto130P = "1:00 PM to 1:30 PM";
        string f130Pto2P = "1:30 PM to 2:00 PM";
        string f2Pto230P = "2:00 PM to 2:30 PM";
        string f230Pto3P = "2:30 PM to 3:00 PM";
        string f3Pto330P = "3:00 PM to 3:30 PM";
        string f330Pto4P = "3:30 PM to 4:00 PM";
        string f4Pto430P = "4:00 PM to 4:30 PM";
        string f430Pto5P = "4:30 PM to 5:00 PM";
        string f5Pto530P = "5:00 PM to 5:30 PM";
        string f530Pto6P = "5:30 PM to 6:00 PM";
        string f6Pto630P = "6:00 PM to 6:30 PM";
        string f630Pto7P = "6:30 PM to 7:00 PM";
        string anyTime = "Any Time on this day";

        string closedSunday     = "Closed Sunday";
        string closedSaturday   = "Closed Saturday";

        string oMin = "</option>" + " " + "<option>";

        #region Hawaii Hours
        string from_6AM_to_1PM = "<option>" + "Select a Time" + oMin + "from 10:00AM to 10:30AM" + oMin + "from 10:30AM to 11:00AM" + oMin + "from 11:00AM to 11:30AM" + oMin + "from 11:30AM to 12:00PM" + oMin + "from 12:00PM to 12:30PM" + oMin + "from 12:30PM to 1:00PM" + oMin + "from 1:00PM to 1:30PM" + oMin + "from 1:30PM to 2:00PM" + oMin + "from 2:00PM to 2:30PM" + oMin + "from 2:30PM to 3:00PM" + oMin + "from 3:00PM to 3:30PM" + oMin + "from 3:30PM to 4:00PM" + oMin + "from 4:00PM to 4:30PM" + oMin + "from 4:30PM to 5:00PM" + "</option>";
        string from_6AM_to_8AM = "<option>" + "Select a Time" + oMin + "from 10:00AM to 10:30AM" + oMin + "from 10:30AM to 11:00AM" + oMin + "from 11:00AM to 11:30AM" + oMin + "from 11:30AM to 12:00PM" + "</option>";
        string from_noon_to_4PM = "<option>" + "from 4:00PM to 4:30PM" + oMin + "from 4:30PM to 5:00PM" + oMin + "from 5:00PM to 5:30PM" + oMin + "from 5:30PM to 6:00PM" + oMin + "from 6:00PM to 6:30PM" + oMin + "from 6:30PM to 7:00PM" + oMin + "from 7:00PM to 7:30PM" + oMin + "from 7:30PM to 8:00PM" + oMin;
        #endregion Hawaii Hours

        #region Pacific Hours
        string from_9AM_to_4PM = "<option>" + "Select a Time" + oMin + "from 9:00AM to 9:30AM" + oMin + "from 9:30AM to 10:00AM" + oMin + "from 10:00AM to 10:30AM" + oMin + "from 10:30AM to 11:00AM" + oMin + "from 11:00AM to 11:30AM" + oMin + "from 11:30AM to 12:00PM" + oMin + "from 12:00PM to 12:30PM" + oMin + "from 12:30PM to 1:00PM" + oMin + "from 1:00PM to 1:30PM" + oMin + "from 1:30PM to 2:00PM" + oMin + "from 2:00PM to 2:30PM" + oMin + "from 2:30PM to 3:00PM" + oMin + "from 3:00PM to 3:30PM" + oMin + "from 3:30PM to 4:00PM" + "</option>";
        string from_9AM_to_11AM = "<option>" + "Select a Time" + oMin + "from 10:00AM to 10:30AM" + oMin + "from 10:30AM to 11:00AM" + oMin + "from 11:00AM to 11:30AM" + oMin + "from 11:30AM to 12:00PM" + "</option>";
        string from_3PM_to_7PM = "<option>" + "from 4:00PM to 4:30PM" + oMin + "from 4:30PM to 5:00PM" + oMin + "from 5:00PM to 5:30PM" + oMin + "from 5:30PM to 6:00PM" + oMin + "from 6:00PM to 6:30PM" + oMin + "from 6:30PM to 7:00PM" + oMin + "from 7:00PM to 7:30PM" + oMin + "from 7:30PM to 8:00PM" + oMin;
        #endregion Pacific Hours

        #region Mountain Hours
        string from_10AM_to_5PM = "<option>" + "Select a Time" + oMin + "from 10:00AM to 10:30AM" + oMin + "from 10:30AM to 11:00AM" + oMin + "from 11:00AM to 11:30AM" + oMin + "from 11:30AM to 12:00PM" + oMin + "from 12:00PM to 12:30PM" + oMin + "from 12:30PM to 1:00PM" + oMin + "from 1:00PM to 1:30PM" + oMin + "from 1:30PM to 2:00PM" + oMin + "from 2:00PM to 2:30PM" + oMin + "from 2:30PM to 3:00PM" + oMin + "from 3:00PM to 3:30PM" + oMin + "from 3:30PM to 4:00PM" + oMin + "from 4:00PM to 4:30PM" + oMin + "from 4:30PM to 5:00PM" + "</option>";
        string from_10AM_to_noon = "<option>" + "Select a Time" + oMin + "from 10:00AM to 10:30AM" + oMin + "from 10:30AM to 11:00AM" + oMin + "from 11:00AM to 11:30AM" + oMin + "from 11:30AM to 12:00PM" + "</option>";
        string from_4PM_to_8PM = "<option>" + "from 4:00PM to 4:30PM" + oMin + "from 4:30PM to 5:00PM" + oMin + "from 5:00PM to 5:30PM" + oMin + "from 5:30PM to 6:00PM" + oMin + "from 6:00PM to 6:30PM" + oMin + "from 6:30PM to 7:00PM" + oMin + "from 7:00PM to 7:30PM" + oMin + "from 7:30PM to 8:00PM" + oMin;
        string from_10AM_to_1PM = "<option>" + "Select a Time" + oMin + "from 10:00AM to 10:30AM" + oMin + "from 10:30AM to 11:00AM" + oMin + "from 11:00AM to 11:30AM" + oMin + "from 11:30AM to 12:00PM" + oMin + "from 12:00AM to 12:30AM" + oMin + "from 12:30AM to 1:00PM" + "</option>";
        #endregion Mountain Hours

        #region Central Hours
        string from_11AM_to_6PM = "<option>" + "Select a Time" + oMin + "from 10:00AM to 10:30AM" + oMin + "from 10:30AM to 11:00AM" + oMin + "from 11:00AM to 11:30AM" + oMin + "from 11:30AM to 12:00PM" + oMin + "from 12:00PM to 12:30PM" + oMin + "from 12:30PM to 1:00PM" + oMin + "from 1:00PM to 1:30PM" + oMin + "from 1:30PM to 2:00PM" + oMin + "from 2:00PM to 2:30PM" + oMin + "from 2:30PM to 3:00PM" + oMin + "from 3:00PM to 3:30PM" + oMin + "from 3:30PM to 4:00PM" + oMin + "from 4:00PM to 4:30PM" + oMin + "from 4:30PM to 5:00PM" + "</option>";
        string from_11AM_to_1PM = "<option>" + "Select a Time" + oMin + "from 10:00AM to 10:30AM" + oMin + "from 10:30AM to 11:00AM" + oMin + "from 11:00AM to 11:30AM" + oMin + "from 11:30AM to 12:00PM" + "</option>";
        string from_5PM_to_9PM = "<option>" + "from 4:00PM to 4:30PM" + oMin + "from 4:30PM to 5:00PM" + oMin + "from 5:00PM to 5:30PM" + oMin + "from 5:30PM to 6:00PM" + oMin + "from 6:00PM to 6:30PM" + oMin + "from 6:30PM to 7:00PM" + oMin + "from 7:00PM to 7:30PM" + oMin + "from 7:30PM to 8:00PM" + oMin;
        #endregion Central Hours

        #region Eastern Hours
        string from_12PM_to_7PM = "<option>" + "Select a Time" + oMin + "from 10:00AM to 10:30AM" + oMin + "from 10:30AM to 11:00AM" + oMin + "from 11:00AM to 11:30AM" + oMin + "from 11:30AM to 12:00PM" + oMin + "from 12:00PM to 12:30PM" + oMin + "from 12:30PM to 1:00PM" + oMin + "from 1:00PM to 1:30PM" + oMin + "from 1:30PM to 2:00PM" + oMin + "from 2:00PM to 2:30PM" + oMin + "from 2:30PM to 3:00PM" + oMin + "from 3:00PM to 3:30PM" + oMin + "from 3:30PM to 4:00PM" + oMin + "from 4:00PM to 4:30PM" + oMin + "from 4:30PM to 5:00PM" + "</option>";
        string from_12PM_to_2PM = "<option>" + "Select a Time" + oMin + "from 10:00AM to 10:30AM" + oMin + "from 10:30AM to 11:00AM" + oMin + "from 11:00AM to 11:30AM" + oMin + "from 11:30AM to 12:00PM" + "</option>";
        string from_6PM_to_10PM = "<option>" + "from 4:00PM to 4:30PM" + oMin + "from 4:30PM to 5:00PM" + oMin + "from 5:00PM to 5:30PM" + oMin + "from 5:30PM to 6:00PM" + oMin + "from 6:00PM to 6:30PM" + oMin + "from 6:30PM to 7:00PM" + oMin + "from 7:00PM to 7:30PM" + oMin + "from 7:30PM to 8:00PM" + oMin;
        #endregion Eastern Hours


        //string from_10AM_to_noon = "<option>" + "Select a Time" + oMin + "from 10:00AM to 10:30AM" + oMin + "from 10:30AM to 11:00AM" + oMin + "from 11:00AM to 11:30AM" + oMin + "from 11:30AM to 12:00PM" + "</option>";
        string from_noon_to_5PM = "<option>" + "from 12:00PM to 12:30PM" + oMin + "from 12:30PM to 1:00PM" + oMin + "from 1:00PM to 1:30PM" + oMin + "from 1:30PM to 2:00PM" + oMin + "from 2:00PM to 2:30PM" + oMin + "from 2:30PM to 3:00PM" + oMin + "from 3:00PM to 3:30PM" + oMin + "from 3:30PM to 4:00PM" + oMin + "from 4:00PM to 4:30PM" + oMin + "from 4:30PM to 5:00PM" + "</option>";        
        
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
                        Response.Write("<option>" + select + oMin + f5Ato530A + oMin + f530Ato6A + oMin + f6Ato630A + oMin + f630Ato7A + oMin + f7Ato730A + oMin + f730Ato8A + oMin + f8Ato830A + oMin + f830Ato9A + oMin + f9Ato930A + oMin + f930Ato10AM + oMin + f10Ato1030A + oMin + f1030Ato11A + oMin + f11Ato1130A + oMin + f1130Ato12P + oMin + f12Pto1230P + oMin + f1230Pto1P + oMinNoFrom + anyTime + " " + "before 1:00 PM" + "</option>");
                        break;                                                                                                                                                                                                                                                                                                                                                          
                    case "Tuesday":
                        Response.Write("<option>" + select + oMin + f5Ato530A + oMin + f530Ato6A + oMin + f6Ato630A + oMin + f630Ato7A + oMin + f7Ato730A + oMin + f730Ato8A + oMin + f8Ato830A + oMin + f830Ato9A + oMin + f9Ato930A + oMin + f930Ato10AM + oMin + f10Ato1030A + oMin + f1030Ato11A + oMin + f11Ato1130A + oMin + f1130Ato12P + oMin + f12Pto1230P + oMin + f1230Pto1P + oMinNoFrom + anyTime + " " + "before 1:00 PM" + "</option>");
                        break;                                                                                                                                                                                                                                                                                                                                                          
                    case "Wednesday":
                        Response.Write("<option>" + select + oMin + f5Ato530A + oMin + f530Ato6A + oMin + f6Ato630A + oMin + f630Ato7A + oMin + f7Ato730A + oMin + f730Ato8A + oMin + f8Ato830A + oMin + f830Ato9A + oMin + f9Ato930A + oMin + f930Ato10AM + oMin + f10Ato1030A + oMin + f1030Ato11A + oMin + f11Ato1130A + oMin + f1130Ato12P + oMin + f12Pto1230P + oMin + f1230Pto1P + oMinNoFrom + anyTime + " " + "before 1:00 PM" + "</option>");
                        break;                                                                                                                                                                                                                                                                                                                                                          
                    case "Thursday":
                        Response.Write("<option>" + select + oMin + f5Ato530A + oMin + f530Ato6A + oMin + f6Ato630A + oMin + f630Ato7A + oMin + f7Ato730A + oMin + f730Ato8A + oMin + f8Ato830A + oMin + f830Ato9A + oMin + f9Ato930A + oMin + f930Ato10AM + oMin + f10Ato1030A + oMin + f1030Ato11A + oMin + f11Ato1130A + oMin + f1130Ato12P + oMin + f12Pto1230P + oMin + f1230Pto1P + oMinNoFrom + anyTime + " " + "before 1:00 PM" + "</option>");
                        break;                                                                                                                                                                                                                                                                                                                                                          
                    case "Friday":
                        Response.Write("<option>" + select + oMin + f5Ato530A + oMin + f530Ato6A + oMin + f6Ato630A + oMin + f630Ato7A + oMin + f7Ato730A + oMin + f730Ato8A + oMin + f8Ato830A + oMin + f830Ato9A + oMin + f9Ato930A + oMin + f930Ato10AM + oMin + f10Ato1030A + oMin + f1030Ato11A + oMin + f11Ato1130A + oMin + f1130Ato12P + oMin + f12Pto1230P + oMin + f1230Pto1P + oMinNoFrom + anyTime + " " + "before 1:00 PM" + "</option>");
                        break;
                    case "Saturday":
                        Response.Write("<option>" + closedSaturday + "</option>");
                        break;
                }
            break;
            #endregion Hawaii Times

            #region Pacific Times
            case "Pacific Time":
            switch (timeFrameSelection)
            {
                case "Sunday":
                    Response.Write(from_9AM_to_4PM);
                    break;
                case "Monday":
                    Response.Write(from_9AM_to_4PM);
                    break;
                case "Tuesday":
                    Response.Write(from_9AM_to_4PM);
                    break;
                case "Wednesday":
                    Response.Write(from_9AM_to_4PM);
                    break;
                case "Thursday":
                    Response.Write(from_9AM_to_4PM);
                    break;
                case "Friday":
                    Response.Write(from_9AM_to_4PM);
                    break;
                case "Saturday":
                    Response.Write(from_9AM_to_4PM);
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
                    Response.Write("<option>" + select + oMin + f7Ato730A + oMin + f730Ato8A + oMin + f8Ato830A + oMin + f830Ato9A + oMin + f9Ato930A + oMin + f930Ato10AM + oMin + f10Ato1030A + oMin + f1030Ato11A + oMin + f11Ato1130A + oMin + f1130Ato12P + oMin + f12Pto1230P + oMin + f1230Pto1P + oMin + f1Pto130P + oMin + f130Pto2P + oMin + f2Pto230P + oMin + f230Pto3P + oMin + f3Pto330P + oMin + f330Pto4P + oMin + f4Pto430P + oMin + f430Pto5P + oMin + f5Pto530P + oMin + f530Pto6P + oMin + f6Pto630P + oMin + f630Pto7P + "</option>");
                    break;
                case "Tuesday":
                    Response.Write("<option>" + select + oMin + f7Ato730A + oMin + f730Ato8A + oMin + f8Ato830A + oMin + f830Ato9A + oMin + f9Ato930A + oMin + f930Ato10AM + oMin + f10Ato1030A + oMin + f1030Ato11A + oMin + f11Ato1130A + oMin + f1130Ato12P + oMin + f12Pto1230P + oMin + f1230Pto1P + oMin + f1Pto130P + oMin + f130Pto2P + oMin + f2Pto230P + oMin + f230Pto3P + oMin + f3Pto330P + oMin + f330Pto4P + oMin + f4Pto430P + oMin + f430Pto5P + oMin + f5Pto530P + oMin + f530Pto6P + oMin + f6Pto630P + oMin + f630Pto7P + "</option>");
                    break;
                case "Wednesday":
                    Response.Write("<option>" + select + oMin + f7Ato730A + oMin + f730Ato8A + oMin + f8Ato830A + oMin + f830Ato9A + oMin + f9Ato930A + oMin + f930Ato10AM + oMin + f10Ato1030A + oMin + f1030Ato11A + oMin + f11Ato1130A + oMin + f1130Ato12P + oMin + f12Pto1230P + oMin + f1230Pto1P + oMin + f1Pto130P + oMin + f130Pto2P + oMin + f2Pto230P + oMin + f230Pto3P + oMin + f3Pto330P + oMin + f330Pto4P + oMin + f4Pto430P + oMin + f430Pto5P + oMin + f5Pto530P + oMin + f530Pto6P + oMin + f6Pto630P + oMin + f630Pto7P + "</option>");
                    break;
                case "Thursday":
                    Response.Write("<option>" + select + oMin + f7Ato730A + oMin + f730Ato8A + oMin + f8Ato830A + oMin + f830Ato9A + oMin + f9Ato930A + oMin + f930Ato10AM + oMin + f10Ato1030A + oMin + f1030Ato11A + oMin + f11Ato1130A + oMin + f1130Ato12P + oMin + f12Pto1230P + oMin + f1230Pto1P + oMin + f1Pto130P + oMin + f130Pto2P + oMin + f2Pto230P + oMin + f230Pto3P + oMin + f3Pto330P + oMin + f330Pto4P + oMin + f4Pto430P + oMin + f430Pto5P + oMin + f5Pto530P + oMin + f530Pto6P + oMin + f6Pto630P + oMin + f630Pto7P + "</option>");
                    break;
                case "Friday":
                    Response.Write("<option>" + select + oMin + f7Ato730A + oMin + f730Ato8A + oMin + f8Ato830A + oMin + f830Ato9A + oMin + f9Ato930A + oMin + f930Ato10AM + oMin + f10Ato1030A + oMin + f1030Ato11A + oMin + f11Ato1130A + oMin + f1130Ato12P + oMin + f12Pto1230P + oMin + f1230Pto1P + oMin + f1Pto130P + oMin + f130Pto2P + oMin + f2Pto230P + oMin + f230Pto3P + oMin + f3Pto330P + oMin + f330Pto4P + oMin + f4Pto430P + oMin + f430Pto5P + oMin + f5Pto530P + oMin + f530Pto6P + oMin + f6Pto630P + oMin + f630Pto7P + "</option>");
                    break;
                case "Saturday":
                    Response.Write("<option>" + closedSaturday + "</option>");
                    break;
            }
            break;
            #endregion Pacific Times

            #region Eastern Times
            case "Eastern Time":
            switch (timeFrameSelection)
            {
                case "Sunday":
                    Response.Write("<option>" + select + oMin + f7Ato730A + oMin + f730Ato8A + oMin + f8Ato830A + oMin + f830Ato9A + oMin + f9Ato930A + oMin + f930Ato10AM + oMin + f10Ato1030A + oMin + f1030Ato11A + oMin + f11Ato1130A + oMin + f1130Ato12P + oMin + f12Pto1230P + oMin + f1230Pto1P + oMin + f1Pto130P + oMin + f130Pto2P + oMin + f2Pto230P + oMin + f230Pto3P + oMin + f3Pto330P + oMin + f330Pto4P + oMin + f4Pto430P + oMin + f430Pto5P + oMin + f5Pto530P + oMin + f530Pto6P + oMin + f6Pto630P + oMin + f630Pto7P + "</option>");
                    break;
                case "Monday":
                    Response.Write("<option>" + select + oMin + f7Ato730A + oMin + f730Ato8A + oMin + f8Ato830A + oMin + f830Ato9A + oMin + f9Ato930A + oMin + f930Ato10AM + oMin + f10Ato1030A + oMin + f1030Ato11A + oMin + f11Ato1130A + oMin + f1130Ato12P + oMin + f12Pto1230P + oMin + f1230Pto1P + oMin + f1Pto130P + oMin + f130Pto2P + oMin + f2Pto230P + oMin + f230Pto3P + oMin + f3Pto330P + oMin + f330Pto4P + oMin + f4Pto430P + oMin + f430Pto5P + oMin + f5Pto530P + oMin + f530Pto6P + oMin + f6Pto630P + oMin + f630Pto7P + "</option>");
                    break;
                case "Tuesday":
                    Response.Write("<option>" + select + oMin + f7Ato730A + oMin + f730Ato8A + oMin + f8Ato830A + oMin + f830Ato9A + oMin + f9Ato930A + oMin + f930Ato10AM + oMin + f10Ato1030A + oMin + f1030Ato11A + oMin + f11Ato1130A + oMin + f1130Ato12P + oMin + f12Pto1230P + oMin + f1230Pto1P + oMin + f1Pto130P + oMin + f130Pto2P + oMin + f2Pto230P + oMin + f230Pto3P + oMin + f3Pto330P + oMin + f330Pto4P + oMin + f4Pto430P + oMin + f430Pto5P + oMin + f5Pto530P + oMin + f530Pto6P + oMin + f6Pto630P + oMin + f630Pto7P + "</option>");
                    break;
                case "Wednesday":
                    Response.Write("<option>" + select + oMin + f7Ato730A + oMin + f730Ato8A + oMin + f8Ato830A + oMin + f830Ato9A + oMin + f9Ato930A + oMin + f930Ato10AM + oMin + f10Ato1030A + oMin + f1030Ato11A + oMin + f11Ato1130A + oMin + f1130Ato12P + oMin + f12Pto1230P + oMin + f1230Pto1P + oMin + f1Pto130P + oMin + f130Pto2P + oMin + f2Pto230P + oMin + f230Pto3P + oMin + f3Pto330P + oMin + f330Pto4P + oMin + f4Pto430P + oMin + f430Pto5P + oMin + f5Pto530P + oMin + f530Pto6P + oMin + f6Pto630P + oMin + f630Pto7P + "</option>");
                    break;
                case "Thursday":
                    Response.Write("<option>" + select + oMin + f7Ato730A + oMin + f730Ato8A + oMin + f8Ato830A + oMin + f830Ato9A + oMin + f9Ato930A + oMin + f930Ato10AM + oMin + f10Ato1030A + oMin + f1030Ato11A + oMin + f11Ato1130A + oMin + f1130Ato12P + oMin + f12Pto1230P + oMin + f1230Pto1P + oMin + f1Pto130P + oMin + f130Pto2P + oMin + f2Pto230P + oMin + f230Pto3P + oMin + f3Pto330P + oMin + f330Pto4P + oMin + f4Pto430P + oMin + f430Pto5P + oMin + f5Pto530P + oMin + f530Pto6P + oMin + f6Pto630P + oMin + f630Pto7P + "</option>");
                    break;
                case "Friday":
                    Response.Write("<option>" + select + oMin + f7Ato730A + oMin + f730Ato8A + oMin + f8Ato830A + oMin + f830Ato9A + oMin + f9Ato930A + oMin + f930Ato10AM + oMin + f10Ato1030A + oMin + f1030Ato11A + oMin + f11Ato1130A + oMin + f1130Ato12P + oMin + f12Pto1230P + oMin + f1230Pto1P + oMin + f1Pto130P + oMin + f130Pto2P + oMin + f2Pto230P + oMin + f230Pto3P + oMin + f3Pto330P + oMin + f330Pto4P + oMin + f4Pto430P + oMin + f430Pto5P + oMin + f5Pto530P + oMin + f530Pto6P + oMin + f6Pto630P + oMin + f630Pto7P + "</option>");
                    break;
                case "Saturday":
                    Response.Write("<option>" + select + oMin + f7Ato730A + oMin + f730Ato8A + oMin + f8Ato830A + oMin + f830Ato9A + oMin + f9Ato930A + oMin + f930Ato10AM + oMin + f10Ato1030A + oMin + f1030Ato11A + oMin + f11Ato1130A + oMin + f1130Ato12P + oMin + f12Pto1230P + oMin + f1230Pto1P + oMin + f1Pto130P + oMin + f130Pto2P + oMin + f2Pto230P + oMin + f230Pto3P + oMin + f3Pto330P + oMin + f330Pto4P + oMin + f4Pto430P + oMin + f430Pto5P + oMin + f5Pto530P + oMin + f530Pto6P + oMin + f6Pto630P + oMin + f630Pto7P + "</option>");
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