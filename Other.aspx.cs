using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Security.Cryptography;
using System.IO;

public partial class Other : System.Web.UI.Page
{
    #region Page Load
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    #endregion

    public string Link()
    {
        string foo = "GPR_FORM_FOR_TESTING.aspx" + "?id=22434"; // +Request.QueryString["id"];
        return foo;
    }

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
            ShowMessage.Value = "True";
        }
    }
    private string _message;

    private void ClearMessage()
    {
        Message = string.Empty;
        ShowMessage.Value = "";
    }

    #endregion
}