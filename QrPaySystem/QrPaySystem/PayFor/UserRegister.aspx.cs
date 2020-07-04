using QrPaySystem.Tool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace QrPaySystem.PayFor
{
    public partial class UserRegister : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            UserTool.InitTool();
        }

        protected void Button_register_Click(object sender, EventArgs e)
        {
            string Account = TextBox_account.Text.Trim();
            string Password = TextBox_psw.Text.Trim();
            string Phone = TextBox_phone.Text.Trim();
            string IdCard = TextBox_IdCard.Text.Trim();

            bool result = CheckInput(Account, Password, Phone, IdCard);
            if (result)
            {
                // 判断帐号是否存在，若帐号不存在则添加
                bool isExit = UserTool.Exist(Account);  
                if (isExit) Label_TipInfo.Text = "帐号“" + Account + "”已存在，请使用其它昵称！";
                else
                {
                    string msg = UserTool.Add(Account, Password, Phone, IdCard);
                    Label_TipInfo.Text = "恭喜，帐号注册成功！";

                    //ScTool.Alert("恭喜，帐号注册成功！");
                    Response.Redirect("UserLogin.aspx?Account=" + Account);
                }
            }
        }

        /// <summary>
        /// 检测输入信息是否符合要求
        /// </summary>
        private bool CheckInput(string Account, string Password, string Phone, string IdCard)
        {
            bool result = true;

            if (IdCard == null || IdCard.Equals("") || IdCard.Length < 15)
            {
                Label_TipInfo.Text = "请输入您的身份证号!";
                result = false;
            }
            if (Phone == null || Phone.Equals("") || Phone.Length < 11)
            {
                Label_TipInfo.Text = "请输入您的手机号!";
                result = false;
            }
            if (Password == null || Password.Equals("") || Password.Length < 6)
            {
                Label_TipInfo.Text = "请输入您的密码（不少于6位）!";
                result = false;
            }
            if (Account == null || Account.Equals("") || Account.Length < 2)
            {
                Label_TipInfo.Text = "请输入您的帐号（不少于2位）!";
                result = false;
            }

            return result;
        }
    }
}