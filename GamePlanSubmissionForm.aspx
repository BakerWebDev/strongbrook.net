<%@ Page EnableEventValidation="false" Language="C#" AutoEventWireup="true" CodeFile="GamePlanSubmissionForm.aspx.cs" Inherits="GPRform" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>

    <link href="Assets/Styles/calendarExtender.css" rel="stylesheet" />
    <link href="Assets/Styles/site.min.css" rel="stylesheet" type="text/css" />
    <link href="Assets/Styles/schedule.css" rel="stylesheet" type="text/css" />
    <link href="Assets/Plugins/twitter.bootstrap/css/bootstrap.css" rel="stylesheet" />
    <link href="Assets/Plugins/jquery.validate/style.css" rel="stylesheet" />

    <script src="Assets/Plugins/twitter.bootstrap/js/bootstrap.js"></script>

    <script src="//ajax.googleapis.com/ajax/libs/jquery/1.9.1/jquery.min.js"></script>

    <script type="text/javascript">

        $(function () {
            $('#RadioButtons').hide();
            $('#DatePicker').hide();
            $('#Time').hide();
            $('#LikelyAvailable').hide();
            $('#TimeZoneDropDownOnload').addClass("timeZoneDropDownOnload");
            $('#txtDate1Label').hide();
            $('#ddlAppTimeLabel').hide();
            $('#ddlfirstAvailableTimeLabel').hide();
            $('#txtCommentsLabel').addClass("txtCommentsOnLoad");
        });

        function showSchedule() {
            $('#DatePicker').show();
            $('#LikelyAvailable').hide();
            $('#Date1').addClass("txtDate1OnLoad");
            $('#txtDate1Label').show();
            $('#txtDate1Label').addClass("txtDate1LabelOnLoad");
            $('#ddlfirstAvailableTimeLabel').hide();
        }

        function showRequest() {
            $('#ddlfirstAvailableTimeLabel').show();
            $('#ddlfirstAvailableTimeLabel').addClass("ddlfirstAvailableTimeLabelOnLoad");
            $('#LikelyAvailable').show();
            $('#DatePicker').hide();
            $('#Time').hide();
            $('#txtDate1Label').hide();

        }

        function showRadioButtons() {
            $('#TimeZoneDropDownOnload').addClass("timeZoneDropDownLoaded");
            $('#RadioButtons').show();
        }

        function hideCommentsLabel() {
            $('#txtCommentsLabel').addClass("txtCommentsLoaded");
            $('#txtComments').focus();
        }

        function checkDate(sender, args) {

            if (sender._selectedDate < new Date()) {
                alert("Please select a day no earlier than tomorrow.");
                sender._selectedDate = new Date();
                // set the date back to the current date
                sender._textbox.set_Value(sender._selectedDate.format(sender._format))
            }

            var dateTextbox = $('#Date1').val();
            var theDayOfTheWeek;

            if (dateTextbox.indexOf("Sunday") >= 0) {
                theDayOfTheWeek = "Sunday";
            }
            if (dateTextbox.indexOf("Monday") >= 0) {
                theDayOfTheWeek = "Monday";
            }
            if (dateTextbox.indexOf("Tuesday") >= 0) {
                theDayOfTheWeek = "Tuesday";
            }
            if (dateTextbox.indexOf("Wednesday") >= 0) {
                theDayOfTheWeek = "Wednesday";
            }
            if (dateTextbox.indexOf("Thursday") >= 0) {
                theDayOfTheWeek = "Thursday";
            }
            if (dateTextbox.indexOf("Friday") >= 0) {
                theDayOfTheWeek = "Friday";
            }
            if (dateTextbox.indexOf("Saturday") >= 0) {
                theDayOfTheWeek = "Saturday";
            }
            if (theDayOfTheWeek == "Sunday") {
                alert("Closed Sundays, please select a day Monday thru Saturday.");
            }


            if (dateTextbox == "Wednesday, July 4, 2013") {
                alert("Closed for the July 4th Holiday.\n Please choose another day.");
            }


            // Adjust what fields are visible
            $('#txtDate1Label').hide();
            $('#txtDate1Label').addClass("txtDate1LabelLoaded");
            $('#Time').show();
            $('#ddlAppTimeLabel').show();
            $('#ddlAppTimeLabel').addClass("ddlAppTimeLabelOnLoad");


            // Store and send appointment time information to the server
            var selectedTimeZone = $("#ddlTimeZone").val();

            $.post("GamePlanSubmissionForm.aspx", { timeZone: selectedTimeZone, timeFrame: theDayOfTheWeek }, function (data1) {
                $("#ddlAppTime").html(data1);
            });
        }

        function sendTimeFrame(sender, args) {
            // First, hide the div label.
            $('#ddlAppTimeLabel').hide();
            $('#ddlAppTimeLabel').addClass("ddlAppTimeLabelLoaded");

            // Store and send appointment time information to the server
            var selectedTime = $("#ddlAppTime").val();
            $.post("GamePlanSubmissionForm.aspx", { timeFrameSelected: selectedTime }, function (data2) {
                $("#txtSelectedTimeFrame").html(data2);
            });
        }

        function selectedTimeFrame() {
            $('#ddlfirstAvailableTimeLabel').hide();
            $('#ddlfirstAvailableTimeLabel').addClass("ddlfirstAvailableTimeLabelLoaded");
        }

        function submitClicked() {
            $('#ddlfirstAvailableTimeLabel').hide();
            $('#ddlfirstAvailableTimeLabel').addClass("ddlfirstAvailableTimeLabelLoaded");
        }

    </script>


    <script type="text/javascript">
        var fname = 0;
        var lname = 0;
        var phone = 0;
        var email_ = 0;
        var timzn = 0;
        var date = 0;
        var time = 0;
        var fstAvail = 0;
        var rdosh = 0;
        var rdorq = 0;
        $(function () {
            $("#submitButton").click(function () {

                if ($('#RadioButtonSchedule').is(':checked')) {
                    var rdosh = 1;

                    if ($('#txtFirstName').val() != "") { fname = 1; };
                    if ($('#txtLastName').val() != "") { lname = 1; };
                    if ($('#txtPhone1').val() != "") { phone = 1; };
                    if ($('#txtEmail').val() != "") { email_ = 1; };

                    var a = document.getElementById("ddlAppTime");
                    var b = a.options[a.selectedIndex].value;
                    if (b != "") { time = 1; };

                    if ($('#ddlTimeZone').val() != "") { timzn = 1; };
                    if (time == 1) { date = 1 };


                    if (fname == 1 && lname == 1 && phone == 1 && email_ == 1 && timzn == 1 && date == 1 && time == 1 && rdosh == 1) {
                        this.value = 'Processing...' + $('#Date1').val() + " " + $('#ddlAppTime').val();
                    }
                    else {
                        alert('Missing required information.\nPlease double check and resubmit.');
                    }
                    //alert('fname: ' + $('#txtFirstName').val() + '\n' + 'lname: ' + $('#txtLastName').val() + '\n' + 'phone: ' + $('#txtPhone1').val() + '\n' + 'email: ' + $('#txtEmail').val() + '\n' + 'timezone: ' + $('#drdlTimeZone').val() + '\n' + 'date: ' + $('#Date1').val() + '\n' + 'time: ' + $('#drdlAppTime').val() + '\n' + 'firstAvail: ' + $('#firstAvailableTime').val());
                    //alert('fname: ' + fname + '\n' + 'lname: ' + lname + '\n' + 'phone: ' + phone + '\n' + 'email: ' + email_ + '\n' + 'timezone: ' + timzn + '\n' + 'date: ' + date + '\n' + 'time: ' + time + '\n' + 'firstAvail: ' + fstAvail);
                };


                if ($('#RadioButtonRequest').is(':checked')) {
                    var rdorq = 1;

                    if ($('#txtFirstName').val() != "") { fname = 1; };
                    if ($('#txtLastName').val() != "") { lname = 1; };
                    if ($('#txtPhone1').val() != "") { phone = 1; };
                    if ($('#txtEmail').val() != "") { email_ = 1; };
                    if ($('#ddlTimeZone').val() != "") { timzn = 1; };

                    var c = document.getElementById("firstAvailableTime");
                    var d = c.options[c.selectedIndex].value;
                    if (d != "") { fstAvail = 1; };


                    if (fname == 1 && lname == 1 && phone == 1 && email_ == 1 && timzn == 1 && fstAvail == 1 && rdorq == 1) {
                        this.value = 'Processing...' + 'Any' + " " + $('#firstAvailableTime').val() + " " + 'in the' + " " + $('#ddlTimeZone').val() + " " + 'Zone.';
                    }
                    else {
                        alert('Missing required information.\nPlease double check and resubmit.');
                    }
                    //alert('fname: ' + $('#txtFirstName').val() + '\n' + 'lname: ' + $('#txtLastName').val() + '\n' + 'phone: ' + $('#txtPhone1').val() + '\n' + 'email: ' + $('#txtEmail').val() + '\n' + 'timezone: ' + $('#drdlTimeZone').val() + '\n' + 'date: ' + $('#Date1').val() + '\n' + 'time: ' + $('#drdlAppTime').val() + '\n' + 'firstAvail: ' + $('#firstAvailableTime').val());
                    //alert('fname: ' + fname + '\n' + 'lname: ' + lname + '\n' + 'phone: ' + phone + '\n' + 'email: ' + email_ + '\n' + 'timezone: ' + timzn + '\n' + 'date: ' + date + '\n' + 'time: ' + time + '\n' + 'firstAvail: ' + fstAvail);
                };
            });
        });
    </script>


