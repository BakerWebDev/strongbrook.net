<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Site.Master"
    AutoEventWireup="true" CodeFile="test3.aspx.cs" Inherits="test3" %>

<asp:Content ID="Head1" runat="server" ContentPlaceHolderID="Head">

<script>
    function jsonpCallback(response) {
        alert(response.data);
    }
    $(document).ready(function () {
        $.ajax({
            url: 'test.aspx',
            dataType: 'jsonp',
            error: function (xhr, status, error) {
                alert(error);
            },
            success: jsonpCallback
        });
    });
</script>

</asp:Content>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="Content">


</asp:Content>

