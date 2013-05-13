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

    <script src="Assets/Plugins/jquery.multiselect/arc90_multiselect.js"></script>

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

		<style type="text/css">
			@import "http://lab.arc90.com/tools/c/css/tool_global.css";	/* Style for this page */
			
			.a9multiselect {
				width: 9.9em;
				font-family: Arial, Helvetica, sans-serif;
				position: relative;
				height: 22px;
				padding: 0;
				margin: -.05em 0 1em 0;
				border: 0;
			}
			.a9multiselect .expcol-click, .a9multiselect .expcol-click-open {
				background-color: #fff;
				border: 1px solid #999;
				padding: 0;
				margin: 0;
				cursor: default;
				min-width: 9.8em;
			}
			.a9multiselect div.expcol-click {
				position: absolute;
				z-index: 104;
				height: 20px;
			}
			.a9multiselect div.expcol-click-open {
				border-bottom: 1px solid #fff;
			}
			.a9multiselect .title { 
				font-size: .8em;
				height: 1.3em;
				line-height: 1.2em;
				overflow: hidden;
				padding: .3em 1.1em .1em .5em;
				background: white url(images/multiselect.gif) no-repeat top right;
			}
			.a9multiselect .title:hover { 
				background: white url(images/multiselect-hover.gif) no-repeat top right; 
			}
			.expcol-body {
				position: absolute;
				z-index: 106;
				min-height: 1em;
				background: #e9f3f8;
				padding: .1em;
				display: block;
				font-size: 75%;
				display: none;
				margin-top: -1px;
				border: 1px solid #999;
			}
			.expcol-body ul {
				overflow: auto;
				min-height: 1em;
				min-width: 20em;
				margin: 0;
				padding: 0;
			}
			.expcol-body li { margin: 0 0 .2em 0; list-style:none; }
			.expcol-body li:hover {
				background: #ddd;
			}
			.arc90_multiselect {
				width: 12.5em;
				height: 1.35em;
				visibility: hidden;
			}
			.a9selectall {
				border-bottom: 1px solid #ccc;
			}
			
			/* Styles for page layout */
			DIV.examples {
				margin: 0 0 2em 0;
				width: 17em;
				border: 1px solid #e9e9e9;
				padding: .3em;
			}
			
			DIV.examples LABEL.examples {
				display: block;
				margin: 0 0 .2em 0;
			}

			/* Styles for page layout */
			DIV.example2 {
				margin: 0 0 2em 0;
				width: 27em;
				border: 1px solid #e9e9e9;
				padding: .3em;
			}
			
			DIV.example2 LABEL.example2 {
				display: block;
				margin: 0 0 .2em 0;
			}
		</style>






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
                                                        <%--<asp:DropDownList ID="daysAvailable" ClientIDMode="Static" runat="server" />--%>



                                                        <label class="examples"></label>
                                                        <select id="lang" name="lang" class="arc90_multiselect" multiple="multiple" size="4" title="Languages">
                                                            <option value="fr">French</option>
                                                            <option value="en" selected="false">English</option>
                                                            <option value="sp">Spanish</option>
                                                            <option value="jp" selected="false">Japanese</option>
                                                            <option value="du">Dutch</option>
                                                            <option value="kk">Klingon</option>
                                                            <option value="et">Esperanto</option>
                                                            <option value="ru">Russian</option>
                                                            <option value="ce">Canadian English</option>
                                                            <option value="ar">Arabic</option>
                                                            <option value="gr">German</option>
                                                            <option value="ar">Aramaic</option>
                                                            <option value="ch">Chinese</option>
                                                            <option value="po">Polish</option>
                                                            <option value="yi">Yiddish</option>
                                                            <option value="kr">Korean</option>
                                                            <option value="xx">A really really really very very unbeleivably crazy loooooooooooooooooooong option title</option>
                                                        </select>


                                                    </div>
                                                    
                                                    <div>
                                                        <label class="example2">asfsf</label>
                                                        <select id="lang" name="lang" class="arc90_multiselect" multiple="multiple" size="4" title="Times">
                                                            <option value="fr">9:30 to 10:00</option>
                                                            <option value="en" selected="false">10:00 to 10:30</option>
                                                            <option value="sp">Spanish</option>
                                                            <option value="jp" selected="false">10:30 to 11:00</option>
                                                            <option value="du">11:00 to 11:30</option>
                                                            <option value="kk">11:30 to 12:00</option>
                                                            <option value="et">Esperanto</option>
                                                            <option value="ru">Russian</option>
                                                            <option value="ce">Canadian English</option>
                                                            <option value="ar">Arabic</option>
                                                            <option value="gr">German</option>
                                                            <option value="ar">Aramaic</option>
                                                            <option value="ch">Chinese</option>
                                                            <option value="po">Polish</option>
                                                            <option value="yi">Yiddish</option>
                                                            <option value="kr">Korean</option>
                                                            <option value="xx">A really really really very very unbeleivably crazy loooooooooooooooooooong option title</option>
                                                        </select>


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
