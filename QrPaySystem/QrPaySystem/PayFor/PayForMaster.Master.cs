using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace QrPaySystem.PayFor
{
    public partial class PayForMaster : System.Web.UI.MasterPage
    {
        string account = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            account = UserTool.GetAccount(Session);
            if(!account.Equals("")) LabelLogin.Text = account;
        }
    }
}