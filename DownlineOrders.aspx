<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Site.master" AutoEventWireup="true" CodeFile="DownlineOrders.aspx.cs" Inherits="DownlineOrders" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
    <link href="Assets/Styles/reports.min.css" rel="stylesheet" />
    <link href="Assets/Styles/downlineOrganizationReportHeader.css" rel="stylesheet" />
    <script src="Assets/Scripts/exigo.util.js"></script>
    <script src="Assets/Scripts/exigo.report.js"></script>
    <script src="Assets/Scripts/exigo.report.searching.js"></script>

    <script>
        // Set page variables
        var page = {
            activenavigation: 'organization'
        };

        $(function () {
            // Report
            setInitialSort('OrderDate', report.sortOrderTypes.DESCENDING);
            initializeReport();

            // Searching in report
            searching.listdatasources.countries = <%=SqlReportSearchListJsonDataSource.Countries() %>;
            searching.listdatasources.states = <%=SqlReportSearchListJsonDataSource.States() %>;
            searching.listdatasources.ranks = <%=SqlReportSearchListJsonDataSource.Ranks() %>;
            searching.listdatasources.types = <%=SqlReportSearchListJsonDataSource.CustomerTypes() %>;
            searching.listdatasources.statuses = <%=SqlReportSearchListJsonDataSource.CustomerStatuses() %>;
            initializeReportSearch();    
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="Server">
    <h1>Organization</h1>

    <div class="sidebar">
        <navigation:Organization ID="SideNavigation" ActiveNavigation="downlineorders" runat="server" />
    </div>
    <div class="maincontent">
        <h2>Downline Orders</h2>

        <div class="well well-white list">

            <div id="searchwrapper">
                <h3>Search:</h3>
                <select id="lstSearchField" class="input-medium">
                    <option value="CustomerID" data-fieldtype="int">ID</option>
                    <option value="FirstName" data-fieldtype="string">First Name</option>
                    <option value="LastName" data-fieldtype="string">Last Name</option>
                    <option value="Company" data-fieldtype="string">Company</option>
                    <%--<option value="CustomerType" data-fieldtype="string" data-source="types">Type</option>--%>
                    <%--<option value="CustomerStatus" data-fieldtype="string" data-source="statuses">Status</option>--%>
                    <option value="OrderID" data-fieldtype="int">Order ID</option>
                    <option value="OrderDate" data-fieldtype="int">Order Date</option>
                    <%--<option value="BusinessVolumeTotal" data-fieldtype="int">BV</option>--%>
                    <option value="Total" data-fieldtype="int">Total</option>
                </select>
            </div>

            <div class="gridreporttableheader">
                <table class="table gridreporttableheader" id="TableHeader">
                    <tr class="table-headers">
                        <th id="ID"><a class="sortable" data-field="CustomerID">ID</a></th>
                        <th id="Name"><a class="sortable" data-field="LastName">Name</a></th>
                        <th id="OrderID"><a class="sortable" data-field="OrderID">Order ID</a></th>
                        <th id="OrderDate"><a class="sortable" data-field="OrderDate">Date</a></th>
                        <th id="BV">BV</th>
                        <th id="Total"><a class="sortable" data-field="Total">Total</a></th>
                        <th id="Actions">Actions</th>
                    </tr>
                </table>
            </div>

            <div class="gridreporttablewrapper">
                <table class="table gridreporttable" id="gridreporttable">
                </table>
            </div>

        </div>
    </div>
</asp:Content>

