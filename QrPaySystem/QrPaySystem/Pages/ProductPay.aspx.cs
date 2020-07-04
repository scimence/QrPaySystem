using QrPaySystem.Tool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace QrPaySystem.Pages
{
    public partial class ProductPay : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string tittle = Request["tittle"];
            string price = Request["price"];
            string link = Request["link"];

            if (tittle == null) tittle = "（自定义名称）";
            if (price == null) price = "0.01";
            if (link == null) link = "";

            if (link.Equals(""))
            {
                string msg = "参数link不应为空！";
                msg += "\r\n示例：" + Request.Url + "?" + "tittle=" + "测试资源xxx" + "&price=" + "0.02" + "&link=" + "http://ww.baidu.com";

                Response.Write(ScTool.Alert(msg));
                return;
            }

            LabelTittle.Text = tittle;
            LabelPrice.Text = "待支付金额：" + price + "元";

            BtnDiv.InnerHtml = "<a href=\"" + link + "\"><img src=\"../tools/HB_pic/btn_pay.png\"></a>";

        }
    }
}