using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Security.Cryptography;
using System.IO;

public partial class gameplan : System.Web.UI.Page
{
    #region Page Load
    protected void Page_Load(object sender, EventArgs e)
    {
        //if (!IsPostBack && Request.IsLocal)
        //{
        //    Response.Redirect("GamePlanSubmissionForm.aspx" + "?id=" + "adb");
        //}
    }
    #endregion
}