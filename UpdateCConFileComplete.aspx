<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Site.Master"
    AutoEventWireup="true" CodeFile="UpdateCConFileComplete.aspx.cs" Inherits="UpdateCConFileComplete" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    <script>
        // Set page variables
        var page = {
            activenavigation: 'myaccount'
        };
    </script>

    <link href="Assets/Styles/shopping.min.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
    <h1>My Account</h1>

    <div class="sidebar">
        <navigation:MyAccount ID="SideNavigation" ActiveNavigation="settings" runat="server" />
    </div>
    <div class="maincontent">
        <div class="well well-large well-white">
            <div id="shopping">
                <div id="shoppingcheckout">

                    <p>The payment information has been successfully saved to your account.
                    </p>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
