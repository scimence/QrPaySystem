using QrPaySystem.Tool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace QrPaySystem.Pages
{
    /// <summary>
    /// 扫码支付前，预下单参数存储；
    /// 扫码后，根据对应的支付类型、和支付参数跳转对应的支付
    /// </summary>
    public partial class Pay : BaseWebPage
    {
        DataBase DB;
        string TYPE = "";

        ///// <summary>
        ///// 判断ext中是否含有开发者帐号信息
        ///// </summary>
        ///// <returns></returns>
        //private bool ContainsAuthorInfo()
        //{
        //    String orderExt = Request["ext"];
        //    bool ContainsAuthor = (orderExt != null && orderExt.Contains("author(" + UserAccount + ")"));

        //    if (!ContainsAuthor) Response.Write("创建订单时需包含开发者信息" + "ext=author(" + UserAccount + ")author");
        //    return ContainsAuthor;
        //}

        string SessionKey = "Pay_ResData";

        /// <summary>
        /// 载入后执行参数对应的sql请求
        /// </summary>
        public override void Load(object sender, EventArgs e)
        {
            if (UserType == 0 && param.Equals("")) Response.Redirect("../PayFor/UserLogin.aspx");

            DB = new DataBase(ScTool.DBName("pre"), ScTool.UserName, ScTool.Password);

            if (Page.IsPostBack)
            {
                //Timer1.Enabled = true;
                return;
            }

            body1.Visible = false;

            TYPE = Request["TYPE"];
            if (TYPE == null) TYPE = "";

            if (param.Contains("TYPE"))
            {
                int index = param.IndexOf("TYPE");
                int start = param.IndexOf("&", index + 4);
                param = param.Substring(start + 1);
            }

            //DB.DeletTable(ScTool.ORDER);
            confirmOrderTab();

            //DB = new DataBase("master", ScTool.UserName, ScTool.Password);
            //DB.DeletDataBase(ScTool.DBName("pre"));
            //DB.DeletDataBase(ScTool.DBName(ScTool.PayTypeAli));
            //DB.DeletDataBase(ScTool.DBName(ScTool.PayTypeWechat));

            if (TYPE.Equals("PreOrder"))            // 生成新的pre订单
            {
                //if (!ContainsAuthorInfo()) return;
                this.Controls.Clear();

                string id = createOrder(param);
                Response.Write(ScTool.Reslut(id));
                return;
            }
            else if (TYPE.Equals("ShowPreOrder"))   // 显示预备下单订单号，对应的二维码待支付页面
            {
                string preOrderId = Request["preOrderId"];

                LabelPreOrderId.Text = "当前订单号：" + preOrderId;
                LabelPreOrderPrice.Text = getNeedPayMoney(preOrderId);

                if (preOrderId != null && !preOrderId.Equals(""))
                {
                    string data = url + "?" + "TYPE=" + "CreateOrder" + "&" + "preOrderId=" + preOrderId;
                    showQR(data);
                }
                else Response.Write("支付失败！Pay.aspx创建订单失败");
            }
            else if (TYPE.Equals("OrderResult"))    // 查询指定订单支付结果http://60.205.185.168:8002/pages/pay.aspx?TYPE=OrderResult&machinCode=J78Z-A906-E9ST-RING
            {
                this.Controls.Clear();

                string order_param = Request["P"];
                if (order_param == null || order_param.Equals(""))
                {
                    string result = OrderResult(Request["preOrderId"]); // 通过订单号查询支付结果
                    Response.Write(ScTool.Reslut(result));
                }
                else
                {
                    string result = OrderResult_param(order_param);     // 通过订单参数查询支付结果
                    Response.Write(ScTool.Reslut(result));
                }
                return;
            }
            else if (TYPE.Equals("OrderResultX"))
            {
                this.Controls.Clear();

                if (Request["preOrderId"] != null)
                {
                    string result = OrderResult(Request["preOrderId"], false); // 通过订单号查询支付结果
                    Response.Write(ScTool.Reslut(result));
                }
                else
                {
                    string machinCode = Request["machinCode"];
                    string soft = Request["soft"];

                    string result = OnlineCode.Check(DB, OnlineCode.TAB, machinCode, soft);     // 查询指定的机器码对应订单是否已支付
                    Response.Write(ScTool.Reslut(result));
                }
                return;
            }
            else if (TYPE.Equals("CreateOrder"))    // 使用预下单信息，创建正式订单
            {
                string preOrderId = Request["preOrderId"];
                if (preOrderId != null && !preOrderId.Equals(""))
                {
                    Dictionary<string, string> row = DB.SelectValue(ScTool.ORDER, preOrderId, "ID").RowDic();
                    if (row.Count > 0 && row.ContainsKey("param"))
                    {
                        string oparam = row["param"];
                        string isSuccess = row["isSuccess"].ToLower();

                        if (isSuccess.Equals("true"))
                        {
                            Response.Write(ScTool.Alert("订单" + preOrderId + "已支付成功！"));
                        }
                        else
                        {
                            // 获取客户端类型
                            string agent = Request.Params.Get("HTTP_USER_AGENT");
                            string payType = ScTool.payType(agent);

                            // 创建订单时，指定类型
                            if (payType.Equals(""))
                            {
                                string payTypeParam = Request["PayType"];
                                if (payTypeParam != null && (payTypeParam.Equals("Ali") || payTypeParam.Equals("Wechat")))
                                {
                                    payType = payTypeParam;
                                }
                            }

                            //param = HttpUtility.UrlDecode(param);
                            //Response.Redirect("Order.aspx?" + "PayType=" + payType + "&" + param);

                            Order.StaticParam = "PayType=" + payType + "&" + oparam + "&preOrderId=" + preOrderId;
                            Server.Transfer("Order.aspx");
                        }
                    }
                    else Response.Write(ScTool.Alert("订单" + preOrderId + "不存在！请重新下单"));
                }
            }
            else
            {
                if (!param.Equals(""))
                {
                    //if (!ContainsAuthorInfo()) return;

                    //param = HttpUtility.UrlEncode(param);
                    string preOrderId = createOrder(param);
                    LabelPreOrderId.Text = "当前订单号：" + preOrderId;

                    string needMoney = Request["money"];
                    if(needMoney != null) 
                    {
                        LabelPreOrderPrice.Text = "待支付金额：" + needMoney + " 元";
                    }
                    
                    if (!preOrderId.Equals(""))
                    {
                        string data = url + "?" + "TYPE=" + "CreateOrder" + "&" + "preOrderId=" + preOrderId;
                        //Response.Redirect("QRTool.aspx?" + data);

                        //QRTool.StaticParam = data;
                        //Server.Transfer("QRTool.aspx");

                        showQR(data);
                    }
                    else Response.Write("支付失败！Pay.aspx创建订单失败");
                }
                else
                {
                    if (UserType == 2) ScTool.showTable(this.Controls, DB, ScTool.ORDER);
                    else if (UserType == 1)
                        ScTool.showTable(this.Controls, DB, ScTool.ORDER, "where param like '%" + "author(" + UserAccount + ")" + "%' ");
                    NoteInfo();
                }
            }
        }

        /// <summary>
        /// 获取预下单信息中的待支付金额信息
        /// </summary>
        private string getNeedPayMoney(string preOrderId)
        {
            string msg = "";
            Dictionary<string, string> row = DB.SelectValue(ScTool.ORDER, preOrderId, "ID").RowDic();
            if (row.Count > 0 && row.ContainsKey("param"))
            {
                string oparam = row["param"];
                bool isSuccess = row["isSuccess"].Trim().ToLower().Equals("true");

                msg = (isSuccess ? "已" : "待") + "支付金额：" + getParamMoney(oparam) + " 元";
            }
            return msg;
        }

        //&money=0.01&ext=
        /// <summary>
        /// 解析param中金额信息
        /// </summary>
        private string getParamMoney(string param)
        {
            int s = param.IndexOf("&money=");
            if (s == -1) return "（金额有误）";
            else s = s + "&money=".Length;

            int e = param.IndexOf("&", s);

            string money = param.Substring(s, e - s);
            return money;
        }

        // 在页面展示生成的二维码
        private void showQR(string data)
        {
            body1.Visible = true;

            string NAME = QRTool.GetQR(data);                         // 生成二维码
            Image2.ImageUrl = "~/tools/QRTool/QR/" + NAME;            // 显示二维码图像

            Timer1.Enabled = true;  // 启动Timer,定时刷新查询支付结果
        }

        /// <summary>
        /// 接口使用说明信息
        /// </summary>
        private void NoteInfo()
        {
            String author = "author(" + UserAccount + ")author";// ";备注信息";
            String url = "http://" + Request.Params.Get("HTTP_HOST") + "/" + this.GetType().Name.Replace("_", "/").Replace("/aspx", ".aspx") + "?";
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("接口使用说明：");
            builder.AppendLine("支付类型参数：PayType = Ali、Wechat");
            builder.AppendLine("");
            builder.AppendLine("请求支付，不获取订单号：\t" + url + "machinCode=机器码1&soft=easyIcon软件&product=注册0.1元&money=0.1&ext=" + author + ";其它信息");
            builder.AppendLine("查询订单支付结果：\t" + url + "TYPE=OrderResult&P=请求支付的参数信息");
            builder.AppendLine("");
            builder.AppendLine("预下单，获取订单号：\t" + url + "TYPE=PreOrder&machinCode=机器码1&soft=easyIcon软件&product=注册0.1元&money=0.1&ext=" + author + ";其它信息");
            builder.AppendLine("显示，预下单二维码：\t" + url + "TYPE=ShowPreOrder&preOrderId=100");
            builder.AppendLine("查询订单支付结果：\t" + url + "TYPE=OrderResult&preOrderId=100");
            builder.AppendLine("查询订单支付结果,不加密：\t" + url + "TYPE=OrderResultX&preOrderId=100");
            builder.AppendLine("查询订单支付结果,不加密：\t" + url + "TYPE=OrderResultX&machinCode=机器码1&soft=可为空");
            
            Response.Write(ScTool.Pre(builder.ToString()));

        }

        /// <summary>
        /// 从参数生成待支付参数信息
        /// </summary>
        public static string PayParams(string machinCode, string soft, string moneyYuan, string author=null, string productName=null, string reserve=null)
        {
            if (author == null) author = "scimence";
            if (productName == null) productName = "";
            if (reserve == null) reserve = "";

            string ext = "author(" + author + ")author;";
            if(!reserve.Equals("")) ext = ext+ "reserve(" + reserve + ")reserve";
            string paramsData = "machinCode=" + machinCode + "&soft=" + soft + "&product=" + productName + "&money=" + moneyYuan + "&ext=" + ext;

            return paramsData;
        }

        /// <summary>
        /// 确保数据库中，存在订单信息表，若不存在则创建
        /// </summary>
        private void confirmOrderTab()
        {
            // 创建数据表
            if (!DB.ExistTab(ScTool.ORDER))
            {
                Dictionary<string, int> ColumnInfo = new Dictionary<string,int>();
                ColumnInfo.Add("param", 300);
                ColumnInfo.Add("dateTime", 20);
                ColumnInfo.Add("isSuccess", 10);
                ColumnInfo.Add("ext", 100);

                DB.CreateTable(ScTool.ORDER, ColumnInfo);
            }
        }

        /// <summary>
        /// 创建订单，在数据库中记录当前订单信息
        /// </summary>
        /// <param name="param"></param>
        /// <param name="dateTime"></param>
        /// <param name="isSuccess"></param>
        /// <param name="ext"></param>
        /// <returns></returns>
        private string createOrder(string pram)
        {
            // 添加新的数据
            List<string> values = new List<string>();
            values.Add(pram);
            values.Add(ScTool.Date());
            values.Add("False");
            values.Add("");

            string id = DB.InsetValue(ScTool.ORDER, values);
            return id;
        }

        /// <summary>
        /// 设置对应的订单号为支付成功
        /// </summary>
        /// <param name="DB"></param>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public static string OrderSuccess(DataBase DB, string orderId)
        {
            Dictionary<string, string> datas = new Dictionary<string, string>();
            datas.Add("isSuccess", "True");

            string result = DB.UpdateValue(ScTool.ORDER, orderId, datas, "ID");
            return result;
        }

        /// <summary>
        /// 检测指定的订单是否支付成功
        /// </summary>
        /// <param name="DB"></param>
        /// <param name="orderId">预下单订单号</param>
        /// <returns></returns>
        public static bool CheckSuccess(DataBase DB, string orderId)
        {
            Dictionary<string, string> Dic = DB.SelectValue(ScTool.ORDER, orderId, "ID").RowDic();
            if (Dic.Count > 0 && Dic.ContainsKey("isSuccess"))
            {
                string result = Dic["isSuccess"].ToLower();
                return result.Equals("true");
            }
            return false;
        }

        /// <summary>
        /// 检测指定的machinCode、soft，是否已支付成功。
        /// 需在指定的下单表中进行查询
        /// </summary>
        /// <param name="DB"></param>
        /// <param name="machinCode">支付时使用的机器码</param>
        /// <param name="soft">软件名称</param>
        public static bool CheckSuccess(DataBase DB, string machinCode, string soft=null)
        {
            Dictionary<string, string> ortherConditions = new Dictionary<string, string>();
            if (soft != null && !soft.Equals("")) ortherConditions.Add("soft", soft);
            string AppendCondition = "and isSuccess like '%True%'";

            Dictionary<string, string> Dic = DB.SelectValue(ScTool.ORDER, machinCode, "machinCode", null, ortherConditions, AppendCondition).RowDic();
            return (Dic.Count > 0);
        }

        /// <summary>
        /// 当订单支付成功时（若当前订单对应）
        /// </summary>
        private void CheckShow_ProductData(DataBase DB, string orderId)
        {
            Dictionary<string, string> Dic = DB.SelectValue(ScTool.ORDER, orderId, "ID").RowDic();
            if (Dic.Count > 0 && Dic.ContainsKey("param") && Dic.ContainsKey("isSuccess"))
            {
                bool isSuccess = Dic["isSuccess"].Trim().ToLower().Equals("true");
                if (isSuccess)
                {
                    //Response.Write(ScTool.Alert("isSuccess"));

                    string param = Dic["param"].Trim();                     // machinCode=ProductInfo100&soft=付费资源&product=资源xx测试&money=0.10&ext=author(scimence)author;
                    string strS = "machinCode=ProductInfo";

                    if (param.StartsWith(strS) && param.Contains("&"))      // 以该字段开头则为付费资源，ProductInfo.aspx -> CreateOrder
                    {
                        int start = param.IndexOf(strS) + strS.Length;
                        int end = param.IndexOf("&", start);
                        string resId = param.Substring(start, end - start).Trim();   // 获取对应的资源id

                        //Response.Write(ScTool.Alert("resId -> " + resId));

                        // 查询对应的资源数据
                        string InfoUrl = "http://" + Request.Params.Get("HTTP_HOST") + "/Pages/Productinfo.aspx";
                        String url = InfoUrl + "?" + "TYPE=Select&ID=" + resId + "&key=data";
                        string data = ScTool.getWebData(url);     // Result(100)Result

                        LabelTipInfo.Text = data;

                        //Response.Write(ScTool.Alert(data));

                        // 展示资源对应数据
                        //ShowInfo(data);
                    }
                }
            }
        }

        ///// <summary>
        ///// 在网页中显示数据data
        ///// </summary>
        //private void ShowInfo(string data)
        //{
        //    //string ShowInfoUrl = "http://" + Request.Params.Get("HTTP_HOST") + "/" + "PageHB/ShowInfo.aspx?p=" + data;
        //    string ShowInfoUrl = "../PageHB/ShowInfo.aspx?p=" + data;
        //    if (data.ToLower().StartsWith("http://") || data.ToLower().StartsWith("https://")) ShowInfoUrl = data;
        //    Response.Redirect(ShowInfoUrl);
        //}


        /// <summary>
        /// 检测指定的订单是否扫码成功
        /// </summary>
        /// <param name="DB"></param>
        /// <param name="orderId">预下单订单号</param>
        /// <returns></returns>
        public static bool ScanSuccess(DataBase DB, string orderId)
        {
            List<string> column = new string[] { "ext" }.ToList();
            string ext = DB.SelectValue(ScTool.ORDER, orderId, "ID", column).FirstData();
            return ext.Contains("#ScanSuccess#");
        }

        protected void Timer1_Tick(object sender, EventArgs e)
        {
            // 获取当前订单号
            string orderId = LabelPreOrderId.Text.Replace("：", ":");
            if (orderId.Contains(":"))
            {
                orderId = orderId.Substring(orderId.IndexOf(":") + 1);
            }

            // 显示扫码前的提示信息
            if (LabelCount.Text.Equals(""))
            {
                LabelCount.Text = (ScTool.CashierWaittingTime - 1) + "";
                LabelTipInfo.Text = "请使用支付宝，扫码支付\r\n(微信暂不可用)";
            }
            int count = Int32.Parse(LabelCount.Text);

            // 检测当前是否扫码成功
            if (LabelTipInfo.Text.Contains("扫码成功"))
            {
                LabelCount.Text = (count - 1) + "";
                LabelTipInfo.Text = "扫码成功，等待支付完成，剩余 " + count + " s";
            }
            else if (ScanSuccess(DB, orderId))
            {
                Image2.ImageUrl = "~/tools/QRTool/Icon/" + "scan_success_a.png"; // 现实支付成功二维码图像
                LabelCount.Text = (count - 1) + "";
                LabelTipInfo.Text = "扫码成功，等待支付完成，剩余 " + count + " s";

                return;
            }

            // 检测当前是否支付成功,未支付成功时检测
            if (!LabelTipInfo.Text.Contains("支付成功") && CheckSuccess(DB, orderId))
            {
                Timer1.Enabled = false;

                //body1.Visible = false;
                Image2.ImageUrl = "~/tools/QRTool/Icon/" + "pay_success_a.png"; // 显示支付成功二维码图像
                LabelTipInfo.Text = "订单：" + orderId + " 支付成功";

                CheckShow_ProductData(DB, orderId);
                return;
            }

            // 检测当前是否已超时
            if (count <= 0)
            {
                Timer1.Enabled = false;   // 提示超时后，仍然等待30秒，查询是否支付成功，若成功给出成功提示

                //Timer1.Enabled = false;

                Image2.ImageUrl = "~/tools/QRTool/Icon/" + "timeout_a.png"; // 现实支付成功二维码图像
                //LabelTipInfo.Text = "支付超时，当前订单已关闭！\n请勿再支付，可能会无法到账！";
                LabelTipInfo.Text = "支付超时，订单：" + orderId + " 已关闭！\r\n若已支付未到账，请微信通知我: scimence";

                return;
            }
        }

        /// <summary>
        /// 设置对应的订单号为扫码成功
        /// </summary>
        /// <param name="DB"></param>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public static string SetScanSuccess(DataBase DB, string preOrderId, string PayType)
        {
            // 获取订单对应的ext信息
            List<string> column = new string[] { "ext" }.ToList();
            string ext = DB.SelectValue(ScTool.ORDER, preOrderId, "ID", column).FirstData();
            if (!ext.Contains("#ScanSuccess#")) ext += "#ScanSuccess#";
            if (!ext.Contains("#" + PayType + "#")) ext += "#" + PayType + "#";

            // 设置当前订单为扫码成功
            Dictionary<string, string> datas = new Dictionary<string, string>();
            datas.Add("ext", ext);

            string result = DB.UpdateValue(ScTool.ORDER, preOrderId, datas, "ID");
            return result;
        }


        /// <summary>
        /// 检测指定的订单是否支付成功,返回信息加密
        /// </summary>
        private string OrderResult(string orderId, bool encrypte = true)
        {
            if (orderId == null || orderId.Equals("")) return "fail";

            Dictionary<string, string> Dic = DB.SelectValue(ScTool.ORDER, orderId).RowDic(); // 查询信息
            if (Dic.Count > 0)
            {
                string isSuccess = Dic["isSuccess"].ToLower();
                if (!encrypte) return isSuccess;    // 不加密，直接返回结果

                string reslut = "isSuccess(" + isSuccess + ")isSuccess";
                //reslut += "\r\n" + "param(" + Dic["param"] + ")param";

                reslut = Locker.Encrypt(reslut, Dic["ID"] + Dic["param"]);
                return reslut;
            }
            else return "fail";
        }


        /// <summary>
        /// 检测指定的参数是否支付成功（获取最后一项）,返回信息加密
        /// </summary>
        private string OrderResult_param(string param)
        {
            if (param == null || param.Equals("")) return "fail";

            Dictionary<string, string> Dic = DB.SelectValue(ScTool.ORDER, param, "param").RowDic(-2); // 查询信息
            if (Dic.Count > 0)
            {
                string isSuccess = Dic["isSuccess"].ToLower();

                string reslut = "isSuccess(" + isSuccess + ")isSuccess";
                //reslut += "\r\n" + "param(" + Dic["param"] + ")param";

                //reslut = Locker.Encrypt(reslut, Dic["param"]);
                return reslut;
            }
            else return "fail";
        }
    }
}