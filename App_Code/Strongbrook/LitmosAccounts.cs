using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Xml;
using Exigo.API;
using ExigoWebService;

/// <summary>
/// Class containing methods to create, modify, and delete
/// Litmos Users
/// </summary>
public class LitmosAccounts
{
	public LitmosAccounts()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    #region settings
    Uri uri = new Uri("https://api.litmos.com/v1.svc");
    private string apiKey
    {
        get { return "558755F0-2546-48CE-925C-18681D4D5909"; }
    }
    private string source
    {
        get { return "StrongBrook"; }
    }
    #endregion

    #region Creation Methods
    public string Create_LitmosUser(LitmosUser user)
    {
        string ResponseMessage = "";

        try
        {
            #region XML Creation
            //Create XML for User Creation
            MemoryStream stream = new MemoryStream();

            XmlTextWriter writer = new XmlTextWriter(stream, Encoding.UTF8);
            writer.Formatting = Formatting.Indented;
            writer.WriteStartDocument();
            writer.WriteStartElement("User");
            writer.WriteStartElement("UserName");
            writer.WriteString(user.UserName);
            writer.WriteEndElement();
            writer.WriteStartElement("FirstName");
            writer.WriteString(user.FirstName);
            writer.WriteEndElement();
            writer.WriteStartElement("LastName");
            writer.WriteString(user.LastName);
            writer.WriteEndElement();
            writer.WriteStartElement("Email");
            writer.WriteString(user.Email);
            writer.WriteEndElement();
            writer.WriteStartElement("DisableMessages");
            writer.WriteString("false");
            writer.WriteEndElement();
            writer.WriteStartElement("Active");
            writer.WriteString("true");
            writer.WriteEndElement();
            writer.WriteStartElement("PhoneWork");
            writer.WriteString(user.PhoneWork);
            writer.WriteEndElement();
            writer.WriteStartElement("SkipFirstLogin");
            writer.WriteString("false");
            writer.WriteEndElement();
            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Flush();
            writer.Close();
            #endregion

            //take XML into an array of bytes for transfer
            byte[] dataByte = stream.ToArray();

            //create request and send byte[] to Litmos
            HttpWebRequest req = WebRequest.Create(uri + "/users?apikey=" + apiKey + "&source=" + source) as HttpWebRequest;
            req.Method = "POST";

            req.ContentType = "text/xml";
            req.KeepAlive = false;
            req.Timeout = 400000;
            req.ContentLength = dataByte.Length;

            Stream POSTstream = req.GetRequestStream();
            POSTstream.Write(dataByte, 0, dataByte.Length);

            HttpWebResponse res = req.GetResponse() as HttpWebResponse;

            //If successful, examine results
            if (res.StatusCode == HttpStatusCode.Created)
            {
                string xmlString = new StreamReader(res.GetResponseStream(), Encoding.UTF8).ReadToEnd();
                using (XmlReader reader = XmlReader.Create(new StringReader(xmlString)))
                {
                    //Read the userID given back from Litmos
                    reader.ReadToFollowing("Id");
                    user.UserID = reader.ReadElementContentAsString();
                }

                ResponseMessage = user.UserID;

                UpdateExigoAccount(user);
            }
            else
            {
                ResponseMessage += "There was an error creating your account";
            }
        }
        catch (Exception ex)
        {
            ResponseMessage += ex.ToString();
        }

        return ResponseMessage;
    }

    public string Create_LitmosUser(List<LitmosUser> users)
    {
        string ResponseMessage = "";

        foreach (LitmosUser user in users)
        {
            try
            {
                #region XML Creation
                //Create XML for User Creation
                MemoryStream stream = new MemoryStream();

                XmlTextWriter writer = new XmlTextWriter(stream, Encoding.UTF8);
                writer.Formatting = Formatting.Indented;
                writer.WriteStartDocument();
                writer.WriteStartElement("User");
                writer.WriteStartElement("UserName");
                writer.WriteString(user.UserName);
                writer.WriteEndElement();
                writer.WriteStartElement("FirstName");
                writer.WriteString(user.FirstName);
                writer.WriteEndElement();
                writer.WriteStartElement("LastName");
                writer.WriteString(user.LastName);
                writer.WriteEndElement();
                writer.WriteStartElement("Email");
                writer.WriteString(user.Email);
                writer.WriteEndElement();
                writer.WriteStartElement("DisableMessages");
                writer.WriteString("false");
                writer.WriteEndElement();
                writer.WriteStartElement("Active");
                writer.WriteString("true");
                writer.WriteEndElement();
                writer.WriteStartElement("PhoneWork");
                writer.WriteString(user.PhoneWork);
                writer.WriteEndElement();
                writer.WriteStartElement("SkipFirstLogin");
                writer.WriteString("false");
                writer.WriteEndElement();
                writer.WriteEndElement();
                writer.WriteEndDocument();
                writer.Flush();
                writer.Close();
                #endregion

                //take XML into an array of bytes for transfer
                byte[] dataByte = stream.ToArray();

                //create request and send byte[] to Litmos
                HttpWebRequest req = WebRequest.Create(uri + "/users?apikey=" + apiKey + "&source=" + source) as HttpWebRequest;
                req.Method = "POST";

                req.ContentType = "text/xml";
                req.KeepAlive = false;
                req.Timeout = 400000;
                req.ContentLength = dataByte.Length;

                Stream POSTstream = req.GetRequestStream();
                POSTstream.Write(dataByte, 0, dataByte.Length);

                HttpWebResponse res = req.GetResponse() as HttpWebResponse;

                //If successful, examine results
                if (res.StatusCode == HttpStatusCode.Created)
                {
                    string xmlString = new StreamReader(res.GetResponseStream(), Encoding.UTF8).ReadToEnd();
                    using (XmlReader reader = XmlReader.Create(new StringReader(xmlString)))
                    {
                        //Read the userID given back from Litmos
                        reader.ReadToFollowing("Id");
                        user.UserID = reader.ReadElementContentAsString();
                    }


                    UpdateExigoAccount(user);
                }
                else
                {
                    ResponseMessage += "There was an error creating your account";
                }
            }
            catch (Exception ex)
            {
                ResponseMessage += ex.ToString();
            }
        }

        if (ResponseMessage == "")
        {
            ResponseMessage = "User Accounts created successfully";
        }

        return ResponseMessage;
    }

