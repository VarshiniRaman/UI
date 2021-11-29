using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;

namespace HP.Robotics.OSSCommentsUploadUI.Model
{
    
    public class User
    {
        public Object userInfo { get; set; }
        public String User_ID { get; set; }
        public String FullName { get; set; }
        public String FirstName { get; set; }
        public String MiddleName { get; set; }
        public String LastName { get; set; }
        public String WorkEmail { get; set; }
        public String NtID { get; set; }
        public String Approver_NtID { get; set; }
        public String RoleId { get; set; }
        public String Role { get; set; }
        public String IsActive { get; set; }
        public String MODIFIED_BY { get; set; }
        public String CREATED_BY { get; set; }
        public String MgrName { get; set; }
        public String AUser_ID { get; set; }
        public String AWorkEmail { get; set; }
        public String AFullName { get; set; }
        public String AIsActive { get; set; }
        public String ARoleID { get; set; }
        public String ARoleValue { get; set; }
        public String Title { get; set; }
        public String Company { get; set; }
        public String City { get; set; }
        public String Street { get; set; }
        public String Country { get; set; }
        public String PostalCode { get; set; }
        public String TelephoneNo { get; set; }
        public String employeeNumber { get; set; }
        public String ARTM { get; set; }
        public String ARTMvalue { get; set; }
        public List<User> ManagerDetails { get; set; }
        public String ManagerEmpNumber { get; set; }
        public String hpStatus { get; set; }
        public String Status { get; set; }
        

        

    }
   

    public class Result
    {
        public string msg { get; set; }
        public string status { get; set; }
    }
    public class file
    {
        public string filename { get; set; }
        public string size { get; set; }
        public string extension { get; set; }
        public string url { get; set; }
        public List<file> filelist { get; set; }

    }
    public class datacollection
    {
        public List<datatypes> datatypes { get; set; }
        public List<dynamic> rowData { get; set; }
        public List<dynamic> columnschema { get; set; }
        public List<dynamic> rowData_1 { get; set; }
        public List<dynamic> rowData_2 { get; set; }
        public List<dynamic> rowData_3 { get; set; }
        public List<dynamic> rowData_4 { get; set; }
        public List<dynamic> rowData_5 { get; set; }
        public List<dynamic> rowData_6 { get; set; }
        public List<dynamic> rowData_7 { get; set; }
        public List<dynamic> rowData_8 { get; set; }
        public List<dynamic> rowData_9 { get; set; }
        public List<dynamic> rowData_10 { get; set; }
        public string mailcontent { get; set; }
        public string additionaldata1 { get; set; }
        public string additionaldata2 { get; set; }

    }
    public class datacollection_otherstatus
    {
        public List<datatypes> datatypes { get; set; }
        public List<dynamic> rowData { get; set; }
        public List<dynamic> columnschema { get; set; }
        public List<dynamic> rowData1 { get; set; }
        public List<dynamic> rowData2 { get; set; }
        public List<dynamic> rowData3 { get; set; }
        public List<dynamic> rowData4 { get; set; }
        public List<dynamic> gridschema { get; set; }
    }
    public class datatypes
    {
        public string columname { get; set; }
        public string columntype { get; set; }
    }
    public class PDIMHtml
    {
        public string HTML { get; set; }
        public string Filtered_HTML{ get; set; }
        public string result { get; set; }

    }
    public class dp
    {
        public string CategoryID { get; set; }
        public string CategoryValue { get; set; }
        public string CategoryType { get; set; }

    }
    public class category
    {
        public List<dp> categoryvalues{get; set;}
    }
    public class parseout
    {
        public string filename{ get; set; }
        public string filecontent { get; set; }
        public string error { get; set; }

    }
    public class resultcollection
    {
        public string name { get; set; }
        public string status { get; set; }
        public string message { get; set; }
        public string info { get; set; }

    }
   
   
}