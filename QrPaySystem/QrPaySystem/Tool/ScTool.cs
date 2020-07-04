using QrPaySystem.Pages;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace QrPaySystem.Tool
{
    /// <summary>
    /// 定义通用的方法，方便调用
    /// </summary>
    public class ScTool
    {
        /// <summary>
        /// 获取当前的日期时间
        /// </summary>
        /// <returns></returns>
        public static string Date()
        {
            return DateTime.Now.ToString("yyyy-MM-dd_HH:mm:ss");
        }

        /// <summary>
        /// 获取从当前时间偏离指定的时间，对应的日期时间
        /// </summary>
        public static string Date(int year, double addDay, double addHour, double addMinute, double addSecond)
        {
            DateTime time = DateTime.Now;

            time = time.AddYears(year);
            time = time.AddDays(addDay);
            time = time.AddHours(addHour);
            time = time.AddMinutes(addMinute);
            time = time.AddSeconds(addSecond);

            return time.ToString("yyyy-MM-dd_HH:mm:ss");
        }

        public static string CASHER = "Cashier";    // 收款二维码信息表名称
        public static string ORDER = "Order";       // 订单信息表名称
        public static string UserSerial = "UserSerial";     // 存储软件相关的用户序列号信息
        public static string UserCode = "UserCode";         // 存储已支付的用户机器码软件信息

        public static string PayTypeAli = "Ali";       // 创建订单请求类型Ali、Wechat
        public static string PayTypeWechat = "Wechat"; // 创建订单请求类型Ali、Wechat


        public static string DBName(string payType)
        {
            return "QrDataBase" + "_" + payType;
        }

        public static string UserName = "sa";
        public static string Password = "Sa12345789";

        public static int CashierWaittingTime = 60;    // 扫码后等待支付完成时间/秒

        /// <summary>
        /// 为数据添加段落标签
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private static string P(string data, string label = "p")
        {
            return "<" + label + ">" + "\r\n" + data + "\r\n" + "</" + label + ">";
        }

        /// <summary>
        /// 以段落形式输出显示；
        /// Response.Write(string);输出现实
        /// </summary>
        /// <param name="data"></param>
        public static string Pre(string data)
        {
            if (!isSci) return "";

            //data = P(data, "code");
            data = P(data, "pre");

            return "\r\n" + data + "\r\n";
        }

        /// <summary>
        /// 消息弹窗；
        /// Response.Write(string);输出现实
        /// </summary>
        /// <param name="msg"></param>
        public static string Alert(string msg)
        {
            return "<script>alert('" + msg + "')</script>";
        }


        /// <summary>
        /// 消息弹窗
        /// </summary>
        public static string Reslut(string msg)
        {
            return "Result(" + msg + ")Result";
        }


        public static bool isSci = true;   

        /// <summary>
        /// 查询数据表DB.TAB中的数据，以TAB表进行显示;
        /// 根据ID选取最后1000条，逆序显示
        /// </summary>
        /// <param name="DB">数据库</param>
        /// <param name="TAB">表名称</param>
        public static void showTable(ControlCollection root, DataBase DB, string TAB, string AppendCondition = "", bool clearPre = true)
        {
            if (!isSci) return;

            if (DB.ExistTab(TAB))
            {
                //string sql = "select * from [" + TAB + "]";
                string sql = "select top 1000 * from [" + TAB + "]";
                if (AppendCondition != null && !AppendCondition.Equals("")) sql += " " + AppendCondition;
                sql = sql + " order by ID desc";

                Table table = DB.Execute(sql).Table();

                Label label = new Label();
                label.ForeColor = Color.BlueViolet;

                if(clearPre) root.Clear();  // 是否清除之前包含的数据表

                label.Text = "\r\n数据表" + TAB + "：";
                root.Add(label);

                //print("数据表" + TAB +"：");
                root.Add(table);
            }
        }


        /// <summary>
        /// 根据HTTP_USER_AGENT判断客户端类型
        /// string agent = Request.Params.Get("HTTP_USER_AGENT");
        /// </summary>
        /// <param name="HTTP_USER_AGENT"></param>
        /// <returns></returns>
        public static string payType(string HTTP_USER_AGENT)
        {
            //string agent = Request.Params.Get("HTTP_USER_AGENT");
            if (HTTP_USER_AGENT.Contains("AlipayClient") || HTTP_USER_AGENT.Contains("AliApp")) return PayTypeAli;
            else if (HTTP_USER_AGENT.Contains("MQQBrowser")) return PayTypeWechat;
            else return "";
        }

        /// <summary>
        /// 将data按首个以sp分隔两个字符串
        /// </summary>
        public static string[] splitTwo(string data, string sp)
        {
            if (!data.Equals("") && data.Contains(sp))
            {
                int index = data.IndexOf(sp);
                string first = data.Substring(0, index);
                string second = data.Substring(index + sp.Length);

                return new string[] { first, second };
            }
            else return new string[] { data, "" };
        }

        /// <summary>
        /// 解析参数字符串为Dicnary数据；
        /// 如：machinCode=机器码1&soft=easyIcon软件&product=注册0.1元
        /// </summary>
        /// <param name="StaticParam">参数字符串信息</param>
        /// <returns></returns>
        public static Dictionary<string, string> ToParamsDic(string StaticParam)
        {
            Dictionary<string, string> pramsDic = new Dictionary<string, string>();

            if (StaticParam.Contains("&"))
            {

                string[] A = StaticParam.Split('&');
                foreach (string a in A)
                {
                    if (a.Contains("="))
                    {
                        string[] B = ScTool.splitTwo(a, "=");
                        pramsDic.Add(B[0], B[1]);
                    }
                }
            }

            return pramsDic;
        }

        /// <summary>
        /// 获取指定url的网页数据
        /// </summary>
        public static String getWebData(String url)
        {
            String data = "";

            WebClient client = new WebClient();
            client.Encoding = System.Text.Encoding.UTF8;
            data = client.DownloadString(url);

            return data;
        }

        //static bool isTest = false;
        //private static void TestLogic(String jsonData)
        //{
        //    isTest = true;
        //    String Id = getJsonValue(jsonData, "ID");
        //    String QrUrl = getJsonValue(jsonData, "QrUrl");
        //    String dateTime = getJsonValue(jsonData, "dateTime");
        //    String count = getJsonValue(jsonData, "count");
        //    String HbUrl = getJsonValue(jsonData, "HbUrl");
        //    String t = "";
        //}

        /// <summary>
        /// {"ID":100,"QrUrl":"https://qr.alipay.com/tsx031041ajtuiviwd978b6","HbUrl":"https://qr.alipay.com/c1x01990gbhjvuvwaxwkqa3","Tittle":"通用红包商","ext":"","dateTime":"2018-10-04_16:18:41","count":"14"}
        /// 
        /// 解析json数据中指定的key
        /// </summary>
        /// <param name="jsonData"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static String getJsonValue(String jsonData, String key)
        {
            //if (!isTest) TestLogic(jsonData);

            String value = "";
            if (jsonData.StartsWith("{") && jsonData.EndsWith("}"))
            {
                jsonData = jsonData.Substring(1, jsonData.Length - 2);

                key = "\"" + key + "\":";   // 引号包含的键名
                if (jsonData.Contains(key))
                {
                    if (jsonData.StartsWith(key)) jsonData = jsonData.Substring(key.Length);
                    else
                    {
                        int index = jsonData.IndexOf("," + key);
                        jsonData = jsonData.Substring(index + key.Length + 1);
                    }

                    if (jsonData.StartsWith("\""))
                    {
                        int index = 1;

                        String subValue = "";
                        do
                        {
                            index = jsonData.IndexOf("\"", index);
                            if (index != -1) subValue = jsonData.Substring(1, index - 1);
                        }
                        while (subValue.EndsWith("\\"));    // 若是以转义符号结尾的则继续向后解析
                        value = subValue;
                    }
                    else
                    {
                        int index = jsonData.IndexOf(",");
                        if (index != -1) value = jsonData.Substring(0, index);
                        else value = jsonData;
                    }
                }
            }

            return value;
        }

        /// <summary>
        /// 替换jsonData中的key对应数据为newValue
        /// </summary>
        /// <param name="jsonData"></param>
        /// <param name="key"></param>
        /// <param name="newValue"></param>
        /// <returns></returns>
        public static String ReplaceJsonValue(String jsonData, String key, String newValue)
        {
            String value = ScTool.getJsonValue(jsonData, key);
            String valueStr = "\"HbUrl\":\"" + value + "\"";
            String valueStr2 = "\"HbUrl\":" + value;

            if (jsonData.Contains(valueStr))
            {
                String valueStrNew = "\"HbUrl\":\"" + newValue + "\"";
                jsonData = jsonData.Replace(valueStr, valueStrNew);
            }
            else if (jsonData.Contains(valueStr2))
            {
                String valueStrNew = "\"HbUrl\":" + newValue ;
                jsonData = jsonData.Replace(valueStr2, valueStrNew);
            }

            return jsonData;
        }

        public static string UserAgentTable = "HttpUserAgent";
        /// <summary>
        /// 记录用户Agent信息
        /// </summary>
        public static void RecordUserAgent(HttpRequest Request)
        {
            // 20190312_23.22 出于运行速度考虑，屏蔽userAgent的记录

            //try
            //{
            //    string agent = Request.Params.Get("HTTP_USER_AGENT");
            //    ThreadTool.ThreadRun(RecordUserAgent, (object)agent);
            //}
            //catch (Exception ex)
            //{
            //}
        }

        private static void RecordUserAgent(object agent0)
        {
            String agent = agent0 as String;
            UserAgent.AddIteam(agent, "");    // 记录UserAgent信息

            //String agent = agent0 as String;
            //if (agent != null && !agent.Equals(""))
            //{
            //    string agentKey = "\"信息\":\"" + agent + "\"";

            //    string md5 = MD5.Encrypt(agent);                                            // 计算md5值
            //    string md5Key = "\"标签\":\"" + md5 + "\"";

            //    string value = WebInfo.Get("HttpUserAgent", md5);                           // 查询已保存的值

            //    //[{"日期":"2018-10-30_20:00:47_407124","标签":"9d6e5576531c38953e05804e13fa8e74","信息":"Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/63.0.3239.132 Safari/537.36"},{"日期":"2018-10-30_20:01:31_604507","标签":"9d6e5576531c38953e05804e13fa8e74","信息":"Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/63.0.3239.132 Safari/537.36"}]


            //    if (!value.Contains(md5Key)) WebInfo.Set(UserAgentTable, md5, agent);              // 若不存在，则添加
            //    else if (!value.Contains(agentKey)) WebInfo.Update(UserAgentTable, md5, agent);    // 修改已保存的数据
            //}
        }

        /// <summary>
        /// 判断是否为安卓设备
        /// </summary>
        /// <param name="Request"></param>
        /// <returns></returns>
        public static bool isAndroidDevice(HttpRequest Request)
        {
            string agent = Request.Params.Get("HTTP_USER_AGENT");
            if (agent != null && agent.Contains("Android")) return true;
            else return false;
        }

        /// <summary>
        /// 判断是否为支付宝客户端
        /// </summary>
        /// <param name="Request"></param>
        /// <returns></returns>
        public static bool isAlipayClient(HttpRequest Request)
        {
            string agent = Request.Params.Get("HTTP_USER_AGENT");
            if (agent != null && agent.Contains("Android") && (agent.Contains("AliApp") || agent.Contains("AlipayClient"))) return true;
            else return false;
        }


        /// <summary>
        /// 判断是否为微信客户端
        /// </summary>
        /// <param name="Request"></param>
        /// <returns></returns>
        public static bool isWechatClient(HttpRequest Request)
        {
            string agent = Request.Params.Get("HTTP_USER_AGENT");
            if (agent != null && agent.Contains("Android") && agent.Contains("MQQBrowser") && agent.Contains("MicroMessenger")) return true;
            else return false;
        }


        //-------------------------------------------

        /// <summary>
        /// 格式化金额信息, 为小数点后n位
        /// </summary>
        public static string FormatPrice(string price, int n)
        {
            double num = 0;
            try
            {
                num = double.Parse(price);
            }
            catch (Exception ex)
            {
            }

            string fprice = num.ToString("F" + n);
            return fprice;
        }

        /// <summary>
        /// 获取节点中的数据信息，如：key(100)key -> 100
        /// </summary>
        public static string getNodeData(string data, string key)
        {
            string keyS = key + "(";
            string keyE = ")" + key;

            int S = 0;
            int E = 0;
            if (data.Contains(keyS)) S = data.IndexOf(keyS) + keyS.Length;
            if (data.Contains(keyE)) E = data.IndexOf(keyE, S);

            string sub = "";
            if (E > S) sub = data.Substring(S, E - S);
            return sub;
        }
    }
}