    public string Create_LitmosUser(LitmosUser user, string course)
    {
        string ResponseMessage = "";

        //First Create the User
        #region User Creation
        try
        {
            #region XML Creation
            //Create XML for User Creation
            MemoryStream stream = new MemoryStream();

            XmlTextWriter writer = new XmlTextWriter(stream, Encoding.UTF8);
            writer.Formatting = Formatting.Indented;
            writer.WriteStartDocument();
            writer.WriteStartElement("User");
            writer.WriteStartElement("UserName");
            writer.WriteString(user.UserName);
            writer.WriteEndElement();
            writer.WriteStartElement("FirstName");
            writer.WriteString(user.FirstName);
            writer.WriteEndElement();
            writer.WriteStartElement("LastName");
            writer.WriteString(user.LastName);
            writer.WriteEndElement();
            writer.WriteStartElement("Email");
            writer.WriteString(user.Email);
            writer.WriteEndElement();
            writer.WriteStartElement("DisableMessages");
            writer.WriteString("false");
            writer.WriteEndElement();
            writer.WriteStartElement("Active");
            writer.WriteString("true");
            writer.WriteEndElement();
            writer.WriteStartElement("PhoneWork");
            writer.WriteString(user.PhoneWork);
            writer.WriteEndElement();
            writer.WriteStartElement("SkipFirstLogin");
            writer.WriteString("false");
            writer.WriteEndElement();
            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Flush();
            writer.Close();
            #endregion

            #region API Calls
            //take XML into an array of bytes for transfer
            byte[] dataByte = stream.ToArray();

            //create request and send byte[] to Litmos
            HttpWebRequest req = WebRequest.Create(uri + "/users?apikey=" + apiKey + "&source=" + source) as HttpWebRequest;
            req.Method = "POST";

            req.ContentType = "text/xml";
            req.KeepAlive = false;
            req.Timeout = 400000;
            req.ContentLength = dataByte.Length;

            Stream POSTstream = req.GetRequestStream();
            POSTstream.Write(dataByte, 0, dataByte.Length);

            HttpWebResponse res = req.GetResponse() as HttpWebResponse;

            //If successful, examine results
            if (res.StatusCode == HttpStatusCode.Created)
            {
                string xmlString = new StreamReader(res.GetResponseStream(), Encoding.UTF8).ReadToEnd();
                using (XmlReader reader = XmlReader.Create(new StringReader(xmlString)))
                {
                    //Read the userID given back from Litmos
                    reader.ReadToFollowing("Id");
                    user.UserID = reader.ReadElementContentAsString();
                }

                ResponseMessage = "User Created Successfully";

                //Save the ID to their Exigo Account
                UpdateExigoAccount(user);
            }
            else
            {
                ResponseMessage += "There was an error creating your account";
            }
            #endregion
        }
        catch (Exception ex)
        {
            ResponseMessage += ex.ToString();
        }
        #endregion

        //Then Add the Course to the User
        #region Course Creation
        try
        {
            #region XML
            //Then use the response ID to assign them to a course
            MemoryStream courseStream = new MemoryStream();

            XmlTextWriter courseWriter = new XmlTextWriter(courseStream, Encoding.UTF8);
            courseWriter.Formatting = Formatting.Indented;
            courseWriter.WriteStartDocument();
            courseWriter.WriteStartElement("Courses");
            courseWriter.WriteStartElement("Course");
            courseWriter.WriteStartElement("Id");
            courseWriter.WriteString(user.Course);
            courseWriter.WriteEndElement();
            courseWriter.WriteEndElement();
            courseWriter.WriteEndElement();
            courseWriter.WriteEndDocument();
            courseWriter.Flush();
            courseWriter.Close();
            #endregion

            #region API calls
            byte[] dataByte = courseStream.ToArray();

            HttpWebRequest req = WebRequest.Create(uri + "/users/" + user.UserID + "/courses?apikey=" + apiKey + "&source=" + source) as HttpWebRequest;
            req.Method = "POST";

            req.ContentType = "text/xml";
            req.KeepAlive = false;
            req.Timeout = 400000;
            req.ContentLength = dataByte.Length;

            Stream POSTstream = req.GetRequestStream();
            POSTstream.Write(dataByte, 0, dataByte.Length);

            HttpWebResponse res = req.GetResponse() as HttpWebResponse;

            if (res.StatusCode != HttpStatusCode.Created)
            {
                ResponseMessage += "Could not add course";
            }
            else
                ResponseMessage += ".  Team added Successfully";
            #endregion
        }
        catch (Exception ex)
        {
            ResponseMessage += ex.ToString();
        }
        #endregion

        return ResponseMessage;
    }

