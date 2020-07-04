using QrPaySystem.Tool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace QrPaySystem.Pages
{
    /// <summary>
    /// 在iframe中打开当前页面；
    /// 等待若干秒；
    /// 跳转至新的页面。
    /// </summary>
    public partial class CWJ : System.Web.UI.Page
    {
        string Jumpto = "";
        string CurOpen = "";
        int WaitSecond = 3;    

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack) ScTool.RecordUserAgent(Request);    // 记录客户端信息

            CurOpen = Request["CurOpen"];// 当前打开
            Jumpto = Request["Jumpto"];  // 跳转至
            string value = Request["WaitSecond"];
            WaitSecond = value == null ? 5 : Int32.Parse(value); // 等待跳转延时

            if (CurOpen == null) CurOpen = "https://www.baidu.com/";
            if (Jumpto == null) Jumpto = "https://fanyi.baidu.com/";

            CurOpen = "https://qr.alipay.com/c1x01990gbhjvuvwaxwkqa3";
            Timer1.Enabled = false;

            if (Request["HideFloat"] != null) DivFloat.Visible = false;
            //Response.Write("<script>window.open('https://www.baidu.com/','_blank')</script>");

            //Timer Time2 = new Timer();
            //Time2.Interval = 2000;

            //Time2.Tick += Timer1_Tick;
            //Time2.Enabled = true;

            //String  keys = Object1.Attributes.Keys.ToString();
            //string keys2 = "";
            //g("data") = "http://www.baidu.com";
            
            //Object1.data = "http://www.baidu.com";
            //Object1.data = "";

            //OpenNewPage("https://qr.alipay.com/c1x01990gbhjvuvwaxwkqa3");
            //OpenNewPage("https://qr.alipay.com/tsx031041ajtuiviwd978b6");
            //CloseThisPage();
            
            //OpenNewPage("https://fanyi.baidu.com/");

            
            //string content = "<object ID=\"Object1\" data=\"https://www.baidu.com/\" height=\"300\" type=\"text/x-scriptlet\" width=\"100%\"></object>\r\n";
            //DivObjSci.InnerHtml = content;

            // 打开内嵌页面
            //iframe("https://www.baidu.com/");
            //iframe(CurOpen);
        }

        //protected void Button1_Click(object sender, EventArgs e)
        //{
        //    //Response.Write("<script>window.open('~/FileView.aspx','_blank')</script>");
        //    //Response.Write("<script>window.open('https://www.baidu.com/','_blank')</script>");

        //    //Response.Redirect("https://www.baidu.com/");
        //    //Response.Redirect("https://fanyi.baidu.com/");

        //    //window 
        //    //OpenNewPage("https://www.baidu.com/");
        //    //OpenNewPage("https://fanyi.baidu.com/");

        //    //CloseThisPage();
        //    //Response.Write("<script>window.close();</script>");// 会弹出询问是否关闭
        //    //Response.Write("<script>window.opener=null;window.close();</script>");// 不会弹出询问
        //}

        protected void Timer1_Tick(object sender, EventArgs e)
        {
            int count = Int32.Parse(LabelNote.Text.Trim());
            LabelNote.Text = (count + 1) + "";

            if (count == WaitSecond)
            {
                //Response.Redirect("https://qr.alipay.com/tsx031041ajtuiviwd978b6");
                Response.Redirect(Jumpto);
                Timer1.Enabled = false;
            }
            //else if (count == 1)
            //{
                
                //string content = "<object ID=\"Object1\" data=\"https://www.baidu.com/\" height=\"1280\" type=\"text/x-scriptlet\" width=\"100%\"></object>";

                //Response.Write("<script>document.getElementById(\"DivObjSci\").innerHTML='" + content + "'</script>");

                //Response.Write("<object ID=\"Object1\" data=\"https://www.baidu.com/\" height=\"1280\" type=\"text/x-scriptlet\" width=\"100%\"></object>");
            //}
            //if (count == 5) Response.Write("<script>window.open('https://www.baidu.com/','_blank')</script>");
            //if (count == 10) OpenNewPage("https://www.baidu.com/");
            //else if (count == 2) OpenNewPage("https://fanyi.baidu.com/");
            //else if (count > 2) CloseThisPage();
        }

        /// <summary>
        /// 打开信息url页面
        /// </summary>
        /// <param name="url"></param>
        private void OpenNewPage(string url)
        {
            Response.Write("<script>window.open('" + url + "','_blank')</script>");
        }


        private void CloseThisPage()
        {
            Response.Write("<script>window.opener=null;window.close();</script>");
        }

        /// <summary>
        /// 以iframe的形式，在当前页面中展示指定的网页
        /// </summary>
        private void iframe(string url = "https://www.baidu.com/", int height = 1280)
        {
        //    <div id="DivObjSci" runat="server"  >
        //    </div>

            string content = "<object ID=\"Object1\" data=\"" + url + "\" height=\"" + height + "\" type=\"text/x-scriptlet\" width=\"100%\" frameborder=\"0\" ></object>\r\n";

            DivObjSci.InnerHtml = content;
        }
    }
}