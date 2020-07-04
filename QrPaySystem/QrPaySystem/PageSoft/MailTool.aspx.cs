using QrPaySystem.Tool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace QrPaySystem.PageSoft
{
    // 设置发件人 http://localhost:39380/pagesoft/mailtool.aspx?from=scimence@163.com&psw=123&R=true
    // 发送邮件   http://localhost:39380/pagesoft/mailtool.aspx?to=536400495@qq.com&subject=MailTool&body=123457&send=true
    public partial class MailTool : System.Web.UI.Page
    {
        String from = "";   // 发件人邮箱
        String psw = "";    // 发件人密码

        String to = "";     // 收件人邮箱
        String subject = "";// 邮件主题
        String body = "";   // 邮件内容
        String attch = "";  // 附件

        bool showRecord = false; // 显示记录按钮

        LogTool log = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            String LogName = this.GetType().Name;   // 当前页面名称
            log = new LogTool(LogName);             // 记录至log中

            // 显示接口信息
            String url = "http://" + Request.Params.Get("HTTP_HOST") + "/" + this.GetType().Name.Replace("_", "/").Replace("/aspx", ".aspx");
            String tipInfo = "发送邮件: " + url + "?" + "from=发件人帐号&psw=密码&to=收件人帐号&subject=标题&body=内容&send=true";
            Label_TipInfo.Text = tipInfo;

            if (Request["from"] != null) TextBox_from.Text = Request["from"];
            if (Request["psw"] != null) TextBox_psw.Text = Request["psw"];
            if (Request["to"] != null) TextBox_to.Text = Request["to"];
            if (Request["subject"] != null) TextBox_subject.Text = Request["subject"];
            if (Request["body"] != null) TextBox_body.Text = Request["body"];

            if (Request["attch"] != null) attch = Request["attch"];

            //是否记录帐号密码信息
            if (Request["R"] != null) showRecord = Request["R"].Trim().ToLower().Equals("true");
            if (!showRecord) Button_record.Visible = false;
            else
            {
                if (!TextBox_psw.Text.Equals("") && !TextBox_from.Text.Equals(""))
                    Button_record_Click(null, null);
            }

            LoadRegistryInfo();

            if (Request["send"] != null)
            {
                DivPanel.Visible = false;
                Label_TipInfo.Text = "";
                CheckBox_sp.Checked = true;

                Button_send_Click(null, null);
            }
        }


        //此函数用于从注册表中获取初始化时的软件设置，设置用户名和密码
        private void LoadRegistryInfo()
        {
            if (!showRecord) return;
            if (TextBox_from.Text.Equals("") || TextBox_psw.Text.Equals(""))
            {
                Microsoft.Win32.RegistryKey keySet = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"Software\scimence\Email\Set", true);
                //设置一个具有写权限的键 访问键注册表"HKEY_CURRENT_USER\Software"
                if (keySet != null)
                {
                    TextBox_from.Text = Convert.ToString(keySet.GetValue("邮箱", ""));    //从注册表中获取用户名的值，显示在文本框中
                    TextBox_psw.Text = Convert.ToString(keySet.GetValue("密码", ""));     //获取密码

                    Button_record.Text = "清除密码";
                }
                else
                {
                    Button_record.Text = "记住密码";
                }
            }
        }

        string EncryptHead = "Eps";
        /// <summary>
        /// 记住帐号密码信息
        /// </summary>
        protected void Button_record_Click(object sender, EventArgs e)
        {
            //设置一个具有写权限的键 访问键注册表"HKEY_CURRENT_USER\Software"
            Microsoft.Win32.RegistryKey keyCur = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("Software", true);
            Microsoft.Win32.RegistryKey keySet = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"Software\scimence\Email\Set", true);

            if (Button_record.Text.Equals("记住密码"))
            {   //键不存在时创建键,创建键"HKEY_CURRENT_USER\Software\scimence\Email\Set"
                if (keySet == null) keySet = keyCur.CreateSubKey(@"Scimence\Email\Set");

                //然后将用户名和密码作为键值存储到注册表中
                keySet.SetValue("邮箱", TextBox_from.Text);

                String Epsw = TextBox_psw.Text;
                if(!Epsw.StartsWith(EncryptHead)) Epsw = EncryptHead +  Locker.Encrypt(TextBox_psw.Text, TextBox_from.Text);  // 对密码信息进行加密
                keySet.SetValue("密码", Epsw);

                Button_record.Text = "清除密码";
            }
            else
            {
                TextBox_from.Text = "";
                TextBox_psw.Text = "";

                if (keySet != null) keyCur.DeleteSubKeyTree(@"scimence\Email\Set");         //删除所有的设置信息

                Button_record.Text = "记住密码";
            }
        }

        /// <summary>
        /// 发送邮件
        /// </summary>
        protected void Button_send_Click(object sender, EventArgs e)
        {
            if (from.Equals("")) from = TextBox_from.Text;
            if (psw.Equals("")) psw = TextBox_psw.Text;
            if (psw.StartsWith(EncryptHead)) psw = Locker.Decrypt(psw.Substring(EncryptHead.Length), TextBox_from.Text);  // 对密码信息进行解密

            if (to.Equals("")) to = TextBox_to.Text;
            if (subject.Equals("")) subject = TextBox_subject.Text;
            if (subject.Equals("")) subject = DateTime.Now.ToString();

            if (body.Equals("")) body = TextBox_body.Text;


            if (!check()) return;               // 输入信息格式不正确
            bool isSp = CheckBox_sp.Checked;    // 单独发送


            bool sendResult = true;
            if (!isSp)
            {
                if (!sendEmail(from, psw, to, subject, body, attch)) sendResult = false;
            }
            else
            {
                foreach (string str0 in to.Split(';'))
                {
                    string str = str0.Trim();
                    if (!str.Equals(""))
                    {
                        if (!sendEmail(from, psw, to, subject, body, attch)) sendResult = false;
                    }
                }
            }
            Label_TipInfo.Text = sendResult ? "邮件发送成功" : ("邮件发送失败 -> " + errorInfo);

            // 记录邮件信息
            string info = "from=" + from + "&psw=" + TextBox_psw.Text + "&to=" + to + "&subject=" + subject + "&body=" + body + "&attch=" + attch + "";
            log.WriteLine(info);

            if (sendResult)
            {
                // 清空已发送信息
                to = TextBox_to.Text = "";
                subject = TextBox_subject.Text = "";
                body = TextBox_body.Text = "";
            }
        }


        /// <summary>
        /// 消息弹窗；
        /// Response.Write(string);输出显示
        /// </summary>
        /// <param name="msg"></param>
        public static string Alert(string msg)
        {
            String info = "<script>alert('" + msg + "')</script>";
            //Response.Write(info);
            return info;
        }


        //输入检验
        private bool check()
        {
            if (from.IndexOf('@') <= 0)
            {
                Label_TipInfo.Text = "发件人邮箱格式不正确！\n形如：sci@163.com";
                return false;
            }

            if (psw.Equals(""))
            {
                Label_TipInfo.Text = "请输入发件人密码";
                return false;
            }

            if (to.Trim().IndexOf('@') <= 0)
            {
                Label_TipInfo.Text = "收件人邮箱格式不正确！\n形如：sci@163.com\n多个收件人使用';'分隔";
                return false;
            }

            return true;
        }

        String errorInfo = "";

        //发送邮件
        private bool sendEmail(string from, string psw, string to, string subject, string body, string attach)
        {   //使用时先添加引用 using System.Net.Mail;
            //设置smtp
            SmtpClient client = new System.Net.Mail.SmtpClient();

            //client.DeliveryMethod = SmtpDeliveryMethod.Network;
            //client.EnableSsl = true;
            //client.UseDefaultCredentials = false;

            //client.Port = 25;
            //client.Port = 465;
            //client.Port = 994;
            //client.Port = 995;

            client.Host = "smtp." + from.Substring(from.IndexOf('@') + 1);          //根据用户账号设置邮件服务器
            client.Credentials = new System.Net.NetworkCredential(from, psw);       //设置账户密码



            //设置邮件
            MailMessage message = new MailMessage();
            message.From = new MailAddress(from);
            message.Subject = subject;
            message.Body = body;

            if (to.IndexOf(';') > 0)        //添加收件人
            {
                foreach (string str in to.Split(';'))
                    message.To.Add(str);
            }
            else if (!to.Equals(""))
                message.To.Add(to);

            if (attach.IndexOf(';') > 0)    //添加附件
            {
                foreach (string str in attach.Split(';'))
                    message.Attachments.Add(new Attachment(str));
            }
            else if (!attach.Equals(""))
                message.Attachments.Add(new Attachment(attach));

            try { client.Send(message); return true; }
            catch (Exception ex)
            {
                errorInfo = ex.ToString();
                return false; 
            }
        }


    }
}