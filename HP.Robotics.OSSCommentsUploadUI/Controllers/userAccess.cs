using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using HP.Robotics.OSSCommentsUploadUI.Model;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.Services;
using System.Web.Script.Services;
using System.Web.Script.Serialization;
using HP.Robotics.OSSCommentsUploadUI.Controllers;

namespace HP.Robotics.OSSCommentsUploadUI.Common
{
    public class userAccess
    {
        public SqlConnection SqlConn = Configurations.getConnection();
        public static SqlDataReader returnData(string query)
        {
            SqlDataReader dr = null;
            SqlConnection SqlConn = Configurations.getConnection();
            SqlCommand cmd = new SqlCommand(query, SqlConn);
            SqlConn.Open();
            dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            return dr;
        }
        public static SqlDataReader getRow(string ID)
        {
            SqlDataReader dr = null;
            SqlConnection SqlConn = Configurations.getConnection();
            SqlCommand cmd = new SqlCommand("select top 1* from [M_]" , SqlConn);
            SqlConn.Open();
            dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            return dr;
        }
        public static SqlDataReader checkUser(User data,string additionparam)
        {

            SqlConnection SqlConn = Configurations.getConnection();
            SqlDataReader dr = null;

            SqlCommand sqlCommand = new SqlCommand("Admin_Access", SqlConn);
            sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
            sqlCommand.CommandTimeout = 60;
            if (additionparam == "Updateuser")
            {
                sqlCommand.Parameters.Add(new SqlParameter("@task", additionparam));
                sqlCommand.Parameters.Add(new SqlParameter("@Status", data.Status));
                sqlCommand.Parameters.Add(new SqlParameter("@IsActive", data.IsActive));
                sqlCommand.Parameters.Add(new SqlParameter("@Approver_NtID", data.NtID));
                sqlCommand.Parameters.Add(new SqlParameter("@Role", data.Role));
                sqlCommand.Parameters.Add(new SqlParameter("@modified_by", data.NtID));
                sqlCommand.Parameters.Add(new SqlParameter("@UserID", data.User_ID));
            }
            else
            {
                sqlCommand.Parameters.Add(new SqlParameter("@task", additionparam));
                sqlCommand.Parameters.Add(new SqlParameter("@NtID", data.NtID));
                sqlCommand.Parameters.Add(new SqlParameter("@Approver_NtID", data.Approver_NtID));
                sqlCommand.Parameters.Add(new SqlParameter("@Role", data.Role));
                sqlCommand.Parameters.Add(new SqlParameter("@emailid", data.WorkEmail));
                sqlCommand.Parameters.Add(new SqlParameter("@fullname", data.FullName));
                sqlCommand.Parameters.Add(new SqlParameter("@Firstname", data.FirstName));
                sqlCommand.Parameters.Add(new SqlParameter("@Lastname", data.LastName));
                sqlCommand.Parameters.Add(new SqlParameter("@created_by", data.NtID));
                sqlCommand.Parameters.Add(new SqlParameter("@modified_by", data.NtID));
            }
            SqlConn.Open();
            
            dr = sqlCommand.ExecuteReader(CommandBehavior.CloseConnection);
            return dr;
        }


        public static SqlDataReader getAllUsers(Dictionary<string, string> param)
        {
            SqlConnection SqlConn = Configurations.getConnection();
            SqlDataReader dr = null;

            SqlCommand sqlCommand = new SqlCommand("Admin_Access", SqlConn);
            sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
            sqlCommand.CommandTimeout = 60;
            foreach (string key in param.Keys)
                sqlCommand.Parameters.Add(new SqlParameter("@" + key, param[key]));
            SqlConn.Open();
            dr = sqlCommand.ExecuteReader(CommandBehavior.CloseConnection);
            return dr;
        }
        public static List<User> SelectAll()
        {

            JavaScriptSerializer json = new JavaScriptSerializer();
            var consdata = json.Deserialize<Dictionary<string, string>>("{task: 'AllUsers'}");
            SqlDataReader dataReader = getAllUsers(consdata);
            List<User> lstEmpDetails = new List<User>();
            dataReader.NextResult();
            while (dataReader.Read())
            {
                User ObjUser = new User();
                ObjUser.FullName = (Convert.IsDBNull(dataReader["fullname"])) ? null : dataReader["fullname"].ToString();
                ObjUser.FirstName = (Convert.IsDBNull(dataReader["Firstname"])) ? null : dataReader["Firstname"].ToString();
                ObjUser.LastName = (Convert.IsDBNull(dataReader["Lastname"])) ? null : dataReader["Lastname"].ToString();
                ObjUser.WorkEmail = (Convert.IsDBNull(dataReader["emailid"])) ? null : dataReader["emailid"].ToString();
                ObjUser.NtID = (Convert.IsDBNull(dataReader["NtID"])) ? null : dataReader["NtID"].ToString();
                ObjUser.Approver_NtID = (Convert.IsDBNull(dataReader["Approver_NtID"])) ? null : dataReader["Approver_NtID"].ToString();
                ObjUser.Role = (Convert.IsDBNull(dataReader["Role"])) ? null : dataReader["Role"].ToString();
                ObjUser.IsActive = (Convert.IsDBNull(dataReader["IsActive"])) ? null : dataReader["IsActive"].ToString();
                lstEmpDetails.Add(ObjUser);
            }
            return lstEmpDetails;
        }
        
    }
}
