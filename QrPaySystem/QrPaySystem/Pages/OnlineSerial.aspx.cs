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
    /// 记录和查询用户序列号信息
    /// </summary>
    public partial class OnlineSerial : System.Web.UI.Page
    {
        string TYPE = "";       // 自定义操作类型
        DataBase DB = null;     // 操作的数据库
        public static string TAB = ScTool.UserSerial;   // 用户序列号信息

        /// <summary>
        /// 获取请求参数信息
        /// </summary>
        private String getParam(String LogName = "")
        {
            TYPE = Request["TYPE"];


            String Url = Request.Url.ToString();
            String param = "";
            if (Url.Contains("?"))
            {
                param = Url.Substring(Url.IndexOf("?") + 1);                // 获取参数信息

                if (LogName.Equals("")) LogName = this.GetType().Name;
                LogTool log = new LogTool(LogName);                         // 记录至log中
                log.WriteLine(param);
            }
            return param;
        }

        /// <summary>
        /// 接口使用说明信息
        /// </summary>
        private void NoteInfo()
        {
            String url = "http://" + Request.Params.Get("HTTP_HOST") + "/" + this.GetType().Name.Replace("_", "/").Replace("/aspx", ".aspx") + "?";
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("记录和查询用户序列号信息");
            builder.AppendLine("接口使用说明：");
            builder.AppendLine("");
            builder.AppendLine("查询已注册序列号：\t" + url + "TYPE=GetRegSerial&machinCode=XRUM-LYKS-4R2P-QP6H&soft=easyIcon&computerName=计算机名称&userName=用户名称&ext=拓展参数&counter=true");
            builder.AppendLine("修改表中的数据：\t" + url + "TYPE=UpdateSerial&ID=10001&machinCode=XRUM-LYKS-4R2P-QP6H&soft=easyIcon&computerName=计算机名称&userName=用户名称&ext=拓展参数&startTimes=1&onlineSerial=注册码信息&msg=消息");
            builder.AppendLine("查询指定列数据：\t" + url + "TYPE=GetValue&KEY=startTimes&machinCode=P7WK-R306-A91B-059E&soft=easyIcon");
            builder.AppendLine("");

            Response.Write(ScTool.Pre(builder.ToString()));

            //print("接口使用说明：");
            //print("");
            //print("添加、修改qrTab：\t" + url + "TYPE=SetQrTable&ID=100&TabName=qrTab100");
            //print("向qrTab添加数据项：\t" + url + "TYPE=AddToQrTable&TabName=qrTab100&price=0.1&qrLink=http://www.baidu.com");
            //print("在qrTab中修改数据：\t" + url + "TYPE=UpdateQrTable&TabName=qrTab100&ID=100&price=0.1&qrLink=http://www.w3school.com.cn&orderId=100&isUsing=True");
            //print("删除表中指定值所有行：\t" + url + "TYPE=DeletInTable&TabName=qrTab100&KeyValue=104&KeyName=ID");

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack) ScTool.RecordUserAgent(Request);    // 记录客户端信息

            getParam();     //获取参数记录log信息

            DB = new DataBase(ScTool.DBName("pre"), ScTool.UserName, ScTool.Password);
            //DB.DeletTable(TAB);
            CreatTable(DB, TAB);

            if (TYPE != null)
            {
                string reslut = "";
                if (TYPE.Equals("GetRegSerial"))
                {
                    reslut = GetRegSerial(Request["machinCode"], Request["soft"], Request["computerName"], Request["userName"], Request["ext"], Request["counter"]);
                    Response.Write(reslut);
                    return;
                }
                if (TYPE.Equals("GetValue"))
                {
                    reslut = GetValue(Request["KEY"], Request["machinCode"], Request["soft"]);
                    Response.Write(reslut);
                    return;
                }
                else if (TYPE.Equals("UpdateSerial"))
                {
                    reslut = Update(DB, Request["ID"], Request["machinCode"], Request["soft"], Request["computerName"], Request["userName"], Request["ext"], Request["startTimes"], Request["onlineSerial"], Request["msg"]);
                }

                Response.Write("UpdateSerial -> " + reslut);
            }

            NoteInfo();
            ScTool.showTable(this.Controls, DB, TAB);
        }


        /// <summary>
        /// 创建序列号信息表
        /// </summary>
        private static void CreatTable(DataBase DB, string TAB)
        {
            // 若映射表不存在，则创建
            if (!DB.ExistTab(TAB))
            {
                Dictionary<string, int> ColumnInfo = new Dictionary<string, int>();

                ColumnInfo.Add("machinCode", 50);
                ColumnInfo.Add("soft", 200);
                ColumnInfo.Add("computerName", 200);
                ColumnInfo.Add("userName", 200);
                ColumnInfo.Add("ext", 200);

                ColumnInfo.Add("dateTime", 20);     // 最后修改时间
                ColumnInfo.Add("startTimes", 20);   // 软件启动次数
                ColumnInfo.Add("onlineSerial", 50);    // 序列号信息
                ColumnInfo.Add("msg", 300);         // 针对特定客户端的预留消息

                DB.CreateTable(TAB, ColumnInfo);
            }
        }

        /// <summary>
        /// 查询当前机器码和软件名称，对应的注册码信息
        /// </summary>
        /// <param name="machinCode">机器码</param>
        /// <param name="soft">软件名称</param>
        /// <param name="computerName">计算机名称</param>
        /// <param name="userName">用户名称</param>
        /// <returns>返回注册码信息</returns>
        private string GetRegSerial(string machinCode, string soft, string computerName = "", string userName = "", string ext = "", string counter = "false")
        {
            if (machinCode.Trim().Equals("")) return "";

            // 查询机器码、软件名称对应的,注册信息
            Dictionary<string, string> condition = new Dictionary<string, string>();
            condition.Add("soft", soft);

            Dictionary<string, string> Dic = DB.SelectValue(TAB, machinCode, "machinCode", null, condition).RowDic();
            if (Dic.Keys.Count > 0 && Dic.ContainsKey("ID"))
            {   
                // 若存在，则修改相应信息，并返回注册码信息
                string ID = Dic["ID"];

                string startTimes = Dic["startTimes"];
                if (counter != null && counter.Equals("true"))
                {
                    startTimes = (long.Parse(startTimes) + 1).ToString();   // 启动次数递增
                }

                Update(DB, ID, null, null, computerName, userName, ext, startTimes, null, null);    // 更新用户相关信息

                string onlineSerial = Dic["onlineSerial"];
                return onlineSerial;
            }
            else
            {   
                // 若不存在则记录当前信息
                createSerialIteam(machinCode, soft, computerName, userName, ext);
                return "";
            }
        }

        /// <summary>
        /// 查询指定列数据，查询条件:当前机器码和软件名称
        /// </summary>
        /// <param name="KEY">列名称</param>
        /// <param name="machinCode">机器码</param>
        /// <param name="soft">软件名称</param>
        private string GetValue(string KEY, string machinCode, string soft)
        {
            if (machinCode == null || soft == null || machinCode.Trim().Equals("") || soft.Trim().Equals("")) 
                return "";

            // 查询机器码、软件名称对应的,注册信息
            Dictionary<string, string> condition = new Dictionary<string, string>();
            condition.Add("soft", soft);

            string result = "";
            Dictionary<string, string> Dic = DB.SelectValue(TAB, machinCode, "machinCode", null, condition).RowDic();
            if (Dic.Keys.Count > 0 && Dic.ContainsKey(KEY))
            {
                string Value = Dic[KEY];

                result = KEY + "(" + Value + ")" + KEY;
                result = Locker.Encrypt(result, machinCode + soft); // 使用机器码和软件名称，对数据信息进行加密
            }
            return result;

        }

        /// <summary>
        /// 创建，在数据库中记录当前序列号信息
        /// </summary>
        /// <param name="machinCode"></param>
        /// <param name="soft"></param>
        /// <param name="product"></param>
        /// <param name="money"></param>
        /// <param name="ext"></param>
        /// <returns></returns>
        private string createSerialIteam(string machinCode, string soft, string computerName = "", string userName = "", string ext = "")
        {
            // 添加新的数据
            List<string> values = new List<string>();

            values.Add(machinCode);
            values.Add(soft);
            values.Add(computerName);
            values.Add(userName);
            values.Add(ext);

            values.Add(DateTime.Now.ToString("yyyy-MM-dd_HH:mm:ss"));
            values.Add("1");
            values.Add("");
            values.Add("");

            string id = DB.InsetValue(TAB, values);
            return id;
        }

        /// <summary>
        /// 修改指定ID对应的数据信息
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="machinCode"></param>
        /// <param name="soft"></param>
        /// <param name="computerName"></param>
        /// <param name="userName"></param>
        /// <param name="ext"></param>
        /// <param name="startTimes"></param>
        /// <param name="onlineSerial"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        private static string Update(DataBase DB, string ID, string machinCode, string soft, string computerName = "", string userName = "", string ext = "", string startTimes = "", string onlineSerial = "", string msg = "")
        {
            Dictionary<string, string> datas = new Dictionary<string, string>();

            if (machinCode != null && !machinCode.Equals("")) datas.Add("machinCode", machinCode);
            if (soft != null && !soft.Equals("")) datas.Add("soft", soft);
            if (computerName != null && !computerName.Equals("")) datas.Add("computerName", computerName);
            if (userName != null && !userName.Equals("")) datas.Add("userName", userName);
            if (ext != null && !ext.Equals("")) datas.Add("ext", ext);

            datas.Add("dateTime", ScTool.Date());   // 日期时间自动修改
            if (startTimes != null && !startTimes.Equals("")) datas.Add("startTimes", startTimes);
            if (onlineSerial != null && !onlineSerial.Equals("")) datas.Add("onlineSerial", onlineSerial);
            if (msg != null && !msg.Equals("")) datas.Add("msg", msg);

            return DB.UpdateValue(TAB, ID, datas, "ID");
        }

        /// <summary>
        /// 生成注册码信息
        /// </summary>
        /// <param name="machinCode">机器码</param>
        /// <param name="soft">软件名称</param>
        public static string GenOnlineSerial(string machinCode, string soft, DataBase DB = null)
        {
            if (machinCode == null || machinCode.Trim().Equals("")) return "fail";
            if (soft == null) return "fail";

            if(DB == null) DB = new DataBase(ScTool.DBName("pre"), ScTool.UserName, ScTool.Password);
            CreatTable(DB, TAB);

            // 查询机器码、软件名称对应的,注册信息
            Dictionary<string, string> condition = new Dictionary<string, string>();
            condition.Add("soft", soft);

            Dictionary<string, string> Dic = DB.SelectValue(TAB, machinCode, "machinCode", null, condition).RowDic();
            if (Dic.Keys.Count > 0 && Dic.ContainsKey("ID"))
            {
                // 若存在，则修改相应信息，并返回注册码信息
                string ID = Dic["ID"];

                string softSerial = SoftSerial(machinCode, soft);
                string onlineSerial = RegSerial(softSerial);

                return Update(DB, ID, null, null, null, null, null, null, onlineSerial, null);
            }

            return "fail";
        }



        #region 自定义机器码、注册码生成算法

        /// <summary>
        /// 根据硬件机器码，生成软件码
        /// </summary>
        public static string SoftSerial(string machineSerial,  string SoftName)
        {
            if (machineSerial == null || machineSerial.Equals("")) return "";

            string Key = format("SCIMENCE" + SoftName);     // "SCIM-ENCE-EASY-ICON"
            string serial = lockLogic(machineSerial, Key);  // 软件附加码 // "XRUM-LYKS-4R2P-QP6H"

            return serial;
        }

        /// <summary>
        /// 根据软件码生成注册码
        /// </summary>
        public static string RegSerial(string softSerial)
        {
            if (softSerial == null || softSerial.Equals("")) return "";
            string regSerial = lockLogic(softSerial, "SCIM-ENCE-REGE-DITS");
            return regSerial;
        }

        /// <summary>
        /// 获取Data尾部len长度的串
        /// </summary>
        private static string getTails(string Data, int len)
        {
            return Data.Substring(Data.Length - len, len);
        }

        /// <summary>
        /// 对字符串Data用字符串Key进行加密
        /// </summary>
        private static string lockLogic(string Data, string Key)
        {
            Data = Data.Replace("-", "").ToUpper();
            Key = Key.Replace("-", "").ToUpper();

            char[] data = Data.ToCharArray();
            char[] key = Key.ToCharArray();

            string tmp = "";
            for (int i = 0; i < data.Length; i++)
            {
                tmp += AlpahLock(data[i], key[i % key.Length]);
            }
            return format(tmp);
        }

        /// <summary>
        /// 对字符C用K加密，转化为A-Z、0-9对应的字符
        /// </summary>
        private static char AlpahLock(char C, char K)
        {
            int n = (C + K) % 35;
            if (n < 26) return (char)(n + 'A'); // 0-25映射到 A-Z
            else return (char)(n - 26 + '1');   // 26-34映射到 1-9
        }

        /// <summary>
        /// 规整化字符串"BFEBFBFF000306A9"为"BFEB-FBFF-0003-06A9"
        /// </summary>
        public static string format(string Data)
        {
            Data = Data.Replace("-", "").ToUpper();
            char[] data = Data.ToCharArray();

            string tmp = "";
            for (int i = 0; i < data.Length; i++)
            {
                tmp += data[i];
                tmp += (((i + 1) % 4 == 0 && i < data.Length - 1) ? "-" : "");
            }
            return tmp;
        }

        #endregion

    }
}