using QrPaySystem.Tool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace QrPaySystem.Pages
{
    public partial class Jump : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack) ScTool.RecordUserAgent(Request);    // 记录客户端信息

            string key = "HB";
            string date = DateTime.Now.ToString("yyyyMMdd");    // 每天可领取一次，首次跳转

            string value = "";
            if (Session[key] == null || !(Session[key] as string).StartsWith(date))  // 若key不存在，或非今天的则生成新的值
            {
                value = date;
                Session[key] = date;
             }
            else 
            {
                value = Session[key] as string;                             // 获取之前的session值
                if (!value.Contains("noFrist")) value = value + "noFrist";  // 记录为非首次访问
                Session[key] = value;
            }
            //Label1.Text = value;

            if (value.Contains("noFrist"))  // 非首次访问，直接跳转收款
            {
                //NewLinkDiv.InnerHtml = "<a href =\"https://qr.alipay.com/tsx031041ajtuiviwd978b6\" >支付宝收款</a>";
                Response.Redirect("https://qr.alipay.com/tsx031041ajtuiviwd978b6");
            }
            else
            {                               // 首次访问，跳转领取红包
                NewLinkDiv.InnerHtml = "<a href =\"https://qr.alipay.com/c1x01990gbhjvuvwaxwkqa3\" >支付宝领红包</a>";
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            
        }

        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            Response.Redirect("https://qr.alipay.com/c1x01990gbhjvuvwaxwkqa3");
        }

        protected void LinkButton2_Click(object sender, EventArgs e)
        {
            Response.Redirect("https://qr.alipay.com/tsx031041ajtuiviwd978b6");
        }
    }
}