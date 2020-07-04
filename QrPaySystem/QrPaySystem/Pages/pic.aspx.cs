using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Drawing;
using System.Net;

namespace QrPaySystem.Pages
{
    public partial class pic : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //string dirPath = Request.PhysicalApplicationPath + @"uploads\" + "1.png";
            //Bitmap img = (Bitmap)Bitmap.FromFile(dirPath);

            String dirPath = Request["path"];
            String reName = Request["reName"];
            bool isDownload = (Request["download"] != null && Request["download"].Equals("true"));

            if (dirPath == null)
            {
                Response.Write(Request.Url + "?" + "path=" + "http://t2.hddhhn.com/uploads/tu/201707/115/56.jpg" + "&" + "reName=" + "000.jpg" + "&" + "download=" + "false");
            }
            else
            {
                Bitmap img = getBitmap(dirPath);
                if (reName == null) reName = System.IO.Path.GetFileName(dirPath);

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

        /// <summary>
        /// 获取本地获取网络路径中的图像
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private Bitmap getBitmap(String path)
        {
            Bitmap tmp = null;

            try
            {
                if (path.ToLower().StartsWith("http://") || path.ToLower().StartsWith("https://"))
                {
                    tmp = getWebImage(path);
                }
                else if (File.Exists(path))
                {
                    tmp = (Bitmap)Bitmap.FromFile(path);
                }
            }
            catch (Exception ex)
            {
            }

            return tmp;
        }


        /// <summary>
        /// 获取picUrl的图像
        /// </summary>
        private Bitmap getWebImage(String picUrl)
        {
            WebClient client = new WebClient();
            byte[] data = client.DownloadData(picUrl);       // 下载url对应图像数据

            Bitmap image = null;
            if (data.Length > 0) image = BytesToBitmap(data);// 转化为图像

            return image;
        }

        /// <summary>
        /// byte[] 转换 Bitmap
        /// </summary>
        public static Bitmap BytesToBitmap(byte[] Bytes)
        {
            MemoryStream stream = null;
            try
            {
                stream = new MemoryStream(Bytes);
                return new Bitmap(stream);
            }
            catch (Exception ex)
            { }
            stream.Close();
            return null;
        }

    }
}