using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Exigo.OData;
using Exigo.WebService;
using System.Web.Script.Serialization;

public partial class test3 : Page
{
    #region Page Load
    protected void Page_Load(object sender, EventArgs e)
    {
        string json = "This string is from the server side code.";
        Response.ClearHeaders();
        Response.ClearContent();
        Response.Clear();
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.ContentType = "application/json";
        Response.ContentEncoding = Encoding.UTF8;
        Response.Write(json);
        Response.End();
    }
    #endregion


}