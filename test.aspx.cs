using Exigo.OData;
using Exigo.WebService;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class test : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        // These are just words.  I need to figure out how to display these words in a modal.
        ApplicationErrors.ErrorMessage += "This is a test of the ApplicationErrors.ErrorMessage.";
    }

    #region Render
    protected override void Render(HtmlTextWriter writer)
    {
        var html = new StringBuilder();

        //html.AppendFormat(ApplicationErrors.ErrorMessage += "This is a test of the ApplicationErrors.ErrorMessage.");
        

        writer.Write(html.ToString());
    }
    #endregion
}