<%@ Control Language="C#" AutoEventWireup="true" CodeFile="WealthHeader.ascx.cs" Inherits="Secure_Includes_WealthHeader" %>

<div id="header-wrapper">
    	<div id="header-container">
            <div id="navTrigger">
                    
            </div>
        	<div id="header-member-info"> <%=FullName %> <asp:LinkButton ID="cmdLogout" runat="server" OnClick="Click_Logout" Text="Logout" /></div>
        </div>
</div>
