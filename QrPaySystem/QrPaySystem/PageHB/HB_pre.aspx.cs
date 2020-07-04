using QrPaySystem.Tool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace QrPaySystem.PageHB
{
    public partial class HB_pre : System.Web.UI.Page
    {
        string serverUrl = "";

        string InfoUrl = "";
        string HbPayUrl = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            FileUpload1.Style.Add("display", "none");   // 隐藏控件

            serverUrl = "http://" + Request.Params.Get("HTTP_HOST") + "/" + this.GetType().Name.Replace("_", "/").Replace("/aspx", ".aspx");
            InfoUrl = "http://" + Request.Params.Get("HTTP_HOST") + "/PageHB/HbInfo.aspx";
            //HbPayUrl = "http://" + Request.Params.Get("HTTP_HOST") + "/PageHB/HbPay.aspx";
            HbPayUrl = "~/PageHB/HbPay.aspx";

            string ID = Request["p"];
            if (ID != null && !ID.Equals(""))
            {
                string QrUrl = getHbInfo(ID, "QrUrl");
                string Tittle = getHbInfo(ID, "Tittle");
                string HbUrl = getHbInfo(ID, "HbUrl");

                string url = HbPayUrl + "?" + "QrUrl=" + QrUrl + "&HbUrl=" + HbUrl + "&Tittle=" + Tittle + "&ID=" + ID;

                Server.Transfer(url);
            }
        }

        /// <summary>
        /// 从信息表中获取数据
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="keyName"></param>
        /// <returns></returns>
        private string getHbInfo(string ID, string keyName)
        {
            string commond = "TYPE=Get&ID=" + ID + "&KeyName=" + keyName;
            string url = InfoUrl + "?" + commond;
            string value = ScTool.getWebData(url);
            return value;
        }


        protected void Button1_Click(object sender, EventArgs e)
        {
            string QrUrl = TextBox1.Text.Trim();
            string HbUrl = TextBox2.Text.Trim();
            string Tittle = TextBox3.Text.Trim();

            
            // 从网页接口Sql.aspx获取数据
            string commond = "TYPE=Add&QrUrl=" + QrUrl + "&HbUrl=" + HbUrl + "&Tittle="+ Tittle +"&ext=";

            String url = InfoUrl + "?" + commond;
            string Id = ScTool.getWebData(url);

            String PageUrl = "http://" + Request.Params.Get("HTTP_HOST") + "/" + this.GetType().Name.Replace("_", "/").Replace("/aspx", ".aspx");
            HyperLink1.NavigateUrl = PageUrl + "?" + "p=" + Id;
            HyperLink1.Text = "红包收款码:" + HyperLink1.NavigateUrl;
            
        }

        /// <summary>
        /// 上传选择的文件至服务器
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void UploadButton_Click(object sender, EventArgs e)
        {
            // 设置文件保存目录
            string appPath = Request.PhysicalApplicationPath + @"Uploads\";
            if (!System.IO.Directory.Exists(appPath)) System.IO.Directory.CreateDirectory(appPath);

            if (FileUpload1.HasFile)
            {
                String fileName = FileUpload1.FileName;
                string savePath = appPath + Server.HtmlEncode(FileUpload1.FileName);    // 生成保存路径

                FileUpload1.SaveAs(savePath);                                           // 保存文件

                UploadStatusLabel.Text = "Your file was saved as ->" + savePath;
            }
            else
            {
                UploadStatusLabel.Text = "You did not specify a file to upload.";
            }
        }

    }
}