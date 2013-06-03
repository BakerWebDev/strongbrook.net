using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for LitmosCourse
/// </summary>
public class LitmosCourse
{
	public LitmosCourse()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    #region private settings
    private string _id;
    private string _code;
    private string _name;
    private string _active;
    private string _complete;
    private string _percentageComplete;
    private string _dateCompleted;
    private string _upToDate;
    #endregion

    #region public settings
    public string Id
    {
        get { return _id; }
        set { _id = value; }
    }

    public string Code
    {
        get { return _code; }
        set { _code = value; }
    }

    public string Name
    {
        get { return _name; }
        set { _name = value; }
    }

    public string Active
    {
        get { return _active; }
        set { _active = value; }
    }

    public string Complete
    {
        get { return _complete; }
        set { _complete = value; }
    }

    public string PercentageComplete
    {
        get { return _percentageComplete; }
        set { _percentageComplete = value; }
    }

    public string DateCompleted
    {
        get { return _dateCompleted; }
        set { _dateCompleted = value; }
    }

    public string UpToDate
    {
        get { return _upToDate; }
        set { _upToDate = value; }
    }
    #endregion
}