using HP.Robotics.OSSCommentsUploadUI.Controllers;
using HP.Robotics.OSSCommentsUploadUI.Model;
using HP.Robotics.OSSCommentsUploadUI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.ServiceModel;
using System.Net.Mail;
using System.Diagnostics;
using System.Configuration;

namespace HP.Robotics.OSSCommentsUploadUI.webservices
{
    /// <summary>
    /// Summary description for Manage
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class Manage : System.Web.Services.WebService
    {

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }
        
        public class datacollection
        {
            public List<datatypes> datatypes { get; set; }
            public List<dynamic> rowData { get; set; }
            public List<dynamic> columnschema { get; set; }
            public List<dynamic> rowData_1 { get; set; }
            public List<dynamic> rowData_2 { get; set; }
            public string mailcontent { get; set; }
            public string additionaldata1 { get; set; }
            public string additionaldata2 { get; set; }

        }
         [WebMethod]
        public string checkDatatype(List<datatypes> dt, string columnname)
        {
            for (int i = 0; i < dt.Count; i++)
                if (dt[i].columname == columnname)
                    return dt[i].columntype;
            return "text";
        }
         [WebMethod]
         public datacollection_otherstatus retrieveOtherPOs(string data)
         {
             JavaScriptSerializer json = new JavaScriptSerializer();
             var consdata = json.Deserialize<Dictionary<string, string>>(data);

             SqlDataReader dataReader = HomeController.retrieveTypeandData(consdata);
             datacollection_otherstatus d = new datacollection_otherstatus();
             List<dynamic> rowdata = new List<dynamic>();


             var obj = new Dictionary<string, object>();
             var obj1 = new Dictionary<string, object>();
             var col = new Dictionary<string, int>();
             List<dynamic> columnschema = new List<dynamic>();
             List<datatypes> datatypes = new List<datatypes>();
             datatypes dt;
             if (dataReader.HasRows)
             {
                 while (dataReader.Read())
                 {
                     for (int i = 0; i < dataReader.FieldCount; i++)
                     {
                         obj1 = new Dictionary<string, object>();
                         obj1.Add("field", dataReader.GetName(i));
                         obj1.Add("displayname", dataReader.GetName(i));
                         obj1.Add("width", "100");
                         columnschema.Add(obj1);

                     }

                 }
             }

             dataReader.NextResult();
             d.rowData = retrieveHasRows(dataReader, "0", datatypes);
             
             d.columnschema = columnschema;
             return d;
         }
         [WebMethod]
         public datacollection_otherstatus retrieveBacklog(string data)
         {
             JavaScriptSerializer json = new JavaScriptSerializer();
             var consdata = json.Deserialize<Dictionary<string, string>>(data);

             SqlDataReader dataReader = HomeController.retrieveTypeandData(consdata);
             datacollection_otherstatus d = new datacollection_otherstatus();
             List<dynamic> rowdata = new List<dynamic>();


             var obj = new Dictionary<string, object>();
             var obj1 = new Dictionary<string, object>();
             var col = new Dictionary<string, int>();
             List<dynamic> columnschema = null;
             List<datatypes> datatypes = new List<datatypes>();
             datatypes dt;

             d.rowData = retrieveHasRows(dataReader, "0", datatypes);
             d.datatypes = datatypes;
             d.columnschema = columnschema;

             return d;
         }

