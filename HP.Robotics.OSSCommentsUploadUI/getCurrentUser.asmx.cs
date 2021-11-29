using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Mvc;
using System.Web.Routing;
using System.Data.SqlClient;
using System.Data;
using HP.Robotics.OSSCommentsUploadUI.Model;
using System.IO;
using System.Collections;
using System.Web.Script.Serialization;
using HP.Robotics.OSSCommentsUploadUI.webservices;
using System.Configuration;
using System.DirectoryServices;
using HP.Robotics.OSSCommentsUploadUI.Common;

namespace HP.Robotics.OSSCommentsUploadUI
{
    /// <summary>
    /// Summary description for getCurrentUser
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class getCurrentUser : System.Web.Services.WebService
    {

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }
        [WebMethod(EnableSession = true)]
        public User getUserLDAP()
        {
            
            User UserInfo = new User();
            //string strUserNtID = "EMEA\\poalexan";//Request.ServerVariables["LOGON_USER"];
           // HttpContext.Current.Response.Write("<script>alert('You have access :before');</script>");
            string strUserNtID = HttpContext.Current.Request.ServerVariables["REMOTE_USER"];
            if (strUserNtID== null || strUserNtID == "")
                strUserNtID = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            //System.Windows.Forms.MessageBox.Show(strUserNtID);
           // HttpContext.Current.Response.Write("<script>alert('You have access " + strUserNtID + "');</script>"); 

            //strUserNtID = @"ASIAPACIFIC\gopkrish";
            
            string userName = strUserNtID;
            //string userName = "AUTH\\spr";
            string sAMAccountName = userName;
            userName = userName.Replace(@"\", ":");
            //userName = "ASIAPACIFIC:rmar";
            string ldapserver = ConfigurationManager.ConnectionStrings["LDAPServer"].ConnectionString;
            string strServerDNS = ldapserver+":389";
            string strSearchBaseDN = "ou=People,o=hp.com";
            string strLDAPPath;
            strLDAPPath = "LDAP://" + strServerDNS + "/" + strSearchBaseDN;
            DirectoryEntry objDirEntry = new DirectoryEntry(strLDAPPath, "", "", AuthenticationTypes.None);

            // object nativeObject = objDirEntry.NativeObject;
            DirectorySearcher searcher = new DirectorySearcher(objDirEntry);

            ArrayList arrGal = new ArrayList();
            searcher.Filter = "(ntuserdomainid=" + userName + ")";
            searcher.PropertiesToLoad.Add("*");
           // HttpContext.Current.Response.Write("<script>alert('You have access " + userName + "');</script>");
            
            try
            {
                SearchResult result = searcher.FindOne();
                
                if (result != null)
                {
                    if (result.Properties["mail"].Count>0 && result.Properties["mail"][0].GetType().IsArray)
                    {
                        //UserInfo.userInfo.NtID=Request.ServerVariables["LOGON_USER"];
                        UserInfo.NtID = userName;
                        UserInfo.WorkEmail = System.Text.Encoding.UTF8.GetString((byte[])result.Properties["uid"][0]);
                        UserInfo.FirstName = System.Text.Encoding.UTF8.GetString((byte[])result.Properties["givenname"][0]);
                        UserInfo.LastName = System.Text.Encoding.UTF8.GetString((byte[])result.Properties["sn"][0]);
                        UserInfo.Company = System.Text.Encoding.UTF8.GetString((byte[])result.Properties["hpSplitCompany"][0]);
                        UserInfo.MgrName = System.Text.Encoding.UTF8.GetString((byte[])result.Properties["manager"][0]);
                        UserInfo.FullName = UserInfo.LastName + ", " + UserInfo.FirstName;
                        UserInfo.hpStatus = System.Text.Encoding.UTF8.GetString((byte[])result.Properties["hpStatus"][0]);
                        UserInfo.Approver_NtID = "";
                        UserInfo.Role = "User";
                    }
                    else
                    {
                        UserInfo.NtID = userName;
                        UserInfo.WorkEmail = result.Properties["uid"][0].ToString();
                        UserInfo.FirstName = result.Properties["givenname"][0].ToString();
                        UserInfo.Company = result.Properties["hpSplitCompany"][0].ToString();
                        UserInfo.LastName = result.Properties["sn"][0].ToString();
                        UserInfo.MgrName = result.Properties["manager"][0].ToString();
                        UserInfo.FullName = UserInfo.LastName + ", " + UserInfo.FirstName;
                        UserInfo.hpStatus = result.Properties["hpStatus"][0].ToString();
                        UserInfo.Approver_NtID = "";
                        UserInfo.Role = "User";
                    }
                    //User udetails = new User();
                    //UserController UserData = new UserController();
                    //udetails = UserData.Select(UserInfo.NtID.Replace(@"\", ":"));
                    HttpContext.Current.Session["UserInfo"] = UserInfo;

                }
                else
                {
                    UserInfo.NtID = userName;
                    UserInfo.Role = "User";

                }
                 
            }
            catch (Exception E)
            {
                Console.Write("E" + E);
                HttpContext.Current.Response.Write("<script>alert('Error " + E + "');</script>");
            }

            
            int Stp = strUserNtID.ToString().IndexOf("\\") + 1;
            string NtID = strUserNtID.ToString().Substring(Stp);
            HttpContext.Current.Session["NTID"] = userName;

            Stp = strUserNtID.ToString().IndexOf("\\") + 1;
            NtID = strUserNtID.ToString().Substring(Stp);
            HttpContext.Current.Session["NTID_"] = userName;
            HttpContext.Current.Session["NTID_FOLDERPURPOSE"] = userName.Replace(":", "");
            
            return UserInfo;
        }
        public string getUser()
        {
            User UserInfo = new User();
            UserInfo = getUserLDAP();
            return HttpContext.Current.Session["NTID"].ToString();
            //return "hello";
        }
        [WebMethod(EnableSession = true)]
        public List<User> getAdmins()
        {
            List<User> lstEmpDetails = userAccess.SelectAll();
            List<User> admins = new List<User>();
            var id = lstEmpDetails.Where(b => b.Role == "Admin").Select(c => new { c.FullName, c.WorkEmail,c.NtID }).Distinct().ToList();
            for (int i = 0; i < id.Count;i++ )
            { 
                User check=new User();
                check.WorkEmail=id[i].WorkEmail;
                check.FullName=id[i].FullName;
                check.NtID=id[i].NtID;
                admins.Add(check);
            }
            return admins;
        }
        [WebMethod(EnableSession = true)]
        public User validateUser()
        {
            User UserInfo = new User();
            try
            {
                
                UserInfo = getUserLDAP();
                SqlDataReader result = userAccess.checkUser(UserInfo,"Validateuser");
                UserInfo.Role = "";
                UserInfo.IsActive = "-1";
                UserInfo.Status = "0";
                if (result.HasRows)
                {
                    while (result.Read())
                    {
                        UserInfo.Role = (Convert.IsDBNull(result["Role"])) ? null : result["Role"].ToString();
                        UserInfo.IsActive = (Convert.IsDBNull(result["IsActive"])) ? "0" : result["IsActive"].ToString();
                        UserInfo.Status = (Convert.IsDBNull(result["Status"])) ? "0" : result["Status"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return UserInfo;
        }
        [WebMethod(EnableSession = true)]
        public string addUser(string data)
        {
            JavaScriptSerializer json = new JavaScriptSerializer();
            var consdata = json.Deserialize<Dictionary<string, string>>(data);

            User UserInfo = new User();
            string result = "";
            try
            {

                UserInfo = getUserLDAP();
                UserInfo.Approver_NtID = consdata["NtID"];
                SqlDataReader dr = userAccess.checkUser(UserInfo, "Adduser");
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        result = (Convert.IsDBNull(dr["Success"])) ? null : dr["Success"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return result;
        }
        [WebMethod(EnableSession = true)]
        public string updateUser(string data)
        {
            JavaScriptSerializer json = new JavaScriptSerializer();
            var consdata = json.Deserialize<Dictionary<string, string>>(data);

            User UserInfo = new User();
            string result = "";
            try
            {

                UserInfo = getUserLDAP();
                UserInfo.Approver_NtID = consdata["NtID"];
                UserInfo.Role = consdata["Role"];
                UserInfo.Status = consdata["Status"];
                UserInfo.User_ID = consdata["UserID"];
                UserInfo.IsActive = consdata["IsActive"];
                SqlDataReader dr = userAccess.checkUser(UserInfo, "Updateuser");
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        result = (Convert.IsDBNull(dr["Success"])) ? null : dr["Success"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return result;
        }
        [WebMethod(EnableSession = true)]
        public datacollection getAllUsers(string data)
        {

            JavaScriptSerializer json = new JavaScriptSerializer();
            var consdata = json.Deserialize<Dictionary<string, string>>(data);
            SqlDataReader dataReader = userAccess.getAllUsers(consdata);
            datacollection d = new datacollection();
            List<datatypes> datatypes = new List<datatypes>();
            List<dynamic> rowdata = new List<dynamic>();
            datatypes dt;
            if (dataReader.HasRows)
            {
                while (dataReader.Read())
                {
                    dt = new datatypes();
                    dt.columname = (Convert.IsDBNull(dataReader["COLUMN_NAME"])) ? null : dataReader["COLUMN_NAME"].ToString();
                    dt.columntype = (Convert.IsDBNull(dataReader["DATA_TYPE"])) ? null : dataReader["DATA_TYPE"].ToString();
                    datatypes.Add(dt);
                }
            }
            d.datatypes = datatypes;
            dataReader.NextResult();
            //Manage1 mm = new Manage1();
           // d.rowData = mm.retrieveHasRows(dataReader, "0", datatypes);
            return d;
        }
        
    }
}
