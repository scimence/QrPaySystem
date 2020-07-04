using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace QrPaySystem.Pages
{
    public partial class Browser : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string url = Request["p"];
            if (url != null && !url.Equals(""))
            {
                TextBox_url.Text = url;
                setUrl(url);
            }
        }

        protected void Button_jump_Click(object sender, EventArgs e)
        {
            String url = TextBox_url.Text.Trim();
            setUrl(url);

            //String curUrl = Request.RawUrl + "?" + "p=" + url;
            //Response.Redirect(curUrl);
        }

        private void setUrl(String url)
        {
            DivContent.InnerHtml = "<object width=\"100%\" height=\"1280\" type=\"text/x-scriptlet\" data=\"" + url + "\" id=\"Object1\">";
        }
    }
}