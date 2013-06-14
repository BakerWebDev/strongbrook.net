<%@ Control Language="C#" AutoEventWireup="true" CodeFile="WealthNavigation.ascx.cs" Inherits="Secure_Includes_WealthNavigation" %>
<%--<%@ OutputCache Duration="1800" VaryByParam="none" Shared="true" %>--%>

<img src="Assets/Images/navigation.png" width="135" height="20" alt="Navigation" />
<!-- Begin Error Handling -->
<link href="Assets/Plugins/jquery.msgbox/styles/jquery.msgbox.css" rel="stylesheet" type="text/css" />
<script src="Assets/Plugins/jquery.msgbox/jquery.msgbox.min.js" type="text/javascript"></script>
<script type="text/javascript" language="javascript">
    //$(document).ready(function () {
    //    if ($('INPUT[type="hidden"][id*="ApplicationErrors"]').val() != '') {
    //        $(function () {
    //            var error = '<%=Server.UrlDecode(ApplicationErrors.Value) %>';
    //            $.msgbox(error, { type: "error", button: "Ok" });
    //        });
    //    };
    //});

    $(document).ready(function () {
        if ($('INPUT[type="hidden"][id*="ApplicationErrors"]').val() != '') {
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
<asp:HiddenField ID="ApplicationErrors" runat="server" />
<!-- End Error Handling -->
<ul>
    <% RenderPortalLinks(); %>
</ul>
