using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using Exigo.OData;
using Exigo.WebService;


public partial class Secure_LibraryItems : System.Web.UI.Page
{
    #region Page Load
    protected void Page_Load(object sender, EventArgs e)
    {
        ClearMessage();

        //asign public properties with values
        FirstName = Identity.Current.FirstName;
        MyIBDEnrollSite = "http://strongbrookenroll.secure-backoffice.net/default.aspx?ID=" + Identity.Current.CustomerID;
        MyCustomerEnrollSite = "http://strongbrookshop.secure-backoffice.net/default.aspx?ID=" + Identity.Current.CustomerID;
    }
    #endregion Page Load

    #region Public Properties
    public string FirstName;
    public string BackOfficeURL = "http://strongbrookoffice.com";
    public string BackOfficeURLNew = "Default.aspx";
    public string MyIBDEnrollSite;
    public string MyCustomerEnrollSite;
    public string OpenInSeparateWindow = "target='_blank'";
    public string ShoppingCart = "P1_ShoppingProductList.aspx";

    public string item1 = "10050";
    public string item2 = "10060";
    public string item3 = "10070";
    public string item4 = "10080";
    public string item5 = "10090";

    public string IMG_1 = "Assets/Images/Library/forms.jpg";
    public string IMG_2 = "Assets/Images/VideoTraining/btnAddToCart.gif";
    public string IMG_3 = "Assets/Images/VideoTraining/btnAddToCart.gif";
    public string IMG_4 = "Assets/Images/VideoTraining/btnAddToCart.gif";
    public string IMG_5 = "Assets/Images/VideoTraining/btnAddToCart.gif";
    public string IMG_6 = "Assets/Images/VideoTraining/btnAddToCart.gif";
    public string IMG_7 = "Assets/Documents/Library/IBDResources/strongbrook-flyer.jpg"; // Strongbrook Event Flyer (blank)


    public string PDF_1 = "NewsRoomNew.aspx";
    public string PDF_2 = "http://christinegraham.wix.com/trainingcallarchive";

    public string PDF_3 = "Assets/Documents/Library/Policies/IBD APPLICATION and AGREEMENT FINAL 4-1-12.pdf?download=true";
    public string PDF_4 = "Assets/Documents/Library/Policies/IBD Policies and Procedures - FINAL August 1 2012.pdf?download=true";
    public string PDF_5 = "Assets/Documents/Library/Policies/TERMS AND CONDITIONS FINAL 9-01-12.pdf?download=true";
    public string PDF_6 = "Assets/Documents/Library/Policies/Privacy Policy FINAL 9-01-2012.pdf?download=true";
    public string PDF_7 = "Assets/Documents/Library/Policies/REFUND POLICY FINAL - APPROVED FOR USE 2-15-12.pdf?download=true";

    public string PDF_8 = "Assets/Documents/Library/CompensationPlan/Detailed Compensation Plan January 2013.pdf?download=true";
    public string PDF_9 = "Assets/Documents/Library/CompensationPlan/Compensation Plan Overview Chart.pdf?download=true";
    public string PDF_10 = "Assets/Documents/Library/CompensationPlan/Per Product Commission Chart.pdf?download=true";
    public string PDF_11 = "Assets/Documents/Library/CompensationPlan/Glossary of Terms FINAL 8-01-12.pdf?download=true";
    public string PDF_12 = "Assets/Documents/Library/CompensationPlan/CommissionSchedule.pdf?download=true";
    public string PDF_17 = "Assets/Documents/Library/CompensationPlan/IncomeDisclosureStatement.pdf?download=true";                  // Income Disclosure Statement

    public string PDF_13 = "PerformancePDFs.aspx";