    public string Create_LitmosUser(string team, LitmosUser user)
    {
        string ResponseMessage = "";

        //First Create the User
        #region User Creation
        try
        {
            #region XML Creation
            //Create XML for User Creation
            MemoryStream stream = new MemoryStream();

            XmlTextWriter writer = new XmlTextWriter(stream, Encoding.UTF8);
            writer.Formatting = Formatting.Indented;
            writer.WriteStartDocument();
            writer.WriteStartElement("User");
            writer.WriteStartElement("UserName");
            writer.WriteString(user.UserName);
            writer.WriteEndElement();
            writer.WriteStartElement("FirstName");
            writer.WriteString(user.FirstName);
            writer.WriteEndElement();
            writer.WriteStartElement("LastName");
            writer.WriteString(user.LastName);
            writer.WriteEndElement();
            writer.WriteStartElement("Email");
            writer.WriteString(user.Email);
            writer.WriteEndElement();
            writer.WriteStartElement("DisableMessages");
            writer.WriteString("false");
            writer.WriteEndElement();
            writer.WriteStartElement("Active");
            writer.WriteString("true");
            writer.WriteEndElement();
            writer.WriteStartElement("PhoneWork");
            writer.WriteString(user.PhoneWork);
            writer.WriteEndElement();
            writer.WriteStartElement("SkipFirstLogin");
            writer.WriteString("false");
            writer.WriteEndElement();
            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Flush();
            writer.Close();
            #endregion

            #region API Calls
            //take XML into an array of bytes for transfer
            byte[] dataByte = stream.ToArray();

            //create request and send byte[] to Litmos
            HttpWebRequest req = WebRequest.Create(uri + "/users?apikey=" + apiKey + "&source=" + source) as HttpWebRequest;
            req.Method = "POST";

            req.ContentType = "text/xml";
            req.KeepAlive = false;
            req.Timeout = 400000;
            req.ContentLength = dataByte.Length;

            Stream POSTstream = req.GetRequestStream();
            POSTstream.Write(dataByte, 0, dataByte.Length);

            HttpWebResponse res = req.GetResponse() as HttpWebResponse;

            //If successful, examine results
            if (res.StatusCode == HttpStatusCode.Created)
            {
                string xmlString = new StreamReader(res.GetResponseStream(), Encoding.UTF8).ReadToEnd();
                using (XmlReader reader = XmlReader.Create(new StringReader(xmlString)))
                {
                    //Read the userID given back from Litmos
                    reader.ReadToFollowing("Id");
                    user.UserID = reader.ReadElementContentAsString();
                }

                ResponseMessage = "Success";

                //Save the ID to their Exigo Account
                UpdateExigoAccount(user);
            }
            else
            {
                ResponseMessage += "There was an error creating your account";
            }
            #endregion

        }
        catch (Exception ex)
        {
            ResponseMessage += ex.ToString();
        }
        #endregion

        //Then Add the Team to the User
        #region Team Creation
        try
        {
            #region XML
            MemoryStream teamStream = new MemoryStream();

            XmlTextWriter teamWriter = new XmlTextWriter(teamStream, Encoding.UTF8);
            teamWriter.Formatting = Formatting.Indented;
            teamWriter.WriteStartDocument();
            teamWriter.WriteStartElement("Users");
            teamWriter.WriteStartElement("User");
            teamWriter.WriteStartElement("Id");
            teamWriter.WriteString(user.UserID);
            teamWriter.WriteEndElement();
            teamWriter.WriteStartElement("UserName");
            teamWriter.WriteString(user.UserName);
            teamWriter.WriteEndElement();
            teamWriter.WriteStartElement("FirstName");
            teamWriter.WriteString(user.FirstName);
            teamWriter.WriteEndElement();
            teamWriter.WriteStartElement("LastName");
            teamWriter.WriteString(user.LastName);
            teamWriter.WriteEndElement();
            teamWriter.WriteEndElement();
            teamWriter.WriteEndElement();
            teamWriter.WriteEndDocument();
            teamWriter.Flush();
            teamWriter.Close();
            #endregion

            #region API Calls
            byte[] dataByte = teamStream.ToArray();

            HttpWebRequest req = WebRequest.Create(uri + "/teams/" + team + "/users?apikey=" + apiKey + "&source=" + source) as HttpWebRequest;
            req.Method = "POST";

            req.ContentType = "text/xml";
            req.KeepAlive = false;
            req.Timeout = 400000;
            req.ContentLength = dataByte.Length;

            Stream POSTstream = req.GetRequestStream();
            POSTstream.Write(dataByte, 0, dataByte.Length);

            HttpWebResponse res = req.GetResponse() as HttpWebResponse;

            if (res.StatusCode != HttpStatusCode.Created)
            {
                ResponseMessage += "Could not assign team";
            }
            #endregion
        }
        catch (Exception ex)
        {
            ResponseMessage += ex.ToString();
        }
        #endregion

        return ResponseMessage;
    }