</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <div class="scheduler">
            <h1 class="heading">Request Your Free Personalized Game Plan Report</h1>
            <table id="grid1">
                <tbody>
                    <tr style="margin-top: 0px; vertical-align: top;">
                        <td id="ExplanationOfPage" colspan="2" class="leftSide">
                            <div id="LeftText">

                                <h3>Discover How Strongbrook's Power Team Can Build Your Wealth and Turbo-Charge Your Retirement Cash-Flow Through Real Estate!
                                </h3>

                                Your Personalized Game Plan Report Will:<br />
                                • Develop a custom 5 to 10 year plan to true financial freedom<br />
                                • Reveal how easily you can create positive cash-flow for life<br />
                                • Uncover 'Hidden Assets' you may not know you already have<br />
                                • Accurately predict whether your money will outlast you or not<br />
                                • Offer 3 actionable 'right-now' options to secure your retirement<br />

                                <br />
                                Schedule your FREE game plan report today! There is absolutely no commitment. Nobody will come to your home, the Game Plan Report interview is done 100% over the telephone, and at a time that is convenient for you.* 
                                        <br />
                                <br />
                                <i style="font-size: 80%">* We will do our very best to accommodate your requested date & time, 
                                            but we cannot guarantee availability of a Strongbrook Game Plan Assistant at your exact desired time. 
                                            Thank you for your understanding and we look forward to speaking with you!
                                </i>
                            </div>
                        </td>
                        <td id="FormFields" class="rightSide">
                            <table class="record">
                                <tbody enableviewstate="False">


                                    <%--
                                    <tr id="FirstName" class="recordrow">
                                        <td class="recordvalue">
                                            <div class="fieldvalue">
                                                <asp:TextBox CssClass="input textfield" ID="txtFirstName" name="FirstName" Placeholder="First Name" runat="server" Data="First Name" 
                                                    Text="FirstName" />
                                            </div>
                                        </td>
                                        <td class="recordvalue">
                                            <div class="fieldvalue">
                                                <asp:TextBox ID="txtLastName" name="LastName" Placeholder="Last Name"
                                                    runat="server" Data="Last Name" CssClass="validate[required,custom[email]] text-input"
                                                    Text="LastName" />
                                            </div>
                                        </td>
                                    </tr>

                                    <tr id="Phone1" class="recordrow">
                                        <td class="recordvalue full">
                                            <div class="fieldvalue">
                                                <asp:TextBox CssClass="input textfield" ID="txtPhone1" name="homephone" runat="server" Placeholder="Phone 1" Data="Phone" 
                                                    Text="444" />
                                            </div>
                                        </td>
                                        <td class="recordvalue">
                                            <div class="fieldvalue">
                                                <asp:TextBox CssClass="input textfield" ID="txtPhone2" name="cellphone" runat="server" Placeholder="Phone 2 (optional)" Data="Phone"
                                                    Text="555" />
                                            </div>
                                        </td>
                                    </tr>

                                    <tr id="Email" class="recordrow">
                                        <td class="recordvalue">
                                            <div class="fieldvalue">
                                                <asp:TextBox CssClass="input textfield" ID="txtEmail" Placeholder="Email" name="email" runat="server" Data="email" 
                                                    Text="aaronbaker315@me.com" />
                                            </div>
                                        </td>
