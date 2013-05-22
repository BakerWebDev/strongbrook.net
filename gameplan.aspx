<%@ Page Language="C#" MasterPageFile="~/MasterPages/Public.master" AutoEventWireup="true" CodeFile="gameplan.aspx.cs" Inherits="gameplan" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">


    <link href="Assets/Styles/schedule.css"                  type="text/css"     rel="stylesheet" />

    <link href="Assets/Plugins/jquery.fancybox/jquery.fancybox.css" rel="stylesheet" />
    <script src="Assets/Plugins/jquery.fancybox/jquery.fancybox.js"></script>


</asp:Content>

<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="Content">


<script type="text/javascript">


    $(document).ready(function () {

        $.fancybox({
            'type': 'iframe',
            'href': 'GamePlanSubmissionForm.aspx',
		    maxWidth	: 860,
		    maxHeight	: 600,
		    fitToView	: false,
		    width		: '70%',
		    height		: '70%',
		    autoSize	: true,
		    closeClick	: true,
		    openEffect	: 'none',
		    closeEffect	: 'none'
        });
    });
</script>









</asp:Content>
