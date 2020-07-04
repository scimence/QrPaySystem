using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Drawing;
using System.Net;

namespace QrPaySystem.PageHB
{
    public partial class PicHB : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            String dirPath = Request["path"];
            String reName = Request["reName"];
            bool isDownload = (Request["download"] != null && Request["download"].Equals("true"));

            if (dirPath == null)
            {
                Response.Write(Request.Url + "?" + "path=" + "http://t2.hddhhn.com/uploads/tu/201707/115/56.jpg" + "&" + "reName=" + "0.png" + "&" + "download=" + "false");
            }
            else
            {
                Bitmap img = QrTool_HB.ToHbPic(dirPath);
                if (reName == null) reName = "0.png";

                // 输出图像
                try
                {
                    MemoryStream ms = new MemoryStream();
                    img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                    Response.ClearContent();
                    Response.ContentType = "Image/png";
                    Response.AddHeader("Content-Disposition", (isDownload ? "attachment; " : "") + "filename=" + HttpUtility.UrlEncode(reName, System.Text.Encoding.UTF8));
                    Response.BinaryWrite(ms.ToArray());
                }
                catch (Exception ex) { }
            }
        }
    }
}