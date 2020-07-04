using QrPaySystem.Tool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace QrPaySystem.PayFor
{
    public partial class UserLogin : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if ((!Page.IsPostBack)) ScTool.RecordUserAgent(Request);    // 记录客户端信息

            UserTool.InitTool();
            UserTool.ClearAccount(Session);

            string Account = Request["Account"];
            if(Account != null && !Account.Equals("")) TextBox_account.Text = Account;
        }

        protected void Button_login_Click(object sender, EventArgs e)
        {
            string account = TextBox_account.Text.Trim();
            string password = TextBox_psw.Text.Trim();

            if (account.Equals(""))
            {
                Label_TipInfo.Text = "请输入您的帐号信息！";
                return;
            }
            if (password.Equals("")) 
            {
                Label_TipInfo.Text = "请输入您的密码信息！"; 
                return;
            }

            string psw = UserTool.Get(account, "Password");
            if (psw.Equals(password))
            {
                Label_TipInfo.Text = "登录成功！";

                UserTool.SaveAccount(Session, account, password);       // 记录帐号密码信息
                UserTool.CountLogin(account);                           // 统计登录次数

                Response.Redirect("SDK.aspx");
            }
            else
            {
                Label_TipInfo.Text = "密码不正确！"; 
            }
        }


    }
}