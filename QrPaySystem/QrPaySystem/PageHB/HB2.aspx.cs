using QrPaySystem.Pages;
using QrPaySystem.Tool;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace QrPaySystem.PageHB
{
    public partial class HB : System.Web.UI.Page
    {
        string InfoUrl = "";
        string HbPayUrl = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack) ScTool.RecordUserAgent(Request);    // 记录客户端信息

            FileUpload_HB.Style.Add("display", "none");   // 隐藏控件
            FileUpload_SK.Style.Add("display", "none");   // 隐藏控件

            //FileUpload_HB.Attributes.Add("onchange", "document.getElementById('ImageButton_Create').click();"); // FileUload选择文件后触发控件ImageButton的Click()事件

            Button_HB.Style.Add("display", "none");   // 隐藏控件
            Button_SK.Style.Add("display", "none");   // 隐藏控件

            //Button_HB.Visible = false;
            //Button_SK.Visible = false;


            // 根据红包码参数，跳转红包码、或 收款码

            //serverUrl = "http://" + Request.Params.Get("HTTP_HOST") + "/" + this.GetType().Name.Replace("_", "/").Replace("/aspx", ".aspx");
            InfoUrl = "http://" + Request.Params.Get("HTTP_HOST") + "/PageHB/HbInfo.aspx";
            //HbPayUrl = "http://" + Request.Params.Get("HTTP_HOST") + "/PageHB/HbPay.aspx";
            HbPayUrl = "~/PageHB/HbPay.aspx";

            string ID = Request["p"];
            if (ID != null && !ID.Equals(""))
            {
                //string QrUrl = getHbInfo(ID, "QrUrl");
                //string Tittle = getHbInfo(ID, "Tittle");
                //string HbUrl = getHbInfo(ID, "HbUrl");

                string jsonData = getHbInfo(ID);    // 查询对应的数据

                string QrUrl = ScTool.getJsonValue(jsonData, "QrUrl");
                string Tittle = ScTool.getJsonValue(jsonData, "Tittle");
                string HbUrl = ScTool.getJsonValue(jsonData, "HbUrl");
                string ext = ScTool.getJsonValue(jsonData, "ext");

                if (ext.Contains("mode(") && ext.Contains(")"))
                {
                    int start = ext.IndexOf("mode(") + "mode(".Length;
                    int end = ext.IndexOf(")", start);
                    String mode = ext.Substring(start, end - start);
                    HbPayUrl = "~/PageHB/HbPay" + mode + ".aspx";
                }

                string url = HbPayUrl + "?" + "QrUrl=" + QrUrl + "&HbUrl=" + HbUrl + "&Tittle=" + Tittle + "&ID=" + ID + "&ext=" + ext;

                Server.Transfer(url);
            }
        }

        /// <summary>
        /// 从数据表中，获取二维码信息
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="keyName"></param>
        /// <returns></returns>
        private string getHbInfo(string ID, string keyName=null)
        {
            string commond = "";
            if (keyName != null) commond = "TYPE=Get&ID=" + ID + "&KeyName=" + keyName;
            else commond = "TYPE=Get&ID=" + ID;

            string url = InfoUrl + "?" + commond;
            string value = ScTool.getWebData(url);
            return value;
        }

        /// <summary>
        /// 上传红包码图像
        /// </summary>
        protected void Button_HB_Click(object sender, EventArgs e)
        {
            string fileName = Upload_Pic(FileUpload_HB, TextBox_HB, "HB");
            if (!fileName.Equals("")) img_HB.Src = "../Uploads/HB/" + fileName;
        }

        /// <summary>
        /// 上传收款码图像
        /// </summary>
        protected void Button_SK_Click(object sender, EventArgs e)
        {
            string fileName = Upload_Pic(FileUpload_SK, TextBox_SK, "SK");
            if (!fileName.Equals("")) img_SK.Src = "../Uploads/SK/" + fileName;
        }

        /// <summary>
        /// 上传图像，解析二维码信息
        /// </summary>
        /// <param name="uploader">通过FileUpload上传二维码图像</param>
        /// <param name="textQr">解析二维码图像上传至此处</param>
        /// <param name="subDir">Uploads子目录</param>
        /// <returns>返回二维码图像名称，用于显示</returns>
        protected string Upload_Pic(FileUpload uploader, TextBox textQr, string subDir)
        {
            // 设置文件保存目录
            string appPath = Request.PhysicalApplicationPath + @"Uploads\" + subDir + "\\";
            if (!System.IO.Directory.Exists(appPath)) System.IO.Directory.CreateDirectory(appPath);

            if (uploader.HasFile)
            {
                String fileName = uploader.FileName;
                string savePath = appPath + Server.HtmlEncode(uploader.FileName);    // 生成保存路径

                uploader.SaveAs(savePath);                                           // 保存文件

                Bitmap pic = null;
                // 解析二维码信息
                try
                {
                    pic = (Bitmap)Bitmap.FromFile(savePath);

                    textQr.Text = QrTool_HB.ToCode(pic);     // 解析二维码信息
                    if (textQr.Text.Equals("")) textQr.Text = "Error:请重新上传有效的二维码图像";
                    else if (!textQr.Text.ToLower().StartsWith("http://") && !textQr.Text.ToLower().StartsWith("https://"))
                    {
                        textQr.Text = "Error:" + textQr.Text;
                        ScTool.Alert("您上传的二维码未能识别，可能是图像不够清晰，请重新上传！");
                    }
                    else textQr.Enabled = false;             // 获取到二维码，则不允许修改

                    pic.Dispose();
                    pic = null;
                }
                catch (Exception ex)
                {
                    textQr.Text = "";
                    pic.Dispose();
                    pic = null;
                }

                //UploadStatusLabel.Text = "Your file was saved as ->" + savePath;
                return fileName;
            }
            else
            {
                //UploadStatusLabel.Text = "You did not specify a file to upload.";
                return "";
            }
        }

        /// <summary>
        /// 记录红包码、二维码、商家名称，返回记录id，生成新的红包收款码供商家收款
        /// </summary>
        /// <param name="e"></param>
        protected void ImageButton_Create_Click(object sender, ImageClickEventArgs e)
        {
            string QrUrl = TextBox_SK.Text.Trim();      // 收款码
            string HbUrl = TextBox_HB.Text.Trim();      // 红包码
            string Tittle = TextBox_Tittle.Text.Trim(); // 商家名称

            if (HbUrl.Equals("") || HbUrl.Contains("红包码") || HbUrl.StartsWith("Error:"))
            {
                Label_tip.Text = "请先点击，添加您的红包码！";
                return;
            }
            if (QrUrl.Equals("") || QrUrl.Contains("收款码") || QrUrl.StartsWith("Error:"))
            {
                Label_tip.Text = "请先点击，添加您的收款码！";
                return;
            }
            if (Tittle.Equals("") || Tittle.Contains("第8号当铺"))
            {
                Label_tip.Text = "请添加您的商家名称！";
                return;
            }

            // 红包码，收款码 数据校验
            if (!ChekQrTrue(ref QrUrl, ref HbUrl))
            {
                //Label_tip.Text = "红包码或收款码上传错误，请重新上传！";
                return;
            }

            // 记录红包码、收款码信息，返回记录id
            string commond = "TYPE=Add&QrUrl=" + QrUrl + "&HbUrl=" + HbUrl + "&Tittle=" + Tittle + "&ext=";

            string url = InfoUrl + "?" + commond;
            string Id = ScTool.getWebData(url);
            if (Id.Equals(""))
            {
                Label_tip.Text = "红包收款码制作失败！ -> Id为空";
                return;
            }
            //string Id = "100";



            // 生成红包收款码
            string PageUrl = "http://" + Request.Params.Get("HTTP_HOST") + "/" + this.GetType().Name.Replace("_", "/").Replace("/aspx", ".aspx");
            string HB_QR = PageUrl + "?" + "p=" + Id;

            // 制作红包收款码
            string picName = QrTool_HB.genHbSkPic(HB_QR, Tittle, Id, "tools\\QRTool\\QR_HB\\");
            if (!picName.Equals(""))
            {
                img_Example.Src = "~/tools/QRTool/QR_HB/" + picName;           // 显示生成的二维码
                Label_tip.Text = "您的红包收款码已生成！";
            }
            else
            {
                Label_tip.Text = "红包收款码制作失败！";
            }

            //img_Example.Src = "~/tools/QRTool/QR/" + NAME;            // 现实二维码图像

            //Bitmap = QrTool_HB.ToQr()
            //showQR(HB_QR);

            // 制作红包收款码
            //...
            //HyperLink1.Text = "红包收款码:" + HyperLink1.NavigateUrl;
        }

        /// <summary>
        /// 对红包码、收款码信息进行校验
        /// </summary>
        /// <param name="QrUrl">收款码</param>
        /// <param name="HbUrl">红包码</param>
        private bool ChekQrTrue(ref  string QrUrl, ref string HbUrl)
        {
            bool QrError = QrUrl.ToLower().StartsWith("https://qr.alipay.com/c1");
            bool HbError = HbUrl.ToLower().StartsWith("https://qr.alipay.com/ts") || HbUrl.ToLower().StartsWith("https://qr.alipay.com/fk");

            // 若红包码、收款码传反了，则自动交换纠正
            if (QrError && HbError)
            {
                string tmp = QrUrl;
                QrUrl = HbUrl;
                HbUrl = tmp;
            }
            else if (QrError)
            {
                string msg = "您是不是错传了两个红包码？请上传一个收款码";
                Label_tip.Text = msg;
                ScTool.Alert(msg);

                return false;
            }
            else if (HbError)
            {
                string msg = "您是不是错传了两个收款码？请上传一个红包码";
                Label_tip.Text = msg;
                ScTool.Alert(msg);

                return false;
            }

            return true;
        }

        //protected void Qr_Text_TextChanged(object sender, EventArgs e)
        //{
        //    TextBox_HB.Text = Qr_Text.Text;
        //    TextBox_SK.Text = Qr_Text.Text;
        //}

        //// 在页面展示生成的二维码
        //private void showQR(string data)
        //{
        //    //body1.Visible = true;

        //    string NAME = QRTool.GetQR(data);                         // 生成二维码
        //    //Image2.ImageUrl = "~/tools/QRTool/QR/" + NAME;            // 现实二维码图像
        //    img_Example.Src = "~/tools/QRTool/QR/" + NAME;            // 现实二维码图像

        //    //Timer1.Enabled = true;  // 启动Timer,定时刷新查询支付结果
        //}
    }
}