    public string Create_LitmosUser(LitmosUser user, string team, string course)
    {
        string ResponseMessage = "";
        //First Create the User
        #region User Creation
        try
        {
            #region XML Creation
            //Create XML for User Creation
            MemoryStream stream = new MemoryStream();

            XmlTextWriter writer = new XmlTextWriter(stream, Encoding.UTF8);
            writer.Formatting = Formatting.Indented;
            writer.WriteStartDocument();
            writer.WriteStartElement("User");
            writer.WriteStartElement("UserName");
            writer.WriteString(user.UserName);
            writer.WriteEndElement();
            writer.WriteStartElement("FirstName");
            writer.WriteString(user.FirstName);
            writer.WriteEndElement();
            writer.WriteStartElement("LastName");
            writer.WriteString(user.LastName);
            writer.WriteEndElement();
            writer.WriteStartElement("Email");
            writer.WriteString(user.Email);
            writer.WriteEndElement();
            writer.WriteStartElement("DisableMessages");
            writer.WriteString("false");
            writer.WriteEndElement();
            writer.WriteStartElement("Active");
            writer.WriteString("true");
            writer.WriteEndElement();
            writer.WriteStartElement("PhoneWork");
            writer.WriteString(user.PhoneWork);
            writer.WriteEndElement();
            writer.WriteStartElement("SkipFirstLogin");
            writer.WriteString("false");
            writer.WriteEndElement();
            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Flush();
            writer.Close();
            #endregion

            #region API Calls
            //take XML into an array of bytes for transfer
            byte[] dataByte = stream.ToArray();

            //create request and send byte[] to Litmos
            HttpWebRequest req = WebRequest.Create(uri + "/users?apikey=" + apiKey + "&source=" + source) as HttpWebRequest;
            req.Method = "POST";

            req.ContentType = "text/xml";
            req.KeepAlive = false;
            req.Timeout = 400000;
            req.ContentLength = dataByte.Length;

            Stream POSTstream = req.GetRequestStream();
            POSTstream.Write(dataByte, 0, dataByte.Length);

            HttpWebResponse res = req.GetResponse() as HttpWebResponse;

            //If successful, examine results
            if (res.StatusCode == HttpStatusCode.Created)
            {
                string xmlString = new StreamReader(res.GetResponseStream(), Encoding.UTF8).ReadToEnd();
                using (XmlReader reader = XmlReader.Create(new StringReader(xmlString)))
                {
                    //Read the userID given back from Litmos
                    reader.ReadToFollowing("Id");
                    user.UserID = reader.ReadElementContentAsString();
                }

                ResponseMessage = "User Created Successfully";

                //Save the ID to their Exigo Account
                UpdateExigoAccount(user);
            }
            else
            {
                ResponseMessage += "There was an error creating your account";
            }
            #endregion

        }
        catch (Exception ex)
        {
            ResponseMessage += ex.ToString();
        }
        #endregion

        //Then Add the Team to the User
        #region Team Creation
        try
        {
            #region XML
            MemoryStream teamStream = new MemoryStream();

            XmlTextWriter teamWriter = new XmlTextWriter(teamStream, Encoding.UTF8);
            teamWriter.Formatting = Formatting.Indented;
            teamWriter.WriteStartDocument();
            teamWriter.WriteStartElement("Users");
            teamWriter.WriteStartElement("User");
            teamWriter.WriteStartElement("Id");
            teamWriter.WriteString(user.UserID);
            teamWriter.WriteEndElement();
            teamWriter.WriteStartElement("UserName");
            teamWriter.WriteString(user.UserName);
            teamWriter.WriteEndElement();
            teamWriter.WriteStartElement("FirstName");
            teamWriter.WriteString(user.FirstName);
            teamWriter.WriteEndElement();
            teamWriter.WriteStartElement("LastName");
            teamWriter.WriteString(user.LastName);
            teamWriter.WriteEndElement();
            teamWriter.WriteEndElement();
            teamWriter.WriteEndElement();
            teamWriter.WriteEndDocument();
            teamWriter.Flush();
            teamWriter.Close();
            #endregion

            #region API Calls
            byte[] dataByte = teamStream.ToArray();

            HttpWebRequest req = WebRequest.Create(uri + "/users/" + user.UserID + "/courses?apikey=" + apiKey + "&source=" + source) as HttpWebRequest;
            req.Method = "POST";

            req.ContentType = "text/xml";
            req.KeepAlive = false;
            req.Timeout = 400000;
            req.ContentLength = dataByte.Length;

            Stream POSTstream = req.GetRequestStream();
            POSTstream.Write(dataByte, 0, dataByte.Length);

            HttpWebResponse res = req.GetResponse() as HttpWebResponse;

            if (res.StatusCode != HttpStatusCode.Created)
            {
                ResponseMessage += "Could not add course";
            }
            #endregion
        }
        catch (Exception ex)
        {
            ResponseMessage += ex.ToString();
        }
        #endregion

        //Then Add the Course to the User
        #region Course Creation
        try
        {
            #region XML
            //Then use the response ID to assign them to a course
            MemoryStream courseStream = new MemoryStream();

            XmlTextWriter courseWriter = new XmlTextWriter(courseStream, Encoding.UTF8);
            courseWriter.Formatting = Formatting.Indented;
            courseWriter.WriteStartDocument();
            courseWriter.WriteStartElement("Courses");
            courseWriter.WriteStartElement("Course");
            courseWriter.WriteStartElement("Id");
            courseWriter.WriteString(user.Course);
            courseWriter.WriteEndElement();
            courseWriter.WriteEndElement();
            courseWriter.WriteEndElement();
            courseWriter.WriteEndDocument();
            courseWriter.Flush();
            courseWriter.Close();
            #endregion

            #region API calls
            byte[] dataByte = courseStream.ToArray();

            HttpWebRequest req = WebRequest.Create(uri + "/users/" + user.UserID + "/courses?apikey=" + apiKey + "&source=" + source) as HttpWebRequest;
            req.Method = "POST";

            req.ContentType = "text/xml";
            req.KeepAlive = false;
            req.Timeout = 400000;
            req.ContentLength = dataByte.Length;

            Stream POSTstream = req.GetRequestStream();
            POSTstream.Write(dataByte, 0, dataByte.Length);

            HttpWebResponse res = req.GetResponse() as HttpWebResponse;

            if (res.StatusCode != HttpStatusCode.Created)
            {
                ResponseMessage += "Could not add course";
            }
            else
                ResponseMessage += ".  Team added Successfully";
            #endregion
        }
        catch (Exception ex)
        {
            ResponseMessage += ex.ToString();
        }
        #endregion

        return ResponseMessage;
    }
    #endregion

