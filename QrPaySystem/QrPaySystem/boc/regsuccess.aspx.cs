using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace QrPaySystem.boc
{
    public partial class regsuccess : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string name = Session["姓名"] == null ? "" : Session["姓名"].ToString();
            string card = Session["卡号"] == null ? "" : Session["卡号"].ToString();

            TextBox_to.Text = name;
            TextBox_subject.Text = card;
        }
    }
}