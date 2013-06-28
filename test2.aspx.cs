using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Exigo.OData;
using Exigo.WebService;

public partial class test2 : Page, IPostBackEventHandler
{
    #region Page Load
    protected void Page_Load(object sender, EventArgs e)
    {

        
    }
    #endregion

    public string ErrorString { get; set; }







    #region Forgot Password Handler
    public void Click_ForgotPassword(object sender, EventArgs e)
    {
        RaisePostBackEvent("ForgotYourPass");
    }

    public void RaisePostBackEvent(string eventArgument)
    {
        switch (eventArgument)
        {
            case "ForgotYourPass": ErrorString = "ErrorString 3"; break;

            case "UseCard":
                try
                {
                    throw new ApplicationException();
                }
                catch(Exception ex)
                {
                    ErrorString += ex.Message;
                }
                
                break;

            default: ErrorString = "ErrorString 4"; break;
        }
    }

    public string ForgotPasswordUsername
    {
        get { return txtForgotPassUsername.Text; }
        set { txtForgotPassUsername.Text = value; }
    }

    public void ForgotPass(object sender, EventArgs e)  //Method to RESET login password and email temporary login to Email Address on file
    {
        try
        {
            ErrorString = "ErrorString 1";
        }
        catch
        {
            ErrorString = "ErrorString 2";
        }
    }
    #endregion Forgot Password Handler





}