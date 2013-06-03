using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for LitmosUser
/// </summary>
public class LitmosUser
{
	public LitmosUser()
	{
		//
		// TODO: Add constructor logic here
		//
    }

    #region private settings
    private string _userID;
    private string _userName;
    private string _firstName;
    private string _lastName;
    private string _fullName;
    private string _email;
    private string _accessLevel;
    private string _disableMessages;
    private string _active;
    private string _skype;
    private string _phoneWork;
    private string _phoneMobile;
    private string _lastLogin;
    private string _loginKey;
    private string _timeZone;
    private string _course;
    private string _team;
    private int _ExigoCustomerID;
    private string _response;
    private Dictionary<string, string> Teams;
    #endregion

    #region public settings
    public string UserID
    {
        get { return _userID; }
        set { _userID = value; }
    }
    public string UserName
    {
        get { return _userName; }
        set { _userName = value; }
    }
    public string FirstName
    {
        get { return _firstName; }
        set { _firstName = value; }
    }
    public string LastName
    {
        get { return _lastName; }
        set { _lastName = value; }
    }
    public string FullName
    {
        get { return _fullName; }
        set { _fullName = value; }
    }
    public string Email
    {
        get { return _email; }
        set { _email = value; }
    }
    public string AccessLevel
    {
        get { return _accessLevel; }
        set { _accessLevel = value; }
    }
    public string DisableMessages
    {
        get { return _disableMessages; }
        set { _disableMessages = value; }
    }
    public string Active
    {
        get { return _active; }
        set { _active = value; }
    }
    public string Skype
    {
        get { return _skype; }
        set { _skype = value; }
    }
    public string PhoneWork
    {
        get { return _phoneWork; }
        set { _phoneWork = value; }
    }
    public string PhoneMobile
    {
        get { return _phoneMobile; }
        set { _phoneMobile = value; }
    }
    public string LastLogin
    {
        get { return _lastLogin; }
        set { _lastLogin = value; }
    }
    public string LoginKey
    {
        get { return _loginKey; }
        set { _loginKey = value; }
    }
    public string TimeZone
    {
        get { return _timeZone; }
        set { _timeZone = value; }
    }
    public string Course
    {
        get { return _course; }
        set { _course = value; }
    }
    public string Team
    {
        get { return _team; }
        set { _team = value; }
    }
    public int ExigoCustomerID
    {
        get { return _ExigoCustomerID; }
        set { _ExigoCustomerID = value; }
    }
    public string Response
    {
        get { return _response; }
        set { _response = value; }
    }
    public string SkipFirstLogin
    {
        get;
        set;
    }
    public List<LitmosCourse> Courses;
    #endregion

}