using QrPaySystem.PayFor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QrPaySystem.Tool
{
    public partial class BaseWebPage : System.Web.UI.Page
    {
        /// <summary>
        /// log工具类对象，用于记录所有当前页相关的所有log
        /// </summary>
        public LogTool log = null;

        /// <summary>
        /// 当前页面的请求参数信息
        /// </summary>
        public String param = "";


        /// <summary>
        /// 当前页面的请求url地址
        /// </summary>
        public String url = "";

        /// <summary>
        /// 获取请求参数信息
        /// </summary>
        private String getParam()
        {
            String Url = Request.Url.ToString();
            String param = "";
            if (Url.Contains("?"))
            {
                int index = Url.IndexOf("?");
                param = Url.Substring(index + 1);
                url = Url.Substring(0, index);
            }
            else url = Url;

            return param;
        }

        public String UserAccount = "";
        public int UserType = 0;

        /// <summary>
        /// 页面载入
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
        {
            UserAccount = UserTool.GetAccount(Session); // 获取登录的用户名
            UserType = UserTool.UserType(Session);      // 获取当前登录的用户类型信息

            //if((!Page.IsPostBack)) ScTool.RecordUserAgent(Request);    // 记录客户端信息

            //if (!Page.IsPostBack)  // 首次载入时
            {
                String LogName = this.GetType().Name;   // 当前页面名称
                log = new LogTool(LogName);             // 记录至log中
                param = getParam();                     // 获取参数信息

                if (!param.Equals("")) log.WriteLine(param);
            }

            Load(sender, e);
        }

        /// <summary>
        /// 子类重写的Load事件
        /// </summary>
        public virtual void Load(object sender, EventArgs e){}

    }
}