using Exigo.OData;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Sandbox2 : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    public void ArrayTest1()
    {
        var nodes = FetchReportData();

        #region Turn the list into an array, and get the average of those numbers.
        decimal[] listNumbersConverted = list.ToArray();

        decimal[] numbers = { listNumbersConverted[0] }; 
  
        decimal averageNum = numbers.Average();
        #endregion Turn the list into an array, and get the average of those numbers.

        var html = new StringBuilder();

        #region Render the count of nodes.
        html.AppendFormat(@"
            Count: {0}
            <br /><br />
            "
            , nodes.Count()
            );
        #endregion
        #region Render the number of GPR's for each person in UniLeval downline.
        foreach (var number in list)
        {
            html.AppendFormat(@"
                {0}
                "
                , number
                );
        }
        #endregion Render the number of GPR's for each person in UniLeval downline.
        #region Render the Average number of GPR's for UniLevel downline.
        html.AppendFormat(@"<br />
            The Average Number is: {0}
            <br /><br />
            "
            , averageNum.ToString()
            );
        #endregion Render the Average number of GPR's for UniLevel downline.

        var writer = new HtmlTextWriter(Response.Output);
        writer.Write(html.ToString());
    }

    




    // CHANGE THIS -----------------------------\/
    public int TheCustomerIDToUseForThisTest = Identity.Current.CustomerID ;
    // CHANGE THIS -----------------------------\/
    public int ThePriodType     = PeriodTypes.Monthly ;


    #region Fetch GPR Data


    #region top stuff
    public List<decimal> list = new List<decimal>();
    public List<decimal> list2 = new List<decimal>();
    public List<decimal> MonthlyList = new List<decimal>();

    public List<ReportDataNode> FetchReportData()
    {
        #region Query the OData tables
        var query = ExigoApiContext.CreateODataContext().UniLevelTreePeriodVolumes
            .Where(c => c.TopCustomerID == TheCustomerIDToUseForThisTest)
            .Where(c => c.PeriodTypeID == ThePriodType)
            .Where(c => c.Period.IsCurrentPeriod);
            //.Where(c => c.PeriodVolume.Volume83 != 0);
        #endregion Query the OData tables

        #region Fetch the nodes
        var nodes = query.Select(c => new ReportDataNode
        {
            CustomerID = c.CustomerID,
            VolumeBucket83 = c.PeriodVolume.Volume83, // GPR Credits Monthly
        }).ToList();
        #endregion Fetch the nodes

        #region Add values to the Monthly GPR List
        foreach (var customer in nodes)
        {
        #endregion Add values to the Monthly GPR List
        #endregion top stuff

            // CHANGE THIS ------------------------------------------\/
            decimal theValueOfTheVolumeBucket = customer.VolumeBucket83;
            decimal custID = customer.CustomerID; 

        #region other stuff
            list.Add(theValueOfTheVolumeBucket);
            //list.Add(custID);
        }
        

        // Return the nodes
        return nodes;





        #endregion other stuff


    }


        #endregion Fetch GPR Data

    #region Render GPR Data









    public void Render_UniLevelDownlineGPR_Count()
    {
        var html = new StringBuilder();

        #region Render the count of nodes.
        var nodes = FetchReportData();

        html.AppendFormat(@"
            {0}
            "
            , nodes.Count()
            );
        #endregion

        var writer = new HtmlTextWriter(Response.Output);
        writer.Write(html.ToString());    
    }

    public void Render_UniLevelDownlineGPR_CountPerPerson()
    {
        var html = new StringBuilder();

        #region Render the number of GPR's for each person in UniLeval downline.
        var nodes = FetchReportData();

        foreach (var person in nodes)
        {
            html.AppendFormat(@"
                {0}: {1}<br />
                "
                , person.CustomerID
                , person.VolumeBucket83.ToString("N")
                );
        }
        #endregion Render the number of GPR's for each person in UniLeval downline.

        var writer = new HtmlTextWriter(Response.Output);
        writer.Write(html.ToString());    
    }

    public void Render_UniLevelDownlineGPR_Average()
    {
        var html = new StringBuilder();

        #region Render the Average number of GPR's for UniLevel downline.
        #region Turn the list into an array, and get the average of those numbers.
        decimal[] listNumbersConverted = list.ToArray();

        decimal[] numbers = { listNumbersConverted[0] };



        // Add the credits for each qualified person together.
        decimal sumOfGPRs = 0;
        foreach (decimal x in listNumbersConverted)
        {
            sumOfGPRs = x + sumOfGPRs++;
        }

        // Get the total number of qualified downline customers.
        decimal peopleInMyDownline = listNumbersConverted.Count();
        decimal GPRsCreatedByPeopleInMyDownlind = sumOfGPRs;

        // Divide the total number of organizational credits by the number of customers in the organization.
        decimal averageNum = GPRsCreatedByPeopleInMyDownlind / peopleInMyDownline; // sumOfGPRs / count;


        #endregion Turn the list into an array, and get the average of those numbers.

        html.AppendFormat(@"<br />
            {0}
            "
            , averageNum.ToString("N")
            );
        #endregion Render the Average number of GPR's for UniLevel downline.

        var writer = new HtmlTextWriter(Response.Output);
        writer.Write(html.ToString());    
    }








    #endregion  Render GPR Data

    public class ReportDataNode
    {
        public int CustomerID { get; set; }
        public decimal VolumeBucket82 { get; set; }
        public decimal VolumeBucket83 { get; set; }
        public decimal VolumeBucket98 { get; set; }
        public decimal VolumeBucket99 { get; set; }
        public decimal VolumeBucket100 { get; set; }
    }
        
}