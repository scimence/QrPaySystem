using QrPaySystem.Tool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace QrPaySystem.Pages
{
    public partial class MiniDialogInfo : System.Web.UI.Page
    {
        string TYPE = "";   // 自定义操作类型

        protected void Page_Load(object sender, EventArgs e)
        {
            TYPE = Request["TYPE"];

            // 连接指定的数据库，若不存在则创建
            Init();

            //UserAgent.AddIteam(Request.Params.Get("avatar"), "");    // 记录UserAgent信息

            if (TYPE == null) TYPE = "";

            if (TYPE.Equals("S"))
            {
                NoteInfo();
                ScTool.showTable(this.Controls, DB, TAB);
            }
            else
            {
                string result = "";
                if (TYPE.Equals("Add")) result = AddIteam(Request["showRectangle"], Request["showIconUrl"], Request["showNoteInfo"], Request["showButtonInfo"], Request["showPicUrl"], Request["ext"]);
                else if (TYPE.Equals("Delet")) result = DeletIteam(Request["ID"]).ToString();
                else if (TYPE.Equals("Update"))
                {
                    long count = -1;
                    try
                    {
                        count = long.Parse(Request["count"]);
                    }
                    catch (Exception) { }
                    result = UpdateIteam(Request["ID"], Request["showRectangle"], Request["showIconUrl"], Request["showNoteInfo"], Request["showButtonInfo"], Request["showPicUrl"], Request["ext"]);
                }
                else
                {
                    result = getRandomResult();
                }

                Response.Write(result);
            }
        }

        /// <summary>
        /// 随机获取一个结果集
        /// </summary>
        /// <returns></returns>
        private string getRandomResult()
        {
            Init();

            if (DB.ExistTab(TAB))
            {
                //string sql = "select * from [" + TAB + "]";
                string sql = "select top 1000 * from [" + TAB + "]";
                sql = sql + " order by ID desc";
                
                List<Dictionary<String, String>> rowList = DB.Execute(sql).RowList();   // 查询结果集合
                if (rowList.Count == 0) return "";

                // 随机选取一行结果
                Random rnd = new Random(DateTime.Now.Millisecond);
                int index = rnd.Next(0, rowList.Count);
                Dictionary<String, String> rowDic = rowList[index];

                string ID = rowDic["ID"];
                sql = "select * from [" + TAB + "]" + " where ID='" + ID + "'";
                string result = DB.Execute(sql).ToString();
                return result;
            }
            return "";
        }

        /// <summary>
        /// 接口使用说明信息
        /// QrUrl, string HbUrl, string Tittle, string ext
        /// </summary>
        private void NoteInfo()
        {
            String url = "http://" + Request.Params.Get("HTTP_HOST") + "/" + this.GetType().Name.Replace("_", "/").Replace("/aspx", ".aspx") + "?";
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("接口使用说明：");
            builder.AppendLine("");
            builder.AppendLine("添加：\t" + url + "TYPE=Add&showRectangle=true&showIconUrl=1&showNoteInfo=2&showButtonInfo=3&showPicUrl=4&ext=");
            builder.AppendLine("删除：\t" + url + "TYPE=Delet&ID=1005");
            builder.AppendLine("修改：\t" + url + "TYPE=Update&ID=1005&showRectangle=true&showIconUrl=1&showNoteInfo=2&showButtonInfo=3&showPicUrl=4&ext=5");
            builder.AppendLine("");

            Response.Write(ScTool.Pre(builder.ToString()));
        }

        //----------------------------------------------------------------


        private static String DBName = "DataBase_Mini";
        private static DataBase DB = null;              // 本地数据库连接对象
        private static string TAB = "DialogInfo";      // 记录用户初次使用信息

        private static void Init()
        {
            if (DB == null)
            {
                // 连接指定的数据库，若不存在则创建
                DB = new DataBase(DBName, ScTool.UserName, ScTool.Password);
                confirmOrderTab();
            }
        }

        /// <summary>
        /// 确保数据库中，存在订单信息表，若不存在则创建
        /// </summary>
        private static void confirmOrderTab()
        {
            // 创建数据表
            if (!DB.ExistTab(TAB))
            {
                Dictionary<string, int> ColumnInfo = new Dictionary<string, int>();
                ColumnInfo.Add("showRectangle", 10);
                ColumnInfo.Add("showIconUrl", 400);
                ColumnInfo.Add("showNoteInfo", 200);
                ColumnInfo.Add("showButtonInfo", 200);
                ColumnInfo.Add("showPicUrl", 400);
                ColumnInfo.Add("ext", 200);
                ColumnInfo.Add("creatDateTime", 20);
                ColumnInfo.Add("lastDateTime", 20);

                DB.CreateTable(TAB, ColumnInfo);
            }
        }

        /// <summary>
        /// 创建新的UserAgent信息，在数据库中记录信息
        /// </summary>
        public static string AddIteam(string showRectangle, string showIconUrl, string showNoteInfo, string showButtonInfo, string showPicUrl, string ext)
        {
            Init();
            string id = "fail";

            if (showRectangle == null || showRectangle.Equals("")) return id;
            if (showIconUrl == null || showIconUrl.Equals("")) return id;
            if (showNoteInfo == null || showNoteInfo.Equals("")) return id;
            if (showButtonInfo == null || showButtonInfo.Equals("")) return id;
            if (showPicUrl == null || showPicUrl.Equals("")) return id;

            // 查询已存在的数据信息对应Id,若无则添加新的
            Dictionary<string, string> conditions = new Dictionary<string, string>();
            conditions.Add("showRectangle", showRectangle);
            //conditions.Add("showIconUrl", showIconUrl);
            conditions.Add("showNoteInfo", showNoteInfo);
            conditions.Add("showButtonInfo", showButtonInfo);
            conditions.Add("showPicUrl", showPicUrl);
            if(ext != null) conditions.Add("ext", ext);

            Dictionary<string, string> Iteam = DB.SelectValue(TAB, showIconUrl, "showIconUrl", null, conditions).RowDic();  // 查询ID指定的行信息
            if (Iteam.ContainsKey("ID")) id = Iteam["ID"];
            else
            {
                // 添加新的数据
                List<string> values = new List<string>();

                values.Add(showRectangle);
                values.Add(showIconUrl);
                values.Add(showNoteInfo);
                values.Add(showButtonInfo);
                values.Add(showPicUrl);
                values.Add(ext);
                values.Add(ScTool.Date());
                values.Add(ScTool.Date());

                id = DB.InsetValue(TAB, values);
            }

            return id;
        }

        /// <summary>
        /// 删除指定Id对应项
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        private static bool DeletIteam(string Id)
        {
            return DB.DeletValue(TAB, Id, "ID");
        }

        /// <summary>
        /// 更新TAB表中指定项的数据
        /// </summary>
        /// <param name="TAB"></param>
        /// <param name="ID"></param>
        /// <param name="avatar"></param>
        /// <param name="ext"></param>
        /// <param name="count"></param>
        public static string UpdateIteam(string ID, string showRectangle, string showIconUrl, string showNoteInfo, string showButtonInfo, string showPicUrl, string ext)
        {
            Init();

            Dictionary<string, string> datas = new Dictionary<string, string>();
            if (showRectangle != null) datas.Add("showRectangle", showRectangle);
            if (showIconUrl != null) datas.Add("showIconUrl", showIconUrl);
            if (showNoteInfo != null) datas.Add("showNoteInfo", showNoteInfo);
            if (showButtonInfo != null) datas.Add("showButtonInfo", showButtonInfo);
            if (showPicUrl != null) datas.Add("showPicUrl", showPicUrl);
            if (ext != null) datas.Add("ext", ext);
            datas.Add("lastDateTime", ScTool.Date()); // 日期时间自动修改

            return DB.UpdateValue(TAB, ID, datas, "ID");
        }


    }
}