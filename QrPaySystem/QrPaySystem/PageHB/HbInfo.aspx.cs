using QrPaySystem.Tool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace QrPaySystem.PageHB
{
    public partial class HbInfo : BaseWebPage
    {
        public string PayType = "HongBao";  // 创建订单请求类型Ali、Wechat

        public DataBase DB = null;          // 本地数据库连接对象
        string TAB = "HbInfo";              // 表名称（红包码、收款码信息）

        private static String HbUrlDefault = ""; // 默认红包码信息

        string TYPE = "";   // 自定义操作类型

        /// <summary>
        /// 载入后执行参数对应的sql请求
        /// </summary>
        public override void Load(object sender, EventArgs e)
        {
            TYPE = Request["TYPE"];

            // 连接指定的数据库，若不存在则创建
            DB = new DataBase(ScTool.DBName(PayType), ScTool.UserName, ScTool.Password);

            //DB.DeletTable(ScTool.ORDER);
            confirmOrderTab();

            if (TYPE != null)
            {
                string result = "";
                if (TYPE.Equals("Add")) result = AddIteam(Request["QrUrl"], Request["HbUrl"], Request["Tittle"], Request["ext"]);
                else if (TYPE.Equals("_Delet")) result = DeletIteam(Request["ID"]).ToString();
                else if (TYPE.Equals("_Update"))
                {
                    long count = -1;
                    try
                    {
                        count = long.Parse(Request["count"]);
                    }
                    catch (Exception) { }

                    result = UpdateIteam(Request["ID"], Request["QrUrl"], Request["HbUrl"], Request["Tittle"], Request["ext"], count);
                }
                else if (TYPE.Equals("Get")) result = GetIteam(Request["ID"], Request["KeyName"]);
                else if (TYPE.Equals("CountAdd")) result = CountAdd(Request["ID"]);

                Response.Write(result);
            }
            else
            {
                NoteInfo();
                ScTool.showTable(this.Controls, DB, TAB);
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
            builder.AppendLine("添加：\t" + url + "TYPE=Add&QrUrl=https://www.baidu.com&HbUrl=https://fanyi.baidu.com&Tittle=第8号当铺&ext=");
            builder.AppendLine("删除：\t" + url + "TYPE=Delet&ID=1005");
            builder.AppendLine("修改：\t" + url + "TYPE=Update&ID=1005&QrUrl=https://www.baidu.com2&HbUrl=https://fanyi.baidu.com2&Tittle=第9号当铺&ext=&count=1");
            builder.AppendLine("查询：\t" + url + "TYPE=Get&ID=1005&KeyName=QrUrl");
            builder.AppendLine("(可查询表中的任意列)");
            builder.AppendLine("计数：\t" + url + "TYPE=CountAdd&ID=1005");
            builder.AppendLine("链接：\t" + "http://" + Request.Params.Get("HTTP_HOST") + "/" + "tools/QRTool/QR_HB/100.png");
            builder.AppendLine("");

            Response.Write(ScTool.Pre(builder.ToString()));
        }

        /// <summary>
        /// 确保数据库中，存在订单信息表，若不存在则创建
        /// </summary>
        private void confirmOrderTab()
        {
            // 创建数据表
            if (!DB.ExistTab(TAB))
            {
                Dictionary<string, int> ColumnInfo = new Dictionary<string, int>();
                ColumnInfo.Add("QrUrl", 200);
                ColumnInfo.Add("HbUrl", 200);
                ColumnInfo.Add("Tittle", 50);
                ColumnInfo.Add("ext", 200);
                ColumnInfo.Add("dateTime", 20);
                ColumnInfo.Add("count", 20);

                DB.CreateTable(TAB, ColumnInfo);
            }
        }



        /// <summary>
        /// 创建新的红包收款码信息，在数据库中记录信息
        /// </summary>
        /// <param name="QrUrl">收款二维码</param>
        /// <param name="HbUrl">红包二维码</param>
        /// <param name="Tittle">商户名称</param>
        /// <param name="ext">拓展参数</param>
        /// <returns></returns>
        private string AddIteam(string QrUrl, string HbUrl, string Tittle, string ext)
        {
            // 查询已存在的数据信息对应Id,若无则添加新的
            Dictionary<string, string> conditions = new Dictionary<string, string>();
            conditions.Add("HbUrl", HbUrl);
            conditions.Add("Tittle", Tittle);
            Dictionary<string, string> Iteam = DB.SelectValue(TAB, QrUrl, "QrUrl", null, conditions).RowDic();  // 查询ID指定的行信息
            if (Iteam.ContainsKey("ID")) return Iteam["ID"];
            
            // 添加新的数据
            List<string> values = new List<string>();

            values.Add(QrUrl);
            values.Add(HbUrl);
            values.Add(Tittle);
            values.Add(ext);
            values.Add(DateTime.Now.ToString("yyyy-MM-dd_HH:mm:ss"));
            values.Add("0");

            string id = DB.InsetValue(TAB, values);
            return id;
        }



        /// <summary>
        /// 删除指定Id对应项
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        private bool DeletIteam(string Id)
        {
            return DB.DeletValue(TAB, Id, "ID");
        }

        /// <summary>
        /// 更新TAB表中指定项的数据
        /// </summary>
        /// <param name="TAB"></param>
        /// <param name="ID"></param>
        /// <param name="QrUrl"></param>
        /// <param name="HbUrl"></param>
        /// <param name="Tittle"></param>
        /// <param name="ext"></param>
        private string UpdateIteam(string ID, string QrUrl, string HbUrl, string Tittle, string ext, long count)
        {
            Dictionary<string, string> datas = new Dictionary<string, string>();
            if (QrUrl != null) datas.Add("QrUrl", QrUrl);
            if (HbUrl != null) datas.Add("HbUrl", HbUrl);
            if (Tittle != null) datas.Add("Tittle", Tittle);
            if (ext != null) datas.Add("ext", ext);
            if (count >= 0) datas.Add("count", count.ToString());
            datas.Add("dateTime", ScTool.Date());   // 日期时间自动修改

            return DB.UpdateValue(TAB, ID, datas, "ID");
        }

        /// <summary>
        /// 获取指定Id项的，KeyName列对应数据
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        private string GetIteam(string ID)
        {
            String row = DB.SelectValue(TAB, ID, "ID").ToString();  // 查询ID指定的行信息

            long count = long.Parse(ScTool.getJsonValue(row, "count"));
            if (count > 20 && count % 3 == 0)
            {
                //String HbUrl = ScTool.getJsonValue(row, "HbUrl");
                //String hbValue = "\"HbUrl\":\"" + HbUrl + "\"";

                //string defaultHB = GetIteam("100", "HbUrl");        // 默认红包码
                //String HbUrlNew = "\"HbUrl\":\"" + defaultHB + "\"";

                string defaultHB = GetIteam("100", "HbUrl");        // 默认红包码
                row = ScTool.ReplaceJsonValue(row, "HbUrl", defaultHB);
            }

            return row;
        }

        /// <summary>
        /// 获取指定Id项的，KeyName列对应数据
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="KeyName"></param>
        /// <returns></returns>
        private string GetIteam(string ID, string KeyName)
        {
            if(KeyName == null || KeyName.Equals("")) return GetIteam(ID);

            Dictionary<string, string> Iteam = DB.SelectValue(TAB, ID, "ID").RowDic();  // 查询ID指定的行信息
            
            if (KeyName.Equals("HbUrl"))    // 获取红包码时，自动更新cout统计值
            {
                long count = long.Parse(Iteam["count"]);
                //UpdateIteam(ID, null, null, null, null, count + 1);    // 更新红包码获取数信息

                if (!ID.Equals("100"))
                {
                    if (count > 20 && count % 3 == 0)
                    {
                        string defaultHB = GetIteam("100", KeyName); // 默认红包码
                        return defaultHB;
                    }
                    else if (!Iteam.ContainsKey(KeyName) || Iteam[KeyName].Trim().Equals(""))   // 若未设置红包码，则使用默认红包码
                    {
                        string defaultHB = GetIteam("100", KeyName); // 默认红包码
                        return defaultHB;
                    }
                }
            }

            if (Iteam.ContainsKey(KeyName)) return Iteam[KeyName];
            else return "";
        }

        /// <summary>
        /// 计数值加1
        /// </summary>
        /// <param name="ID"></param>
        private string CountAdd(string ID)
        {
            if (ID != null)
            {
                Dictionary<string, string> Iteam = DB.SelectValue(TAB, ID, "ID").RowDic();  // 查询ID指定的行信息
                long count = long.Parse(Iteam["count"]);
                return UpdateIteam(ID, null, null, null, null, count + 1);                  // 更新红包码计数值信息
            } 
            return "fail";
        }

    }
}