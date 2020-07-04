using QrPaySystem.Tool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace QrPaySystem.PageHB
{
    public partial class HbPay2 : System.Web.UI.Page
    {
        string ServerUrl = "";  // 当前服务器的Url地址信息
        string QrUrl = "";      // 收款Url
        string HbUrl = "";      // 红包Url
        string Tittle = "";     // 商户名称

        //Boolean testMode = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack) ScTool.RecordUserAgent(Request);    // 记录客户端信息

            //FileUpload_HB.Style.Add("display", "none");   // 隐藏控件
            Body1.Style.Add("display", "none");   // 隐藏body
            //BtnDiv.Style.Add("display", "none");   // 隐藏按钮
            BtnDiv.Style.Add("display", "none");   // 隐藏按钮

            //Request.Params[HTTP_HOST]：60.205.185.168:8001
            //Request.Url：http://60.205.185.168:8001/Pages/request.aspx
            //string ServerUrl = "http://" + Request.Params["HTTP_HOST"];

            QrUrl = Request["QrUrl"];       // 当前打开
            HbUrl = Request["HbUrl"];   // 跳转至
            Tittle = Request["Tittle"]; // 

            if (QrUrl == null) QrUrl = "";
            //if (testMode)
            //{
            //    if (QrUrl == null) QrUrl = "https://www.baidu.com";
            //    if (HbUrl == null) HbUrl = "https://fanyi.baidu.com"; // 默认红包码
            //}
            //else
            //{
            //if (QrUrl == null) QrUrl = "https://qr.alipay.com/tsx031041ajtuiviwd978b6";
            if (HbUrl == null) HbUrl = "https://qr.alipay.com/c1x01990gbhjvuvwaxwkqa3"; // 默认红包码
            //}

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

            string key = "HbPay2";
            string date = DateTime.Now.ToString("yyyyMMdd");        // 每天可领取一次，首次跳转


            if (Session.Timeout != 60 * 24) Session.Timeout = 60 * 24;   // 设置Session有效时间为24小时
            string value = "";
            if (Session[key] == null || !(Session[key] as string).StartsWith(date))  // 若key不存在，或非今天的则生成新的值
            {
                value = date + "_0";
                Session[key] = value;
            }

            if ((Session[key] as string).Equals(date + "_0"))       // 打开收款码
            {
                Session[key] = date + "_1";
                //if (testMode)
                //{
                //    BtnDiv.InnerHtml = "<a id=\"BtnName1\"  target=\"_blank\" " + " onclick=\"reloadPage()\" " + " href=\"" + QrUrl + "\" " + "><img src=\"../tools/HB_pic/btn.png\"></a>";
                //}
                //else
                //{
                //BtnDiv.InnerHtml = "<a id=\"BtnName1\" " + " onclick=\"reloadPage()\" " + " href=\"" + "alipayqr://platformapi/startapp?saId=10000007&qrcode=" + QrUrl + "\" " + "><img src=\"../tools/HB_pic/btn.png\"></a>";
                BtnDiv.InnerHtml = "<a id=\"BtnName1\" " + " onclick=\"reloadPage()\" " + " href=\"" + "alipayqr://platformapi/startapp?saId=10000007&qrcode=" + QrUrl + "\" " + "> </a>";
                //}
            }
            else if ((Session[key] as string).Equals(date + "_1"))  // 重定向至红包码
            {
                // 红包首次访问计数
                if (Request["ID"] != null)
                {
                    string InfoUrl = "http://" + Request.Params.Get("HTTP_HOST") + "/PageHB/HbInfo.aspx";
                    string commond = InfoUrl + "?" + "TYPE=CountAdd&ID=" + Request["ID"];
                    ScTool.getWebData(commond);
                }

                Session[key] = date + "_2";
                Response.Redirect(HbUrl);

                // 若要回到红包页面
                //BtnDiv.InnerHtml = "<a id=\"BtnName1\" href=\"" + HbUrl + "\"><img src=\"../tools/HB_pic/btn.png\"></a>";
            }
            else 
                //if ((Session[key] as string).Equals(date + "_2"))
            {
                //Response.Redirect(QrUrl);
                Session[key] = date + "_3";
                //Response.Redirect("alipayqr://platformapi/startapp?saId=10000007&qrcode=" + QrUrl);
                Response.Redirect(QrUrl);
                //BtnDiv.InnerHtml = "<a id=\"BtnName1\" " + " href=\"" + QrUrl + "\"><img src=\"../tools/HB_pic/btn.png\"></a>"; //直接链接收款码，返回会关闭页面
                //BtnDiv.InnerHtml = "<a id=\"BtnName1\" " + " href=\"" + "alipayqr://platformapi/startapp?saId=10000007&qrcode=" + QrUrl + "\"><img src=\"../tools/HB_pic/btn.png\"></a>";
            }
            //else if ((Session[key] as string).Equals(date + "_3"))
            //{
            //    Session[key] = date + "_2";
            //    BtnDiv.InnerHtml = "<a id=\"BtnName3\" " + " href=\"" + "alipayqr://platformapi/startapp?saId=10000007&qrcode=" + QrUrl + "\"><img src=\"../tools/HB_pic/btn.png\"></a>";
            //}

            //String Re = "http://" + Request.Params.Get("HTTP_HOST") + "/PageHB/Redirect.aspx";
            //String Hb = "http://" + Request.Params.Get("HTTP_HOST") + "/PageHB/HB.aspx";
            //String mackUrl = Re + "?p=" + Hb;
            //LinkDiv.InnerHtml = "<a href=\"" + mackUrl + "\"><asp:Label Text=\"制作我的红包收款码\" ForeColor=\"White\"></asp:Label></a>";
        }

    }

}