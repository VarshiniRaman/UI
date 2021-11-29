using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HP.Robotics.OSSCommentsUploadUI.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public static SqlDataReader retrieveTypeandData(Dictionary<string, string> param)
        {
            SqlConnection SqlConn = Configurations.getConnection();
            SqlDataReader dr = null;

            //SqlCommand sqlCommand = new SqlCommand("Select * from SKUMODE", SqlConn);

            SqlCommand sqlCommand = new SqlCommand("Retrieve_Backlog_Data", SqlConn);
            sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;

            sqlCommand.CommandTimeout = 420;
            foreach (string key in param.Keys)
                sqlCommand.Parameters.Add(new SqlParameter("@" + key, param[key]));
            SqlConn.Open();
            dr = sqlCommand.ExecuteReader(CommandBehavior.CloseConnection);
            return dr;
        }
        public static SqlDataReader updateTable(Dictionary<string, string> param)
        {
            SqlConnection SqlConn = Configurations.getConnection();
            SqlDataReader dr = null;

            SqlCommand sqlCommand = new SqlCommand("Retrieve_Backlog_Data", SqlConn);
            sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
            sqlCommand.CommandTimeout = 420;
            foreach (string key in param.Keys)
                if (key != "URL")
                    sqlCommand.Parameters.Add(new SqlParameter("@" + key, param[key]));
            SqlConn.Open();
            dr = sqlCommand.ExecuteReader(CommandBehavior.CloseConnection);
            return dr;
        }
    }
}