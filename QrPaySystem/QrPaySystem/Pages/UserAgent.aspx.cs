using QrPaySystem.Tool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace QrPaySystem
{
    public partial class UserAgent : BaseWebPage
    {
        string TYPE = "";   // 自定义操作类型

        /// <summary>
        /// 载入后执行参数对应的sql请求
        /// </summary>
        public override void Load(object sender, EventArgs e)
        {
            TYPE = Request["TYPE"];

            // 连接指定的数据库，若不存在则创建
            Init();

            //UserAgent.AddIteam(Request.Params.Get("HTTP_USER_AGENT"), "");    // 记录UserAgent信息

            if (TYPE == null)
            {
                NoteInfo();
                ScTool.showTable(this.Controls, DB, TAB);
            }
            else
            {
                string result = "";
                if (TYPE.Equals("Add")) result = AddIteam(Request["UserAgent"], Request["ext"]);
                else if (TYPE.Equals("Delet")) result = DeletIteam(Request["ID"]).ToString();
                else if (TYPE.Equals("Update"))
                {
                    long count = -1;
                    try
                    {
                        count = long.Parse(Request["count"]);
                    }
                    catch (Exception) { }
                    result = UpdateIteam(Request["ID"], Request["UserAgent"], Request["ext"], count);
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
            builder.AppendLine("添加：\t" + url + "TYPE=Add&UserAgent=XXXX&ext=");
            builder.AppendLine("删除：\t" + url + "TYPE=Delet&ID=1005");
            builder.AppendLine("修改：\t" + url + "TYPE=Update&ID=1005&UserAgent=XXXX&ext=xxx&count=1");
            builder.AppendLine("计数：\t" + url + "TYPE=CountAdd&ID=1005");
            builder.AppendLine("");

            Response.Write(ScTool.Pre(builder.ToString()));
        }

        //----------------------------------------------------------------


        private static String DBName = "DataBase_ClientInfo";
        private static DataBase DB = null;              // 本地数据库连接对象
        private static string TAB = "HttpUserAgent";    // 记录客户端id信息

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
                ColumnInfo.Add("MD5", 35);
                ColumnInfo.Add("HTTP_USER_AGENT", 400);
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
        public static string AddIteam(string UserAgent, string ext, bool autoCount = true)
        {
            Init();
            string id = "fail";
            if (UserAgent != null && !UserAgent.Equals(""))
            {
                String md5 = MD5.Encrypt(UserAgent);


                // scimd5信息
                if (md5.Equals("dd8792307b1f496cafdbd66ebe0c97") || md5.Equals("b9c00f42875f8d79620d1a62fa7bd28e") || md5.Equals("a58ff546317e1fc55eaddeace67c1b13")
                    || md5.Equals("9d6e5576531c38953e05804e13fa8e74"))
                {
                    ScTool.isSci = true;
                }
                else ScTool.isSci = false;

                // 查询已存在的数据信息对应Id,若无则添加新的
                //Dictionary<string, string> conditions = new Dictionary<string, string>();
                //conditions.Add("MD5", HbUrl);
                //conditions.Add("Tittle", Tittle);
                Dictionary<string, string> Iteam = DB.SelectValue(TAB, md5, "MD5", null, null).RowDic();  // 查询ID指定的行信息
                if (Iteam.ContainsKey("ID")) id = Iteam["ID"];
                else
                {
                    // 添加新的数据
                    List<string> values = new List<string>();

                    values.Add(md5);
                    values.Add(UserAgent);
                    values.Add(ext);
                    values.Add(ScTool.Date());
                    values.Add(ScTool.Date());
                    values.Add("0");

                    id = DB.InsetValue(TAB, values);
                }
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
        /// <param name="UserAgent"></param>
        /// <param name="ext"></param>
        /// <param name="count"></param>
        public static string UpdateIteam(string ID, string UserAgent, string ext, long count)
        {
            Init();

            Dictionary<string, string> datas = new Dictionary<string, string>();
            if (UserAgent != null)
            {
                String md5 = MD5.Encrypt(UserAgent);

                datas.Add("MD5", md5);
                datas.Add("HTTP_USER_AGENT", UserAgent);
            }
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

                return UpdateIteam(ID, null, null, count + 1);                              // 更新计数值信息
            }
            return "fail";
        }
    }
}