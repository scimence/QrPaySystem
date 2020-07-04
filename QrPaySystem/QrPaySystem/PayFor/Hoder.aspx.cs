using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace QrPaySystem.PayFor
{
    public partial class Hoder : System.Web.UI.Page
    {
        /// <summary>
        /// 获取请求参数信息
        /// </summary>
        private String getParam()
        {
            String Url = Request.Url.ToString();
            String param = "";
            if (Url.Contains("?"))
            {
                int index = Url.IndexOf("?");
                param = Url.Substring(index + 1);
            }
            return param;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            String url = getParam();
            if(!url.Equals("")) setUrl(url);
        }

        private void setUrl(String url)
        {
            DivContent.InnerHtml = "<object width=\"100%\" height=\"720\" type=\"text/x-scriptlet\" data=\"" + url + "\" id=\"Object1\">";
        }
    }
}