         public List<dynamic> retrieveHasRows(SqlDataReader dataReader, string flag, List<datatypes> dt)
         {
             var obj = new Dictionary<string, object>();
             var col = new Dictionary<string, int>();
             var obj1 = new Dictionary<string, object>();
             List<dynamic> alldata = new List<dynamic>();
             if (dataReader.HasRows)
             {
                 for (int i = 0; i < dataReader.FieldCount; i++)
                 {
                     col.Add(dataReader.GetName(i), 0);
                 }
                 while (dataReader.Read())
                 {
                     obj = new Dictionary<string, object>();
                     for (int i = 0; i < dataReader.FieldCount; i++)
                     {
                         //columns.Add(dataReader.GetName(i));


                         if (checkDatatype(dt, dataReader.GetName(i)) == "date")
                             obj[dataReader.GetName(i)] = (Convert.IsDBNull(dataReader[dataReader.GetName(i)])) ? "" : DateTime.Parse(dataReader[dataReader.GetName(i)].ToString()).ToString("MM/dd/yyyy");
                         else
                             obj[dataReader.GetName(i)] = (Convert.IsDBNull(dataReader[dataReader.GetName(i)])) ? null : dataReader[dataReader.GetName(i)].ToString();


                     }
                     if (flag == "0" && (dataReader.GetSchemaTable().Columns.Contains("Attachments")))
                         obj["Attach"] = (Convert.IsDBNull(dataReader["Attachments"])) ? null : dataReader["Attachments"].ToString().Replace("\\", "/");

                     alldata.Add(obj);
                 }

             }
             return alldata;
         }

         [WebMethod]

         public string updateBacklogReport(string data)
         {
             //MessageBox.Show(data, "s", MessageBoxButtons.OK, MessageBoxIcon.Stop, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
             data = data.Replace(",OverallStatus='000',overallstatus='000'", "");
             data = data.Replace(",OverallStatus='000'", "");
             data = data.Replace(",PackingListComment_Actual=''", "");
             data = data.Replace(",function () { return this.filter(a) }=''", "");

             JavaScriptSerializer json = new JavaScriptSerializer();
             var consdata = json.Deserialize<Dictionary<dynamic, dynamic>>(data);
             List<Dictionary<dynamic, dynamic>> selectedvalues = new List<Dictionary<dynamic, dynamic>>();
            //selectedvalues = json.Deserialize<List<Dictionary<dynamic, dynamic>>>("["+consdata["val"]+"]");
            selectedvalues = json.Deserialize<List<Dictionary<dynamic, dynamic>>>( consdata["val"] );
            int totalrows=0;
             string outputmsg = "";
             string failedorders = "";
             for (int i = 0; i < selectedvalues.Count; i++)
             {
                 try
                 {
                     //var temp = json.Deserialize<Dictionary<string, string>>(selectedvalues[i]);
                     Dictionary<string, string> temp = new Dictionary<string, string>();
                     //temp.Add("condition", " set UserLevel=" + selectedvalues[i]["UserLevel"] + ", SubLevel=" + selectedvalues[i]["SubLevel"] + ",ModifiedDate=GETDATE(),ModifiedBy='" + consdata["user"] + "', Active=" + selectedvalues[i]["Active"] + " where Sno=" + selectedvalues[i]["Sno"]);
                     temp.Add("condition", " set OSS_Comments='" + selectedvalues[i]["FreeTextComments"] + "', ModifiedBy='" + consdata["currentuser"] + "', ModifiedDateTime=GETDATE() where SalesOrderId='" + selectedvalues[i]["SAPOrdNo"] + "' and SalesOrderItemId='" + selectedvalues[i]["ItemNo"] + "' and GoodsIssueQuantityEA='" + selectedvalues[i]["AckedQuantity"] + "'");
                     temp.Add("tablename", consdata["tablename"]);
                     temp.Add("currentuser", consdata["currentuser"]);
                     temp.Add("task", consdata["task"]);
                     SqlDataReader result = HomeController.updateTable(temp);

                     result.Read();
                     if (result.HasRows)
                     {
                         if (Convert.ToInt32(result[0]) == 1)
                         {
                             totalrows++;

                         }
                         else if (Convert.ToInt32(result[0]) == 2)
                         {
                             failedorders += selectedvalues[i]["Name"];
                         }
                     }
                    if (result.RecordsAffected == 1 || result.RecordsAffected > 1) {
                        totalrows++;
                    }
                 }
                 catch (Exception ex)
                 {
                     failedorders += "Not Authorized | SQL Exception" + ex.Message + ":" + selectedvalues[i]["SAPOrdNo"] + ",";
                 }
             }
             if(selectedvalues.Count==totalrows)
                outputmsg = "Updated successfully";
             else
                 outputmsg = "Partial success, Failed orders :" + failedorders+",";

             return outputmsg;
         }

