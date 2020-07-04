using QrPaySystem.PayFor;
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
    /// 记录已支付的MachineCode/soft信息
    /// </summary>
    public partial class OnlineCode : System.Web.UI.Page
    {
        string TYPE = "";       // 自定义操作类型
        DataBase DB = null;     // 操作的数据库
        public static string TAB = ScTool.UserCode;   // 用户序列号信息

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
            builder.AppendLine("查询已支付的用户MachineCode信息");
            builder.AppendLine("接口使用说明：");
            builder.AppendLine("");
            builder.AppendLine("查询是否存在：\t" + url + "TYPE=Check&machinCode=XRUM-LYKS-4R2P-QP6H&soft=可为空");
            builder.AppendLine("添加：\t" + url + "TYPE=Add&machinCode=XRUM-LYKS-4R2P-QP6H&soft=easyIcon&ext=拓展参数");
            builder.AppendLine("");

            Response.Write(ScTool.Pre(builder.ToString()));
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
                if (TYPE.Equals("Check"))
                {
                    reslut = Check(DB, TAB, Request["machinCode"], Request["soft"]);
                    Response.Write(reslut);
                    return;
                }
                if (TYPE.Equals("Add"))
                {
                    int UserType = UserTool.UserType(Session);      // 获取当前登录的用户类型信息

                    reslut = "false no allowed! " + "../PayFor/UserLogin.aspx";
                    if (UserType == 2) reslut = Add(DB, TAB, Request["machinCode"], Request["soft"], Request["ext"], Request["msg"]);
                    
                    Response.Write(reslut);
                    return;
                }
                Response.Write("TYPE -> " + TYPE);
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
                ColumnInfo.Add("soft", 50);
                ColumnInfo.Add("dateTime", 20);     // 最后修改时间

                ColumnInfo.Add("ext", 200);
                ColumnInfo.Add("msg", 300);         // 针对特定客户端的预留消息

                DB.CreateTable(TAB, ColumnInfo);
            }
        }

        /// <summary>
        /// 查询machineCode/soft对应的数据项是否存在
        /// </summary>
        /// <param name="DB"></param>
        /// <param name="TAB"></param>
        /// <param name="machinCode"></param>
        /// <param name="soft"></param>
        /// <returns></returns>
        public static string Check(DataBase DB, string TAB, string machinCode, string soft)
        {
            string result = "fail";

            Dictionary<string, string> ortherConditions = new Dictionary<string, string>();
            if (soft != null && !soft.Equals("")) ortherConditions.Add("soft", soft);
            Dictionary<string, string> Dic = DB.SelectValue(TAB, machinCode, "machinCode", null, ortherConditions, "").RowDic();
            if (Dic.Count > 0) result = "success";

            return result;
        }

        /// <summary>
        /// 添加新的数据项
        /// </summary>
        public static string Add(DataBase DB, string TAB, string machinCode, string soft = null, string ext = null, string msg = null)
        {
            if (machinCode == null || machinCode.Trim().Equals("")) return "fail" + " -> machinCode不可为空";
            if (soft == null) soft = "";
            if (ext == null) ext = "";
            if (msg == null) msg = "";

            string result = Check(DB, TAB, machinCode, soft);
            if (result.Equals("fail"))
            {
                // 添加新的数据
                List<string> values = new List<string>();

                values.Add(machinCode);
                values.Add(soft);
                values.Add(ScTool.Date());
                values.Add(ext);
                values.Add(msg);
                string id = DB.InsetValue(TAB, values);

                return id;
            }
            else return result;
        }
    }

}