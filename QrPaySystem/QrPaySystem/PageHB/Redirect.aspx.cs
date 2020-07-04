using QrPaySystem.Tool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace QrPaySystem.PageHB
{
    /// <summary>
    /// 网页重定向
    /// </summary>
    public partial class Redirect : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack) ScTool.RecordUserAgent(Request);    // 记录客户端信息

            string p = Request["p"];
            //if (p != null && !p.Equals("")) Server.Transfer(p);
            if (p != null && !p.Equals("")) Response.Redirect(p);

        }
    }
}