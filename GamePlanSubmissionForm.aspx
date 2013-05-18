<%@ Page EnableEventValidation="false" Language="C#" AutoEventWireup="true" CodeFile="GamePlanSubmissionForm.aspx.cs" Inherits="GPRform" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <link href="Assets/Styles/calendarExtender.css" rel="stylesheet" />
    <link href="Assets/Styles/site.min.css" rel="stylesheet" type="text/css" />
    <link href="Assets/Styles/schedule.css" rel="stylesheet" type="text/css"  />
    <link href="Assets/Plugins/twitter.bootstrap/css/bootstrap.css" rel="stylesheet" />
    <link href="Assets/Icons/glyphicons/glyphicons.css" rel="stylesheet" />

    <script src="Assets/Plugins/twitter.bootstrap/js/bootstrap.js"></script>

    <script src="//ajax.googleapis.com/ajax/libs/jquery/1.9.1/jquery.min.js"></script>

    <script type="text/javascript">

        $(function () {
            $('#RadioButtons').hide();
            $('#Date').hide();
            $('#Time').hide();
            $('#LikelyAvailable').hide();
        });

        function showSchedule() {
            $('#Date').show();
            $('#Time').show();
            $('#LikelyAvailable').hide();
        }

        function showRequest() {
            $('#LikelyAvailable').show();
            $('#Date').hide();
            $('#Time').hide();
        }

        function showRadioButtons() {

            $('#RadioButtons').show();

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
                //CalendarExtender1.Enabled = false;
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


            var selectedTimeZone = $("#drdlTimeZone").val();

            $.post("GPR_FORM_FOR_TESTING.aspx", { timeZone: selectedTimeZone, timeFrame: theDayOfTheWeek }, function (data1) {
                $("#drdlAppTime").html(data1);

            });
        }

        function sendTimeFrame(sender, args) {


            var selectedTimeFrame = $("#drdlAppTime").val();
            $.post("GPR_FORM_FOR_TESTING.aspx", { timeFrameSelected: selectedTimeFrame }, function (data2) {
                $("#txtSelectedTimeFrame").html(data2);

            });
        }

    </script>

</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <div class="main-wrapper">
        <div class="scheduler panel">
            <div id="Main_Content" class="panels" id="panels" style="position: relative;">
                <div id="introduction" style="display: block; position: absolute; width: 100%;">
                    <h1 class="heading">Request Your Free Personalized Game Plan Report</h1>
                    <table id="grid1">
                        <tbody>
                            <tr style="margin-top: 0px; vertical-align: top;">
                                <td id="ExplanationOfPage" colspan="2" class="leftSide">
                                    <div id="LeftText">

                                        <h3>
                                            Discover How Strongbrook's Power Team Can Build Your Wealth and Turbo-Charge Your Retirement Cash-Flow Through Real Estate!
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
                                        <i style="font-size: 80%">
                                            * We will do our very best to accommodate your requested date & time, 
                                            but we cannot guarantee availability of a Strongbrook Game Plan Assistant at your exact desired time. 
                                            Thank you for your understanding and we look forward to speaking with you!
                                        </i>
                                    </div>
                                </td>
                                <td id="FormFields" class="rightSide">
                                    <table class="record">
                                        <tbody enableviewstate="False">
                                            <tr id="FirstName" class="recordrow">
                                                <td class="recordvalue">
                                                    <div class="fieldvalue">
                                                        <asp:TextBox CssClass="input textfield" ID="txtFirstName" name="FirstName" Placeholder="First Name"
                                                            runat="server" Data="First Name" />
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr id="LastName" class="recordrow">
                                                <td class="recordvalue">
                                                    <div class="fieldvalue">
                                                        <asp:TextBox CssClass="input textfield" ID="txtLastName" name="LastName" Placeholder="Last Name"
                                                            runat="server" Data="Last Name" />
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr id="Phone1" class="recordrow">
                                                <td class="recordvalue full">
                                                    <div class="fieldvalue">
                                                        <asp:TextBox CssClass="input textfield" ID="txtPhone1" name="homephone" runat="server" Placeholder="Phone 1"
                                                            Data="Phone" />
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr id="Phone2" class="recordrow">
                                                <td class="recordvalue">
                                                    <div class="fieldvalue">
                                                        <asp:TextBox CssClass="input textfield" ID="txtPhone2" name="cellphone" runat="server" Placeholder="Phone 2"
                                                            Data="Phone" />
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr id="Email" class="recordrow">
                                                <td class="recordvalue">
                                                    <div class="fieldvalue">
                                                        <asp:TextBox CssClass="input textfield" ID="txtEmail" Placeholder="email"
                                                            name="email" runat="server" Data="email" />
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr id="NetWorth" class="recordrow">
                                                <td class="dropdowns">
                                                    <div class="fieldlabel">
                                                        <asp:DropDownList ID="netWorth" ClientIDMode="Static" runat="server" />
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr id="TimeZone" class="recordrow">
                                                <td class="recordvalue">
                                                    <div>
                                                        <asp:DropDownList ID="drdlTimeZone" runat="server" onchange="showRadioButtons();" />
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr id="Date_Request" class ="recordrow">
                                                <td id="DatePicker" class="recordvalue">
                                                    <div id="Date">
                                                        <asp:TextBox ID="Date1" runat="server" Text="Choose a Date" CssClass="textbox"></asp:TextBox>
                                                        <asp:CalendarExtender ID="CalendarExtender1" runat="server"
                                                            TargetControlID="Date1" 
                                                            CssClass="calendar"
                                                            Format="dddd -- MMMM, dd yyyy"
                                                            OnClientDateSelectionChanged="checkDate"
                                                            Animated="true">
                                                        </asp:CalendarExtender>
                                                    </div>
                                                </td>

                                            </tr>
                                            <tr id="Time_Request" class ="recordrow">
                                                <td id="TimePicker" class="recordvalue">
                                                    <div id="Time">
                                                        <asp:DropDownList ID="drdlAppTime" runat="server" onchange="sendTimeFrame();" ></asp:DropDownList>
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr id="Select_Options" class="recordrow">
                                                <td>
                                                    <div id="RadioButtons">

                                                        <input id="RadioButtonSchedule" runat="server" type="radio" title="Schedule" onclick="showSchedule()" name="choice" /> Schedule an Appointment
                                                        <br />
                                                        <input id="RadioButtonRequest" runat="server" type="radio" title="Request" onclick="showRequest()" name="choice" /> Request to be Contacted

                                                    </div>
                                                </td>
                                            </tr>
                                            <tr id="LikelyAvailable" class="recordrow">
                                                <td class="recordvalue">
                                                    <asp:DropdownList ID="firstAvailableTime" runat="server"></asp:DropdownList>
                                                </td>
                                            </tr>
                                            <tr id="Comments" class="recordrow">
                                                <td class="recordvalue">
                                                    Any Special Instructions?
                                                    <div class="fieldvalue">
                                                        <div class="border line" id="commentsborder" style="display: inline-block;">
                                                            <div class="textareabounds" id="commentsbody" style="width: 200px; min-height: 50px;">
                                                                <textarea id="txtComments" class="input textfield" cols="20" rows="5" runat="server" ></textarea>
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
                                            <tr>
                                                <td>
                                                    <div id="loginButton" class="login-button">
                                                        <asp:Button ID="submitButton" runat="server" Text="Submit" CausesValidation="true" />
                                                    </div>
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </div>
        </div>
    </div>
    </form>
</body>
</html>
