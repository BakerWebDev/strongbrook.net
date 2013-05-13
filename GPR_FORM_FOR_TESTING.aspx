<%@ Page Language="C#" AutoEventWireup="true" CodeFile="GPR_FORM_FOR_TESTING.aspx.cs" Inherits="GPRform" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <link href="Assets/Styles/datepicker.css" rel="stylesheet" />


    <link href="Assets/Styles/site.min.css" rel="stylesheet" type="text/css" />
    <link href="Assets/Styles/schedule.css" rel="stylesheet" type="text/css"  />

    <link href="Assets/Styles/themes.min.css" rel="stylesheet" />
    <link href="Assets/Plugins/twitter.bootstrap/css/bootstrap.css" rel="stylesheet" />
    <link href="Assets/Icons/glyphicons/glyphicons.css" rel="stylesheet" />

    <script src="Assets/Plugins/twitter.bootstrap/js/bootstrap.js"></script>

    <script src="//ajax.googleapis.com/ajax/libs/jquery/1.9.1/jquery.min.js"></script>



    <script type="text/javascript">
        $(function () {
            $('#RadioButtons').hide();
            $('#Date').hide();
            $('#Time').hide();
            $('#DaysAvailable').hide();
            $('#Comments').hide();
            $('#loginButton').hide();


            
            
        });



        function showSchedule() {
            $('#Date').show();
            $('#Time').show();
            $('#DaysAvailable').hide();
        }

        function showRequest() {
            $('#DaysAvailable').show();
            $('#Date').hide();
            $('#Time').hide();
        }


        function checkDate(sender, args) {
            if (sender._selectedDate < new Date()) {
                alert("Please select a date after tomorrows date.");
                sender._selectedDate = new Date();
                // set the date back to the current date
                sender._textbox.set_Value(sender._selectedDate.format(sender._format))
            }
        }


        function getdata() {

            $('#RadioButtons').show();

            var selectedTimeZone = $("#drdlTimeZone").val();
            //alert(selectedTimeZone);
            $.post("Default7.aspx", { timeZone: selectedTimeZone }, function (data1) {
                //alert(data1);
                $("#drdlAppTime").html(data1);

            });
        }


        function checkDate(sender, args) {
            if (sender._selectedDate < new Date()) {
                alert("Please select a day no earlier than tomorrow.");
                sender._selectedDate = new Date();
                // set the date back to the current date
                sender._textbox.set_Value(sender._selectedDate.format(sender._format))
            }

            var asdf = $('#Date1').val();

            if (asdf.indexOf("Sunday") >= 0) {
                alert("Sunday");
            }
            if(asdf.indexOf("Monday") >= 0) {
                alert("Monday");
            }
            if (asdf.indexOf("Tuesday") >= 0) {
                alert("Tuesday");
            }
            if (asdf.indexOf("Wednesday") >= 0) {
                alert("Wednesday");
            }
            if (asdf.indexOf("Thursday") >= 0) {
                alert("Thursday");
            }
            if (asdf.indexOf("Friday") >= 0) {
                alert("Friday");
            }
            if (asdf.indexOf("Saturday") >= 0) {
                alert("Saturday");
            }
        }




    </script>


    <script type="text/javascript">

        $(function () {
            $('#MondayTimeFrames').hide();
            $('#MondayMorningHours').hide();
            //$('#MondayAfternoonHours').hide();



        });


        $(function MondayTimeFrames() {
            $('#MondayCheckbox').on('change', function () {
                if ($(this).is(':checked')) {
                    $('#MondayTimeFrames').show();
                } else {
                    $('#MondayTimeFrames').hide();
                    $('#MondayMorningCheckbox').prop('checked', false);
                    $('#MondayMorningHours').hide();
                    $('#MondayMorning_10to11_Checkbox').prop('checked', false);
                    $('#MondayMorning_11to12_Checkbox').prop('checked', false);
                    $('#MondayMorning_12to1_Checkbox').prop('checked', false);
                }
            });
        });


        $(function mondayMorningChecked() {
            $('#MondayMorningCheckbox').on('change', function () {
                if ($(this).is(':checked')) {
                    $('#MondayMorningHours').show();
                    $('#MondayAfternoonCheckbox').hide();
                    
                } else {
                    $('#MondayMorningHours').hide();
                    $('#MondayMorning_10to11_Checkbox').prop('checked', false);
                    $('#MondayMorning_11to12_Checkbox').prop('checked', false);
                    $('#MondayMorning_12to1_Checkbox').prop('checked', false);
                }
            });
        });




        
    </script>


