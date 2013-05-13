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
        string select = "Select a Time";

        string Hawaii_1 = "10 to 11" + " " + Request.Form["timeZone"] + "Time";
        string Hawaii_2 = "11 to 12" + " " + Request.Form["timeZone"] + "Time";
        string Hawaii_3 = "12 to 1" + " " + Request.Form["timeZone"] + "Time";

        string Mountain_1 = "10 to 11" + " " + Request.Form["timeZone"] + "Time";
        string Mountain_2 = "11 to 12" + " " + Request.Form["timeZone"] + "Time";
        string Mountain_3 = "12 to 1" + " " + Request.Form["timeZone"] + "Time";


        if (timeZone == "Hawaii")
        {
            Response.Write("<option>" + select + "</option>" + "<option>" + Hawaii_1 + "</option>" + "<option>" + Hawaii_2 + "</option>" + "<option>" + Hawaii_3 + "</option>");
        }
        else
        {
            Response.Write("<option>" + select + "</option>" + "<option>" + Mountain_1 + "</option>" + "<option>" + Mountain_2 + "</option>" + "<option>" + Mountain_3 + "</option>");           
        }

        

        Response.End();
        #endregion
    }

}