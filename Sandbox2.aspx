<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Site.master" AutoEventWireup="true" CodeFile="Sandbox2.aspx.cs" Inherits="Sandbox2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
    <link href="Assets/Styles/commissions.min.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="Server">
    <h1>Sandbox 2</h1>

    <div class="sidebar">
        <navigation:Organization ID="SideNavigation" ActiveNavigation="personallyenrolled" runat="server" />
    </div>

    <div class="maincontent">


                <div id="Row_2">












                    <a id="Render_UniLevelDownlineGPR_Count">
                        <div class="tile theme-lightblue" title="Definition">
                            <h2><% Render_UniLevelDownlineGPR_Count(); %></h2>
                            <h4>The Number of People in the Downline</h4>
                        </div>
                    </a>


                    <a id="Render_UniLevelDownlineGPR_CountPerPerson">
                        <div class="tile theme-lightblue" title="Definition">
                            <h2><% Render_UniLevelDownlineGPR_CountPerPerson(); %></h2>
                            <h4>The Number of GPRs each Person in the Downline has created in this period</h4>
                        </div>
                    </a>


                    <a id="Render_UniLevelDownlineGPR_Average">
                        <div class="tile theme-lightblue" title="Definition">
                            <h2><% Render_UniLevelDownlineGPR_Average(); %></h2>
                            <h4>The Average Number of GPRs each Person in the Downline has created in this period</h4>
                        </div>
                    </a>













                </div>

    </div>
</asp:Content>