    public string PDF_14 = "Assets/Documents/Library/IBDResources/Strongbrook_IBD_FAQs_3.pdf?download=true";
    public string PDF_15 = "Assets/Documents/Library/IBDResources/Game Plan Request Card.pdf?download=true";
    public string PDF_16 = "Assets/Documents/Library/IBDResources/Client PSA Enrollment Form.pdf?download=true";
    public string PDF_18 = "Assets/Documents/Library/IBDResources/PFC Program Vesting Qualifications.pdf?download=true";
    public string PDF_19 = "Assets/Documents/Library/IBDResources/IBD Enrollment Form.pdf?download=true";
    public string PDF_20 = "Assets/Documents/Library/IBDResources/Realtor-Program_BW.pdf?download=true";
    public string PDF_21 = "Assets/Documents/Library/IBDResources/strongbrook_3-way-call_script.pdf?download=true";
    public string PDF_22 = "Assets/Documents/Library/IBDResources/Engineering_Strongbrook_Income_COLOR.pdf?download=true";           // Engineering My Strongbrook Income
    public string PDF_23 = "Assets/Documents/Library/IBDResources/GamePlanExpectations_Script.pdf?download=true";                    // Game Plan Expectations_Script
    public string PDF_24 = "Assets/Documents/Library/IBDResources/MakingMoneyWithGamePlanRequests_rev1.pdf?download=true";           // Making Money with Game Plan Requests
    

    public string PPT_1 = "http://strongbrook.com/common/_media/powerpoint/IBD-training/training-pfc.zip"; // Training: PFC


    
    public string Link_1 = "http://strongbrook.com/common/_media/powerpoint/IBD-training/IBD-BASIC-TRAINING.zip";
    public string Link_2 = "http://strongbrook.com/common/_media/powerpoint/IBD-training/REAL-ESTATE-FULLFILLMENT-TRAINING.zip";
    public string Link_3 = "http://strongbrook.com/common/_media/powerpoint/IBD-training/IBD-INTERVIEW-TOP-30-NAMES.zip";
    public string Link_4 = "http://strongbrook.com/common/_media/powerpoint/IBD-training/3-WAY-CALL-BASICS.zip";

    public string Link_5 = "http://strongbrook.com/common/_media/powerpoint/IBD-training/PRODUCT-TRAINING.zip";
    public string Link_6 = "http://strongbrook.com/common/_media/powerpoint/IBD-training/IBD-SUCCESS-FORMULAS.zip";
    public string Link_7 = "http://strongbrook.com/common/_media/powerpoint/Strongbrook_Short_PPT.zip";
    public string Link_8 = "http://strongbrook.com/common/_media/powerpoint/Presentation_2012_video.zip";


    


    public string Button_1 = "TrainingVideoPlayer.aspx";

    public int PagerCurrentPage = 1;    // Pager - Current Page
    public int Columns = 1;                      // Columns
    public int ProductsPerPage = 9999;   // Products Per Page
    public int LanguageID = 0;
    public int PriceType = 1;
    public int WarehouseID = 1;
    public int WebID = 3;
    public int WebCategoryID = 21;
    public bool ReturnLongDescription = true;
    public string CurrencyCode = "usd";

    public bool ForceValidInventory { get; set; }
    public int ProductCounter { get; set; }
    public int CellCounter { get; set; }




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

    #region API Methods
    public List<Item> listOfItems;

    public void GetItems()
    {
        string[] items = new string[] { "10050", "10060", "10070", "10080", "10090" };

        listOfItems = new List<Item>();

        var context = ExigoApiContext.CreateODataContext().Items;

        foreach (var item in items)
        {
            var query = (from i in context
                         where i.ItemCode == item
                         select new
                         {
                             i.ShortDetail,
                             i.ShortDetail2,
                             i.ShortDetail3,
                             i.ShortDetail4,
                             i.TinyImageUrl,
                             i.SmallImageUrl,
                             i.LargeImageUrl,
                             i.ItemCode
                         }).FirstOrDefault();

            if (query != null)
            {
                Item theItem = new Item()
                {
                    ShortDetail = query.ShortDetail,
                    ShortDetail2 = query.ShortDetail2,
                    ShortDetail3 = query.ShortDetail3,
                    ShortDetail4 = query.ShortDetail4,
                    TinyImageUrl = query.TinyImageUrl,
                    SmallImageUrl = query.SmallImageUrl,
                    LargeImageUrl = query.LargeImageUrl,
                    ItemCode = query.ItemCode
                };

                listOfItems.Add(theItem);
            }
        }
    }