        [WebMethod]
        public string updateBacklogComments(string data)
        {
            //MessageBox.Show(data, "s", MessageBoxButtons.OK, MessageBoxIcon.Stop, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
            data = data.Replace(",OverallStatus='000',overallstatus='000'", "");
            data = data.Replace(",OverallStatus='000'", "");
            data = data.Replace(",PackingListComment_Actual=''", "");
            data = data.Replace(",function () { return this.filter(a) }=''", "");

            JavaScriptSerializer json = new JavaScriptSerializer();
            var consdata = json.Deserialize<Dictionary<dynamic, dynamic>>(data);
            // List<Dictionary<dynamic, dynamic>> selectedvalues = new List<Dictionary<dynamic, dynamic>>();
            //selectedvalues = json.Deserialize<List<Dictionary<dynamic, dynamic>>>("[" + consdata["val"] + "]");
            int totalrows = 0;
            string outputmsg = "";
            string failedorders = "";
            //for (int i = 0; i < selectedvalues.Count; i++)
            //{
            try
            {
                //var temp = json.Deserialize<Dictionary<string, string>>(selectedvalues[i]);
                Dictionary<string, string> temp = new Dictionary<string, string>();
                //temp.Add("condition", " where "+ selectedvalues[i]["Sno"]);
                temp.Add("condition", consdata["condition"]);
                temp.Add("tablename", consdata["tablename"]);
                temp.Add("task", consdata["task"]);
                temp.Add("currentuser", consdata["currentuser"]);
                temp.Add("setfields", consdata["setfields"]);
                SqlDataReader result = HomeController.updateTable(temp);

                result.Read();
                if (result.HasRows)
                {
                    if (Convert.ToInt32(result[0]) == 1)
                    {
                        totalrows++;

                    }
                    else if (Convert.ToInt32(result[0]) == 2)
                    {
                        //failedorders += selectedvalues[i]["Name"];
                    }
                }
                if (result.RecordsAffected == 1 || result.RecordsAffected > 1)
                {
                    totalrows++;
                }
            }
            catch (Exception ex)
            {
                failedorders += "SQL Exception" + ex.Message;
                //failedorders += "SQL Exception" + ex.Message + ":" + selectedvalues[i]["Name"] + ",";
            }
            // }
            if (totalrows == 1)
                outputmsg = "Updated successfully";
            else
                outputmsg = "Not Authorozed | Failed to update :" + failedorders + ",";

            return outputmsg;
        }

        [WebMethod]
        public string deleteUser(string data)
        {
            //MessageBox.Show(data, "s", MessageBoxButtons.OK, MessageBoxIcon.Stop, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
            data = data.Replace(",OverallStatus='000',overallstatus='000'", "");
            data = data.Replace(",OverallStatus='000'", "");
            data = data.Replace(",PackingListComment_Actual=''", "");
            data = data.Replace(",function () { return this.filter(a) }=''", "");

            JavaScriptSerializer json = new JavaScriptSerializer();
            var consdata = json.Deserialize<Dictionary<dynamic, dynamic>>(data);
           // List<Dictionary<dynamic, dynamic>> selectedvalues = new List<Dictionary<dynamic, dynamic>>();
            //selectedvalues = json.Deserialize<List<Dictionary<dynamic, dynamic>>>("[" + consdata["val"] + "]");
            int totalrows = 0;
            string outputmsg = "";
            string failedorders = "";
            //for (int i = 0; i < selectedvalues.Count; i++)
            //{
                try
                {
                    //var temp = json.Deserialize<Dictionary<string, string>>(selectedvalues[i]);
                    Dictionary<string, string> temp = new Dictionary<string, string>();
                    //temp.Add("condition", " where "+ selectedvalues[i]["Sno"]);
                    temp.Add("condition", consdata["condition"]);
                    temp.Add("tablename", consdata["tablename"]);
                    temp.Add("task", consdata["task"]);
                    temp.Add("currentuser", consdata["currentuser"]);
                    temp.Add("setfields", consdata["setfields"]);
                    SqlDataReader result = HomeController.updateTable(temp);

                    result.Read();
                    if (result.HasRows)
                    {
                        if (Convert.ToInt32(result[0]) == 1)
                        {
                            totalrows++;

                        }
                        else if (Convert.ToInt32(result[0]) == 2)
                        {
                            //failedorders += selectedvalues[i]["Name"];
                        }
                    }
                    if (result.RecordsAffected == 1 || result.RecordsAffected > 1)
                    {
                        totalrows++;
                    }
                }
                catch (Exception ex)
                {
                    failedorders += "SQL Exception" + ex.Message;
                    //failedorders += "SQL Exception" + ex.Message + ":" + selectedvalues[i]["Name"] + ",";
                }
           // }
            if (totalrows == 1)
                outputmsg = "Updated successfully";
            else
                outputmsg = "Partial success, Failed orders :" + failedorders + ",";

            return outputmsg;
        }

