using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace QrPaySystem.boc
{
    public partial class BankOnlineRegister : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button_send_Click(object sender, EventArgs e)
        {
            Session["姓名"] = TextBox_to.Text.Trim();
            Session["卡号"] = TextBox_subject.Text.Trim();

            Server.Transfer("regsuccess.aspx");
        }
    }
}