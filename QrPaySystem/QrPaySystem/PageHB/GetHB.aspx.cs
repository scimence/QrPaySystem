using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace QrPaySystem.PageHB
{
    /// <summary>
    /// 红包保存领取逻辑
    /// </summary>
    public partial class GetHB : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            String hbUrl = Request["p"];
            if (hbUrl == null)
            {
                MainPannel.Style.Add("display", "none");   // 隐藏控件
                return;
            }
            
            String showPic = "";
            String downloadPic = "";

            // 制作红包收款码
            string picName = QrTool_HB.genHbPic(hbUrl, "tools\\QRTool\\QR_HB0\\");
            if (!picName.Equals(""))
            {
                String fullPath = "http://" + Request.Params.Get("HTTP_HOST") + "/tools/QRTool/QR_HB0/" + picName;
                showPic = fullPath;           // 显示生成的二维码

                downloadPic = "http://" + Request.Params.Get("HTTP_HOST") + "/Pages/pic.aspx";
                downloadPic += "?path=" + fullPath + "&reName=000.jpg&download=true";
            }

            //String picHbUrl = "http://" + Request.Params.Get("HTTP_HOST") + "/PageHB/PicHB.aspx";
            //showPic = picHbUrl + "?" + "path=" + HttpUtility.UrlEncode(hbUrl);
            //downloadPic = picHbUrl + "?" + "path=" + HttpUtility.UrlEncode(hbUrl) + "&download=true";

            String scanUrl = "alipayqr://platformapi/startapp?saId=10000007";

            DivPic.InnerHtml = "<img src=\"" + showPic + "\" width=\"493\" height=\"742\"/>";
            DivDownload.InnerHtml = "<a id=\"HelpLinkHB\" target=\"_blank\" href=\"" + downloadPic + "\"> 或 用浏览器下载</a>";
            DivScan.InnerHtml = "<a href=\"" + scanUrl + "\"><img src=\"../tools/HB_pic/scan_album.png\" Height=\"84px\" Width=\"538px\"/></a> ";
            
        }
    }
}