    #endregion

    #region Render Methods
    public void RenderDynamicWebsiteLinksNew()
    {
        GetItems();
        try
        {
            string WealthClubLevel = null;
            HtmlTextWriter writer = new HtmlTextWriter(Response.Output);
            StringBuilder s = new StringBuilder();
            User user = new User();
            try
            {
                WealthClubLevel = user.DecryptSingleUserPermission(Request.Cookies["userCookie"].Values["WealthClubLevel"]);
            }
            catch
            {
                Message = "We've encountered an error with your cookie, please try logging out and logging back in.";
            }
            s.AppendLine(string.Format(@"<div id=""ItemTable"" class=""itemTable""><table>"));

            #region These are the categories.


            #region Item 1 - News
            foreach (var item in listOfItems.Where(i => i.ItemCode == item1))
            {
                if (item.ItemCode != "")
                {
                    s.AppendLine(string.Format(@"<tr><td colspan=""5""><div class=""left""><h3>{0}</h3></div></td></tr>", item.ShortDetail2));
                    s.AppendLine(string.Format(@"<tr>
                                                     <td class=""imageTD""><div class=""itemImageContainer""><img src=""{0}"" alt=""Image"" /></div></td>
                                                     <td><div class=""middleOuter""><div class=""middleInner""><a href=""{3}""{2} class=""button"">Newsletter Archives</a></div></div></td>
                                                     <td><div class=""middleOuter""><div class=""middleInner""><a href=""{4}""{2} class=""button"">National Team Call Archives</a></div></div></td>
                                                 </tr"
                                                    , item.SmallImageUrl    // 0
                                                    , item.ShortDetail      // 1
                                                    , OpenInSeparateWindow  // 2
                                                    , PDF_1                 // 3
                                                    , PDF_2                 // 4
                                                 ));
                    s.AppendLine(string.Format(@"<tr><td class=""spacer"" colspan=""5""></td></tr>"));
                }
            }
            #endregion Item 1

            #region Item 2 - Policies
            foreach (var item in listOfItems.Where(i => i.ItemCode == item2))
            {
                if (item.ItemCode != "")
                {
                    s.AppendLine(string.Format(@"<tr><td colspan=""5""><div class=""left""><h3>{0}</h3></div></td></tr>", item.ShortDetail2));
                    s.AppendLine(string.Format(@"<tr>
                                                    <td class=""imageTD"">
                                                        <div class=""itemImageContainer""><img src=""{0}"" alt=""Image"" /></div></td>
                                                    <td>
                                                       <div class=""middleOuter"">
                                                           <div class=""middleInner"">
                                                               <a href=""{3}""{2} class=""button"">Application & Agreement</a>
                                                           </div>
                                                       </div>
                                                       <div class=""middleOuter"">
                                                           <div class=""middleInner"">
                                                               <a href=""{4}""{2} class=""button"">Policies & Procedures</a>
                                                           </div>
                                                       </div>
                                                       <div class=""middleOuter"">
                                                           <div class=""middleInner"">
                                                               <a href=""{5}""{2} class=""button"">Terms of Service</a>
                                                           </div>
                                                       </div>
                                                    </td>
                                                    <td>
                                                       <div class=""middleOuter"">
                                                           <div class=""middleInner"">
                                                               <a href=""{6}""{2} class=""button"">Privacy Policy</a>
                                                           </div>
                                                       </div>
                                                       <div class=""middleOuter"">
                                                           <div class=""middleInner"">
                                                               <a href=""{7}""{2} class=""button"">Return Policy</a>
                                                           </div>
                                                       </div>
                                                       <div class=""middleOuter"">
                                                           <div class=""middleInner"">
                                                               <div style=""height:30px""></div>
                                                           </div>
                                                       </div>
                                                    </td>
                                                 </tr>"
                                                    , item.SmallImageUrl    // 0
                                                    , item.ShortDetail      // 1
                                                    , OpenInSeparateWindow  // 2
                                                    , PDF_3                 // 3
                                                    , PDF_4                 // 4
                                                    , PDF_5                 // 5
                                                    , PDF_6                 // 6
                                                    , PDF_7                 // 7
                                                 ));
                    s.AppendLine(string.Format(@"<tr><td class=""spacer"" colspan=""5""></td></tr>"));
                }
            }
            #endregion Item 2

            #region Item 3 - Compensation Plan
            foreach (var item in listOfItems.Where(i => i.ItemCode == item3))
            {
                if (item.ItemCode != "")
                {
                    s.AppendLine(string.Format(@"<tr><td colspan=""5""><div class=""left""><h3>{0}</h3></div></td></tr>", item.ShortDetail2));
                    s.AppendLine(string.Format(@"<tr>
                                                    <td class=""imageTD"">
                                                        <div class=""itemImageContainer""><img src=""{0}"" alt=""Image"" /></div></td>
                                                    <td>
                                                       <div class=""middleOuter"">
                                                           <div class=""middleInner"">
                                                               <a href=""{3}""{2} class=""button"">Detailed Compensation Plan</a>
                                                           </div>
                                                       </div>
                                                       <div class=""middleOuter"">
                                                           <div class=""middleInner"">
                                                               <a href=""{4}""{2} class=""button"">Compensation Plan Overview</a>
                                                           </div>
                                                       </div>
                                                       <div class=""middleOuter"">
                                                           <div class=""middleInner"">
                                                               <a href=""{5}""{2} class=""button"">Per Product Commission Chart</a>
                                                           </div>
                                                       </div>
                                                    </td>
                                                    <td>
                                                       <div class=""middleOuter"">
                                                           <div class=""middleInner"">
                                                               <a href=""{6}""{2} class=""button"">Glossary of Terms</a>
                                                           </div>
                                                       </div>
                                                       <div class=""middleOuter"">
                                                           <div class=""middleInner"">
                                                               <a href=""{7}""{2} class=""button"">Commissions Schedule</a>
                                                           </div>
                                                       </div>
                                                       <div class=""middleOuter"">
                                                           <div class=""middleInner"">
                                                               <a href=""{8}""{2} class=""button"">Income Disclosure Statement</a>
                                                           </div>
                                                       </div>

                                                    </td>
                                                 </tr>"
                                                    , item.SmallImageUrl    // 0
                                                    , item.ShortDetail      // 1
                                                    , OpenInSeparateWindow  // 2
                                                    , PDF_8                 // 3
                                                    , PDF_9                 // 4
                                                    , PDF_10                // 5
                                                    , PDF_11                // 6
                                                    , PDF_12                // 7
                                                    , PDF_17                // 8
                                                 ));
                    s.AppendLine(string.Format(@"<tr><td class=""spacer"" colspan=""5""></td></tr>"));
                }
            }
            #endregion Item 3

            #region Item 4 - IBD Resources
            foreach (var item in listOfItems.Where(i => i.ItemCode == item4))
            {
                if (item.ItemCode != "")
                {
                    s.AppendLine(string.Format(@"<tr><td colspan=""5""><div class=""left""><h3>{0}</h3></div></td></tr>", item.ShortDetail2));
                    s.AppendLine(string.Format(@"<tr>
                                                    <td class=""imageTD"">
                                                        <div class=""itemImageContainer""><img src=""{0}"" alt=""Image"" /></div></td>
                                                    <td>
                                                       <div class=""middleOuter"">
                                                           <div class=""middleInner"">
                                                               <a href=""{3}""{2} class=""button"">Performance Formulas</a>
                                                           </div>
                                                       </div>
                                                       <div class=""middleOuter"">
                                                           <div class=""middleInner"">
                                                               <a href=""{4}""{2} class=""button"">IBD FAQs</a>
                                                           </div>
                                                       </div>
                                                       <div class=""middleOuter"">
                                                           <div class=""middleInner"">
                                                               <a href=""{5}""{2} class=""button"">Game Plan Request Form</a>
                                                           </div>
                                                       </div>
                                                       <div class=""middleOuter"">
                                                           <div class=""middleInner"">
                                                               <a href=""{6}""{2} class=""button"">Client PSA Enrollment Form</a>
                                                           </div>
                                                       </div>
                                                       <div class=""middleOuter"">
                                                           <div class=""middleInner"">
                                                               <a href=""{11}""{2} class=""button"">Strongbrook Event Flyer (blank)</a>
                                                           </div>
                                                       </div>
                                                       <div class=""middleOuter"">
                                                           <div class=""middleInner"">
                                                               <a href=""{13}""{2} class=""button"">Game Plan Expectations Script</a>
                                                           </div>
                                                       </div>
                                                    </td>


                                                    <td>
                                                       <div class=""middleOuter"">
                                                           <div class=""middleInner"">
                                                               <a href=""{7}""{2} class=""button"">PFC Vesting Qualifications</a>
                                                           </div>
                                                       </div>
                                                       <div class=""middleOuter"">
                                                           <div class=""middleInner"">
                                                               <a href=""{8}""{2} class=""button"">IBD Enrollment Form</a>
                                                           </div>
                                                       </div>
                                                       <div class=""middleOuter"">
                                                           <div class=""middleInner"">
                                                               <a href=""{9}""{2} class=""button"">Realtor Program</a>
                                                           </div>
                                                       </div>
                                                       <div class=""middleOuter"">
                                                           <div class=""middleInner"">
                                                               <a href=""{10}""{2} class=""button"">3-Way Call Script</a>
                                                           </div>
                                                       </div>
                                                       <div class=""middleOuter"">
                                                           <div class=""middleInner"">
                                                               <a href=""{12}""{2} class=""button"">Engineering My Strongbrook Income</a>
                                                           </div>
                                                       </div>
                                                       <div class=""middleOuter"">
                                                           <div class=""middleInner"">
                                                               <a href=""{14}""{2} class=""button"">Making Money with Game Plan Requests</a>
                                                           </div>
                                                       </div>
                                                    </td>
                                                 </tr>"
                                                    , item.SmallImageUrl    // 0
                                                    , item.ShortDetail      // 1
                                                    , OpenInSeparateWindow  // 2
                                                    , PDF_13                // 3
                                                    , PDF_14                // 4
                                                    , PDF_15                // 5
                                                    , PDF_16                // 6
                                                    , PDF_18                // 7
                                                    , PDF_19                // 8
                                                    , PDF_20                // 9
                                                    , PDF_21                // 10
                                                    , IMG_7                 // 11
                                                    , PDF_22                // 12
                                                    , PDF_23                // 13
                                                    , PDF_24                // 14
                                                 ));
                    s.AppendLine(string.Format(@"<tr><td class=""spacer"" colspan=""5""></td></tr>"));
                }
            }
            #endregion Item 4

            #region Item 5 - Power Points
            foreach (var item in listOfItems.Where(i => i.ItemCode == item5))
            {
                if (item.ItemCode != "")
                {
                    s.AppendLine(string.Format(@"<tr><td colspan=""5""><div class=""left""><h3>{0}</h3></div></td></tr>", item.ShortDetail2));
            		s.AppendLine(string.Format(@"
                                                <tr>
                                                    <td class=""imageTD"">
                                                        <div class=""itemImageContainer""><img src=""{0}"" alt=""Image"" /></div>
                                                    </td>
                                                    <td>
                                                        <div id=""Left_Column_1"" class=""middleOuter"">
                                                            <div class=""middleInner"">
                                                                <a href=""{9}""{2} class=""button"">Opportunity Presentation Short</a>
                                                            </div>
                                                        </div>
                                                        <div id=""Left_Column_2"" class=""middleOuter"">
                                                            <div class=""middleInner"">
                                                                <a href=""{3}""{2} class=""button"">Training: New IBD Basics</a>
                                                            </div>
                                                        </div>
                                                        <div id=""Left_Column_3"" class=""middleOuter"">
                                                            <div class=""middleInner"">
                                                                <a href=""{6}""{2} class=""button"">Training: 3-Way Calls</a>
                                                            </div>
                                                        </div>
                                                        <div id=""Left_Column_4"" class=""middleOuter"">
                                                            <div class=""middleInner"">
                                                                <a href=""{4}""{2} class=""button"">Training: PSA Fulfillment Training</a>
                                                            </div>
                                                        </div>
                                                       <div class=""middleOuter"">
                                                           <div class=""middleInner"">
                                                               <a href=""{11}""{2} class=""button"">Training: PFC</a>
                                                           </div>
                                                       </div>
                                                    </td>


                                                    <td>
                                                        <div id=""Right_Column_1"" class=""middleOuter"">
                                                            <div class=""middleInner"">
                                                                <a href=""{10}""{2} class=""button"">Opportunity Presentation Long</a>
                                                            </div>
                                                        </div>
                                                        <div id=""Right_Column_2"" class=""middleOuter"">
                                                            <div class=""middleInner"">
                                                                <a href=""{5}""{2} class=""button"">Training: Top 30 Names</a>
                                                            </div>
                                                        </div>
                                                        <div id=""Right_Column_3"" class=""middleOuter"">
                                                            <div class=""middleInner"">
                                                                <a href=""{7}""{2} class=""button"">Training: Strongbrook Products</a>
                                                            </div>
                                                        </div>
                                                        <div id=""Right_Column_4"" class=""middleOuter"">
                                                            <div class=""middleInner"">
                                                                <a href=""{8}""{2} class=""button"">Training: Success Formulas</a>
                                                            </div>
                                                        </div>
                                                       <div class=""middleOuter"">
                                                           <div class=""middleInner"">
                                                               <div style=""height:30px""></div>
                                                           </div>
                                                       </div>
                                                    </td>
                                                </tr>
                                                "
                                                , item.SmallImageUrl    // 0
                                                , item.ShortDetail      // 1
                                                , OpenInSeparateWindow  // 2
                                                , Link_1                // 3 // IBD Basic Training
                                                , Link_2                // 4 // Real Estate Fulfillment Training
                                                , Link_3                // 5 // IBD Interview & Top 30 Names
                                                , Link_4                // 6 // 3 Way Call Basics 
                                                , Link_5                // 7 // Product Training
                                                , Link_6                // 8 // IBD Success Formulas
                                                , Link_7                // 9 // Opportunity Presentation Short
                                                , Link_8                // 10 // Opportunity Presentation Long
                                                , PPT_1                 // 11
                                                ));
                    s.AppendLine(string.Format(@"<tr><td class=""spacer"" colspan=""5""></td></tr>"));
                }
            }
            #endregion Power Points


            #endregion These are the categories.

            s.AppendLine(string.Format(@"</table></div>"));

            s.AppendLine(string.Format(@"{0}{0}{0}{0}{0}{0}{0}{0}{0}{0}", "<br />"));

            writer.Write(s);
        }
        catch
        {
            Message += "We're sorry, something unexpected occurred.  If you continue to receive this message, please contact support";
        }
    }
    #endregion
}