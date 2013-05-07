<%@ Page Language="C#" MasterPageFile="~/MasterPages/test.Master" AutoEventWireup="true" CodeFile="Default2.aspx.cs" Inherits="Default2" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
            <!-- CSS References -->
            <link href="../AJAX/jqueryUI/css/ui-lightness/jquery.datepick.css" rel="stylesheet"
                type="text/css" />
            <script src="../AJAX/jqueryUI/js/jquery-1.5.1.min.js" type="text/javascript"></script>





    <script type="text/javascript" src="Assets/Scripts/jquery.datepicker.js"></script>
    <link rel="stylesheet" href="http://code.jquery.com/ui/1.10.2/themes/smoothness/jquery-ui.css" />

        </asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="MasterContentArea" runat="server">
                <div id="Div2" runat="server" class="longValue">
                     <asp:TextBox ID="txtDate" runat="server" CssClass="TxtStyle"
                         Width="295px" MaxLength="50"></asp:TextBox>
                </div>
            </asp:Content>