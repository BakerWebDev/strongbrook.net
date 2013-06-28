<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Site.Master"
    AutoEventWireup="true" CodeFile="test2.aspx.cs" Inherits="test2" %>

<asp:Content ID="Head1" runat="server" ContentPlaceHolderID="Head">

    <script type="text/javascript">
        $(document).ready(function () {

            var inp = $('#<%=txtForgotPassUsername.ClientID %>').val();

            if (jQuery.trim(inp).length > 0) {
                //if ($('INPUT[type="hidden"][id*="ApplicationErrors"]').val() == '') {
                    alert($('INPUT[type="hidden"][id*="ApplicationErrors"]').val());
                    $(function () {
                        $(this).css('background-color', '#315885');
                        $('DIV#Message').children('p').hide();
                        $('DIV#Message DIV.Close').children('p').hide();
                        $('DIV#Message').show();
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
                //}
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

                <%=Page.ClientScript.GetPostBackEventReference(this, "UseCard|New")%>
            
        }


    </script>








</asp:Content>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="Content">



    <div class="row-fluid">
        <span class="span4 offset4">



            <asp:HiddenField ID="ApplicationErrors" runat="server" />
            <div id="Message" class="loginwrapper well" style="display: none; position:absolute; right:0px; top:25px;">
 
                <div class="Close">
                    <p style="width:10px; padding:2px; margin-bottom:20px; text-align:center; background-color:#ccc; border-radius:3px; font-weight:bold; color:white;">X</p>
                </div>
                
                <div style="width:100%;">
                    <p style="padding-left:20px; font-weight:bold;"><%=ErrorString %></p>
                </div>
            </div>




        </span>

        <a href="javascript:UseNewCard();" class="btn btn-success Next">Add / Update</a>

        <div class="row-fluid">
            <span class="span3 offset5">
                <div class="loginwrapper well">
                    <div id="ForgotPass">
                        <p>Please enter your username so we can locate your Password</p>
                        <div class="information">
                            <asp:TextBox CssClass="span12" ID="txtForgotPassUsername" runat="server" />

                            <div class="login-button">
                                <asp:Button ID="cmdForgotPassword" runat="server" Text="Reset my password" OnClick="ForgotPass" CssClass="btn btn-primary btn-large btn-block" CausesValidation="false" />
                            </div>

                            <asp:RequiredFieldValidator ID="rtxtForgotPassUsername" runat="server" Display="Dynamic"
                                ControlToValidate="txtForgotPassUsername" ErrorMessage="Please enter a username" SetFocusOnError="true" />
                        </div>
                    </div>
                </div>
            </span>
        </div>



    </div>


</asp:Content>

