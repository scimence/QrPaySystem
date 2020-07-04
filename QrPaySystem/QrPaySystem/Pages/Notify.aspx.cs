using QrPaySystem.Tool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace QrPaySystem.Pages
{
    /// <summary>
    /// 通知信息解析：
    /// com.sc.notificationservices#681190477515911#1533107942652#标题#内容
    /// </summary>
    public class NotifyData
    {
        public string package = "";
        public string phoneId = "";
        public string msgId = "";
        public string tittle = "";
        public string content = "";

        /// <summary>
        /// 解析通知信息内容
        /// </summary>
        public NotifyData(string data)
        {
            int i = 0;
            while (!data.Equals(""))
            {
                string[] A = ScTool.splitTwo(data, "#");

                data = A[1];

                i++;
                if (i == 1) package = A[0];
                else if (i == 2) phoneId = A[0];
                else if (i == 3) msgId = A[0];
                else if (i == 4) tittle = A[0];
                else if (i == 5) content = A[0];
            }
        }

    }

    /// <summary>
    /// 负责对通知信息进行过滤，并记录至对应的数据表中
    /// </summary>
    public partial class Notify : System.Web.UI.Page
    {
        String TAB = "Notify表";
        String infoURL = "http://118.25.40.47/WebInfo.aspx";

        DataBase DB = null;
        string CASHIER_TAB = ScTool.CASHER;

        /// <summary>
        /// 获取请求参数信息
        /// </summary>
        private String getParam(String LogName = "")
        {
            String Url = Request.Url.ToString();
            String param = "";
            Url = System.Web.HttpUtility.UrlDecode(Url);                    // 解码参数
            if (Url.Contains("?"))
            {
                param = Url.Substring(Url.IndexOf("?") + 1);                // 获取参数信息

                if (LogName.Equals("")) LogName = this.GetType().Name;
                LogTool log = new LogTool(LogName);                         // 记录至log中
                if (param.Contains("收款") || param.Contains("到账"))
                {
                    log.WriteLine(param);
                }
            }
            return param;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack) ScTool.RecordUserAgent(Request);    // 记录客户端信息

            infoURL = "http://" + Request.Params.Get("HTTP_HOST") + "/WebInfo.aspx";    // 本地表信息路径

            String param = getParam();                                  // 获取通知消息
            //param = "param=com.sc.notificationservices#866170022772077#1518361220871#标题#内容";
            //param=com.sc.notificationservices#681190477515911#1533107942652#标题#内容
            //param = "com.eg.android.AlipayGphone#866170022772077#1533477407672#支付宝通知#成功收款0.1元。享免费提现等更多专属服务，点击查看";
            if (param.StartsWith("param=")) param = param.Substring("param=".Length);

            NotifyData data = new NotifyData(param);

            string result = "fail";
            if (data.package.Equals("com.sc.notificationservices"))
            {
                DB = new DataBase(ScTool.DBName(ScTool.PayTypeAli), ScTool.UserName, ScTool.Password);
                result = recordInCahsier(DB, data.phoneId);

                if (result.Equals("success"))
                {
                    DB = new DataBase(ScTool.DBName(ScTool.PayTypeWechat), ScTool.UserName, ScTool.Password);
                    result = recordInCahsier(DB, data.phoneId);
                }
            }
            else if (data.package.Equals("com.eg.android.AlipayGphone"))
            {
                result = param;

                if (data.content.Contains("成功收款") && data.content.Contains("元"))
                {
                    int index1 = data.content.IndexOf("成功收款") + "成功收款".Length;
                    int index2 = data.content.IndexOf("元", index1);
                    string money = data.content.Substring(index1, index2 - index1).Trim();

                    DB = new DataBase(ScTool.DBName(ScTool.PayTypeAli), ScTool.UserName, ScTool.Password);
                    string orderId = Cashier.PriceFinish(DB, data.phoneId, money);
                    if(!orderId.Equals("")) result = Order.OrderSuccess(DB, orderId);    // 设置对应订单为支付成功
                    else result = "success";
                }
                else result = "success";
            }
            else if (data.package.Equals("com.tencent.mm"))
            {
                int index1 = data.content.IndexOf("收款") + "收款".Length;
                if(index1 <2 && data.content.Contains("到账")) index1 = data.content.IndexOf("到账") + "到账".Length;
                int index2 = data.content.IndexOf("元", index1);
                string money = data.content.Substring(index1, index2 - index1).Trim();

                DB = new DataBase(ScTool.DBName(ScTool.PayTypeWechat), ScTool.UserName, ScTool.Password);
                string orderId = Cashier.PriceFinish(DB, data.phoneId, money);
                if (!orderId.Equals("")) result = Order.OrderSuccess(DB, orderId);    // 设置对应订单为支付成功
                else result = "success";
            }
            else
            {
                result = "【package: " + data.package + "】\r\n" + param + "";
            }
            //else if (data.package.Equals("com.sc.notificationservices"))
            //{
            //    DB = new DataBase(ScTool.DBName(ScTool.PayTypeAli), ScTool.UserName, ScTool.Password);

            //}

            Response.Write(result);

            //if (!param.Equals("") && param.Contains("#"))
            //{
            //    TAB = getTableName(param);              // 生成表名
            //    String key = getPhoneSerial(param);     // 获取手机序列号
            //    String result = SaveInfo(key, param);   // 通知信息保存结果

            //    Response.Write(result);
            //}
            //else
            //{
            //    Response.Write("fail");
            //}
        }

        /// <summary>
        /// 在Cashier表中记录当前phoneId
        /// 若已存在，则修改其isOnline状态
        /// </summary>
        private string recordInCahsier(DataBase DB, string phoneId)
        {
            // 判断表是否存在，若不存在，则创建
            bool exist = DB.ExistTab(CASHIER_TAB);
            if (!exist)
            {
                Dictionary<string, int> ColumnInfo = new Dictionary<string, int>();

                ColumnInfo.Add("phoneId", 50);
                ColumnInfo.Add("qrTabName", 100);
                ColumnInfo.Add("isOnline", 10);
                ColumnInfo.Add("extend", 300);
                ColumnInfo.Add("dateTime", 20);

                exist = DB.CreateTable(CASHIER_TAB, ColumnInfo);
            }

            string result = "fail";
            if (exist)
            {
                // 判断当前phoneId是否已存在。不存在则添加，已存在则修改状态
                List<string> colums = new string[] { "isOnline" }.ToList();
                List<string> phoneInfo = DB.SelectValue(CASHIER_TAB, phoneId, "phoneId", colums).ColmnList();

                if (phoneInfo.Count == 0)
                {
                    string dateTime = DateTime.Now.ToString("yyyy-MM-dd_HH:mm:ss");
                    List<string> data = (new string[] { phoneId, "", "true", "", dateTime }).ToList();
                    result = DB.InsetValue(CASHIER_TAB, data);
                    if (!result.Equals("")) result = "success";
                }
                else
                {
                    string isOnline = phoneInfo[0];

                    Dictionary<string, string> datas = new Dictionary<string, string>();
                    datas.Add("isOnline", (!isOnline.ToLower().Equals("true")).ToString());   // 修改isOnline状态

                    result = DB.UpdateValue(CASHIER_TAB, phoneId, datas, "phoneId");
                }
            }
            return result;
        }


        ///// <summary>
        ///// 获取手机序列号信息，识别手机
        ///// </summary>
        //private String getPhoneSerial(String param)
        //{
        //    int start = param.IndexOf("#") + 1;
        //    int end = param.IndexOf("#", start);
        //    String Serial = param.Substring(start, end - start);
        //    return Serial;
        //}

        ///// <summary>
        ///// 获取通知消息的应用包名信息，生成对应的表
        ///// </summary>
        //private String getTableName(String param)
        //{
        //    int start = param.IndexOf("#");
        //    String Name = param.Substring(0, start).Replace(".", "_");
        //    return Name;
        //}

        ///// <summary>
        ///// 保存信息
        ///// </summary>
        //public String SaveInfo(String key, String vlaue)
        //{
        //    vlaue = System.Web.HttpUtility.UrlEncode(vlaue);    //  数据信息中若含有特定字符，需要转码
        //    key = System.Web.HttpUtility.UrlEncode(key);

        //    String url = infoURL + "?" + "TAB=" + TAB + "&" + "KEY=" + key + "&" + "VALUE=" + vlaue;
        //    String data = getWebData(url);
        //    return data;
        //}

        ///// <summary>
        ///// 获取指定url的网页数据
        ///// </summary>
        //public static String getWebData(String url)
        //{
        //    //url = System.Web.HttpUtility.UrlEncode(url);
        //    String data = "";

        //    WebClient client = new WebClient();
        //    client.Encoding = System.Text.Encoding.UTF8;
        //    data = client.DownloadString(url);

        //    return data;
        //}
    }
}