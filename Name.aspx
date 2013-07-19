<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Name.aspx.cs" Inherits="Name" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body style="margin-top:12px; margin-bottom:0px; margin-right:0px;">
    <form id="form1" runat="server">
    <div style="font-size: 12px; color:#1b3e73; font-family: Verdana,Geneva,sans-serif; text-decoration:none; text-align:right;">
        <%=FullName %> | <%=String.Format("{0:(###) ###-####}", Convert.ToInt64(PhoneNumber)) %> |
    </div>
    </form>
</body>
</html>