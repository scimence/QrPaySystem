using QrPaySystem.Tool;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace QrPaySystem.Pages
{
    /// <summary>
    /// 数据库信息查询示例
    /// 查询Json数据：http://localhost:5516/Sql.aspx?SELECT * FROM 数据表1
    /// 查询Tab数据：http://localhost:5516/Sql.aspx?TAB:SELECT * FROM 数据表1
    /// </summary>
    public partial class Sql : BaseWebPage
    {
        /// <summary>
        /// 载入后执行参数对应的sql请求
        /// </summary>
        public override void Load(object sender, EventArgs e)
        {
            if (UserType != 2) Response.Redirect("../PayFor/SDK.aspx");

            if (Request["DB"] != null && Request["TAB"] != null)
            {
                string DBName = Request["DB"];
                string TAB = Request["TAB"];

                this.Controls.Clear();
                DataBase DB = new DataBase(DBName, ScTool.UserName, ScTool.Password);
                ScTool.showTable(this.Controls, DB, TAB);

                return;
            }
            
            if (param.Equals(""))             // 查询提示信息
            {
                //String Url = Request.Url.ToString();
                string sql = "SELECT * FROM 数据表1";

                Response.Write(P("数据库信息查询示例"));
                Response.Write(P("查询Json数据：" + url + "?" + sql));
                Response.Write(P("查询Tab数据：" + url + "?" + "TAB:" + sql));
            }
            else
            {
                if (param.StartsWith("TAB:")) // 查询数据信息，返回Tab表
                {
                    param = param.Substring(4);
                    Table table = ExecuteTable(param);
                    this.Controls.Add(table);
                }
                else
                {                                   // 查询数据信息，返回Json数据
                    String data = Execute(param);
                    Response.Write(data);
                }
            }
        }


        /// <summary>
        /// 为数据添加段落标签
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private string P(string data)
        {
            return "<p>" + data + "</p>";
        }

        #region 本地数据库操作逻辑

        /// <summary>
        /// 本地数据库连接串信息
        /// </summary>
        //public static string connectionString = @"Data Source=.\JSQL2008;Initial Catalog=DataBase1;User ID=sa;Password=Sa12345789"; // 连接本地数据库DataBase1
        public static string connectionString = @"Data Source=.\JSQL2008;Initial Catalog=QrDataBase_pre;User ID=sa;Password=Sa12345789"; // 连接本地数据库DataBase1
        

        /// <summary>
        /// 连接数据库,执行sql语句
        /// connectionString = @"Data Source=.\SQLEXPRESS;AttachDbFilename=|DataDirectory|\NoteBook.mdf;Integrated Security=True;User Instance=True";  // 连接附加数据库
        /// connectionString = @"Data Source=.\JSQL2008;Initial Catalog=DataBase1;User ID=sa;Password=Sa12345789"; // 连接本地数据库DataBase1
        /// queryString = "SELECT * FROM 数据表1";
        /// </summary>
        public static String Execute(string queryString, string connectionString = null)
        {
            try
            {
                if (connectionString == null || connectionString.Equals("")) connectionString = Sql.connectionString;
                //string queryString =  "SELECT * FROM 数据表1";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand(queryString, connection);
                    SqlDataReader reader = command.ExecuteReader();

                    String jsonData = ToJson(reader);

                    connection.Close();

                    if (jsonData.Trim().Equals("")) jsonData = "success";
                    return jsonData;
                }
            }
            catch (Exception ex)
            {
                return "fail";
            }
        }

        /// <summary>
        /// DataReader转换为Json串
        /// </summary>
        public static string ToJson(SqlDataReader dataReader)
        {
            StringBuilder Builder = new StringBuilder();

            int rows = 0;
            while (dataReader.Read())
            {
                if (rows++ > 0) Builder.Append(",");

                // 行数据转Json
                Builder.Append("{");
                for (int i = 0; i < dataReader.FieldCount; i++)
                {
                    if (i > 0) Builder.Append(",");

                    // 列名称
                    string strKey = dataReader.GetName(i);
                    strKey = "\"" + strKey + "\"";

                    // 列数据
                    Type type = dataReader.GetFieldType(i);
                    string strValue = dataReader[i].ToString();
                    strValue = String.Format(strValue, type).Trim();
                    if (type == typeof(string) || type == typeof(DateTime)) strValue = "\"" + strValue + "\"";

                    Builder.Append(strKey + ":" + strValue);
                }
                Builder.Append("}");
            }
            dataReader.Close();

            if (rows > 1) return "[" + Builder.ToString() + "]";
            else return Builder.ToString();
        }


        /// <summary>
        /// 连接数据库,执行sql语句,返回Table表
        /// queryString = "SELECT * FROM 数据表1";
        /// </summary>
        public static Table ExecuteTable(string queryString, string connectionString = null)
        {
            try
            {
                if (connectionString == null || connectionString.Equals("")) connectionString = Sql.connectionString;
                //string queryString =  "SELECT * FROM 数据表1";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand(queryString, connection);
                    SqlDataReader reader = command.ExecuteReader();

                    Table table = ToTable(reader);    // 转化为列list数据
                    connection.Close();

                    return table;
                }
            }
            catch (Exception ex)
            {
                return new Table();
            }
        }

        /// <summary>
        /// DataReader转换为Table表
        /// </summary>
        public static Table ToTable(SqlDataReader dataReader)
        {
            Table table = new Table();
            table.Attributes.Add("border", "1");    // 添加边框线
            table.Attributes.Add("BorderStyle", "Solid");
            table.Attributes.Add("width", "100%");  // 表格宽度
            table.Attributes.Add("cellspacing", "0");
            table.Attributes.Add("bordercolor", "DarkGray");

            TableHeaderRow header = new TableHeaderRow();

            bool firstrow = true;
            while (dataReader.Read())
            {
                TableRow row = new TableRow();

                // 行数据转Json
                for (int i = 0; i < dataReader.FieldCount; i++)
                {
                    // Tab表头
                    if (firstrow)
                    {
                        string strKey = dataReader.GetName(i);  // 列名称

                        TableHeaderCell headCell = new TableHeaderCell();
                        headCell.Text = strKey;

                        header.Cells.Add(headCell);
                    }

                    // Tab行数据
                    Type type = dataReader.GetFieldType(i);
                    string strValue = dataReader[i].ToString();
                    strValue = String.Format(strValue, type).Trim();

                    TableCell cell = new TableCell();
                    cell.Text = strValue;

                    row.Cells.Add(cell);
                }

                if (firstrow)
                {
                    table.Rows.Add(header);
                    firstrow = false;
                }
                table.Rows.Add(row);
            }

            dataReader.Close();

            return table;
        }

        #endregion


    }
}