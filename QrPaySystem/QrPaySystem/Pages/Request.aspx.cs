using QrPaySystem.Tool;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace QrPaySystem.Pages
{
    public partial class Request : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack) ScTool.RecordUserAgent(Request);    // 记录客户端信息

             string agent =    Request.Params.Get("HTTP_USER_AGENT");
             Response.Write(ScTool.Pre("HTTP_USER_AGENT -> \r\n" + agent));

            int loop1, loop2;
            NameValueCollection coll;

            //// Load Header collection into NameValueCollection object.
            ////coll = Request.Headers;

            coll = Request.Params;

            // Put the names of all keys into a string array.
            String[] arr1 = coll.AllKeys;
            for (loop1 = 0; loop1 < arr1.Length; loop1++)
            {
                Response.Write(ScTool.Pre("Key: " + arr1[loop1] + " -> "));

                // Get all values under this key.
                String[] arr2 = coll.GetValues(arr1[loop1]);
                for (loop2 = 0; loop2 < arr2.Length; loop2++)
                {
                    Response.Write(ScTool.Pre("Value " + loop2 + ": " + Server.HtmlEncode(arr2[loop2])));
                }
            }

            //Label1.Text = Request.QueryString["param"];
            //Label1.Text = Request.Params.Get("HTTP_HOST");

            //Label1.Text = Request.Url.ToString();
            //Label1.Text = Request.RawUrl;
            //Label1.Text = Request.Url.DnsSafeHost;
        }
    }
}