        [WebMethod]
         public string bulk_insert(string data)
         {
            string outputmsg = "";
            SqlDataReader result = null;
            JavaScriptSerializer json = new JavaScriptSerializer();
             var consdata = json.Deserialize<Dictionary<string, string>>(data);
             List<Dictionary<string, string>> selectedvalues = new List<Dictionary<string, string>>();
             selectedvalues = json.Deserialize<List<Dictionary<string, string>>>("[" + consdata["val"] + "]");
             try
             {
                result = BulkInsertUpdate(consdata, selectedvalues);
                if (result.RecordsAffected > 0)
                {
                    HttpContext.Current.Response.Write("Successfully inserted data");
                    outputmsg = "Successfully inserted data";
                }
             }
             catch (System.Data.SqlClient.SqlException e)
             {
                 HttpContext.Current.Response.Write(e.Message);
                //System.Console.WriteLine(e.Message);
                outputmsg = "Error while inserting" + e.Message;
             }
            return outputmsg;
         }

        public static SqlDataReader BulkInsertUpdate(Dictionary<string, string> parentdata, List<Dictionary<string, string>> selectedvalues)
         {
            SqlDataReader result = null;
             System.Data.DataSet objDS;
             SqlConnection objCon = Configurations.getConnection();
             SqlCommand objCom1;
             SqlDataAdapter objAdpt1;
             objDS = new DataSet();
             objCon.Open();
             objCom1 = new SqlCommand();
             objCom1.Connection = objCon;
             objAdpt1 = new SqlDataAdapter();

             System.Text.StringBuilder sb = new System.Text.StringBuilder(1000);
             System.IO.StringWriter sw = new System.IO.StringWriter(sb);

             objDS.WriteXml(sw, System.Data.XmlWriteMode.WriteSchema);

             SqlCommand objCom = new SqlCommand();
             objCom.Connection = objCon;
             //objCom.CommandType = CommandType.StoredProcedure;
             for (int i = 0; i < selectedvalues.Count; i++)
             {
                //" + parentdata["currentuser"] + "


                //                     objCom.CommandText = "Insert Into BulkOrderChanges(OrderNo ,ItemNo ,SetBillingBlock, Bulk_Status,ModifiedDate,ModifiedBy)" +
                //"SELECT '" + selectedvalues[i]["OrderNo"] + "','" + selectedvalues[i]["ItemNo"] + "','" + parentdata["setbillingblock"] + "','Awaiting Order Change',GETDATE(),'" + parentdata["user"] + "'" +

                //"Where '" + selectedvalues[i]["OrderNo"] + "-"+ selectedvalues[i]["ItemNo"] +"' Not IN (Select concat(OrderNo,'-',ItemNo) from BulkOrderChanges)";

                //                     int temp88 = objCom.ExecuteNonQuery();
                //                     if (temp88 == 0)
                //                     {

                //                         objCom.CommandText = "update BulkOrderChanges set SetBillingBlock = '" + parentdata["setbillingblock"] + "', Bulk_Status = 'Awaiting Order Change', ModifiedDate = GETDATE(),ModifiedBy='" + parentdata["user"] + "' where OrderNo = '" + selectedvalues[i]["OrderNo"] + "' and ItemNo = '" + selectedvalues[i]["ItemNo"] + "'";
                //                         objCom.ExecuteNonQuery();
                //                     }
                //string cond = "('" + selectedvalues[i]["UserLevel"] + "','" + selectedvalues[i]["SubLevel"] + "','" + selectedvalues[i]["Org"] + "','" + selectedvalues[i]["Name"] + "','" + selectedvalues[i]["Email"] + "','" + selectedvalues[i]["Active"] + "',GETDATE(),GETDATE(),'" + parentdata["user"] + "','" + parentdata["user"] + "')";
                //objCom.Parameters.Add(new SqlParameter("@task", "insertUser"));
                //objCom.Parameters.Add(new SqlParameter("@condition", cond));
                objCom.Parameters.Add(new SqlParameter("condition", parentdata["condition"]));
                objCom.Parameters.Add(new SqlParameter("tablename", parentdata["tablename"]));
                objCom.Parameters.Add(new SqlParameter("task", parentdata["task"]));
                objCom.Parameters.Add(new SqlParameter("currentuser", parentdata["currentuser"]));
                objCom.Parameters.Add(new SqlParameter("setfields", parentdata["setfields"]));
                objCom.CommandText = "Retrieve_Backlog_Data";
                objCom.CommandType = CommandType.StoredProcedure;
                //objCom.CommandText = "insert into UserDetails values('" + selectedvalues[i]["UserLevel"] + "','" + selectedvalues[i]["SubLevel"] + "','" + selectedvalues[i]["Org"] + "','"+ selectedvalues[i]["Name"] + "','" + selectedvalues[i]["Email"] + "','"+selectedvalues[i]["Active"]+"',GETDATE(),GETDATE(),'" + parentdata["user"] + "','" + parentdata["user"] + "')";
                 result = objCom.ExecuteReader(CommandBehavior.CloseConnection);
                 return result;
               
                 
             }
            //objCom.CommandText = "insert into BulkOrderChanges values('" + parentdata["orderno"] + "','" + parentdata["itemno"] + "','" + parentdata["rdd"] + "','" + parentdata["route"] + "','" + parentdata["setbillingblock"] + "','" + parentdata["setdeliveryblock"] + "','" + parentdata["rdd_change"] + "','" + parentdata["route_change"] + "','" + parentdata["rejection_line_items"] + "','" + parentdata["shipment_ref_removal"] + "','" + parentdata["bulk_status"] + "','" + parentdata["actionstatus"] + "')";
            //objCom.ExecuteNonQuery();
            return result;
        }

