﻿using System;
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

        string f7Ato730A        = "7:00 AM to 7:30 AM";
        string f730Ato8A        = "7:30 AM to 8:00 AM";
        string f8Ato830A        = "8:00 AM to 8:30 AM";
        string f830Ato9A        = "8:30 AM to 9:00 AM";
        string f9Ato930A        = "9:00 AM to 9:30 AM";
        string f930Ato10AM      = "9:30 AM to 10:00 AM";
        string f10Ato1030A      = "10:00 AM to 10:30 AM";
        string f1030Ato11A      = "10:30 AM to 11:00 AM";
        string f11Ato1130A      = "11:00 AM to 11:30 AM";
        string f1130Ato12P      = "11:30 AM to 12:00 PM";
        string f12Pto1230P      = "12:00 PM to 12:30 PM";
        string f1230Pto1P       = "12:30 PM to 1:00 PM";
        string f1Pto130P        = "1:00 PM to 1:30 PM";
        string f130Pto2P       = "1:30 PM to 2:00 PM";
        string f2Pto230P        = "2:00 PM to 2:30 PM";
        string f230Pto3P        = "2:30 PM to 3:00 PM";
        string f3Pto330P        = "3:00 PM to 3:30 PM";
        string f330Pto4P        = "3:30 PM to 4:00 PM";
        string f4Pto430P        = "4:00 PM to 4:30 PM";
        string f430Pto5P        = "4:30 PM to 5:00 PM";
        string f5Pto530P        = "5:00 PM to 5:30 PM";
        string f530Pto6P       = "5:30 PM to 6:00 PM";
        string f6Pto630P        = "6:00 PM to 6:30 PM";
        string f630Pto7P        = "6:30 PM to 7:00 PM";

        //string from_700AM_to_730AM      = "7:00 AM to 7:30 AM";
        //string from_730AM_to_800AM      = "7:30 AM to 8:00 AM";
        //string from_800AM_to_830AM      = "8:00 AM to 8:30 AM";
        //string from_830AM_to_900AM      = "8:30 AM to 9:00 AM";
        //string from_900AM_to_930AM      = "9:00 AM to 9:30 AM";
        //string from_930AM_to_1000AM     = "9:30 AM to 10:00 AM";
        //string from_1000AM_to_1030AM    = "10:00 AM to 10:30 AM";
        //string from_1030AM_to_1100AM    = "10:30 AM to 11:00 AM";
        //string from_1100AM_to_1130AM    = "11:00 AM to 11:30 AM";
        //string from_1130AM_to_1200PM    = "11:30 AM to 12:00 PM";
        //string from_1200PM_to_1230PM    = "12:00 PM to 12:30 PM";
        //string from_1230AM_to_100PM     = "12:30 PM to 1:00 PM";
        //string from_100PM_to_130PM      = "1:00 PM to 1:30 PM";
        //string from_130PM_to_200PM      = "1:30 PM to 2:00 PM";
        //string from_200PM_to_230PM      = "2:00 PM to 2:30 PM";
        //string from_230PM_to_300PM      = "2:30 PM to 3:00 PM";
        //string from_300PM_to_330PM      = "3:00 PM to 3:30 PM";
        //string from_330PM_to_400PM      = "3:30 PM to 4:00 PM";
        //string from_400PM_to_430PM      = "4:00 PM to 4:30 PM";
        //string from_430PM_to_500PM      = "4:30 PM to 5:00 PM";
        //string from_500PM_to_530PM      = "5:00 PM to 5:30 PM";
        //string from_530PM_to_600PM      = "5:30 PM to 6:00 PM";
        //string from_600PM_to_630PM      = "6:00 PM to 6:30 PM";
        //string from_630PM_to_700PM      = "6:30 PM to 7:00 PM";

        string closedSunday     = "Closed Sunday";
        string closedSaturday   = "Closed Saturday";

        string oMin = "</option>" + "<option>" + "from" + " ";

        switch (timeZone)
        { 
            case "Hawaii Time":
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
            case "Pacific Time":
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
        }

        Response.End();
    
        #endregion
    }

}