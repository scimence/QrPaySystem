using QrPaySystem.Tool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace QrPaySystem.Pages
{
    public partial class Product : System.Web.UI.Page
    {
        public static Boolean UseStaticIpMode = false;    // 是否使用静态Ip网址模式

        string HostUrl = "";
        //string key = "ProductPage_Account";
        protected void Page_Load(object sender, EventArgs e)
        {
            string ID = Request["p"];
            if (ID != null && !ID.Equals(""))
            {
                string url = "~/Pages/ProductInfo.aspx" + "?" + "ID=" + ID;
                Server.Transfer(url);   // 跳转至商品支付页
            }

            HostUrl = "http://" + Request.Params.Get("HTTP_HOST");

            //// 载入以前的账号名称
            //if (Session.Timeout != 60 * 24) Session.Timeout = 60 * 24;   // 设置Session有效时间为24小时
            //if (Session[key] != null)
            //{
            //    string SessionAccount = (Session[key] as string).Trim();
            //    if (!SessionAccount.Equals("")) TextBox_Account.Text = SessionAccount;
            //}
        }

        protected void ImageButton_Create_Click(object sender, ImageClickEventArgs e)
        {
            //Session[key] = TextBox_Account.Text.Trim(); // 账号变动时，记录账号信息至Session

            string account = TextBox_Account.Text.Trim();   // 用户账号
            string name = TextBox_Name.Text.Trim();         // 资源名称
            string price = TextBox_Price.Text.Trim();       // 价格
            if(price.EndsWith("元")) price = price.Replace("元", "");

            string pass = TextBox_Pass.Text.Trim();         // 密码
            
            // 记录信息至Product表，返回记录id
            string commond = "TYPE=Add&name=" + name + "&price=" + price + "&author=" + account + "&data=" + pass + "&ext=";
            string url = HostUrl  + "/Pages/ProductInfo.aspx" + "?" + commond;
            string Id = ScTool.getWebData(url);
            if (Id.Equals(""))
            {
                Label_tip.Text = "资源二维码制作失败！ -> Id为空";
                return;
            }

            // 制作收款码
            //string link = HostUrl + "/Pages/Product.aspx" + "?" + "p=" + Id;
            string link = "http://scimence.gitee.io/url/product.html" + "?" + "p=" + Id;    // 使用静态页地址进行转发
            if (UseStaticIpMode) link = HostUrl + "/Pages/Product.aspx" + "?" + "p=" + Id;  // 使用当前静态ip地址

            string picName = Product_QrTool.genPayPic(link, name, Id, "tools\\QRTool\\QR_Product\\");
            if (!picName.Equals(""))
            {
                img_Example.Src = "~/tools/QRTool/QR_Product/" + picName;           // 显示生成的二维码
                Label_tip.Text = "资源二维码已生成！";

                string picUrl = HostUrl + "/tools/QRTool/QR_Product/" + picName;
                ShowDownload(picUrl);

                //if (!account.Equals("")) Session[key] = account;    // 记录账号信息至Session
            }
            else
            {
                Label_tip.Text = "二维码制作失败！";
                DivSave.InnerHtml = "";
            }

        }

        /// <summary>
        /// 设置待下载的图像
        /// </summary
        /// <param name="picUrl">图片地址</param>
        private void ShowDownload(string picUrl)
        {
            picUrl = System.Web.HttpUtility.UrlEncode(picUrl);
            string downloadPic = HostUrl + "/Pages/pic.aspx";
            downloadPic += "?path=" + picUrl + "&download=true";

            DivSave.InnerHtml = "<a target=\"_blank\" href=\"" + downloadPic + "\"><img src=\"../tools/Product_pic/btn_save_qr.png\" Height=\"75px\" Width=\"311px\" /></a>";   
        }

        protected void Button_Account_Click(object sender, EventArgs e)
        {
            string account = TextBox_Account.Text.Trim();
            //if (!account.Equals("")) Session[key] = account;    // 记录账号信息至Session

            string rewardUrl = HostUrl + "/Pages/UserRewards.aspx" + "?" + "p=" + account;

            Response.Redirect(rewardUrl);   
        }

    }
}