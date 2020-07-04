using QrPaySystem.Tool;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace QrPaySystem.Pages
{
    public partial class QRTool : BaseWebPage
    {
        String NAME = "";
        public static string StaticParam = "";  // 静态参数

        /// <summary>
        /// 载入后执行参数对应的sql请求
        /// </summary>
        public override void Load(object sender, EventArgs e)
        {
            //Image1.ImageUrl = "~/tools/QRTool/QR/" + "20180728_093650_321756.png";

            // 若存在静态参数，则使用静态参数
            if (!StaticParam.Equals(""))
            {
                param = StaticParam;
                StaticParam = "";
            }
            
            if (!param.Equals(""))
            {
                //String QrData = url.Substring(url.IndexOf("?") + 1);    // 获取待生成的二维码参数

                //GetQR();
                NAME = GetQR(param);                                   // 生成二维码
                Image1.ImageUrl = "~/tools/QRTool/QR/" + NAME;            // 现实二维码图像

                //// 删除文件
                //String dirPath = HttpContext.Current.Server.MapPath("tools/");                  // 工具路径
                //String picPath = dirPath + "QR\\" + NAME;                                       // 保存文件路径
                //if (File.Exists(picPath)) File.Delete(picPath);
            }
            else
            {
                Label1.Text = "示例: </br>" + Request.Url.ToString() + "?待生成二维码的数据";
                //Response.Write("示例: </br>"  + Request.Url.ToString() + "?待生成二维码的数据");
                //Label1.Text = Request.Url.ToString() + "?待生成二维码的数据";
            }

        }

        public void Page_Unload(object sender, EventArgs e)
        {
            //if (!NAME.Equals(""))
            //{
            //    //Image1.ImageUrl = "";

                
            //    String picPath = AppDomain.CurrentDomain.BaseDirectory + "tools\\QRTool\\QR\\" + NAME;

            //    //// 删除文件
            //    //String dirPath = HttpContext.Current.Server.MapPath("tools/");                  // 工具路径
            //    //String picPath = dirPath + "QR\\" + NAME;                                       // 保存文件路径
            //    //if (File.Exists(picPath)) File.Delete(picPath);
            //}
        }
        

        #region 生成二维码图像逻辑

        /// <summary>
        /// 生成新的文件名
        /// </summary>
        private static string NewName()
        {
            string NAME = DateTime.Now.ToString("yyyyMMdd_HHmmss_ffffff") + ".png";
            return NAME;
        }

        /// <summary>
        /// 为arg添加引号
        /// </summary>
        private static string AddQuotation(string arg)
        {
            if (arg.EndsWith("\\") && !arg.EndsWith("\\\\")) arg += "\\";
            arg = "\"" + arg + "\"";

            return arg;
        }

        /// <summary>
        /// 使用指定data生成二维码图像
        /// </summary>
        public static String GetQR(String QrData)
        {
            String NAME = NewName();                                                        // 生成新的文件名
            //String dirPath = HttpContext.Current.Server.MapPath("tools/");                  // 工具路径
            String dirPath = AppDomain.CurrentDomain.BaseDirectory + "tools\\";
            String picPath = dirPath + "QR\\" + NAME;                                       // 保存文件路径

            if (!File.Exists(picPath))
            {
                String QRTOOL = AddQuotation(dirPath + "QRTool\\QRTool.exe");               // 二维码生成工具路径
                String arg = AddQuotation(QrData) + " " + AddQuotation("RENAME=" + NAME);   // 生成二维码参数

                Start(QRTOOL, arg);                                                         // 执行二维码生成
            }

            return NAME;
        }

        /// <summary>
        /// 以后台进程的形式执行应用程序（d:\*.exe）
        /// </summary>
        public static string Start(String exe, String param)
        {
            Process P = new Process();
            P.StartInfo.CreateNoWindow = true;
            P.StartInfo.FileName = exe;
            P.StartInfo.Arguments = param;
            P.StartInfo.UseShellExecute = false;
            P.StartInfo.RedirectStandardError = true;
            P.StartInfo.RedirectStandardInput = true;
            P.StartInfo.RedirectStandardOutput = true;
            //P.StartInfo.WorkingDirectory = @"C:\windows\system32";
            P.Start();

            string outStr = P.StandardOutput.ReadToEnd();
            P.Close();

            return outStr;
        }

        #endregion

    }
}