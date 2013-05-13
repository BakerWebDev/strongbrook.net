<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Dashboard.aspx.cs" Inherits="Dashboard" MasterPageFile="~/MasterPages/PopUp.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="Assets/Styles/DashboardStyles/Dashboard.css" rel="Stylesheet" />
    <link href="Assets/Styles/DashboardStyles/News.css" rel="Stylesheet" />
    <link href="Assets/Styles/DashboardStyles/DuesManager.css" rel="Stylesheet" />
    <link href="Assets/Styles/DashboardStyles/CurrentRank.css" rel="Stylesheet" />
    <link href="Assets/Styles/DashboardStyles/Volumes.css" rel="Stylesheet" />
    <link href="Assets/Styles/DashboardStyles/commissionEligibility.css" rel="Stylesheet" />
    <link href="Assets/Styles/DashboardStyles/RecentActivity.css" rel="Stylesheet" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">

    <script src="Assets/Plugins/jquery.dashboardsort/dashboardsort.js" language="javascript" type="text/javascript"></script>
    <script src="Assets/Plugins/jquery.currentRank/currentRank.js" language="javascript" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        $(document).ready(function () {
            var rankloaded;
            $('DIV#addreport').bind('click', function () {
                $('DIV#save').animate({
                    bottom: '35px'
                }, 300, 'swing');
                setTimeout(function () {
                    $('DIV#cancel').animate({
                        bottom: '35px'
                    }, 300, 'swing');
                }, 100);
                $('DIV#addcolumn').show();
                $('DIV#addnewreport').show();
                $('DIV#addcolumn').animate({
                    width: '116px',
                    padding: '5px'
                }, 300);
                setTimeout(function () {
                    $('DIV#addnewreport').animate({
                        width: '116px',
                        padding: '5px'
                    }, 300);
                }, 100);
                setTimeout(function () {
                    $('DIV#sideitems').find('span').show();
                }, 650);
                $('DIV.board').dashboardsort('init');
                $('DIV#reports').children().hide();
                $('DIV#addnewreport').bind('click', function () {
                    $('DIV#reports').fadeIn('fast').children().fadeIn('fast');
                });
                $('DIV#reports').bind('mouseleave', function () {
                    $('DIV#reports').fadeOut('fast').children().fadeOut('fast');
                });
                $('DIV#reports').children('div').each(function () {
                    $(this).bind('click', function () {
                        createReport($(this).attr('id'));
                    });
                });
            });

            $('DIV.cancelme').bind('mouseenter', function () {
                $('DIV#hoverCancel').fadeIn(400);
            }).bind('mouseleave', function () {
                $('DIV#hoverCancel').fadeOut(400);
            });

            $('DIV.cancelme').bind('click', function () {
                //$('DIV.board').dashboardsort('unload'); Left in in case you'd like to remove overlay without reloading
                //Close();

                window.location.replace('Dashboard.aspx'); //Reload for ease in canceling changes
            });

            $('DIV.saveme').bind('mouseenter', function () {
                $('DIV#hoverSave').fadeIn(400);
            }).bind('mouseleave', function () {
                $('DIV#hoverSave').fadeOut(400);
            });

            $('DIV.saveme').bind('click', function () {
                $('DIV.board').dashboardsort('unload');
                Save();
            });

            $('DIV#addcolumn').bind('click', function () {
                $('DIV.board').dashboardsort('add');
            });

            //Load the various ajax for the reports
            $('DIV.draggable').each(function () {
                var report = $(this).attr('id').split('-');
                LoadReport(report[1]);
            });
            
            //LoadReport("duesManager");
            //LoadReport("news");
            //LoadReport("currentRank");

            $('DIV.board').ajaxComplete(function (e, xhr, settings) {
                var maxLength = 37,
                    url = settings.url,
                    sb = url.length - maxLength,
                    report = settings.url.substring(0, url.length - sb);
                
                if (report == 'DataStores/Dashboard/currentRank.aspx') {
                    $('DIV.board').currentRank('load', $('#valpcv').val(), 'pcv');
                    $('DIV.board').currentRank('load', $('#valocv').val(), 'ocv');
                    $('DIV.board').currentRank('rank', $('#valrnk').val());
                }
            });
        });

        function createReport(div) {
            var report = div.split('-'),
                header,
                col1 = 0,
                col2 = 0,
                col3 = 0;
            report = report[1];

            switch (report) {
                case "news": header = "Corporate News"; break;
                case "cNotes": header = "Corporate Social Feed"; break;
                case "dNotes": header = "Downline Social Feed"; break;
                case "duesManager": header = "Dues Manager"; break;
                case "recentActivity": header = "Activity in my downline"; break;
                case "recentChecks": header = "This Month's Checks"; break;
                case "volumes": header = "Monthly Volumes"; break;
                case "edownline": header = "Enroller Tree"; break;
                case "udownline": header = "Unilevel Tree"; break;
                case "currentRank": header = "Current Rank"; break;
                case "rankAnalysis": header = "Rank Analysis"; break;
                case "achievements": header = "My Achievements"; break;
                case "commissionEligibility": header = "Commission Eligibility"; break;
            }

            $('DIV.column').each(function () {
                if ($(this).attr('id') == 'column-1') {
                    col1 = $(this).children('div').length;
                }
                if ($(this).attr('id') == 'column-2') {
                    col2 = $(this).children('div').length;
                }
                if ($(this).attr('id') == 'column-3') {
                    col3 = $(this).children('div').length;
                }
            });
            //if (col1 < col2 && col2 <= col3) {
            //    var newrow = col1 + 1,
            //        newrowid = 'dashboardobject-row-' + newrow,
            //        div = 'DIV.' + newrowid,
            //        id = 'dashboard-' + report;

            //    $('DIV#column-1').children('div').last().after('<div class="' + newrowid + '"></div>');
            //    $(div).html('<div id="dashboard-' + report + '" class="draggable sortable"></div>');
            //    $('DIV#' + id).html('<div class="delete" style="display: none;">X</div><h4 class="clickable">' + header + '</h4><div class="contents"></div>');
            //    LoadReport(report);
            //    $('DIV#' + id).dashboardsort('rebind', id);
            //}
            //if (col2 < col1 && col1 <= col3) {
            //    var newrow = col2 + 1,
            //        newrowid = 'dashboardobject-row-' + newrow,
            //        div = 'DIV.' + newrowid,
            //        id = 'dashboard-' + report;

            //    $('DIV#column-2').children('div').last().after('<div class="' + newrowid + '"></div>');
            //    $(div).html('<div id="dashboard-' + report + '" class="draggable sortable"></div>');
            //    $('DIV#' + id).html('<div class="delete" style="display: none;">X</div><h4 class="clickable">' + header + '</h4><div class="contents"></div>');
            //    LoadReport(report);
            //    $('DIV#' + id).dashboardsort('rebind', id);
            //}
            //else {
                var newrow = parseFloat(col1) + 1,
                    newrowid = 'dashboardobject-row-' + newrow,
                    div = 'DIV.' + newrowid,
                    id = 'dashboard-' + report;

                if ($('DIV#' + id).length == 0) {
                    if (col1 == 0) {
                        $('DIV#column-1').children('span').last('span').after('<div class="' + newrowid + '"></div>');
                        $('DIV#column-1').children('div').last('div').html('<div id="dashboard-' + report + '" class="draggable sortable"></div>');
                    }
                    else {
                        $('DIV#column-1').children('div').last('div').after('<div class="' + newrowid + '"></div>');
                        $('DIV#column-1').children('div').last('div').html('<div id="dashboard-' + report + '" class="draggable sortable"></div>');
                    }
                    $('DIV#' + id).html('<div class="delete" style="display: none;">X</div><h4 class="clickable">' + header + '</h4><div class="contents"></div>');
                    LoadReport(report);
                    $('DIV#' + id).dashboardsort('rebind', id);
                }
                else {
                    alert('This report already exists');
                }
            //}
        }

        function LoadReport(report) {
            var path = "DataStores/Dashboard/",
                colnumber = $('DIV.board').attr('id').split('-');
            $('DIV[id="dashboard-' + report + '"]').children('div.contents').html('<center><img src="Assets/Images/imgProcessingSmall.gif" /></center>');
            $.ajax({
                url: path + report + '.aspx?customerid=' + <%=CustomerID %> + '&colnumber=' + colnumber,
                cache: false,
                dataType: 'html',
                success: function (data) {
                    if (report == "currentRank") {
                        $('DIV[id="dashboard-' + report + '"]').children('div.contents').html(data);
                        var pcv = $('#valpcv').val(),
                            ocv = $('#valocv').val(),
                            pcv = pcv.substring(0, pcv.length - 5),
                            ocv = ocv.substring(0, ocv.length - 5);
                        $('SPAN.pcv').text(pcv);
                        $('SPAN.ocv').text(ocv);
                        //AnimateVolumes(pcv, ocv);
                    }
                    else {
                        $('DIV[id="dashboard-' + report + '"]').children('div.contents').html(data);
                    }
                    if (report == "duesManager") {
                        $.getScript('Assets/Scripts/Dashboard/duesManager.js');
                    }
                },
                error: function (error) {
                }
            });
        }

            function Save() {
                $('DIV#saving').show(); //Show saving indicator
                var counter = 1,
                    saveString = "";

                while (counter <= 3) {
                    var currentColumn = "#column-" + counter; //Get the current column to work on

                    $(currentColumn).children('div').each(function () { //for each child div in column, get the row and contents
                        var row = $(this).attr('class').split('-');

                        $(this).children('div').each(function () {
                            var obj = $(this).attr('id').split('-');

                            saveString += obj[1] + ',' + counter + ',' + row[2] + '|'; //Append the string with current object
                        });
                    });

                    counter++;
                }
                saveString = saveString.substring(0, saveString.length - 1); //Remove the last '|'
                var extID = $('INPUT[id*="detailid"]').val();
            
                var args = '"cid": "' + <%=CustomerID %> + '", "extID": "' + extID + '", "s": "' + saveString + '"';

                $.ajax({
                    url: 'Dashboard.aspx/SaveDashboard',
                    data: '{' + args + '}',
                    type: 'POST',
                    dataType: 'JSON',
                    contentType: 'application/json; charset=utf-8',
                    success: function (data) {
                        alert("save");
                        $('DIV#saving').children().fadeOut('200');
                        setTimeout(function () {
                            $('DIV#saving').children('div').fadeIn('600');
                        }, 600);
                    },
                    errror: function (result) {
                        alert(result.status + ' ' + resut.statusText);
                    }
                });

            }

            function Close() {
                $('DIV#saving').fadeOut('400');
                $('DIV#canceling').fadeOut('400');
                $('DIV#sideitems').find('span').hide();
                $('DIV#cancel').animate({
                    bottom: '0px'
                }, 300, 'swing');
                setTimeout(function () {
                    $('DIV#save').animate({
                        bottom: '0px'
                    }, 300, 'swing');
                }, 100);
                $('DIV#addcolumn').animate({
                    width: '0px',
                    padding: '0px'
                }, 300);
                setTimeout(function () {
                    $('DIV#addnewreport').animate({
                        width: '0px',
                        padding: '0px'
                    }, 300);
                }, 100);
                setTimeout(function () {
                    $('DIV#addcolumn').hide();
                    $('DIV#addnewreport').hide();
                }, 700);
            }
    </script>
    <div id="sideitems">
        <div id="addcolumn" class="solid" style="display: none;"><span style="display: none;">Add New column</span></div>
        <div id="addnewreport" class="solid" style="display: none;"><span style="display: none;">Add Report</span></div>
    </div>
    <div id="overlay" style="display: none;"></div>

    <%RenderDashboard(); %>

    <%--<asp:TextBox ID="txt" runat="server" />--%>


    <div id="saving" style="display: none;">
        <table>
            <tr>
                <td style="height: 38px; vertical-align: bottom;"><img src="Assets/Images/imgProcessingLarge.gif" /></td>
            </tr>
            <tr>
                <td style="height: 26px; vertical-align: text-top;">Saving...</td>
            </tr>
        </table>
        <div class="success" style="display: none;">
            Saving Successful!
            <asp:Button ID="cmdCloseSaving" Text="Close" OnClientClick="Close();return false;" runat="server" />
        </div>
    </div>

    <asp:Button ID="button" runat="server" Text="Save" OnClientClick="Save();" />

    <div id="reports" style="display: none;">
        <div id="addreport-news" class="chooser">Corporate News</div>
        <%--<div id="addreport-cNotes" class="chooser">Company Social Feed</div>
        <div id="addreport-dNotes" class="chooser">Downline Social Feed</div>--%>
        <div id="addreport-duesManager" class="chooser">Dues Manager</div>
        <div id="addreport-recentActivity" class="chooser">Recent Activity</div>
        <div id="addreport-recentChecks" class="chooser">This Month's Checks</div>
        <div id="addreport-volumes" class="chooser">Monthly Volumes</div>
        <!--<div id="addreport-eDownline" class="chooser">Enroller Tree</div>-->
        <!--<div id="addreport-uDownline" class="chooser">Unilevel Tree</div>-->
        <div id="addreport-currentRank" class="chooser">Current Rank</div>
        <!--<div id="addreport-rankAnalysis" class="chooser">Rank Analysis</div>-->
        <!--<div id="addreport-achievements" class="chooser">My Achievements</div>-->
        <div id="addreport-commissionEligibility" class="chooser">Commission Eligibility</div>
    </div>

    <input type="hidden" id="detailid" runat="server" />
</asp:Content>