    #region Get Methods
    public LitmosUser Get_UserInfo(string userID)
    {
        LitmosUser user = new LitmosUser();
        try
        {
            HttpWebRequest req = WebRequest.Create(uri + "/users/" + userID + "?apikey=" + apiKey + "&source=" + source) as HttpWebRequest;
            req.Method = "GET";

            HttpWebResponse res = req.GetResponse() as HttpWebResponse;

            if (res.StatusCode == HttpStatusCode.Created)
            {
                string xmlString = new StreamReader(res.GetResponseStream(), Encoding.UTF8).ReadToEnd();
                using (XmlReader reader = XmlReader.Create(new StringReader(xmlString)))
                {
                    reader.ReadToFollowing("UserName");
                    user.UserName = reader.ReadElementContentAsString();
                    reader.ReadToFollowing("FirstName");
                    user.FirstName = reader.ReadElementContentAsString();
                    reader.ReadToFollowing("LastName");
                    user.LastName = reader.ReadElementContentAsString();
                    reader.ReadToFollowing("FullName");
                    user.FullName = reader.ReadElementContentAsString();
                    reader.ReadToFollowing("Email");
                    user.Email = reader.ReadElementContentAsString();
                    reader.ReadToFollowing("AccessLevel");
                    user.AccessLevel = reader.ReadElementContentAsString();
                    reader.ReadToFollowing("DisableMessages");
                    user.DisableMessages = reader.ReadElementContentAsString();
                    reader.ReadToFollowing("Active");
                    user.Active = reader.ReadElementContentAsString();
                    reader.ReadToFollowing("Skype");
                    user.Skype = reader.ReadElementContentAsString();
                    reader.ReadToFollowing("PhoneWork");
                    user.PhoneWork = reader.ReadElementContentAsString();
                    reader.ReadToFollowing("PhoneMobile");
                    user.PhoneMobile = reader.ReadElementContentAsString();
                    reader.ReadToFollowing("LastLogin");
                    user.LastLogin = reader.ReadElementContentAsString();
                    reader.ReadToFollowing("LoginKey");
                    user.LoginKey = reader.ReadElementContentAsString();
                    reader.ReadToFollowing("TimeZone");
                    user.TimeZone = reader.ReadElementContentAsString();
                }
            }
        }
        catch (Exception ex)
        {
            user.Response = "No user was found. " + ex.ToString();
        }

        return user;
    }

    public List<LitmosUser> Get_UserInfo(string[] userIDs)
    {
        List<LitmosUser> users = new List<LitmosUser>();
        foreach (string userID in userIDs)
        {
            LitmosUser user = new LitmosUser();
            try
            {
                HttpWebRequest req = WebRequest.Create(uri + "/users/" + userID + "?apikey=" + apiKey + "&source=" + source) as HttpWebRequest;
                req.Method = "GET";

                HttpWebResponse res = req.GetResponse() as HttpWebResponse;

                if (res.StatusCode == HttpStatusCode.Created)
                {
                    string xmlString = new StreamReader(res.GetResponseStream(), Encoding.UTF8).ReadToEnd();
                    using (XmlReader reader = XmlReader.Create(new StringReader(xmlString)))
                    {
                        reader.ReadToFollowing("UserName");
                        user.UserName = reader.ReadElementContentAsString();
                        reader.ReadToFollowing("FirstName");
                        user.FirstName = reader.ReadElementContentAsString();
                        reader.ReadToFollowing("LastName");
                        user.LastName = reader.ReadElementContentAsString();
                        reader.ReadToFollowing("FullName");
                        user.FullName = reader.ReadElementContentAsString();
                        reader.ReadToFollowing("Email");
                        user.Email = reader.ReadElementContentAsString();
                        reader.ReadToFollowing("AccessLevel");
                        user.AccessLevel = reader.ReadElementContentAsString();
                        reader.ReadToFollowing("DisableMessages");
                        user.DisableMessages = reader.ReadElementContentAsString();
                        reader.ReadToFollowing("Active");
                        user.Active = reader.ReadElementContentAsString();
                        reader.ReadToFollowing("Skype");
                        user.Skype = reader.ReadElementContentAsString();
                        reader.ReadToFollowing("PhoneWork");
                        user.PhoneWork = reader.ReadElementContentAsString();
                        reader.ReadToFollowing("PhoneMobile");
                        user.PhoneMobile = reader.ReadElementContentAsString();
                        reader.ReadToFollowing("LastLogin");
                        user.LastLogin = reader.ReadElementContentAsString();
                        reader.ReadToFollowing("LoginKey");
                        user.LoginKey = reader.ReadElementContentAsString();
                        reader.ReadToFollowing("TimeZone");
                        user.TimeZone = reader.ReadElementContentAsString();
                    }
                }
            }
            catch (Exception ex)
            {
                user.Response = "No User was found. " + ex.ToString();
            }
        }

        return users;
    }

