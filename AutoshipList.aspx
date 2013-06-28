<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Site.Master"
    AutoEventWireup="true" CodeFile="AutoshipList.aspx.cs" Inherits="AutoshipList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    <script src="Assets/Scripts/exigo.util.js"></script>
    <script>
        // Set page variables
        var page = {
            activenavigation: 'autoships'
        };

        var labels = {
            deleteMessage: "<%=Resources.Shopping.DeleteAutoshipMessage%>",
            yes: "<%=Resources.Shopping.Yes%>",
            no: "<%=Resources.Shopping.No%>",
            cancel: "<%=Resources.Shopping.Cancel%>"
        }
    </script>

    <link href="Assets/Styles/shopping.min.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript">
        function CreateNewAutoship() {
            <%=Page.ClientScript.GetPostBackEventReference(this, "NewAutoship") %>
        }

        function EditExistingAutoship(autoOrderID) {
            __doPostBack('<%=this.UniqueID %>', 'EditAutoship|' + autoOrderID);
        }

        function DeleteExistingAutoship(autoOrderID) {
            var description = $('input:hidden[id*="AutoshipDescription' + autoOrderID + '"]').val();
            var frequency = $('input:hidden[id*="AutoshipFrequencyDescription' + autoOrderID + '"]').val();

            $.msgbox(labels.deleteMessage.format(frequency, description), {
                type: "confirm",
                buttons: [
                    { type: "submit", value: labels.yes },
                    { type: "submit", value: labels.no },
                    { type: "cancel", value: labels.cancel }
                ]
            },
                function (result) {
                    if (result == labels.yes) {
                        __doPostBack('<%=this.UniqueID %>', 'DeleteAutoship|' + autoOrderID);
                    }
                }
            );
            }
    </script>


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
    <exigo:ApplicationErrorModal ID="ApplicationErrors" runat="server" />


    <h1><%=Resources.Shopping.MyAutoships %></h1>

    <div class="sidebar">
        <navigation:Autoships ID="SideNavigation" ActiveNavigation="list" runat="server" />
    </div>
    <div class="maincontent">
        <div class="well well-large well-white">
            <div id="shopping">
                <h2><%=Resources.Shopping.Autoships %></h2>

                <p>
                    <a href="javascript:CreateNewAutoship();" class="btn btn-success"><%=Resources.Shopping.CreateNewAutoship %></a>
                </p>

                <% RenderAutoshipList(); %>
            </div>
            <div class="well well-small well-white">
                <div id="Note">
                    <h3><strong>Note:</strong> To modify or cancel your Autoship, please contact Strongbrook Support Services at 801-679-4723 during regular business hours (Mon – Fri, 8:00 am to 5:00 pm MT).</h3>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
