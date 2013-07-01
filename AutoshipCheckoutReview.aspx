<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Site.Master"
    AutoEventWireup="true" CodeFile="AutoshipCheckoutReview.aspx.cs" Inherits="AutoshipCheckoutReview" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    <script>
        // Set page variables
        var page = {
            activenavigation: 'autoships'
        };
    </script>

    <link href="Assets/Styles/shopping.min.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript">
        $(document).ready(function () {
            if ($('INPUT[type="hidden"][id*="ApplicationErrors"]').val() != null) {
                //alert($('INPUT[type="hidden"][id*="ApplicationErrors"]').val());
                $(function () {
                    $(this).css('background-color', '#315885');
                    $('DIV#Message').children('p').hide();
                    $('DIV#Message DIV.Close').children('p').hide();
                    //$('DIV#Message').show();
                    $('DIV#Message').animate({ width: '400px' }, 400, function () {
                        $('DIV#Message DIV.Close').children('p').fadeIn('fast');
                    });
                    $('DIV#Message').children('p').fadeIn('slow');
                    setTimeout(function () {
                        $('DIV#Message DIV.Close').children('p').fadeOut("fast", function () {
                            $('DIV#Message').children('p').fadeOut("fast");
                            $('DIV#Message').animate({ width: '0px' }, 400);
                            $('DIV#Message').fadeOut("slow");
                        });
                    }, 16000);
                });
            }
            else {
                if ($('INPUT[type="hidden"][id*="ApplicationErrors"]').val() != '') {
                    $(function () {
                        $('DIV#Message').hide();
                    });
                }
            }

            $('DIV#Message DIV.Close').hover(function () {
                $(this).css('background-color', '#aaa');
                $(this).css('width', '14px');
                $(this).css('height', '20px');
                $(this).css('border-radius', '3px');
                $(this).css('margin-bottom', '24px');
            }).click(function () {
                $(this).css('background-color', '#5aa1f3');
                $(this).delay(200).fadeOut("fast", function () {
                    $('DIV#Message').children('p').fadeOut("fast");
                    $('DIV#Message').animate({ width: '0px' }, 400);
                    $('DIV#Message').fadeOut("fast");
                });
            });

            $('DIV#Message DIV.Close').mouseout(function () {
                $(this).css('width', '14px');
                $(this).css('height', '20px');
                $(this).css('border-radius', '3px');
                $(this).css('background-color', '#ccc');
            });

        });




        function UseNewCard() {
            <%=Page.ClientScript.GetPostBackEventReference(this, "UseCard")%>
        }


    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">

     <asp:HiddenField ID="ApplicationErrors" runat="server" />
     <div id="Message" class="loginwrapper well" style="display: none; position:absolute; right:0px; top:25px;">
     
         <div class="Close">
             <p style="width:10px; padding:2px; margin-bottom:20px; text-align:center; background-color:#ccc; border-radius:3px; font-weight:bold; color:white;">X</p>
         </div>
<%--         

         <div style="width:100%;">
             <p style="padding-left:20px; font-weight:bold;"><%=ErrorString %></p>
         </div>
