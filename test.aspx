<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Site.master" AutoEventWireup="true" CodeFile="test.aspx.cs" Inherits="test" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    <script>
        // Set page variables
        var page = {
            activenavigation: 'myaccount'
        };
    </script>

    <link href="Assets/Styles/shopping.min.css" rel="stylesheet" type="text/css" />
    <link href="Assets/Plugins/jquery.validationEngine/css/validationEngine.jquery.css" rel="stylesheet" type="text/css" />
    <script src="Assets/Plugins/jquery.validationEngine/js/languages/jquery.validationEngine-<%=Resources.Shopping.LanguageCodePrefix %>.js" type="text/javascript"></script>
    <script src="Assets/Plugins/jquery.validationEngine/js/jquery.validationEngine.js" type="text/javascript"></script>
    <script src="Assets/Plugins/jquery.validationEngine/js/contrib/other-validations.js" type="text/javascript"></script>


    <script type="text/javascript" language="javascript">
        $(function () {
            $("form").first().validationEngine();
        });

        function UseNewCard() {
            if ($('[validationgroup="NewCard"]').validationEngine('validate')) {
                <%=Page.ClientScript.GetPostBackEventReference(this, "UseCard|New")%>
            }
        }

        function UseNewBankAccount() {
            if ($('[validationgroup="NewBankAccount"]').validationEngine('validate')) {
                <%=Page.ClientScript.GetPostBackEventReference(this, "UseBankAccount|New")%>
            }
        }

        function ToggleNewPaymentMethodFieldsWrapper(paymentmethod) {
            $('div[paymentmethod="' + paymentmethod + '"]').slideDown('fast');
            $('a[paymentmethod="' + paymentmethod + '"]').hide();
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">

    <asp:ScriptManager ID="ScriptManager1" runat="server" />

    <h1>My Account</h1>

    <div class="sidebar">
        <navigation:MyAccount ID="SideNavigation" ActiveNavigation="settings" runat="server" />
    </div>
    <div class="maincontent">
        <div class="well well-large well-white">
            <div id="shopping">
                <div id="shoppingcheckout">



                    <exigo:ApplicationErrorModal ID="ApplicationErrors" runat="server" />



                    <h2>Edit or Replace your Credit Card on File</h2>





       


                </div>
            </div>
        </div>
    </div>
</asp:Content>

