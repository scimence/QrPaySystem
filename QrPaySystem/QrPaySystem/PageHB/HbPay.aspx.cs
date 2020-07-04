using QrPaySystem.Tool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace QrPaySystem.PageHB
{
    public partial class HbPay : System.Web.UI.Page
    {
        string ServerUrl = "";  // 当前服务器的Url地址信息
        string QrUrl = "";      // 收款Url
        string HbUrl = "";      // 红包Url
        string Tittle = "";     // 商户名称

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack) ScTool.RecordUserAgent(Request);    // 记录客户端信息

            //Request.Params[HTTP_HOST]：60.205.185.168:8001
            //Request.Url：http://60.205.185.168:8001/Pages/request.aspx
            //string ServerUrl = "http://" + Request.Params["HTTP_HOST"];

            QrUrl = Request["QrUrl"];       // 当前打开
            HbUrl = Request["HbUrl"];   // 跳转至
            Tittle = Request["Tittle"]; // 

            if (QrUrl == null) QrUrl = "";
            if (HbUrl == null) HbUrl = "https://qr.alipay.com/c1x01990gbhjvuvwaxwkqa3"; // 默认红包码
            if (Tittle == null) Tittle = "商户名称（未设置）";
            if (QrUrl.StartsWith("ShowInfo_")) QrUrl = "http://" + Request.Params.Get("HTTP_HOST") + "/" + "PageHB/ShowInfo.aspx?p=" + QrUrl.Substring("ShowInfo_".Length);


            if (QrUrl.Equals(""))
            {
                string msg = "参数QrUrl不应为空！";
                msg += "\r\n示例：" + Request.Url + "?" + "QrUrl=" + "http://www.baidu.com" + "&HbUrl=" + "" + "&Tittle=" + "第8号当铺";

                Response.Write(ScTool.Alert(msg));
                return;
            }

            LabelTittle.Text = Tittle;

            string key = "HbPay";
            string date = DateTime.Now.ToString("yyyyMMdd");        // 每天可领取一次，首次跳转

            if(Session.Timeout != 60*24) Session.Timeout = 60*24;   // 设置Session有效时间为24小时
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
                Response.Redirect(QrUrl);
            }
            else
            {                               // 首次访问，跳转领取红包
                //NewLinkDiv.InnerHtml = "<a href =\"https://qr.alipay.com/c1x01990gbhjvuvwaxwkqa3\" >支付宝领红包</a>";

                //string content = "<img id=\"img1\" src=\"" + ServerUrl + "/tools/HB_pic/bg.jpg\" />";
                //content += "<a href=\"" + HbUrl + "\"><img id=\"img2\" src=\"" + ServerUrl + "/tools/HB_pic/btn.png\" alt=\"点击领取红包\" /></a>";
                //BtnDiv.InnerHtml = content;

                HbUrl = "GetHB.aspx" + "?p=" + HbUrl;   //获取红包码url
                BtnDiv.InnerHtml = "<a href=\"" + HbUrl + "\"><img src=\"../tools/HB_pic/btn.png\"></a>";

                // 红包首次访问计数
                if (Request["ID"] != null)
                {
                    string InfoUrl = "http://" + Request.Params.Get("HTTP_HOST") + "/PageHB/HbInfo.aspx";
                    string commond = InfoUrl + "?" + "TYPE=CountAdd&ID=" + Request["ID"];
                    ScTool.getWebData(commond);
                }
            }

            //// 制作我的红包码
            //string CreateHbUrl = "http://" + Request.Params.Get("HTTP_HOST") + "/PageHB/HB.aspx";
            //LinkA.HRef = CreateHbUrl;
        }

        ///// <summary>
        ///// 点击链接，跳转红包收款码制作界面
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //protected void LinkButtonCreat_Click(object sender, EventArgs e)
        //{
        //    string HbUrl = "http://" + Request.Params.Get("HTTP_HOST") + "/PageHB/HB.aspx";
        //    Response.Redirect(HbUrl);
        //}

    }
}