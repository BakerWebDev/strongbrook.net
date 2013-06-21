<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Site.Master"
    AutoEventWireup="true" CodeFile="UpdateCConFile.aspx.cs" Inherits="UpdateCConFile" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    <script>
        // Set page variables
        var page = {
            activenavigation: 'autoships'
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

    <h1><%=Resources.Shopping.Autoships %></h1>

    <div class="sidebar">
        <navigation:Autoships ID="SideNavigation" ActiveNavigation="cart" runat="server" />
    </div>
    <div class="maincontent">
        <div class="well well-large well-white">
            <div id="shopping">
                <div id="shoppingcheckout">

                    <% if(HasOneOrMoreValidPaymentMethodsOnFile())
                       { %>
                    <h2>Edit or Replace your Credit Card on File</h2>

                    <table cellpadding="0" cellspacing="0" id="paymentmethodsonfile">
                        <tr>
                            <th colspan="2" class="options"><%=Resources.Shopping.YourStoredPaymentMethods %>
                            </th>
                            <th class="nameoncard"><%=Resources.Shopping.NameOnCardAccount %>
                            </th>
                            <th class="expirationdate"><%=Resources.Shopping.ExpiresOn %>
                            </th>
                        </tr>
                        <% RenderCreditCardOnFile(AccountCreditCardType.Primary); %>
                        <% RenderCreditCardOnFile(AccountCreditCardType.Secondary); %>
                    </table>
                    <br />
                    <br />
                    <hr />
                    <br />
                    <% }
                       else
                       { %>
                    <h2>Here</h2>
                    <% } %>

                    <% if(Autoship.Payments.AvailablePaymentMethodTypes.Contains(Exigo.WebService.PaymentType.CreditCard))
                       { %>

                    <div class="newpaymentmethodwrapper">
                        <table class="newpaymentmethod">
                            <tr>
                                <td class="allowablepaymentmethodicons">
                                    <img src="Assets/Images/amex-24.png" />
                                    <img src="Assets/Images/discover-24.png" />
                                    <img src="Assets/Images/mastercard-24.png" />
                                    <img src="Assets/Images/visa-24.png" />
                                </td>
                                <td>
                                    <div class="usenewpaymentmethodbuttonwrapper">
                                        <a class="btn btn-success" paymentmethod="Card" onclick="ToggleNewPaymentMethodFieldsWrapper('Card')">Add or Edit</a>
                                    </div>
                                    <h3><%=Resources.Shopping.CreditOrDebitCards %></h3>
                                    <p>
                                        <%=string.Format(Resources.Shopping.CardsAccepted, GlobalSettings.Company.Name) %>
                                    </p>
                                    <div id="newcreditcardfieldswrapper" paymentmethod="Card" validationgroup="NewCard" style="DISPLAY: none">
                                        <br />
                                        <table cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td class="fieldlabel"><%=Resources.Shopping.YourName %>
                                                </td>
                                                <td class="fields">
                                                    <div class="fieldwrapper width255">
                                                        <asp:TextBox ID="txtCreditCardBillingName" runat="server" CssClass="validate[required] text-input" />
                                                        <div class="fieldinstructions">
                                                            <%=Resources.Shopping.NameAsItAppearsOnCard %>
                                                        </div>
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="fieldlabel"><%=Resources.Shopping.YourCardNumber %>
                                                </td>
                                                <td class="fields">
                                                    <div class="fieldwrapper width255">
                                                        <asp:TextBox ID="txtCreditCardNumber" runat="server" CssClass="validate[required,custom[onlyNumberSp]] text-input" />
                                                        <div class="fieldinstructions">
                                                            <%=Resources.Shopping.NumbersOnly_NoSpecialChar %>
                                                        </div>
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="fieldlabel"><%=Resources.Shopping.YourExpiDate %>
                                                </td>
                                                <td class="fields">
                                                    <div class="fieldwrapper width120">
                                                        <asp:DropDownList ID="lstCreditCardExpirationMonth" runat="server" CssClass="validate[required]" />
                                                        <div class="fieldinstructions">
                                                            <%=Resources.Shopping.Month %>
                                                        </div>
                                                    </div>
                                                    <div class="fieldwrapper width120">
                                                        <asp:DropDownList ID="lstCreditCardExpirationYear" runat="server" CssClass="validate[required]" />
                                                        <div class="fieldinstructions">
                                                            <%=Resources.Shopping.Year %>
                                                        </div>
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="fieldlabel"><%=Resources.Shopping.YourSecurityCode %>
                                                </td>
                                                <td class="fields">
                                                    <div class="fieldwrapper width255">
                                                        <asp:TextBox ID="txtCreditCardCvc" runat="server" CssClass="validate[required,custom[onlyNumberSp]] text-input" />
                                                        <div class="fieldinstructions">
                                                            <%=Resources.Shopping.TheCVCCodeFound %>
                                                        </div>
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="fieldlabel"><%=Resources.Shopping.YourBillingAddress %>
                                                </td>
                                                <td class="fields">
                                                    <div class="fieldwrapper width255">
                                                        <asp:TextBox ID="txtCreditCardBillingAddress" runat="server" CssClass="validate[required] text-input" />
                                                        <div class="fieldinstructions">
                                                            <%=Resources.Shopping.StreetAddress %>
                                                        </div>
                                                    </div>
                                                    <div class="clearfix">
                                                    </div>
                                                    <div class="fieldwrapper width120">
                                                        <asp:TextBox ID="txtCreditCardBillingCity" runat="server" CssClass="validate[required] text-input" />
                                                        <div class="fieldinstructions">
                                                            <%=Resources.Shopping.City %>
                                                        </div>
                                                    </div>
                                                    <div class="fieldwrapper width120">
                                                        <asp:UpdatePanel ID="Update_CreditCardState" runat="server" ChildrenAsTriggers="false"
                                                            UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <asp:DropDownList ID="lstCreditCardBillingState" runat="server" CssClass="validate[required]" />
                                                            </ContentTemplate>
                                                            <Triggers>
                                                                <asp:AsyncPostBackTrigger ControlID="lstCreditCardBillingCountry" EventName="SelectedIndexChanged" />
                                                            </Triggers>
                                                        </asp:UpdatePanel>
                                                        <div class="fieldinstructions">
                                                            <%=Resources.Shopping.State %>
                                                        </div>
                                                    </div>
                                                    <div class="clearfix">
                                                    </div>
                                                    <div class="fieldwrapper width120" style="display: <%=(Autoship.AddressSettings.AllowZipCode) ? "block" : "none" %>">
                                                        <asp:TextBox ID="txtCreditCardBillingZip" runat="server" CssClass="validate[required] text-input" />
                                                        <div class="fieldinstructions">
                                                            <%=Resources.Shopping.ZipCode %>
                                                        </div>
                                                    </div>
                                                    <div class="fieldwrapper width120">
                                                        <asp:DropDownList ID="lstCreditCardBillingCountry" runat="server" OnSelectedIndexChanged="PopulateRegions_SelectedIndexChanged"
                                                            AutoPostBack="true" CssClass="validate[required]" />
                                                        <div class="fieldinstructions">
                                                            <%=Resources.Shopping.Country %>
                                                        </div>
                                                    </div>
                                                    <div class="clearfix">
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="fieldlabel"><%=Resources.Shopping.SaveThisCardAsMy %>
                                                </td>
                                                <td class="fields">
                                                    <div class="fieldwrapper width255">
                                                        <asp:DropDownList ID="lstSaveCardAs" runat="server" CssClass="validate[required]">
                                                        </asp:DropDownList>
                                                        <div class="fieldinstructions">
                                                           <%=Resources.Shopping.ChooseHowToSaveThisCard %>
                                                        </div>
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="fieldlabel">&nbsp;
                                                </td>
                                                <td class="fields">
                                                    <br />
                                                    <a href="javascript:UseNewCard();" class="btn btn-success Next">Add / Update</a>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <% } %>


                </div>
            </div>
        </div>
    </div>
</asp:Content>