    public List<LitmosUser> Get_AllUsers()
    {
        List<LitmosUser> users = new List<LitmosUser>();

        try
        {
            HttpWebRequest req = WebRequest.Create(uri + "/users?apikey=" + apiKey + "&source=" + source) as HttpWebRequest;
            req.Method = "GET";

            HttpWebResponse res = req.GetResponse() as HttpWebResponse;

            if (res.StatusCode == HttpStatusCode.Created)
            {
                string xmlString = new StreamReader(res.GetResponseStream(), Encoding.UTF8).ReadToEnd();
                xmlString.Replace("<User>", "|");
                string[] xmlStringArray = xmlString.Split('|');

                foreach (var s in xmlStringArray)
                {
                    LitmosUser user = new LitmosUser();

                    using (XmlReader reader = XmlReader.Create(new StringReader(s)))
                    {
                        reader.ReadToFollowing("Id");
                        user.UserID = reader.ReadElementContentAsString();
                        reader.ReadToFollowing("UserName");
                        user.UserName = reader.ReadElementContentAsString();
                        reader.ReadToFollowing("FirstName");
                        user.FirstName = reader.ReadElementContentAsString();
                        reader.ReadToFollowing("LastName");
                        user.LastName = reader.ReadElementContentAsString();
                    }

                    users.Add(user);
                }
            }
        }
        catch
        {
            
        }

        return users;
    }

    public List<LitmosCourse> Get_CourseList()
    {
        List<LitmosCourse> courses = new List<LitmosCourse>();

        try
        {
            HttpWebRequest req = WebRequest.Create(uri + "/courses?apikey=" + apiKey + "&source=" + source) as HttpWebRequest;
            req.Method = "GET";

            HttpWebResponse res = req.GetResponse() as HttpWebResponse;

            if (res.StatusCode == HttpStatusCode.Created)
            {
                string xmlString = new StreamReader(res.GetResponseStream(), Encoding.UTF8).ReadToEnd();
                xmlString.Replace("<Course>", "|");
                string[] xmlStringArray = xmlString.Split('|');

                foreach (var s in xmlStringArray)
                {
                    LitmosCourse course = new LitmosCourse();
                    using (XmlReader reader = XmlReader.Create(new StringReader(s)))
                    {
                        reader.ReadToFollowing("Id");
                        course.Id = reader.ReadElementContentAsString();
                        reader.ReadToFollowing("Code");
                        course.Code = reader.ReadElementContentAsString();
                        reader.ReadToFollowing("Name");
                        course.Name = reader.ReadElementContentAsString();
                        reader.ReadToFollowing("Active");
                        course.Active = reader.ReadElementContentAsString();
                    }
                }
            }
        }
        catch
        {

        }

        return courses;
    }

    public Dictionary<string, string> Get_TeamList() //Not fully working - split on | not working
    {
        Dictionary<string, string> teams = new Dictionary<string, string>();

        try
        {
            HttpWebRequest req = WebRequest.Create(uri + "/teams?apikey=" + apiKey + "&source=" + source) as HttpWebRequest;
            req.Method = "GET";

            HttpWebResponse res = req.GetResponse() as HttpWebResponse;

            if (res.StatusCode == HttpStatusCode.OK)
            {
                string xmlString = new StreamReader(res.GetResponseStream(), Encoding.UTF8).ReadToEnd();
                xmlString.Replace("<Team>", "|");
                string[] xmlStringArray = xmlString.Split('|');

                foreach (var s in xmlStringArray)
                {
                    using (XmlReader reader = XmlReader.Create(new StringReader(s)))
                    {
                        reader.ReadToFollowing("Id");
                        teams.Add("Id", reader.ReadElementContentAsString());
                        reader.ReadToFollowing("Name");
                        teams.Add("Name", reader.ReadElementContentAsString());
                    }
                }
            }
        }
        catch
        {

        }

        return teams;
    }

    public LitmosUser Get_UserCourses(LitmosUser user)
    {
        try
        {
            List<LitmosCourse> courses = new List<LitmosCourse>();
            HttpWebRequest req = HttpWebRequest.Create(uri + "/users/" + user.UserID + "/courses?apikey=" + apiKey + "&source=" + source) as HttpWebRequest;
            req.Method = "GET";

            HttpWebResponse res = req.GetResponse() as HttpWebResponse;

            if (res.StatusCode == HttpStatusCode.Created)
            {
                string xmlString = new StreamReader(res.GetResponseStream(), Encoding.UTF8).ReadToEnd();
                xmlString.Replace("<Course>", "|");
                string[] xmlStringArray = xmlString.Split('|');

                foreach (var s in xmlStringArray)
                {
                    LitmosCourse course = new LitmosCourse();
                    using (XmlReader reader = XmlReader.Create(new StringReader(s)))
                    {
                        reader.ReadToFollowing("Id");
                        course.Id = reader.ReadElementContentAsString();
                        reader.ReadToFollowing("Code");
                        course.Code = reader.ReadElementContentAsString();
                        reader.ReadToFollowing("Name");
                        course.Name = reader.ReadElementContentAsString();
                        reader.ReadToFollowing("Active");
                        course.Active = reader.ReadElementContentAsString();
                        reader.ReadToFollowing("Complete");
                        course.Complete = reader.ReadElementContentAsString();
                        reader.ReadToFollowing("DateCompleted");
                        course.DateCompleted = reader.ReadElementContentAsString();
                        reader.ReadToFollowing("UpToDate");
                        course.UpToDate = reader.ReadElementContentAsString();
                    }
                    courses.Add(course);
                }
                user.Courses = courses;
            }

        }
        catch
        {

        }

        return user;
    }

