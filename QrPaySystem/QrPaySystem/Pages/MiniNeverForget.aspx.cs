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
    public partial class MiniNeverForget : System.Web.UI.Page
    {
        string TYPE = "";   // 自定义操作类型

        protected void Page_Load(object sender, EventArgs e)
        {
            TYPE = Request["TYPE"];

            // 连接指定的数据库，若不存在则创建
            Init();

            //UserAgent.AddIteam(Request.Params.Get("avatar"), "");    // 记录UserAgent信息

            if (TYPE == null) TYPE = "Add";

            if (TYPE.Equals("S"))
            {
                NoteInfo();
                ScTool.showTable(this.Controls, DB, TAB);
            }
            else
            {
                string result = "";
                if (TYPE.Equals("Add")) result = AddIteam(Request["nickname"], Request["avatar"], Request["ext"]);
                else if (TYPE.Equals("Delet")) result = DeletIteam(Request["ID"]).ToString();
                else if (TYPE.Equals("Update"))
                {
                    long count = -1;
                    try
                    {
                        count = long.Parse(Request["count"]);
                    }
                    catch (Exception) { }
                    result = UpdateIteam(Request["ID"], Request["nickname"], Request["avatar"], Request["ext"], count);
                }
                else if (TYPE.Equals("CountAdd")) result = CountAdd(Request["ID"]);

                Response.Write(result);
            }
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
            builder.AppendLine("添加：\t" + url + "TYPE=Add&nickname=nickname1&avatar=avatar1&ext=");
            builder.AppendLine("删除：\t" + url + "TYPE=Delet&ID=1005");
            builder.AppendLine("修改：\t" + url + "TYPE=Update&ID=1005&nickname=nickname2&avatar=avatar2&ext=xxx&count=1");
            builder.AppendLine("计数：\t" + url + "TYPE=CountAdd&ID=1005");
            builder.AppendLine("https://tfs.alipayobjects.com/images/partner/");
            builder.AppendLine("");

            Response.Write(ScTool.Pre(builder.ToString()));
        }

        //----------------------------------------------------------------


        private static String DBName = "DataBase_Mini";
        private static DataBase DB = null;              // 本地数据库连接对象
        private static string TAB = "NeverForget";      // 记录用户初次使用信息

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
                ColumnInfo.Add("nickname", 100);
                ColumnInfo.Add("avatar", 400);
                ColumnInfo.Add("ext", 200);
                ColumnInfo.Add("creatDateTime", 20);
                ColumnInfo.Add("lastDateTime", 20);
                ColumnInfo.Add("count", 20);

                DB.CreateTable(TAB, ColumnInfo);
            }
        }

        /// <summary>
        /// 创建新的UserAgent信息，在数据库中记录信息
        /// </summary>
        public static string AddIteam(string nickname, string avator, string ext, bool autoCount = true)
        {
            Init();
            string id = "fail";
            if (nickname == null || nickname.Equals("")) return id;
            if (avator == null || avator.Equals("")) return id;
            
            // 查询已存在的数据信息对应Id,若无则添加新的
            Dictionary<string, string> conditions = new Dictionary<string, string>();
            conditions.Add("avatar", avator);
            //conditions.Add("Tittle", Tittle);
            Dictionary<string, string> Iteam = DB.SelectValue(TAB, nickname, "nickname", null, conditions).RowDic();  // 查询ID指定的行信息
            if (Iteam.ContainsKey("ID")) id = Iteam["ID"];
            else
            {
                // 添加新的数据
                List<string> values = new List<string>();

                values.Add(nickname);
                values.Add(avator);
                values.Add(ext);
                values.Add(ScTool.Date());
                values.Add(ScTool.Date());
                values.Add("0");

                id = DB.InsetValue(TAB, values);
            }
            
            if (autoCount) CountAdd(id);

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
        public static string UpdateIteam(string ID, string nickname, string avator, string ext, long count)
        {
            Init();

            Dictionary<string, string> datas = new Dictionary<string, string>();
            if (nickname != null) datas.Add("nickname", nickname);
            if (avator != null) datas.Add("avatar", avator);
            if (ext != null) datas.Add("ext", ext);
            datas.Add("lastDateTime", ScTool.Date()); // 日期时间自动修改
            if (count >= 0) datas.Add("count", count.ToString());

            return DB.UpdateValue(TAB, ID, datas, "ID");
        }

        /// <summary>
        /// 计数值加1
        /// </summary>
        /// <param name="ID"></param>
        public static string CountAdd(string ID)
        {
            Init();

            if (ID != null)
            {
                Dictionary<string, string> Iteam = DB.SelectValue(TAB, ID, "ID").RowDic();  // 查询ID指定的行信息
                long count = long.Parse(Iteam["count"]);

                return UpdateIteam(ID, null, null, null, count + 1);                              // 更新计数值信息
            }
            return "fail";
        }

    }
}