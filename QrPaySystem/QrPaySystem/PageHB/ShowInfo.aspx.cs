using QrPaySystem.Tool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace QrPaySystem.PageHB
{
    public partial class ShowInfo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack) ScTool.RecordUserAgent(Request);    // 记录客户端信息

            string info = Request["p"];
            if (info != null && !info.Equals(""))
            {
                LabelInfo.Text = info;
            }
            else
            {
                String url = "http://" + Request.Params.Get("HTTP_HOST") + "/" + this.GetType().Name.Replace("_", "/").Replace("/aspx", ".aspx") + "?";
                info = url + "p=待显示信息（自定义）";
                LabelInfo.Text = info;
            }
        }
    }
}