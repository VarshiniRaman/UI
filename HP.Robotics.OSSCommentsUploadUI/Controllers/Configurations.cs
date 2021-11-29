using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace HP.Robotics.OSSCommentsUploadUI.Controllers
{
    public class Configurations : Controller
    {
        //
        // GET: /Configurations/

        public ActionResult Index()
        {
            return View();
        }
        public static String ConnectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["OSS_CMU_ConnectionString"].ConnectionString;

            }
        }
        public static SqlConnection getConnection()
        {

            SqlConnection sqlConnection = new SqlConnection(ConnectionString);

            return sqlConnection;

        }
        /*
        public static String SMTPServerPort
        {
            get
            {
                return ConfigurationManager.AppSettings["SMTPServer_Port"];

            }
        }
        public static String SMTPServer
        {
            get
            {
                return ConfigurationManager.AppSettings["SMTPServer"];

            }
        }
        public static SmtpClient getSMTPServer()
        {

            SmtpClient SmtpServer = new SmtpClient(SMTPServer);
            SmtpServer.Port = Convert.ToInt32(SMTPServerPort);

            return SmtpServer;

        }

        //FileUploadPath  //C:\PMT Projects\LS\wave\om\om\UploadedFiles\

        public static String FileUploadPath
        {
            get
            {
                return ConfigurationManager.AppSettings["FileUploadPath"];

            }
        }
        public static String getFileUploadPath()
        {
            return FileUploadPath;

        }
        */
    }
}
