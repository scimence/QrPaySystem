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
    /// <summary>
    /// 记录和查询 软件信息
    /// </summary>
    public partial class SoftInfo : BaseWebPage
    {
        DataBase DB;
        public static string TAB = "SoftInfo";
        string TYPE = "";

        /// <summary>
        /// 载入后执行参数对应的sql请求
        /// </summary>
        public override void Load(object sender, EventArgs e)
        {
            DB = new DataBase(ScTool.DBName("pre"), ScTool.UserName, ScTool.Password);
            confirmOrderTab(DB);

            TYPE = Request["TYPE"];

            if (TYPE != null)
            {
                string result = "";

                if (TYPE.Equals("Add")) result = Add(DB, Request["softName"], Request["price"], Request["linkUrl"], Request["recomondUrl"], Request["ext"]);
                else if (TYPE.Equals("Update")) result = Update(DB, Request["ID"], Request["softName"], Request["price"], Request["linkUrl"], Request["recomondUrl"], Request["ext"]);
                else if (TYPE.Equals("Delet")) result = Delet(DB, Request["ID"]).ToString();
                else if (TYPE.Equals("Select"))
                {
                    result = Select(DB, Request["softName"], Request["key"]);
                    if (Request["key"].Equals("price") || Request["key"].Equals("ext")) // 若查询金额信息，则进行加密
                    {
                        result = Request["key"] + "(" + result + ")" + Request["key"];
                        result = Locker.Encrypt(result, Request["softName"]);
                    }
                }
                else if (TYPE.Equals("DeletThisTab")) result = DB.DeletTable(TAB).ToString();

                Response.Write(result);
                return;
            }
            else
            {
                // 显示接口使用说明
                NoteInfo();

                // 显示指定类型的订单信息
                ScTool.showTable(this.Controls, DB, TAB);
            }
        }

        /// <summary>
        /// 接口使用说明信息
        /// </summary>
        private void NoteInfo()
        {
            String url = "http://" + Request.Params.Get("HTTP_HOST") + "/" + this.GetType().Name.Replace("_", "/").Replace("/aspx", ".aspx") + "?";
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("软件信息接口，使用说明：");
            builder.AppendLine("");
            builder.AppendLine("添加：\t" + url + "TYPE=Add&softName=easyIcon&price=1&linkUrl=http://scimence.oschina.io/easyicon/&recomondUrl=推荐链接&ext=拓展参数");
            builder.AppendLine("修改：\t" + url + "TYPE=Update&ID=100&softName=easyIcon软件&price=1&linkUrl=http://scimence.oschina.io/easyicon/&recomondUrl=推荐链接&ext=拓展参数");
            builder.AppendLine("删除：\t" + url + "TYPE=Delet&ID=101");
            builder.AppendLine("查询：\t" + url + "TYPE=Select&softName=easyIcon&key=price");
            builder.AppendLine("");

            Response.Write(ScTool.Pre(builder.ToString()));
        }

        /// <summary>
        /// 确保数据库中，存在订单信息表，若不存在则创建
        /// </summary>
        public static void confirmOrderTab(DataBase DB)
        {
            // 创建数据表
            if (!DB.ExistTab(TAB))
            {
                Dictionary<string, int> ColumnInfo = new Dictionary<string, int>();

                ColumnInfo.Add("softName", 200);        // 软件名称
                ColumnInfo.Add("price", 30);            // 金额 RecommendUrl
                ColumnInfo.Add("linkUrl", 300);         // 链接Url
                ColumnInfo.Add("recomondUrl", 300);     // 链接Url:recomondUrl
                ColumnInfo.Add("ext", 200);             // 拓展参数
                ColumnInfo.Add("dateTime", 20);

                DB.CreateTable(TAB, ColumnInfo);
            }
        }

        /// <summary>
        /// 添加软件信息
        /// </summary>
        /// <param name="softName">软件名称</param>
        /// <param name="price">金额</param>
        /// <param name="linkUrl">链接url</param>
        /// <param name="ext">拓展参数</param>
        /// <returns></returns>
        public static string Add(DataBase DB, string softName, string price, string linkUrl, string recomondUrl, string ext)
        {
            string id = DB.SelectValue(TAB, softName, "softName", new string[] { "ID" }.ToList()).FirstData();
            if (!id.Equals("")) return id;

            if (softName == null) return "fail";
            if (price == null) price = "10.00";
            if (linkUrl == null) linkUrl = "";
            if (recomondUrl == null) recomondUrl = "";
            if (ext == null) ext = "";

            // 添加新的数据
            List<string> values = new List<string>();

            values.Add(softName);
            values.Add(price);
            values.Add(linkUrl);
            values.Add(recomondUrl);
            values.Add(ext);
            values.Add(DateTime.Now.ToString("yyyy-MM-dd_HH:mm:ss"));

            id = DB.InsetValue(TAB, values);
            return id;
        }

        /// <summary>
        /// 更新TAB表中指定项的数据
        /// </summary>
        public static string Update(DataBase DB, string ID, string softName, string price, string linkUrl, string recomondUrl, string ext)
        {
            Dictionary<string, string> datas = new Dictionary<string, string>();
            if (softName != null) datas.Add("softName", softName);
            if (price != null) datas.Add("price", price);
            if (linkUrl != null) datas.Add("linkUrl", linkUrl);
            if (recomondUrl != null) datas.Add("recomondUrl", recomondUrl);
            if (ext != null) datas.Add("ext", ext);
            datas.Add("dateTime", ScTool.Date());   // 日期时间自动修改

            return DB.UpdateValue(TAB, ID, datas, "ID");
        }

        /// <summary>
        /// 更新TAB表中指定项的数据, 根据软件名称进行修改
        /// </summary>
        public static string Update2(DataBase DB, string softName, string price, string linkUrl, string recomondUrl, string ext)
        {
            Dictionary<string, string> datas = new Dictionary<string, string>();
            //if (softName != null) datas.Add("softName", softName);
            if (price != null) datas.Add("price", price);
            if (linkUrl != null) datas.Add("linkUrl", linkUrl);
            if (recomondUrl != null) datas.Add("recomondUrl", recomondUrl);
            if (ext != null) datas.Add("ext", ext);
            datas.Add("dateTime", ScTool.Date());   // 日期时间自动修改

            return DB.UpdateValue(TAB, softName, datas, "softName");
        }

        /// <summary>
        /// 删除指定项
        /// </summary>
        public static bool Delet(DataBase DB, string ID)
        {
            return DB.DeletValue(TAB, ID, "ID");
        }

        /// <summary>
        /// 删除指定项
        /// </summary>
        public static bool Delet2(DataBase DB, string softName, string AppendCondition = "")
        {
            return DB.DeletValue(TAB, softName, "softName");
        }

        /// <summary>
        /// 查询指定软件名称，对应的Key列数据
        /// </summary>
        /// <param name="softName"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string Select(DataBase DB, string softName, string key)
        {
            string value = DB.SelectValue(TAB, softName, "softName", new string[] { key }.ToList()).FirstData();
            return value;
        }

        /// <summary>
        /// 判断是否为软件的作者
        /// </summary>
        /// <returns></returns>
        public static bool IsAuthor(DataBase DB, string softName, string author)
        {
            bool isAuthor = false;
            //isAuthor 

            string AppendCondition = " and ext like '%" + "author(" + author + ")" + "%' ";    // 普通用户
            isAuthor = DB.SelectValue(TAB, softName, "softName", null, null, AppendCondition).RowDic().Count > 0;

            return isAuthor;
        }
    }
}