    public List<LitmosUser> Get_UserCourses(List<LitmosUser> users)
    {
        foreach (LitmosUser user in users)
        {
            try
            {
                List<LitmosCourse> courses = new List<LitmosCourse>();
                HttpWebRequest req = HttpWebRequest.Create(uri + "/users/" + user.UserID + "/courses?apikey=" + apiKey + "&source=" + source) as HttpWebRequest;
                req.Method = "GET";

                HttpWebResponse res = req.GetResponse() as HttpWebResponse;

                if (res.StatusCode == HttpStatusCode.Created)
                {
                    string xmlString = new StreamReader(res.GetResponseStream(), Encoding.UTF8).ReadToEnd();
                    xmlString.Replace("<Course>", "|");
                    string[] xmlStringArray = xmlString.Split('|');

                    foreach (var s in xmlStringArray)
                    {
                        LitmosCourse course = new LitmosCourse();
                        using (XmlReader reader = XmlReader.Create(new StringReader(s)))
                        {
                            reader.ReadToFollowing("Id");
                            course.Id = reader.ReadElementContentAsString();
                            reader.ReadToFollowing("Code");
                            course.Code = reader.ReadElementContentAsString();
                            reader.ReadToFollowing("Name");
                            course.Name = reader.ReadElementContentAsString();
                            reader.ReadToFollowing("Active");
                            course.Active = reader.ReadElementContentAsString();
                            reader.ReadToFollowing("Complete");
                            course.Complete = reader.ReadElementContentAsString();
                            reader.ReadToFollowing("DateCompleted");
                            course.DateCompleted = reader.ReadElementContentAsString();
                            reader.ReadToFollowing("UpToDate");
                            course.UpToDate = reader.ReadElementContentAsString();
                        }
                        courses.Add(course);
                    }
                    user.Courses = courses;
                }
            }
            catch
            {
            }
        }
        return users;
    }
    #endregion

    #region Assign Methods
    public string Assign_Team(LitmosUser user, string team)
    {
        string ResponseMessage = "";

        #region XML
        byte[] dataByte;

        MemoryStream stream = new MemoryStream();
        
        XmlTextWriter writer = new XmlTextWriter(stream, Encoding.UTF8);
        writer.Formatting = Formatting.Indented;
        writer.WriteStartDocument();
        writer.WriteStartElement("Users");
        writer.WriteStartElement("User");
        writer.WriteStartElement("Id");
        writer.WriteString(user.UserID);
        writer.WriteEndElement();
        writer.WriteStartElement("UserName");
        writer.WriteString(user.UserName);
        writer.WriteEndElement();
        writer.WriteStartElement("FirstName");
        writer.WriteString(user.FirstName);
        writer.WriteEndElement();
        writer.WriteStartElement("LastName");
        writer.WriteString(user.LastName);
        writer.WriteEndElement();
        writer.WriteEndElement();
        writer.WriteEndElement();
        writer.WriteEndDocument();
        writer.Flush();
        writer.Close();

        dataByte = stream.ToArray();
        #endregion

        #region API Call
        HttpWebRequest req = HttpWebRequest.Create(uri + "/teams/" + team + "/users?apikey=" + apiKey + "&source=" + source) as HttpWebRequest;
        req.Method = "POST";

        req.ContentType = "text/xml";
        req.KeepAlive = false;
        req.Timeout = 400000;
        req.ContentLength = dataByte.Length;

        Stream POSTstream = req.GetRequestStream();

        POSTstream.Write(dataByte, 0, dataByte.Length);

        HttpWebResponse res = req.GetResponse() as HttpWebResponse;

        if (res.StatusCode == HttpStatusCode.Created)
        {
            ResponseMessage = "Success";
        }
        else
        {
            ResponseMessage = "An unexpected error occured";
        }
        #endregion

        return ResponseMessage;
    }

