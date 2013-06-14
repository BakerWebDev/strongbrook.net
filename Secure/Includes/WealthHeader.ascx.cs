using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Secure_Includes_WealthHeader : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        FullName = Identity.Current.FirstName + " " + Identity.Current.LastName;
    }

    public string FullName;

    public void Click_Logout(Object sender, EventArgs e)
    {
        //
    }
}