--%>


                                    <tr id="FirstName_LastName" class="recordrow">
                                        <td class="recordvalue">
                                            <div class="fieldvalue">
                                                <asp:TextBox CssClass="input textfield" ID="txtFirstName" name="FirstName" Placeholder="First Name" runat="server" Data="First Name" />
                                            </div>
                                        </td>
                                        <td class="recordvalue">
                                            <div class="fieldvalue">
                                                <asp:TextBox ID="txtLastName" name="LastName" Placeholder="Last Name"
                                                    runat="server" Data="Last Name" CssClass="validate[required,custom[email]] text-input" />
                                            </div>
                                        </td>
                                    </tr>

                                    <tr id="Phone1_Phone2" class="recordrow">
                                        <td class="recordvalue full">
                                            <div class="fieldvalue">
                                                <asp:TextBox CssClass="input textfield" ID="txtPhone1" name="homephone" runat="server" Placeholder="Phone 1"
                                                    Data="Phone" />
                                            </div>
                                        </td>
                                        <td class="recordvalue">
                                            <div class="fieldvalue">
                                                <asp:TextBox CssClass="input textfield" ID="txtPhone2" name="cellphone" runat="server" Placeholder="Phone 2 (optional)"
                                                    Data="Phone" />
                                            </div>
                                        </td>
                                    </tr>

                                    <tr id="Email_NetWorth" class="recordrow">
                                        <td class="recordvalue">
                                            <div class="fieldvalue">
                                                <asp:TextBox CssClass="input textfield" ID="txtEmail" Placeholder="Email" name="email" runat="server" Data="email" />
                                            </div>
                                        </td>
                                        <td class="recordvalue">
                                            <div class="fieldvalue">
                                                <asp:DropDownList ID="netWorth" ClientIDMode="Static" runat="server" />
                                            </div>
                                        </td>
                                    </tr>

                                    <tr id="TimeZone_Comments" class="recordrow">
                                        <td id="TimeZoneDropDown_DatePicker_TimeDropDown_LikelyAvailable_RadioButtons" class="recordvalue">
                                            <div class="fieldvalueTimeSelection">
                                                <div id="TimeZoneDropDown">
                                                    <asp:DropDownList ID="ddlTimeZone" runat="server" onchange="showRadioButtons();" />
                                                </div>
                                                <div id="TimeZoneDropDownOnload">Select a Time Zone</div>
                                                <div id="DatePicker">
                                                    <asp:TextBox ID="Date1" runat="server"></asp:TextBox>
                                                    <asp:CalendarExtender ID="CalendarExtender1" runat="server"
                                                        TargetControlID="Date1"
                                                        CssClass="calendar"
                                                        Format="dddd, MMMM dd, yyyy"
                                                        OnClientDateSelectionChanged="checkDate"
                                                        Animated="true">
                                                    </asp:CalendarExtender>
                                                </div>
                                                <div id="Time">
                                                    <asp:DropDownList ID="ddlAppTime" runat="server" onchange="sendTimeFrame();"></asp:DropDownList>
                                                </div>
                                                <div id="LikelyAvailable" class="recordvalue">
                                                    <asp:DropDownList ID="firstAvailableTime" runat="server" onchange="selectedTimeFrame();"></asp:DropDownList>
                                                </div>
                                                <div id="RadioButtons">
                                                    <label for="RadioButtonSchedule">Schedule an Appointment</label>
                                                    <input id="RadioButtonSchedule" runat="server" type="radio" title="Schedule" onclick="showSchedule()" name="choice" />
                                                    <br />
                                                    <label for="RadioButtonRequest">Request to be Contacted</label>
                                                    <input id="RadioButtonRequest" runat="server" type="radio" title="Request" onclick="showRequest()" name="choice" />
                                                </div>

                                                <div id="txtDate1Label">Choose a Date</div>
                                                <div id="ddlAppTimeLabel">Select a Time</div>
                                                <div id="ddlfirstAvailableTimeLabel">Best Time to Call</div>

                                            </div>
                                        </td>
                                        <td id="Comments" class="recordvalue" style="vertical-align: top;">
                                            <div class="fieldvalue">
                                                <div class="border line" id="commentsborder" style="display: inline-block;">
                                                    <div class="textareabounds" id="commentsbody" style="width: 200px; min-height: 50px;">
                                                        <textarea id="txtComments" class="input textfield" cols="20" rows="5" runat="server" onclick="hideCommentsLabel();"></textarea>
                                                        <div id="txtCommentsLabel" onclick="hideCommentsLabel();">Any Special Instructions?</div>
                                                    </div>
                                                    <div class="border-resize" id="commentsresize">
                                                    </div>
                                                    <div class="topleft">
                                                    </div>
                                                    <div class="topright">
                                                    </div>
                                                    <div class="bottomleft">
                                                    </div>
                                                    <div class="bottomright">
                                                    </div>
                                                </div>
                                            </div>
                                        </td>
                                    </tr>

                                    <tr id="spacer"><td id="spanner" colspan="2"></td></tr>

                                    <tr id="Submit_Button">
                                        <td colspan="2">
                                            <div style="width: 400px;">
                                                <asp:Button ID="submitButton" runat="server" Text="Submit"
                                                    CausesValidation="true" Style="float: right;" OnClientClick="submitClicked();" />
                                            </div>
                                            <div style="width: 200px; height: 25px; float: right; padding-top:7px; font-weight:bold; font-size:11px;">
                                                Please, only click Submit one time!
                                            </div>
                                        </td>
                                    </tr>

                                </tbody>
                            </table>
                        </td>
                    </tr>
                </tbody>
            </table>
            <div class="well well-small well-white">
                <div id="Note">
                    <h3><strong>Note:</strong> Although the Game Plan Report system functions in different browsers, performance of this system is optimized in Google Chrome. If you are using another browser and experiencing technical difficulties, we recommend you download Google Chrome and resubmit your Game Plan Report request using this browser.</h3>
                </div>
            </div>
        </div>
    </form>

    <!-- This JavaScript must go below everything else! -->

    <!-- Validate plugin -->
    <script src="Assets/Plugins/jquery.validate/assets/js/jquery.validate.min.js"></script>

    <!-- Scripts specific to this page -->
    <script src="Assets/Plugins/jquery.validate/script.js"></script>


</body>
</html>