         [OperationContract]
         public void updateProducts(string data)
         {
             JavaScriptSerializer json = new JavaScriptSerializer();
             var consdata = json.Deserialize<Dictionary<string, string>>(data);
             try
             {
                 BulkInsertUpdate_Products(consdata["setfields"], consdata);
                 HttpContext.Current.Response.Write("Successfully inserted and updated data");
             }
             catch (System.Data.SqlClient.SqlException e)
             {
                 HttpContext.Current.Response.Write(e.Message);
                 //System.Console.WriteLine(e.Message);
             }
         }
         static void BulkInsertUpdate_Products(string data, Dictionary<string, string> parentdata)
         {
             JavaScriptSerializer json = new JavaScriptSerializer();
             List<dynamic> consdata = json.Deserialize<List<dynamic>>(data);

             //Steps:
             //1. Create the dataset.
             //2. Update the dataset.
             //3. Insert some data.
             //4. Save the changed data as XML
             //   and send XML to SQL Server through the stored procedure.

             //Declaration
             System.Data.DataSet objDS;
             SqlConnection objCon = Configurations.getConnection();
             SqlCommand objCom1;
             SqlDataAdapter objAdpt1;
             objDS = new DataSet();
             objCon.Open();
             objCom1 = new SqlCommand();
             objCom1.Connection = objCon;
             objAdpt1 = new SqlDataAdapter();


             //Step 1: Create the dataset.
             CreateDataSetFromPO(objDS, objCom1, objAdpt1, parentdata, "Products");

             //Step 2: Update the dataset.
             System.Data.DataTable tbl = objDS.Tables["BulkOrderChanges"];
             //DataRow aRow;
             int i = 0;
             char[] charsToTrim = { ' ' };
             foreach (DataRow aRow in tbl.Rows)
             {
                 //IEnumerable<dynamic> photoList = consdata.Where(c => c.EmployeeId == 101).Select(x => x.FirstName);
                 //var photoList = consdata.Where(o => o.FirstName == "Fname101111111111111").ToList();

                 for (int j = 0; j < consdata.Count; j++)
                 {
                     Dictionary<string, object> result = consdata[j];
                     if (result["Bulk_ID"].ToString() == aRow["Bulk_ID"].ToString() && result["Bulk_ID"].ToString() != "0")
                     {
                        

                         //aRow["Shipping_ID"] = Convert.IsDBNull(result["Shipping_ID"]) ? 0 : result["Shipping_ID"];
                         aRow["OrderNo"] = Convert.ToInt32(result["OrderNo"]);
                         aRow["ItemNo"] = checkDictionary(result, "ItemNo");
                         aRow["RDD"] = checkDictionary(result, "RDD");
                         aRow["Route"] = checkDictionary(result, "Route");
                         //aRow["Shipping_ID"] = checkDictionary(result, "Shipping_ID");
                         //aRow["Price"] = checkDictionary(result, "Price").ToString().Replace(" ", String.Empty);
                         aRow["SetBillingBlock"] = Convert.ToInt32(result["SetBillingBlock"]);
                         //aRow["Quantity"] = checkDictionary(result, "Quantity").ToString().Replace(" ", String.Empty);
                         aRow["SetDeliveryBlock"] = checkDictionary(result, "SetDeliveryBlock");
                         aRow["RDD_Change"] = checkDictionary(result, "RDD_Change");
                         aRow["Route_Change"] = checkDictionary(result, "Route_Change");
                         aRow["Rejection_Line_Items"] = checkDictionary(result, "Rejection_Line_Items");
                         aRow["Shipment_Ref_Removal"] = checkDictionary(result, "Shipment_Ref_Removal");
                         aRow["Bulk_Status"] = checkDictionary(result, "Bulk_Status");
                         aRow["ActionStatus"] = checkDictionary(result, "ActionStatus");
                         //aRow["Bulk_ID"] = checkDictionary(result, "Bulk_ID");
                         aRow["ModifiedDate"] = checkDictionary(result, "ModifiedDate");
                         aRow["ModifiedBy"] = parentdata["currentuser"];
                     }
                 }

             }

             //Step 3: Insert some data.

             for (int ii = 0; ii < consdata.Count; ii++)
             {
                 Dictionary<string, object> result = consdata[ii];
                 if (result["Bulk_ID"].ToString() == "0")
                 {

                     DataRow newRow = tbl.NewRow();
                     int j = ii + 100;

                     newRow["Bulk_ID"] = Convert.IsDBNull(result["Bulk_ID"]) ? 0 : result["Bulk_ID"];
                     newRow["OrderNo"] = checkDictionary(result, "OrderNo");
                     newRow["ItemNo"] = Convert.ToInt32(result["ItemNo"]);
                     newRow["RDD"] = checkDictionary(result, "RDD");
                     newRow["Route"] = checkDictionary(result, "Route");
                     newRow["Part_No_Localization"] = checkDictionary(result, "Part_No_Localization");
                     //newRow["Shipping_ID"] = checkDictionary(result, "Shipping_ID");
                     newRow["SetBillingBlock"] = checkDictionary(result, "SetBillingBlock");
                     //newRow["Row_ID"] = Convert.ToInt32(result["Row_ID"]);
                     newRow["SetDeliveryBlock"] = checkDictionary(result, "SetDeliveryBlock");
                     newRow["RDD_Change"] = checkDictionary(result, "RDD_Change");
                     newRow["Route_Change"] = checkDictionary(result, "Route_Change");
                     newRow["Rejection_Line_Items"] = checkDictionary(result, "Rejection_Line_Items");
                     newRow["Shipment_Ref_Removal"] = checkDictionary(result, "Shipment_Ref_Removal");
                     newRow["Bulk_Status"] = checkDictionary(result, "Bulk_Status");
                     newRow["ActionStatus"] = checkDictionary(result, "ActionStatus");
                     newRow["ModifiedBy"] = parentdata["currentuser"];
                     newRow["ModifiedDate"] = checkDictionary(result, "ModifiedDate");
                     tbl.Rows.Add(newRow);
                 }
             }


             //Step 4: Save the changed data as XML, 
             //and send the XML to SQL Server through the stored procedure.
             //In SQL Server, you wrote a stored procedure that
             //accepts this XML and updates the corresponding table.

             //SaveThroughXML(objDS, objCon,parentdata);
             tbl = objDS.Tables["BulkOrderChanges"];
             System.Text.StringBuilder sb = new System.Text.StringBuilder(1000);
             System.IO.StringWriter sw = new System.IO.StringWriter(sb);

             foreach (DataColumn col in tbl.Columns)
             {
                 col.ColumnMapping = System.Data.MappingType.Attribute;
             }

             objDS.WriteXml(sw, System.Data.XmlWriteMode.WriteSchema);

             SqlCommand objCom = new SqlCommand();
             objCom.Connection = objCon;
             objCom.CommandType = CommandType.StoredProcedure;
             objCom.CommandText = "sp_Bulkupdate";

             objCom.Parameters.Add(new SqlParameter("@empdata",
                    System.Data.SqlDbType.NText));
             objCom.Parameters.Add(new SqlParameter("@poid",
                    System.Data.SqlDbType.NText));
             objCom.Parameters.Add(new SqlParameter("@task",
                    System.Data.SqlDbType.NText));
             objCom.Parameters[0].Value = sb.ToString();
             objCom.Parameters[1].Value = parentdata["condition"].ToString();
             objCom.Parameters[2].Value = "Products";
             objCom.ExecuteNonQuery();
         }

