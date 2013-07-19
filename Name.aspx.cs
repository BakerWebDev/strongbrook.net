using Exigo.WebService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Name : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        var customerID = Convert.ToInt32(Request.QueryString["id"]);

        #region Fetch Data via OData
        //var context = ExigoApiContext.CreateODataContext();

        //var query = (from c in context.Customers
        //     .Where(c => c.CustomerID == customerID)
	 
        //     select new
        //     {
        //        c.CustomerID,
        //        c.FirstName,
        //        c.LastName,
        //        c.Phone
        //     }).FirstOrDefault();

        //if(query.CustomerID != null)
        //{
        //    FullName    = query.FirstName + " " + query.LastName;
        //    PhoneNumber = query.Phone;
        //}
        #endregion Fetch Data via OData

        #region Fetch Data via Webservice
        var data = ExigoApiContext.CreateWebServiceContext().GetCustomerSite(new GetCustomerSiteRequest
        {
            CustomerID = customerID
        });

        if(data.FirstName != "" && data.LastName != "")
        {
            FullName    = data.FirstName + " " + data.LastName;
            PhoneNumber = data.Phone;
        }
        else if(data.Company != "")
        {
            FullName    = data.Company;
            PhoneNumber = data.Phone;
        }
        else
        {
            var query = ExigoApiContext.CreateWebServiceContext().GetCustomers(new GetCustomersRequest
            {
                CustomerID = customerID
            }).Customers[0];
            if(query.FirstName != "" && query.LastName != "")
            {
                FullName    = query.FirstName + " " + query.LastName;
                PhoneNumber = query.Phone;
            }
            else if(query.Company != "")
            {
                FullName    = query.Company;
                PhoneNumber = query.Phone;
            }
            else
            {
                FullName    = "";
                PhoneNumber = "";
            }
        }
        #endregion Fetch Data via Webservice
    }

    public string FullName;
    public string PhoneNumber { get; set; }
}