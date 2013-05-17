using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Security.Cryptography;
using System.IO;

public partial class GamePlanSubmissionThankYou : System.Web.UI.Page
{
    #region Page Load
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    #endregion

    #region Render
    protected void ShowThisMessage()
    {
        HtmlTextWriter writer = new HtmlTextWriter(Response.Output);
        StringBuilder s = new StringBuilder();

        s.AppendLine(string.Format(@"
             <div class=""panelarea panel"" id=""reciept"" style=""position: absolute; width: 100%;"">
                 <h1 class=""heading"">Thank You</h1>
                 <table class=""grid"" id=""grid2"">
                     <tbody>
                         <tr class=""gridrow first last"">
                             <td class=""col0 gridcell first"">
                                 <div class=""notes panel"">
                                    <p>Congratulations on taking your first step towards receiving your one-on-one custom Game Plan Report. This Game Plan Report will outline your financial options for moving you closer to achieving your retirement goals and dreams.</p>
                                                                         <p>A Game Plan Counselor will attempt to call you at your requested appointment time, or within the next 2 business days. Your Game Plan Counselor will spend a few minutes asking questions to generate your custom Game Plan Report.</p> 
                                    <p>Check your email for confirmation of your Game Plan request.                                       <p>We look forward to showing you options that will help create, manage, protect and grow your wealth.                                      <p>                                         Successfully,                                         <br />                                         Strongbrook Team
                                    <p>
                                 </div>
                             </td>
                             <td class=""col1 gridcell last"">
                                 <div class=""reminder panel"" id=""reminder""></div>
                             </td>
                         </tr>
                     </tbody>
                 </table>
             </div>"
        ));

        writer.Write(s.ToString());
    }
    #endregion

    #region Exigo Service request
    //ExigoApiServices Api = new ExigoApiServices();
    #endregion

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