    public string Assign_Team(List<LitmosUser> users, string team)
    {
        string ResponseMessage = "";
        
        #region XML
        byte[] dataByte;

        using (MemoryStream stream = new MemoryStream())
        {
            XmlTextWriter writer = new XmlTextWriter(stream, Encoding.UTF8);
            writer.Formatting = Formatting.Indented;
            writer.WriteStartDocument();
            writer.WriteStartElement("Users");
            foreach (LitmosUser user in users)
            {
                writer.WriteStartElement("User");
                writer.WriteStartElement("Id");
                writer.WriteString(user.UserID);
                writer.WriteEndElement();
                writer.WriteStartElement("UserName");
                writer.WriteString(user.UserName);
                writer.WriteEndElement();
                writer.WriteStartElement("FirstName");
                writer.WriteString(user.FirstName);
                writer.WriteEndElement();
                writer.WriteStartElement("LastName");
                writer.WriteString(user.LastName);
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
            writer.WriteEndDocument();

            dataByte = stream.ToArray();
        }
        #endregion

        #region API Call
        HttpWebRequest req = HttpWebRequest.Create(uri + "/teams/" + team + "/users?apikey=" + apiKey + "&source=" + source) as HttpWebRequest;
        req.Method = "POST";

        req.ContentType = "text/xml";
        req.KeepAlive = false;
        req.Timeout = 400000;
        req.ContentLength = dataByte.Length;

        using (Stream POSTstream = req.GetRequestStream())
        {
            POSTstream.Write(dataByte, 0, dataByte.Length);

            HttpWebResponse res = req.GetResponse() as HttpWebResponse;

            if (res.StatusCode == HttpStatusCode.Created)
            {
                ResponseMessage = "User Created Successfully";
            }
            else
            {
                ResponseMessage = "An unexpected error occured";
            }
        }
        #endregion

        return ResponseMessage;
    }

    public string Assign_Course(LitmosUser user, string course)
    {
        string ResponseMessage = "";


        #region XML
        byte[] dataByte;

        using (MemoryStream stream = new MemoryStream())
        {
            XmlTextWriter writer = new XmlTextWriter(stream, Encoding.UTF8);
            writer.Formatting = Formatting.Indented;
            writer.WriteStartDocument();
            writer.WriteStartElement("Courses");
            writer.WriteStartElement("Course");
            writer.WriteStartElement("Id");
            writer.WriteString(user.UserID);
            writer.WriteEndElement();
            writer.WriteEndElement();
            writer.WriteEndDocument();

            dataByte = stream.ToArray();
        }
        #endregion

        #region API Call
        HttpWebRequest req = HttpWebRequest.Create(uri + "/users/" + user.UserID + "/courses?apikey=" + apiKey + "&source=" + source) as HttpWebRequest;
        req.Method = "POST";

        req.ContentType = "text/xml";
        req.KeepAlive = false;
        req.Timeout = 400000;
        req.ContentLength = dataByte.Length;

        using (Stream POSTstream = req.GetRequestStream())
        {
            POSTstream.Write(dataByte, 0, dataByte.Length);

            HttpWebResponse res = req.GetResponse() as HttpWebResponse;

            if (res.StatusCode == HttpStatusCode.Created)
            {
                ResponseMessage = "Course Assigned Successfully";
            }
            else
            {
                ResponseMessage = "An unexpected error occured";
            }
        }
        #endregion

        return ResponseMessage;
    }

    public string Assign_Course(List<LitmosUser> users, string course)
    {
        string ResponseMessage = "";


        #region XML
        byte[] dataByte;
        foreach (LitmosUser user in users)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                XmlTextWriter writer = new XmlTextWriter(stream, Encoding.UTF8);
                writer.Formatting = Formatting.Indented;
                writer.WriteStartDocument();
                writer.WriteStartElement("Courses");
                writer.WriteStartElement("Course");
                writer.WriteStartElement("Id");
                writer.WriteString(user.UserID);
                writer.WriteEndElement();
                writer.WriteEndElement();
                writer.WriteEndDocument();

                dataByte = stream.ToArray();
            }
        #endregion

            #region API Call
            HttpWebRequest req = HttpWebRequest.Create(uri + "/users/" + user.UserID + "/users?apikey=" + apiKey + "&source=" + source) as HttpWebRequest;
            req.Method = "POST";

            req.ContentType = "text/xml";
            req.KeepAlive = false;
            req.Timeout = 400000;
            req.ContentLength = dataByte.Length;

            using (Stream POSTstream = req.GetRequestStream())
            {
                POSTstream.Write(dataByte, 0, dataByte.Length);

                HttpWebResponse res = req.GetResponse() as HttpWebResponse;

                if (res.StatusCode == HttpStatusCode.Created)
                {
                    ResponseMessage = "User Created Successfully";
                }
                else
                {
                    ResponseMessage = "An unexpected error occured";
                }
            }
        }
        #endregion

        return ResponseMessage;
    }
    #endregion

    #region Removal Methods
    public string Remove_UserFromTeam(LitmosUser user, string team)
    {
        string ResponseMessage = "";

        return ResponseMessage;
    }

    public string Remove_UserFromTeam(List<LitmosUser> users, string team)
    {
        string ResponseMessage = "";

        return ResponseMessage;
    }

    public string Remove_UserFromCourse(LitmosUser user, string course)
    {
        string ResponseMessage = "";

        return ResponseMessage;
    }

    public string Remove_UserFromCourse(List<LitmosUser> users, string course)
    {
        string ResponseMessage = "";

        return ResponseMessage;
    }
    #endregion

    #region Update User
    public string Update_User(LitmosUser user)
    {
        string ResponseMessage = "";

        return ResponseMessage;
    }

    public string Update_User(List<LitmosUser> users)
    {
        string ResponseMessage = "";

        return ResponseMessage;
    }
    #endregion

    #region Achievements
    #endregion

    #region Exigo Methods
    public void UpdateExigoAccount(LitmosUser user)
    {
        try
        {
            ExigoApiServices auth = new ExigoApiServices();

            UpdateCustomerRequest req = new UpdateCustomerRequest();
            req.CustomerID = user.ExigoCustomerID;
            req.Field4 = user.UserID;

            UpdateCustomerResponse res = auth.WebService.UpdateCustomer(req);
        }
        catch
        {

        }
    }
    #endregion
}