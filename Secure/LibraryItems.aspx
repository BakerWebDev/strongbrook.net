<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="MasterPages/Wealth.Master" CodeFile="LibraryItems.aspx.cs" Inherits="Secure_LibraryItems" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    <link rel="stylesheet" type="text/css" href="Assets/Styles/LibraryItems.css" />
    <script type="text/javascript" src="Assets/Scripts/jquery.min.js" language="javascript"></script>
</asp:Content>
<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="Content">
    <script type="text/javascript" language="javascript">
        $(document).ready(function () {
            if ($('INPUT[type="hidden"][id*="ShowMessage"]').val() != '') {
                $(function () {
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
                            $('DIV#Message').fadeOut("fast");
                        });
                    }, 5000);
                });
            }

            $('DIV#Message DIV.Close').hover(function () {
                $(this).css('background-color', '#315885');
            }).click(function () {
                $(this).css('background-color', '#5aa1f3');
                $(this).delay(200).fadeOut("fast", function () {
                    $('DIV#Message').children('p').fadeOut("fast");
                    $('DIV#Message').animate({ width: '0px' }, 400);
                    $('DIV#Message').fadeOut("fast");
                });

            });
        });
    </script>

    <asp:HiddenField ID="ShowMessage" runat="server" />
    <div id="Message" style="display: none;">
        <div class="Close">
            <p>X</p>
        </div>
        <p><%=Message %></p>
    </div>

    <div class="content-container">
            <% RenderDynamicWebsiteLinksNew(); %>
    </div>
</asp:Content>