</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <div class="main-wrapper">
        <div class="scheduler panel">
            <div id="Main_Content" class="panels" id="panels" style="position: relative;">
                <div class="j" id="introduction" style="display: block; position: absolute; width: 100%;">
                    <h1 class="heading">Request Your Free Personalized Game Plan Report</h1>
                    <table id="grid1">
                        <tbody>
                            <tr style="margin-top: 0px; vertical-align: top;">
                                <td id="ExplanationOfPage" colspan="2" class="leftSide">
                                    <div class="notes panel">
                                        Congratulations on taking your first step to requesting a one-on-one custom Game
                                        Plan Report. Your Game Plan Report will show all of your financial options moving
                                        you closer to achieving your retirement goals and dreams. Receiving this free report
                                        only takes a few minutes after we get a hold of you.
                                        <br />
                                        <br />
                                        Please complete the information to the right to make your request and we will attempt
                                        to contact you in the next business day.
                                        <br />
                                        <br />
                                        <i style="font-size: 80%">NOTICE: The time you select for your Game Plan is simply a
                                            request. We will do our best to accommodate your requested time, but we cannot guarantee
                                            the availability of a Strongbrook Representative at that exact time. Thank you for
                                            your understanding and we look forward to speaking with you!</i>
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
                                                        <asp:TextBox CssClass="input textfield" ID="txtPhone1" name="homephone" runat="server" Placeholder="Home Phone"
                                                            Data="Phone" />
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr id="Phone2" class="recordrow">
                                                <td class="recordvalue">
                                                    <div class="fieldvalue">
                                                        <asp:TextBox CssClass="input textfield" ID="txtPhone2" name="cellphone" runat="server" Placeholder="Cell Phone"
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
                                                        <asp:DropDownList ID="drdlTimeZone" runat="server" onchange="getdata();" />
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr id="Select_Options" class="recordrow">
                                                <td>
                                                    <div id="RadioButtons">

                                                        <input type="radio" title="Schedule" onclick="showSchedule()" name="choice" /> Schedule an Appointment
                                                        <br />
                                                        <input type="radio" title="Request" onclick="showRequest()" name="choice" /> Request to be Contacted

                                                    </div>
                                                </td>
                                            </tr>
                                            <tr id="DaysAvailable" class="recordrow">
                                                <td class="recordvalue">





                                                    <div>
                                                        <input id="MondayCheckbox" type="checkbox" />Monday<br />
         

                                                        <div id="MondayTimeFrames">
                                                            <input id="MondayMorningCheckbox" type="checkbox" />Morning<br />
                                                            
                                                        </div>

                                                        <div id="MondayMorningHours">
                                                            <input id="MondayMorning_10to11_Checkbox" type="checkbox" onclick="mondayMoring_10to11_Checked()" />Between 10 and 11<br />
                                                            <input id="MondayMorning_11to12_Checkbox" type="checkbox" onclick="mondayMoring_11to12_Checked()" />Between 11 and 12<br />
                                                        </div>


                                                        <div id="MondayAfternoonTimeFrames">
                                                            <input id="MondayAfternoonCheckbox" type="checkbox" />Afternoon<br />
                                                            
                                                        </div>


                                                        <div id="MondayAfternoonHours">
                                                            
                                                            <input id="MondayMorning_12to1_Checkbox" type="checkbox" onclick="mondayMoring_12to1_Checked()" />Between 12 and 1<br />

                                                        </div>

                                                        









                                                    </div>

                                                    <div id="spacer"><br /></div>

                                                    <div>
                                                        <input id="Checkbox2" type="checkbox" />Tuesday<br />
                                                    </div>

                                                    <div>
                                                        <input id="Checkbox1" type="checkbox" />Tuesday<br />
                                                    </div>





                                                </td>
                                            </tr>
                                            <tr id="Date_Request" class ="recordrow">
                                                <td id="DatePicker" class="dropdowns">
                                                    <div id="Date" class="time">
                                                        <asp:TextBox ID="Date1" runat="server" Text="Choose a Date" CssClass="textbox"></asp:TextBox>

                                                        
                                                        <asp:CalendarExtender ID="CalendarExtender1" runat="server" 
                                                            TargetControlID="Date1" 
                                                            CssClass="calendar"
                                                            Format="dddd, MMMM, dd yyyy"
                                                            OnClientDateSelectionChanged="checkDate"
                                                            Animated="true">
                                                        </asp:CalendarExtender>


                                                    </div>
                                                    <div id="Time" class="zone">
                                                        <asp:DropDownList ID="drdlAppTime" runat="server"></asp:DropDownList>
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr id="Comments" class="recordrow">
                                                <td class="recordvalue">
                                                    Comments
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
                                                        <asp:Button ID="submitButton" runat="server" Text="Submit" CausesValidation="true"
                                                            OnClientClick="foop();" />
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
