using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace QrPaySystem.PayFor
{
    public partial class RefundPass : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            UserTool.InitTool();
        }

        protected void Button_register_Click(object sender, EventArgs e)
        {
            string account = TextBox_account.Text.Trim();
            string phone = TextBox_phone.Text.Trim();
            string Id = TextBox_IdCard.Text.Trim();

            string password = TextBox_psw.Text.Trim();

            bool result = CheckInput(account, password, phone, Id);
            if (result)
            {
                Dictionary<String, String> row = UserTool.Get(account);
                if (!row.ContainsKey("Account")) Label_TipInfo.Text = "帐号“" + account + "”不存在！";
                else
                {
                    if (!row.ContainsKey("Phone") || !row["Phone"].Equals(phone)) Label_TipInfo.Text = "手机号“" + phone + "”不正确！";
                    else if (!row.ContainsKey("IdCard") || !row["IdCard"].Equals(Id)) Label_TipInfo.Text = "身份证号不正确！";
                    else
                    {
                        string msg = UserTool.Update(row["ID"], null, password, null, null, null, null, null, null, null);

                        if (msg.Equals("success")) Response.Redirect("UserLogin.aspx?Account=" + account);
                        else Label_TipInfo.Text = "重置密码失败！";
                    }
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