using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Exigo.WebService;
using Exigo.OData;
using System.Web.Services;
using System.Globalization;

public partial class Dashboard : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        GetDashboardInfo();
        BuildHtml();
    }

    #region settings
    #region Extended Field Settings
    public static int groupID = 2;
    public int CustomerID;
    #endregion
    #endregion

    #region Public Properties
    
    

    List<DashboardObject> objects = new List<DashboardObject>();
    #endregion

    #region Global StringBuilder For HTML
    public StringBuilder s = new StringBuilder();
    #endregion

    public void GetDashboardInfo()
    {
        var context = ExigoApiContext.CreateODataContext();

        CustomerID = Identity.Current.CustomerID;
        try
        {
            var objectQuery = (from c in context.CustomerExtendedDetails
                               where c.CustomerID == CustomerID
                               where c.CustomerExtendedGroupID == groupID
                               select new
                               {
                                   c.CustomerExtendedDetailID,
                                   c.Field20
                               }).FirstOrDefault();

            if (objectQuery != null && objectQuery.Field20 != "")
            {
                //Split the results
                string[] dashboardObjects = objectQuery.Field20.Trim().Split('|');

                foreach (var o in dashboardObjects)
                {
                    string[] ObjOptions = o.Split(',');
                    DashboardObject obj = new DashboardObject(ObjOptions[0], ObjOptions[1], ObjOptions[2]);
                    objects.Add(obj);
                }

                detailid.Value = objectQuery.CustomerExtendedDetailID.ToString();
            }
            else
            {
                //Run default options
                string ob = "";
                List<string> objs = new List<string>();
                int c = 0;
                while (c < 7)
                {
                    switch (c)
                    {
                        case 0: ob = "currentRank,1,1"; break;
                        case 1: ob = "recentChecks,1,2"; break;
                        case 2: ob = "volumes,2,1"; break;
                        case 3: ob = "duesManager,2,2"; break;
                        case 4: ob = "commissionEligibility,3,1"; break;
                        case 5: ob = "recentActivity,3,2"; break;
                        case 6: ob = "news,3,3"; break;
                    }
                    objs.Add(ob);
                    c++;
                }
                string[] dashboardObjects = objs.ToArray();

                foreach (var o in dashboardObjects)
                {
                    string[] ObjOptions = o.Split(',');
                    DashboardObject obj = new DashboardObject(ObjOptions[0], ObjOptions[1], ObjOptions[2]);
                    objects.Add(obj);
                }

                detailid.Value = objectQuery.CustomerExtendedDetailID.ToString();
            }
        }
        catch
        {
        }
    }

    public void BuildHtml()
    {
        DashboardObject lastObject = objects.OrderBy(r => r.column).LastOrDefault();
        int numberColumns = lastObject.column;
        int counter = 1;

        s.AppendLine(string.Format(@"
        <div id='dashboard-{0}column' class='board'>
        ", numberColumns));

        while (counter <= numberColumns)
        {
            s.AppendLine(string.Format(@"
            <div id='column-{0}' class='column'>
                <span class='moveright' style='display: none;'></span>
                <span class='delete' style='display: none;'>X</span>
                <span class='moveleft' style='display: none;'></span>
            ", counter));

            foreach (var o in objects.OrderBy(r => r.column).ThenBy(r => r.row).Where(r => r.column == counter))
            {
                s.AppendLine(string.Format("<div class='dashboardobject-row-{0}'>", o.row));
                s.AppendLine(o.code.ToString());
                s.AppendLine("</div>");
            }

            s.AppendLine("</div>");
            counter++;
        }

        s.AppendLine(string.Format(@"
        </div>
        "));
    }

    public void RenderDashboard()
    {
        HtmlTextWriter writer = new HtmlTextWriter(Response.Output);
        writer.Write(s);
    }

    #region Save Methods
    [WebMethod()]
    public static void SaveDashboard(string cid, string extID, string s)
    {
        var context = ExigoApiContext.CreateWebServiceContext();

        UpdateCustomerExtendedRequest req = new UpdateCustomerExtendedRequest()
        {
            CustomerID = Convert.ToInt32(cid),
            CustomerExtendedID = Convert.ToInt32(extID),
            ExtendedGroupID = groupID,
            Field20 = s
        };
        
        UpdateCustomerExtendedResponse res = context.UpdateCustomerExtended(req);
    }
    #endregion

}

public class DashboardObject
{
    public DashboardObject(string type, string column, string row)
    {
        this.type = type;
        this.row = Convert.ToInt32(row);
        this.column = Convert.ToInt32(column);
        
        switch (type)
        {
            case "news": code = LoadNews(); break;
            case "cNotes": code = LoadCNotes(); break;
            case "dNotes": code = LoadDNotes(); break;
            case "duesManager": code = LoadDuesManager(); break;
            case "recentActivity": code = LoadRecentActivity(); break;
            case "recentChecks": code = LoadRecentChecks(); break;
            case "volumes": code = LoadVolumes(); break;
            case "edownline": code = LoadEDownline(); break;
            case "udownline": code = LoadUDownline(); break;
            case "currentRank": code = LoadCurrentRank(); break;
            case "rankAnalysis": code = LoadRankAnalysis(); break;
            case "achievements": code = LoadAchievements(); break;
            case "commissionEligibility": code = LoadCommissionEligibility(); break;
        }
    }

    public string type { get; set; }
    public int row { get; set; }
    public int column { get; set; }
    public StringBuilder code;

    public StringBuilder LoadNews() //Corporate News
    {
        StringBuilder s = new StringBuilder();

        s.AppendLine(@"
        <div id='dashboard-news' class='draggable sortable'>
            <div class='delete' style='display: none;'>X</div>
            <h4 class='clickable'>Corporate News</h4>
            <div class='contents'></div>
        </div>");

        return s;
    }

    public StringBuilder LoadCNotes() //Corporate Social Feed
    {
        StringBuilder s = new StringBuilder();

        s.AppendLine(@"
        <div id='dashboard-cNotes' class='draggable sortable'>
            <div class='delete' style='display: none;'>X</div>
            <h4>This is my corporate Twitter/Blog/Social</h4>
            <div class='contents'></div>
        </div>");

        return s;
    }

    public StringBuilder LoadDNotes()  //Downline Social Feed
    {
        StringBuilder s = new StringBuilder();

        s.AppendLine(@"
        <div id='dashboard-dNotes' class='draggable sortable'>
            <div class='delete' style='display: none;'>X</div>
            <h4>This is downline Twitter/Blog/Social</h4>
            <div class='contents'></div>
        </div>");

        return s;
    }

    public StringBuilder LoadDuesManager() //Autoship/Dues manager
    {
        StringBuilder s = new StringBuilder();

        s.AppendLine(@"
        <div id='dashboard-duesManager' class='draggable sortable'>
            <div class='delete' style='display: none;'>X</div>
            <h4>Auto Orders</h4>
            <div class='contents'></div>
        </div>");

        return s;
    }

    public StringBuilder LoadRecentActivity()  //Activity Feed
    {
        StringBuilder s = new StringBuilder();

        s.AppendLine(@"
        <div id='dashboard-recentActivity' class='draggable sortable'>
            <div class='delete' style='display: none;'>X</div>
            <h4>Activity in my downline</h4>
            <div class='contents'></div>
        </div>");

        return s;
    }

    public StringBuilder LoadRecentChecks()  //Last Month's commissions checks
    {
        StringBuilder s = new StringBuilder();

        s.AppendLine(@"
        <div id='dashboard-recentChecks' class='draggable sortable'>
            <div class='delete' style='display: none;'>X</div>
            <h4>This Month's Checks</h4>
            <div class='contents'></div>
        </div>");

        return s;
    }

    public StringBuilder LoadVolumes() // Three months volumes
    {
        StringBuilder s = new StringBuilder();

        s.AppendLine(@"
        <div id='dashboard-volumes' class='draggable sortable'>
            <div class='delete' style='display: none;'>X</div>
            <h4>Rank Qualification & Volumes</h4>
            <div class='contents'></div>
        </div>");

        return s;
    }

    public StringBuilder LoadEDownline() //Enroller Tree
    {
        StringBuilder s = new StringBuilder();

        s.AppendLine(@"
        <div id='dashboard-edownline' class='draggable sortable'>
            <div class='delete' style='display: none;'>X</div>
            <h4>This is my downline</h4>
            <div class='contents'></div>
        </div>");

        return s;
    }

    public StringBuilder LoadUDownline() //Unilevel Tree
    {
        StringBuilder s = new StringBuilder();

        s.AppendLine(@"
        <div class='delete' style='display: none;'>X</div>
        <div id='dashboard-udownline' class='draggable sortable'>
            <h4>This is my Unilevel downline</h4>
            <div class='contents'></div>
        </div>");

        return s;
    }

    public StringBuilder LoadCurrentRank()  //Current Qualified Rank
    {
        string month = DateTimeFormatInfo.CurrentInfo.GetMonthName(DateTime.Now.Month);
        StringBuilder s = new StringBuilder();

        s.AppendLine(string.Format(@"
        <div id='dashboard-currentRank' class='draggable sortable'>
            <div class='delete' style='display: none;'>X</div>
            <h4>Current Qualified Rank - {0}</h4>
            <div class='contents'></div>
        </div>", month));

        return s;
    }

    public StringBuilder LoadRankAnalysis() //Rank Scorecard
    {
        StringBuilder s = new StringBuilder();

        s.AppendLine(@"
        <div id='dashboard-rankAnalysis' class='draggable sortable'>
            <div class='delete' style='display: none;'>X</div>
            <h4>This is rank analysis</h4>
            <div class='contents'></div>
        </div>");

        return s;
    }

    public StringBuilder LoadAchievements()  //My Achievements
    {
        StringBuilder s = new StringBuilder();

        s.AppendLine(@"
        <div id='dashboard-achievements' class='draggable sortable'>
            <div class='delete' style='display: none;'>X</div>
            <h4>This is my achievements</h4>
            <div class='contents'></div>
        </div>");

        return s;
    }

    public StringBuilder LoadCommissionEligibility() //Commission Eligibility
    {
        StringBuilder s = new StringBuilder();

        s.AppendLine(@"
        <div id='dashboard-commissionEligibility' class='draggable sortable'>
            <div class='delete' style='display: none;'>X</div>
            <h4>Commission Eligibility</h4>
            <div class='contents'></div>
        </div>
        ");

        return s;
    }
}