--%>

     </div>


    <h1><%=Resources.Shopping.Autoships %></h1>

    <div class="sidebar">
        <navigation:Autoships ID="SideNavigation" ActiveNavigation="cart" runat="server" />
    </div>
    <div class="maincontent">
        <div class="well well-large well-white">
            <div id="shopping">
                <div id="shoppingcheckout">
                    <div id="ordertotalsreviewwrapper">
                        <asp:LinkButton ID="cmdPlaceOrder" runat="server" CssClass="btn btn-success addtocart placeorderbutton" OnClick="PlaceOrder_Click" />
                        <table cellpadding="0" cellspacing="0" id="ordertotalsreview">
                            <tr>
                                <td colspan="2">
                                    <h3><%=Resources.Shopping.AutoshipSummary %></h3>
                                </td>
                            </tr>
                            <tr class="subtotal">
                                <td class="fieldlabel"><%=Resources.Shopping.Subtotal %>
                                </td>
                                <td class="value">
                                    <%=string.Format("{0:C}", CalculatedOrder.SubTotal)%>
                                </td>
                            </tr>
                            <tr class="shipping">
                                <td class="fieldlabel"><%=Resources.Shopping.EstimatedShipping %><br />
                                    <span style="font-size: 10px;">






























                                        <%if(CalculatedOrder.ShipMethods[0].ShipMethodID == 8) { %>
                                        <%=CalculatedOrder.ShipMethods.Where(s => s.ShipMethodID.ToString() == rdoShipMethod2.SelectedValue).FirstOrDefault().Description %>
                                        <% } else { %>
                                        <%=CalculatedOrder.ShipMethods.Where(s => s.ShipMethodID.ToString() == rdoShipMethod.SelectedValue).FirstOrDefault().Description %>
                                        <% } %>







                                        


































                                    </span>
                                </td>
                                <td class="value">
                                    <%=string.Format("{0:C}", CalculatedOrder.ShippingTotal)%>
                                </td>
                            </tr>
                            <tr class="taxes">
                                <td class="fieldlabel"><%=Resources.Shopping.EstimatedTaxes %>
                                </td>
                                <td class="value">
                                    <%=string.Format("{0:C}", CalculatedOrder.TaxTotal)%>
                                </td>
                            </tr>
                            <tr class="grandtotal">
                                <td colspan="2"><%=Resources.Shopping.EstimatedTotal %><br />
                                    <%=string.Format("{0:C}", CalculatedOrder.Total)%>
                                </td>
                            </tr>
                        </table>
                    </div>

                    <table cellpadding="0" cellspacing="0" class="orderreview">
                        <tr>
                            <th class="shippingaddress"><%=Resources.Shopping.AutoshipDetails %>
                            </th>
                        </tr>
                        <tr>
                            <td class="shippingaddress">

                                <b><%=Autoship.PropertyBag.AutoshipDescription %></b><br />
                                <%=string.Format(Resources.Shopping.AutoshipBillDate_formatted, Autoship.PropertyBag.Frequency.ToString().ToLower(), Autoship.PropertyBag.StartDate.ToString("dddd, MMMM d, yyyy")) %><br />

                                <a href="<%=Page.ClientScript.GetPostBackClientHyperlink(this, "ChangeDetails") %>">(<%=Resources.Shopping.Change %>)</a>

                            </td>
                        </tr>
                    </table>
                    <br />

                    <table cellpadding="0" cellspacing="0" class="orderreview">
                        <tr>
                            <th class="shippingaddress"><%=Resources.Shopping.ShippingInformation %>
                            </th>
                            <th class="billinginformation"><%=Resources.Shopping.BillingInformation %>
                            </th>
                            <th class="billinginformation"><%=Resources.Shopping.ShippingOptions %>
                            </th>
                        </tr>
                        <tr>
                            <td class="shippingaddress">
                                <strong><%=Resources.Shopping.ShippingAddress %>:</strong> <a href="<%=Page.ClientScript.GetPostBackClientHyperlink(this, "ChangeShippingAddress") %>">(<%=Resources.Shopping.Change %>)</a><br />
                                <%=Autoship.PropertyBag.ShippingFirstName %>
                                <%=Autoship.PropertyBag.ShippingLastName%><br />
                                <%=Autoship.PropertyBag.ShippingAddress1 + ((!string.IsNullOrEmpty(Autoship.PropertyBag.ShippingAddress2)) ? "<br />" + Autoship.PropertyBag.ShippingAddress2 : "")%><br />
                                <%=Autoship.PropertyBag.ShippingCity%>,
                                <%=Autoship.PropertyBag.ShippingState%>
                                <%=Autoship.PropertyBag.ShippingZip%><br />
                                <%=Autoship.PropertyBag.ShippingCountry%><br />
                                <br />
                                <%=Resources.Shopping.Phone %>:
                            <%=Autoship.PropertyBag.Phone%><br />
                                <%=Resources.Shopping.Email %>:
                            <%=Autoship.PropertyBag.Email%>
                            </td>
                            <td class="billinginformation">
                                <strong><%=Resources.Shopping.PaymentMethod %>:</strong> <a href="<%=Page.ClientScript.GetPostBackClientHyperlink(this, "ChangePayment") %>">(<%=Resources.Shopping.Change %>)</a><br />
                                <% if(Autoship.PropertyBag.PaymentType == AutoshipCartPropertyBag.PaymentMethodType.PrimaryCreditCard
                                   || Autoship.PropertyBag.PaymentType == AutoshipCartPropertyBag.PaymentMethodType.SecondaryCreditCard
                                   || Autoship.PropertyBag.PaymentType == AutoshipCartPropertyBag.PaymentMethodType.NewCreditCard)
                                   { %>






                                <%=Autoship.PropertyBag.CreditCardType %> <%=Resources.Shopping.CreditDebitEndingIn %>
                                <%=Autoship.PropertyBag.CreditCardNumber.Substring(Autoship.PropertyBag.CreditCardNumber.Length - 4, 4)%>












                                <br />
                                <br />
                                <strong><%=Resources.Shopping.BillingAddress %>:</strong><br />
                                <%=Autoship.PropertyBag.CreditCardNameOnCard%><br />
                                <%=Autoship.PropertyBag.CreditCardBillingAddress%><br />
                                <%=Autoship.PropertyBag.CreditCardBillingCity%>,
                                <%=Autoship.PropertyBag.CreditCardBillingState%>
                                <%=Autoship.PropertyBag.CreditCardBillingZip%><br />
                                <%=Autoship.PropertyBag.CreditCardBillingCountry%>
                                <% } %>
                                <% if(Autoship.PropertyBag.PaymentType == AutoshipCartPropertyBag.PaymentMethodType.BankAccountOnFile)
                                   { %>
                            <%=Resources.Shopping.BankAccountEndingIn %>
                            <%=Autoship.PropertyBag.BankAccountAccountNumber.Substring(Autoship.PropertyBag.CreditCardNumber.Length - 4, 4)%>
                                <br />
                                <br />
                                <strong><%=Resources.Shopping.BankAddress %>:</strong><br />
                                <%=Autoship.PropertyBag.BankAccountBankName%><br />
                                <%=Autoship.PropertyBag.BankAccountBankAddress%><br />
                                <%=Autoship.PropertyBag.BankAccountBankCity%>,
                                <%=Autoship.PropertyBag.BankAccountBankState%>
                                <%=Autoship.PropertyBag.BankAccountBankZip%><br />
                                <%=Autoship.PropertyBag.BankAccountBankCountry%>
                                <% } %>
                            </td>
                            <td class="shippingoptions checkboxes">
                                <%if(CalculatedOrder.ShipMethods[0].ShipMethodID == 8) { %>
                                <strong>No Shipping on this item.</strong><br />
                                <asp:RadioButtonList ID="rdoShipMethod2" runat="server" RepeatLayout="Flow" OnSelectedIndexChanged="ChangeShippingMethod_SelectedIndexChanged"
                                    AutoPostBack="true" Visible="false" />
                                <% } else { %>
                                <strong><%=Resources.Shopping.ChooseAShippingSpeed %>:</strong><br />
                                <asp:RadioButtonList ID="rdoShipMethod" runat="server" RepeatLayout="Flow" OnSelectedIndexChanged="ChangeShippingMethod_SelectedIndexChanged"
                                    AutoPostBack="true" />
                                <% } %>
                            </td>
                        </tr>
                    </table>
                    <br />

                    <table cellpadding="0" cellspacing="0" id="cartcontents">
                        <tr>
                            <th class="description"><%=Resources.Shopping.Item %>
                            </th>
                            <th class="priceeach"><%=Resources.Shopping.PriceEach %>
                            </th>
                            <th class="bv"><%=Resources.Shopping.BV %>
                            </th>
                            <th class="quantity"><%=Resources.Shopping.Quantity %>
                            </th>
                            <th class="total"><%=Resources.Shopping.Total %>
                            </th>
                        </tr>
                        <% foreach(var item in CartItems)
                           { %>
                        <tr>
                            <td class="description">
                                <img src="<%=item.Image %>" alt="<%=item.Description %>" title="<%=item.Description %>" />
                                <span class="producttitle"><a href="<%=Autoship.UrlProductDetail %>?item=<%=item.ItemCode %>">
                                    <%=item.Description %></a></span><br />
                                <span class="itemcode"><%=Resources.Shopping.SKU %>:
                                <%=item.ItemCode %></span>
                                <div class="clearfix">
                                </div>
                            </td>
                            <td class="priceeach">
                                <%=item.PriceEach.ToString("C") %>
                            </td>
                            <td class="bv">
                                <%=item.BV.ToString("N2")%>
                            </td>
                            <td class="quantity">
                                <%=item.Quantity.ToString("N0") %>
                            </td>
                            <td class="total">
                                <%=item.PriceTotal.ToString("C") %>
                            </td>
                        </tr>
                        <% } %>
                        <tr>
                            <td colspan="6" class="options">
                                <a href="<%=Page.ClientScript.GetPostBackClientHyperlink(this, "ChangeItems") %>" class="btn"><%=Resources.Shopping.ChangeItems %></a>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
