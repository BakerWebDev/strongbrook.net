using Exigo.OData;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class OD : System.Web.UI.Page
{
    #region Properties
    public string IBD_CustomerCountClass
    {
        get
        {
            string visibility = "";

            if (IBD_CustomerCount > 0) { visibility = "visible"; } else visibility = "hidden";

            return visibility;
        }
    }
    public string Retail_CustomerCountClass
    {
        get
        {
            string visibility = "";

            if (Retail_CustomerCount > 0) { visibility = "visible"; } else visibility = "hidden";

            return visibility;
        }
    }
    public string Preferred_CustomerCountClass
    {
        get
        {
            string visibility = "";

            if (Preferred_CustomerCount > 0) { visibility = "visible"; } else visibility = "hidden";

            return visibility;
        }
    }
    public string Wholesale_CustomerCountClass
    {
        get
        {
            string visibility = "";

            if (Wholesale_CustomerCount > 0) { visibility = "visible"; } else visibility = "hidden";

            return visibility;
        }
    }
    public string WaitListIBD_CustomerCountClass
    {
        get
        {
            string visibility = "";

            if (WaitListIBD_CustomerCount > 0) { visibility = "visible"; } else visibility = "hidden";

            return visibility;
        }
    }
    public string ReferralAffiliateLead_CustomerCountClass
    {
        get
        {
            string visibility = "";

            if (ReferralAffiliateLead_CustomerCount > 0) { visibility = "visible"; } else visibility = "hidden";

            return visibility;
        }
    }
    public string RestrictedVideoAccess_CustomerCountClass
    {
        get
        {
            string visibility = "";

            if (RestrictedVideoAccess_CustomerCount > 0) { visibility = "visible"; } else visibility = "hidden";

            return visibility;
        }
    }
    public string ReferralAffiliate_CustomerCountClass
    {
        get
        {
            string visibility = "";

            if (ReferralAffiliate_CustomerCount > 0) { visibility = "visible"; } else visibility = "hidden";

            return visibility;
        }
    }
    public string PFCEventRegistration_CustomerCountClass
    {
        get
        {
            string visibility = "";

            if (PFCEventRegistration_CustomerCount > 0) { visibility = "visible"; } else visibility = "hidden";

            return visibility;
        }
    }
    public string AxiosCustomer_CustomerCountClass
    {
        get
        {
            string visibility = "";

            if (AxiosCustomer_CustomerCount > 0) { visibility = "visible"; } else visibility = "hidden";

            return visibility;
        }
    }
    public string GPRLead_CustomerCountClass
    {
        get
        {
            string visibility = "";

            if (GPRLead_CustomerCount > 0) { visibility = "visible"; } else visibility = "hidden";

            return visibility;
        }
    }

    #endregion Properties

    #region Fetching Data
    public List<ReportDataNode> FetchReportData()
    {
        // Create our query
        var query = ExigoApiContext.CreateODataContext().UniLevelTreePeriodVolumes
            .Where(c => c.TopCustomerID == Identity.Current.CustomerID)
            .Where(c => c.PeriodTypeID == PeriodTypes.Default)
            .Where(c => c.Period.IsCurrentPeriod);
        

        // Apply ordering and filtering
        var helper = new GridReportHelper();
        query = helper.ApplyFiltering<UniLevelNodePeriodVolume>(query);
        query = helper.ApplyOrdering<UniLevelNodePeriodVolume>(query);


        // Fetch the nodes
        var nodes = query.Select(c => new ReportDataNode
            {
                CustomerID             = c.CustomerID,
                Level                  = c.Level,
                FirstName              = c.Customer.FirstName,
                LastName               = c.Customer.LastName,
                Company                = c.Customer.Company,
                Email                  = c.Customer.Email,
                Phone                  = c.Customer.Phone,
                Phone2                 = c.Customer.Phone2,
                CustomerType           = c.Customer.CustomerType.CustomerTypeDescription,
                CustomerStatus         = c.Customer.CustomerStatus.CustomerStatusDescription,
                CustomerRank           = c.Customer.Rank.RankDescription,
                JoinedDate             = c.Customer.CreatedDate,
                Volume1                 = c.PeriodVolume.Volume1, //Current Month PCV
                Volume2                 = c.PeriodVolume.Volume3, //Current Month OCV
                Volume3                 = c.PeriodVolume.Volume75, //3 Month PCV
                Volume4                 = c.PeriodVolume.Volume79, //3 Month OCV
                Volume5                 = c.PeriodVolume.Volume56 //Home Transaction Credits
            }).Skip((helper.Page - 1) * helper.RecordCount).Take(helper.RecordCount).ToList();


        // Return the nodes
        return nodes;
    }


    public int IBD_CustomerCount
    {
        get
        {
        var context = ExigoApiContext.CreateODataContext();

        var recordCount = (from c in context.UniLevelTreePeriodVolumes
                           where c.TopCustomerID == Identity.Current.CustomerID
                           where c.PeriodTypeID == PeriodTypes.Default
                           where c.Period.IsCurrentPeriod == true
                           where c.Customer.CustomerType.CustomerTypeDescription == "IBD"
 
                           select new
                           {
                               c.CustomerID
                           }).Count();

        return recordCount;
        }
    }
    public int Retail_CustomerCount
    {
        get
        {
        var context = ExigoApiContext.CreateODataContext();

        var recordCount = (from c in context.UniLevelTreePeriodVolumes
                           where c.TopCustomerID == Identity.Current.CustomerID
                           where c.PeriodTypeID == PeriodTypes.Default
                           where c.Period.IsCurrentPeriod == true
                           where c.Customer.CustomerType.CustomerTypeDescription == "Retail Customer"
 
                           select new
                           {
                               c.CustomerID
                           }).Count();

        return recordCount;
        }
    }
    public int Preferred_CustomerCount
    {
        get
        {
        var context = ExigoApiContext.CreateODataContext();

        var recordCount = (from c in context.UniLevelTreePeriodVolumes
                           where c.TopCustomerID == Identity.Current.CustomerID
                           where c.PeriodTypeID == PeriodTypes.Default
                           where c.Period.IsCurrentPeriod == true
                           where c.Customer.CustomerType.CustomerTypeDescription == "Preferred Customer"
 
                           select new
                           {
                               c.CustomerID
                           }).Count();

        return recordCount;
        }
    }
    public int Wholesale_CustomerCount
    {
        get
        {
        var context = ExigoApiContext.CreateODataContext();

        var recordCount = (from c in context.UniLevelTreePeriodVolumes
                           where c.TopCustomerID == Identity.Current.CustomerID
                           where c.PeriodTypeID == PeriodTypes.Default
                           where c.Period.IsCurrentPeriod == true
                           where c.Customer.CustomerType.CustomerTypeDescription == "Wholesale Customer"
 
                           select new
                           {
                               c.CustomerID
                           }).Count();

        return recordCount;
        }
    }
    public int WaitListIBD_CustomerCount
    {
        get
        {
        var context = ExigoApiContext.CreateODataContext();

        var recordCount = (from c in context.UniLevelTreePeriodVolumes
                           where c.TopCustomerID == Identity.Current.CustomerID
                           where c.PeriodTypeID == PeriodTypes.Default
                           where c.Period.IsCurrentPeriod == true
                           where c.Customer.CustomerType.CustomerTypeDescription == "Wait List IBD"
 
                           select new
                           {
                               c.CustomerID
                           }).Count();

        return recordCount;
        }
    }
    public int ReferralAffiliateLead_CustomerCount
    {
        get
        {
        var context = ExigoApiContext.CreateODataContext();

        var recordCount = (from c in context.UniLevelTreePeriodVolumes
                           where c.TopCustomerID == Identity.Current.CustomerID
                           where c.PeriodTypeID == PeriodTypes.Default
                           where c.Period.IsCurrentPeriod == true
                           where c.Customer.CustomerType.CustomerTypeDescription == "Referral Affiliate Lead"
 
                           select new
                           {
                               c.CustomerID
                           }).Count();

        return recordCount;
        }
    }
    public int RestrictedVideoAccess_CustomerCount
    {
        get
        {
        var context = ExigoApiContext.CreateODataContext();

        var recordCount = (from c in context.UniLevelTreePeriodVolumes
                           where c.TopCustomerID == Identity.Current.CustomerID
                           where c.PeriodTypeID == PeriodTypes.Default
                           where c.Period.IsCurrentPeriod == true
                           where c.Customer.CustomerType.CustomerTypeDescription == "Restricted Video Access"
 
                           select new
                           {
                               c.CustomerID
                           }).Count();

        return recordCount;
        }
    }
    public int ReferralAffiliate_CustomerCount
    {
        get
        {
        var context = ExigoApiContext.CreateODataContext();

        var recordCount = (from c in context.UniLevelTreePeriodVolumes
                           where c.TopCustomerID == Identity.Current.CustomerID
                           where c.PeriodTypeID == PeriodTypes.Default
                           where c.Period.IsCurrentPeriod == true
                           where c.Customer.CustomerType.CustomerTypeDescription == "Referral Affiliate"
 
                           select new
                           {
                               c.CustomerID
                           }).Count();

        return recordCount;
        }
    }
    public int PFCEventRegistration_CustomerCount
    {
        get
        {
        var context = ExigoApiContext.CreateODataContext();

        var recordCount = (from c in context.UniLevelTreePeriodVolumes
                           where c.TopCustomerID == Identity.Current.CustomerID
                           where c.PeriodTypeID == PeriodTypes.Default
                           where c.Period.IsCurrentPeriod == true
                           where c.Customer.CustomerType.CustomerTypeDescription == "PFC Event Registration"
 
                           select new
                           {
                               c.CustomerID
                           }).Count();

        return recordCount;
        }
    }
    public int AxiosCustomer_CustomerCount
    {
        get
        {
        var context = ExigoApiContext.CreateODataContext();

        var recordCount = (from c in context.UniLevelTreePeriodVolumes
                           where c.TopCustomerID == Identity.Current.CustomerID
                           where c.PeriodTypeID == PeriodTypes.Default
                           where c.Period.IsCurrentPeriod == true
                           where c.Customer.CustomerType.CustomerTypeDescription == "Axios Customer" 
                           select new
                           {
                               c.CustomerID
                           }).Count();

        return recordCount;
        }
    }
    public int GPRLead_CustomerCount
    {
        get
        {
        var context = ExigoApiContext.CreateODataContext();

        var recordCount = (from c in context.UniLevelTreePeriodVolumes
                           where c.TopCustomerID == Identity.Current.CustomerID
                           where c.PeriodTypeID == PeriodTypes.Default
                           where c.Period.IsCurrentPeriod == true
                           where c.Customer.CustomerType.CustomerTypeDescription == "GPR Lead"
 
                           select new
                           {
                               c.CustomerID
                           }).Count();

        return recordCount;
        }
    }
    public int Total_CustomerCount
    {
        get
        {
        var context = ExigoApiContext.CreateODataContext();

        var recordCount = (from c in context.UniLevelTreePeriodVolumes
                           where c.TopCustomerID == Identity.Current.CustomerID
                           where c.PeriodTypeID == PeriodTypes.Default
                           where c.Period.IsCurrentPeriod == true 
                           select new
                           {
                               c.CustomerID
                           }).Count();

        return recordCount;
        }
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
                        html.AppendFormat("<td>{0}</td>", record.NestedLevel);

                        html.AppendFormat(@"<td class='customerdetails'>
                                                <a href='Profile.aspx?id={0}' title='View profile'>{0}</a><br />
                                                <span class='title'>{1}</span>
                                            </td>", record.CustomerID,
                                                  record.CustomerRank);


                        html.AppendFormat(@"
                                <td class='customerdetails'>
                                    <a href='Profile.aspx?id={0}' title='View profile'>
                                        <img src='{1}' class='avatar' /></a>
                                    <span class='name'><a href='Profile.aspx?id={0}' title='View profile'>{2}</a></span>
                                    <span class='title'>{3} {4}</span>
                                </td>", 
                            record.CustomerID,
                            GlobalUtilities.GetCustomerTinyAvatarUrl(record.CustomerID),
                            GlobalUtilities.Coalesce(record.Company, record.FirstName + " " + record.LastName),
                            record.CustomerStatus,
                            record.CustomerType);






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

    public void Render_CustomerCount()
    {
        HtmlTextWriter writer = new HtmlTextWriter(Response.Output);
        StringBuilder html = new StringBuilder();

        // Render the data
        html.AppendLine(string.Format(@"
            <div class ='countDetails'>
                <h3>Counts by Customer Type</h3>
                <table>
                    <tr>"));
        html.AppendLine(string.Format(@"
                    <th class=""{0}"">IBD</th>
                    <th class=""{1}"">Retail</th>
                    <th class=""{2}"">Preferred</th>
                    <th class=""{3}"">Wholesale</th>
                    <th class=""{4}"">Wait List IBD</th>
                    <th class=""{5}"">Referral Affiliate Lead</th>
                    <th class=""{6}"">Restricted Video Access</th>
                    <th class=""{7}"">Referral Affiliate</th>
                    <th class=""{8}"">PFC Event Registration</th>
                    <th class=""{9}"">Axios Customer</th>
                    <th class=""{10}"">GPR Lead</th>
                    <th class=""{0}"">Total</th>
                </tr>
                <tr>
                    <td class=""{0}"">{11}</td>
                    <td class=""{1}"">{12}</td>
                    <td class=""{2}"">{13}</td>
                    <td class=""{3}"">{14}</td>
                    <td class=""{4}"">{15}</td>
                    <td class=""{5}"">{16}</td>
                    <td class=""{6}"">{17}</td>
                    <td class=""{7}"">{18}</td>
                    <td class=""{8}"">{19}</td>
                    <td class=""{9}"">{20}</td>
                    <td class=""{10}"">{21}</td>
                    <td class=""{0}"">{22}</td>
                "
                 , IBD_CustomerCountClass
                 , Retail_CustomerCountClass
                 , Preferred_CustomerCountClass
                 , Wholesale_CustomerCountClass
                 , WaitListIBD_CustomerCountClass
                 , ReferralAffiliateLead_CustomerCountClass
                 , RestrictedVideoAccess_CustomerCountClass
                 , ReferralAffiliate_CustomerCountClass
                 , PFCEventRegistration_CustomerCountClass
                 , AxiosCustomer_CustomerCountClass
                 , GPRLead_CustomerCountClass
            
                 , IBD_CustomerCount
                 , Retail_CustomerCount
                 , Preferred_CustomerCount
                 , Wholesale_CustomerCount
                 , WaitListIBD_CustomerCount
                 , ReferralAffiliateLead_CustomerCount
                 , RestrictedVideoAccess_CustomerCount
                 , ReferralAffiliate_CustomerCount
                 , PFCEventRegistration_CustomerCount
                 , AxiosCustomer_CustomerCount
                 , GPRLead_CustomerCount
                 , Total_CustomerCount
            )
        );            
        html.AppendLine(string.Format(@"   
                    </tr>
                </table>
            </div>"));
 
        writer.Write(html.ToString());
    }

    #endregion

    #region Models
    public class ReportDataNode
    {
        public int CustomerID { get; set; }
        public int Level { get; set; }
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
        public string Phone { get; set; }
        public string Phone2 { get; set; }
        public string CustomerType { get; set; }
        public string CustomerStatus { get; set; }
        public string CustomerRank { get; set; }
        public DateTime JoinedDate { get; set; }
        public decimal Volume1 { get; set; }
        public decimal Volume2 { get; set; }
        public decimal Volume3 { get; set; }
        public decimal Volume4 { get; set; }
        public decimal Volume5 { get; set; }

        public string NestedLevel
        {
            get
            {
                var result = "";
                for(var x = 0; x < this.Level; x++)
                {
                    result += ".";
                }
                return result + this.Level.ToString();
            }
        }
    }
    #endregion
}