         static void CreateDataSetFromPO(DataSet objDS,
                 SqlCommand objCom1, SqlDataAdapter objAdpt1, Dictionary<string, string> parentdata, string task)
         {

             if (task == "Products")
             {
                 //Create related objects.
                 objCom1.CommandType = CommandType.Text;
                 objCom1.CommandText = "SELECT * from BulkOrderChanges where PO_ID= " + parentdata["condition"].ToString();

                 //Fill the Orders table.
                 objAdpt1.SelectCommand = objCom1;
                 objAdpt1.TableMappings.Add("Table", "BulkOrderChanges");
                 objAdpt1.Fill(objDS);
             }
         }

         static string checkDictionary(Dictionary<string, object> src, string key)
         {
             if (src.ContainsKey(key) && src[key] != null)
                 return src[key].ToString();
             else
                 return "";
         }

        [WebMethod]
        public void Mail(string data)
        {
            try
            {
                
                Console.WriteLine("Mailing to User");
                JavaScriptSerializer json = new JavaScriptSerializer();
                var consdata = json.Deserialize<Dictionary<string, string>>(data);

                JavaScriptSerializer json1 = new JavaScriptSerializer();
                var setFields = json1.Deserialize<Dictionary<string, string>>(consdata["setfields"]);

                SmtpClient mySmtpClient = new SmtpClient("smtp3.hp.com");

                // set smtp-client with basicAuthentication
                mySmtpClient.UseDefaultCredentials = false;
                //System.Net.NetworkCredential basicAuthenticationInfo = new
                //System.Net.NetworkCredential("username", "password");
                //mySmtpClient.Credentials = basicAuthenticationInfo;

                // add from,to mailaddresses
                MailAddress from = new MailAddress("NOREPLY@hp.com");
                MailAddress to = new MailAddress("varshini.raman@hp.com", "Varshini Raman");
                //MailAddress to = new MailAddress(setFields["Created_By"].ToString());
                MailAddress cc = new MailAddress("varshini.raman@hp.com", "Varshini Raman");
                MailAddress bcc = new MailAddress("varshini.raman@hp.com", "Varshini Raman");
                MailMessage myMail = new System.Net.Mail.MailMessage(from, to);
                myMail.CC.Add(cc);
                myMail.Bcc.Add(bcc);
                // add ReplyTo
                //MailAddress replyto = new MailAddress("varshini.raman@hp.com", "Varshini Raman");
                //myMail.ReplyToList.Add(replyto);
                //String attachment = @"C:\EOP_UpfrontReports\" + destfilename;

                // set body-message and encoding
                string body = null;
                body += "<br />" + "<br />";
                body = "Hello CSM ,<br/><br/>";
                body += "" + " ,<br/><br/>";

                body += "<br />" + "<br />";
                body += "Thank you for your patience.";
                body += "<br />" + "<br />";
                body += "**This is electronically generated mail. Please do not reply**" + "<br />";
                body += "<br />" + "<br />";
                body += "Regards," + "<br />";
                body += "OSS Comments Upload";

                myMail.Body = consdata["condition"].ToString();

                // set subject and encoding
                myMail.Subject = "";
                myMail.SubjectEncoding = System.Text.Encoding.UTF8;


                myMail.BodyEncoding = System.Text.Encoding.UTF8;
                // text or html
                myMail.IsBodyHtml = true;

                mySmtpClient.Send(myMail);
            }

            catch (SmtpException ex)
            {
                Console.WriteLine(ex.Message);
                throw new ApplicationException
                  ("SmtpException has occured: " + ex.Message);
            }
        }

