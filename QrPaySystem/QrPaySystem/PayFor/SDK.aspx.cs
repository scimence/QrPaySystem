using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace QrPaySystem.PayFor
{
    public partial class SDKPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // 设置文件保存目录
            string payforDir = Request.PhysicalApplicationPath + @"tools\Payfor_res";
            if (!System.IO.Directory.Exists(payforDir)) System.IO.Directory.CreateDirectory(payforDir);

            // 载入文件名称信息
            String sdk_windows = "";
            String sdk_android = "";
            String sdk_web = "";

            DirectoryInfo dirInfo = new DirectoryInfo(payforDir);
            FileInfo[] files = dirInfo.GetFiles();
            if (files.Length > 0)
            {
                foreach (FileInfo f in files)
                {
                    if (f.Name.StartsWith("sdk_windows")) sdk_windows = f.Name;
                    if (f.Name.StartsWith("sdk_android")) sdk_android = f.Name;
                    if (f.Name.StartsWith("sdk_web")) sdk_web = f.Name;
                }
            }

            div_windows.InnerHtml = InnerHtml(sdk_windows);
            div_android.InnerHtml = InnerHtml(sdk_android);
            div_web.InnerHtml = InnerHtml(sdk_web);
        }

        private string InnerHtml(string sdkName)
        {
            // 解析sdk文件中包含的文件版本信息
            string version = "2018-12-30";
            if (sdkName.StartsWith("sdk_windows")) version = sdkName.Substring("sdk_windows".Length);
            if (sdkName.StartsWith("sdk_android")) version = sdkName.Substring("sdk_android".Length);
            if (sdkName.StartsWith("sdk_web")) version = sdkName.Substring("sdk_web".Length);
            if (version.StartsWith("_") || version.StartsWith("-")) version = version.Substring(1);
            version = version.Trim().ToLower();
            if (version.EndsWith(".zip") || version.EndsWith(".7z") || version.EndsWith(".rar"))
            {
                int index = version.LastIndexOf(".");
                version = version.Substring(0, index);
            }

            // 生成对应的文件版本下载链接
            string str = "";
            if (!sdkName.Equals(""))
            {
                str += "<p >版本：" + version + "</p>";
                str += "<a href=\"../tools/Payfor_res/" + sdkName + "\"><img src=\"../tools/Payfor_res/sdk_download.png\" /></a>";
            }
            return str;
        }
    }
}