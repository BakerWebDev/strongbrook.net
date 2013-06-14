using Exigo.OData;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class DownlineOrders : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    #region Fetching Data
    public List<ReportDataNode> FetchReportData()
    {
        // Create our query
        var query = ExigoApiServicesToo.CreateODataContext().CustomerData
            .Where(c => c.EnrollerID == Identity.Current.CustomerID);

        // Apply ordering and filtering
        var helper = new GridReportHelper();
        query = helper.ApplyFiltering<Exigo.CustomOData.CustomerData>(query);
        query = helper.ApplyOrdering<Exigo.CustomOData.CustomerData>(query);

        // Fetch the nodes
        var nodes = query.Select(c => new ReportDataNode
            {
                CustomerID      = c.CustomerID,
                FirstName       = c.FirstName,
                LastName        = c.LastName,
                Company         = c.Company,
                CustomerType    = c.CustomerType,
                CustomerStatus  = c.CustomerStatus,
                OrderID         = c.OrderID,
                OrderDate       = c.OrderDate,
                BV              = c.CommissionableVolume,            
                Total           = c.Total            
            }).Skip((helper.Page - 1) * helper.RecordCount).Take(helper.RecordCount).ToList();

        // Return the nodes
        return nodes;
    }
    #endregion

    #region Render
    protected override void Render(HtmlTextWriter writer)
    {
        if(Request.QueryString["action"] != null)
        {
            switch(Request.QueryString["action"])
            {
                case "fetch":
                    // Fetch the nodes
                    var nodes = FetchReportData();
                    
                    // Assemble the records
                    var html = new StringBuilder();

                    // First, add our record count
                    html.AppendFormat("{0}^", nodes.Count());
                    foreach(var record in nodes)
                    {
                        // Assemble our html
                        html.AppendFormat("<tr>");
                        #region Customer ID
                        html.AppendFormat(@"
                                <td>
                                    <span class='id'><a href='Profile.aspx?id={0}' target='_blank' title='View profile'>{0}</a></span>
                                </td>
                        ", record.CustomerID);
                        #endregion Customer ID
                        #region Name
                        html.AppendFormat(@"
                                <td class='customerdetails'>
                                    <a href='Profile.aspx?id={0}' title='View profile' target='_blank'>
                                        <img src='{1}' class='avatar' /></a>
                                    <span class='name'><a href='Profile.aspx?id={0}' target='_blank' title='View profile'>{2}</a></span>
                                    <span class='title'>{3} {4}</span>
                                </td>", 
                            record.CustomerID,
                            GlobalUtilities.GetCustomerTinyAvatarUrl(record.CustomerID),
                            GlobalUtilities.Coalesce(record.Company, record.FirstName + " " + record.LastName),
                            record.CustomerStatus,
                            record.CustomerType);
                        #endregion Name
                        #region Order ID
                        html.AppendFormat(@"
                                <td>
                                    <span class='id'>{0}</span>
                                </td>
                        ", record.OrderID);
                        #endregion Order ID
                        #region Order Date
                        html.AppendFormat("<td>{0:M/d/yyyy}</td>", record.OrderDate);
                        #endregion Order Date
                        #region Order Commissionable Volume
                        html.AppendFormat("<td>{0:N0}</td>", record.BV);
                        #endregion Order Commissionable Volume
                        #region Order Total
                        html.AppendFormat("<td>{0:C}</td>", record.Total);
                        #endregion Order Total
                        #region Actions Column
                        html.AppendFormat(@"
                                <td>
                                    <div class='btn-group pull-right'>
                                        <a href='CreateMessage.aspx?to={0}' class='btn'><i class='icon-envelope'></i></a>
                                        <a href='javascript:;' class='btn dropdown-toggle' data-toggle='dropdown'>
                                            <span class='caret'></span>
                                        </a>
                                        <ul class='dropdown-menu pull-right'>
                                            <li><a href='CreateMessage.aspx?to={0}'><i class='icon-envelope'></i>&nbsp;Send Message</a></li>
                                            <li><a href='Profile.aspx?id={1}'>View profile</a></li>
                                            <li><a href='OrderHistory.aspx?id={1}'>View order history</a></li>
                                        </ul>
                                    </div>
                                </td>
                            ", record.Email,
                             record.CustomerID);
                        #endregion Actions Column
                        html.AppendFormat("</tr>");
                    }

                    Response.Clear();
                    writer.Write(html.ToString());
                    Response.End();
                    break;

                default: 
                    base.Render(writer);
                    break;
            }
        }
        else 
        {
            base.Render(writer);
        }
    }
    #endregion

    #region Models
    public class ReportDataNode
    {
        public int CustomerID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Company { get; set; }
        public string DisplayName
        {
            get
            {
                return GlobalUtilities.Coalesce(this.Company, this.FirstName + " " + this.LastName);
            }
        }
        public string Email { get; set; }
        public string CustomerType { get; set; }
        public string CustomerStatus { get; set; }
        public int OrderID { get; set; }
        public string OrderStatus { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal Total { get; set; }
        public decimal BV { get; set; }
    }
    #endregion
}