        [WebMethod]
        public string updateBotStatus(string data) {
            // Not working in Server
            //try
            //{
            //    string exefilePath = ConfigurationManager.AppSettings["exefilePath"];
            //    //Process.Start(@"C:\Users\ramanv\Documents\Visual Studio 2015\Projects\HP.Robotics.OSSCommentsUpload\HP.Robotics.OSSOutputtoExcel\bin\Debug\HP.Robotics.OSSOutputtoExcel.exe");
            //    Process.Start(exefilePath);
            //    return "Upload Successfull";
            //}
            //catch (Exception e) { return "Error during Upload : "+e.Message; }

            data = data.Replace(",OverallStatus='000',overallstatus='000'", "");
            data = data.Replace(",OverallStatus='000'", "");
            data = data.Replace(",PackingListComment_Actual=''", "");
            data = data.Replace(",function () { return this.filter(a) }=''", "");

            JavaScriptSerializer json = new JavaScriptSerializer();
            var consdata = json.Deserialize<Dictionary<dynamic, dynamic>>(data);
            int totalrows = 0;
            string outputmsg = "";
            string failedorders = "";
                try
                {
                    //var temp = json.Deserialize<Dictionary<string, string>>(selectedvalues[i]);
                    Dictionary<string, string> temp = new Dictionary<string, string>();
                    temp.Add("condition", consdata["condition"]);
                    temp.Add("tablename", consdata["tablename"]);
                    temp.Add("currentuser", consdata["currentuser"]);
                    temp.Add("task", consdata["task"]);
                    SqlDataReader result = HomeController.updateTable(temp);

                    result.Read();
                    if (result.HasRows)
                    {
                        if (Convert.ToInt32(result[0]) == 1)
                        {
                            totalrows++;
                            outputmsg = "Status Updated successfully for "+ consdata["botname"];
                        }
                        else if (Convert.ToInt32(result[0]) == 2)
                        {
                            failedorders += "Failed to Update Status for "+ consdata["botname"];
                        }
                    }
                    if (result.RecordsAffected == 1 || result.RecordsAffected > 1)
                    {
                        totalrows++;
                        outputmsg = consdata["botname"] + " Bot has started, \n Please check your mail for progress";
                    }
                    if (result.RecordsAffected == -1) {
                    failedorders += "Failed to Update Status for " + consdata["botname"];
                    outputmsg = "Failed to start "+consdata["botname"] + " as you are not Authorized";
                    }
                }
                catch (Exception ex)
                {
                    failedorders += "SQL Exception" + ex.Message;
                    outputmsg = failedorders;
                }

            return outputmsg;
        }
    }
}
