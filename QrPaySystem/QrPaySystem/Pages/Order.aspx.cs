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
    /// 创建订单请求页
    /// </summary>
    public partial class Order : BaseWebPage
    {
        public static string StaticParam = "";  // 静态参数

        public string PayType = "";   // 创建订单请求类型Ali、Wechat

        DataBase DB = null;     // 本地数据库连接对象
        string TAB = ScTool.ORDER;   // 订单表名称

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

        /// <summary>
        /// 载入后执行参数对应的sql请求
        /// </summary>
        public override void Load(object sender, EventArgs e)
        {
            if (UserType == 0 && (param.Equals("") || (param.Contains("ShowOrder") && !param.Contains("OrderSuccess")))) 
                Response.Redirect("../PayFor/UserLogin.aspx");

            Dictionary<string, string> staticPramsDic = null;   // 静态参数字典

            // 若存在静态参数，则使用静态参数
            if (!StaticParam.Equals(""))
            {
                staticPramsDic = ScTool.ToParamsDic(StaticParam);
                StaticParam = "";
            }

            //PayType = ScTool.PayTypeAli;

            string type = Request["PayType"];
            if (staticPramsDic != null && staticPramsDic.ContainsKey("PayType")) type = staticPramsDic["PayType"];
            if (type != null && !type.Equals("")) PayType = type;


            if (PayType.Equals(""))
            {
                if (staticPramsDic == null) NoteInfo(); // 从Pay.aspx Transfer过来，不显示接口说明信息
                else
                {
                    Response.Write("扫码客户端类型未知，创建订单失败！<br/>请使用支付宝或微信扫码支付");
                }

                // 显示订单信息
                string showOrder = Request["ShowOrder"];
                if (staticPramsDic != null && staticPramsDic.ContainsKey("ShowOrder")) showOrder = staticPramsDic["ShowOrder"];
                if (showOrder != null && !showOrder.Equals("") )
                {
                    if (showOrder.Equals(ScTool.PayTypeAli) || showOrder.Equals(ScTool.PayTypeWechat))
                    {
                        DB = new DataBase(ScTool.DBName(showOrder), ScTool.UserName, ScTool.Password);

                        // 修改指定的订单为支付成功
                        string orderId = Request["OrderSuccess"];
                        if (staticPramsDic != null && staticPramsDic.ContainsKey("OrderSuccess")) orderId = staticPramsDic["OrderSuccess"];
                        if (orderId != null && !orderId.Equals(""))
                        {
                            string result = "fail";
                            if (UserType == 2) result = OrderSuccess(DB, orderId);
                            //else if (UserType == 1) result = OrderSuccess(DB, orderId, UserAccount);
                            else if (UserType == 1) result = "false no allowed!";

                            Response.Write(ScTool.Pre("OrderSuccess->" + orderId + "： " + result));
                        }

                        // 显示指定类型的订单信息
                        if (UserType == 2) ScTool.showTable(this.Controls, DB, ScTool.ORDER);
                        else if (UserType == 1)
                            ScTool.showTable(this.Controls, DB, ScTool.ORDER, "where ext like '%" + "author(" + UserAccount + ")" + "%' ");

                    }
                }
            }
            else
            {
                
                //if (!ContainsAuthorInfo()) return;

                // 连接指定的数据库，若不存在则创建
                DB = new DataBase(ScTool.DBName(PayType), ScTool.UserName, ScTool.Password);
                //DB.DeletTable(ScTool.ORDER);
                confirmOrderTab();

                //统计金额总数，指定是否清空
                string ParamClear = Request["MoneyAllClear"];
                if (ParamClear != null)
                {
                    bool clear = ParamClear.Trim().ToLower().Equals("true");
                    if (clear)
                    {
                        string result = MoneyAllClear(DB, UserAccount);
                        Response.Write(result);
                    }
                    else
                    {
                        String SuccessValue = ParamClear.Equals("TrueFinish") ? ParamClear : "";
                        string MoneyCount = MoneyAll(DB, UserAccount, SuccessValue);
                        Response.Write(MoneyCount);
                    }

                    return;
                }


                // 解析创建订单参数信息
                string machinCode = Request["machinCode"];
                if (staticPramsDic != null && staticPramsDic.ContainsKey("machinCode")) machinCode = staticPramsDic["machinCode"];
                if (machinCode == null) machinCode = "";

                string soft = Request["soft"];
                if (staticPramsDic != null && staticPramsDic.ContainsKey("soft")) soft = staticPramsDic["soft"];
                if (soft == null) soft = "";

                string product = Request["product"];
                if (staticPramsDic != null && staticPramsDic.ContainsKey("product")) product = staticPramsDic["product"];
                if (product == null) product = "";

                string money = Request["money"];
                if (staticPramsDic != null && staticPramsDic.ContainsKey("money")) money = staticPramsDic["money"];

                string ext = Request["ext"];
                if (staticPramsDic != null && staticPramsDic.ContainsKey("ext")) ext = staticPramsDic["ext"];
                if (ext == null) ext = "";

                string preOrderId = Request["preOrderId"];
                if (staticPramsDic != null && staticPramsDic.ContainsKey("preOrderId")) preOrderId = staticPramsDic["preOrderId"];
                if (preOrderId == null) preOrderId = "";


                #region 同一预下单号不再多次创建订单

                String keyOrderId = "O_Id";
                String keyLink = "O_Link";

                // 若订单号不存在，或不相同则重新记录; 并清空链接地址信息
                if (Session[keyOrderId] == null || !(Session[keyOrderId] as string).Equals(preOrderId))
                {
                    Session[keyOrderId] = preOrderId;
                    Session[keyLink] = null;
                }

                if (Session[keyLink] != null && !(Session[keyLink] as string).Equals(""))
                {
                    string link = (Session[keyLink] as string);
                    Response.Redirect(link);       // 直接重定向到二维码对应链接
                    return;
                }
                #endregion


                // 支付金额不可为空,根据给定的参数创建订单
                if (money != null && !money.Equals(""))
                {
                    string id = createOrder(machinCode, soft, product, money, ext, preOrderId);
                    if (!id.Equals(""))
                    {
                        string link = Cashier.GetQrLink(DB, money, id);
                        if (!link.Equals(""))
                        {
                            if (!preOrderId.Equals("")) // 设置预下单为扫码成功
                            {
                                DataBase preDB = new DataBase(ScTool.DBName("pre"), ScTool.UserName, ScTool.Password);
                                Pay.SetScanSuccess(preDB, preOrderId, PayType);
                            }

                            // 若若链接信息不存在，或不相同则重新记录
                            if (Session[keyLink] == null || !(Session[keyLink] as string).Equals(link))
                            {
                                Session[keyLink] = link;
                            }

                            Response.Redirect(link);   // 重定向到二维码对应链接
                        }
                        else
                        {
                            deletOrder(id);
                            Response.Write(ScTool.Alert("服务器忙或不支持该金额，请稍候再试! Order.aspx"));
                        }
                    }
                    else Response.Write("创建订单失败! Order.aspx");

                }
                else Response.Write("创建订单金额为空! Order.aspx");

                //List<String> names = DB.DataBaseNames();    // 服务器中所有数据库名称
                //List<String> names2 = DB.TableNames();      // 当前数据库中所有表名称

                //DB.DeletDataBase("PayDataBase");
                //List<String> names3 = DB.DataBaseNames();    // 服务器中所有数据库名称

            }
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
            builder.AppendLine("创建订单示例：\t" + url + "PayType=Ali&machinCode=机器码1&soft=easyIcon软件&product=注册0.1元&money=0.1&ext=" + author + "&preOrderId=");
            builder.AppendLine("查询订单信息：\t" + url + "ShowOrder=Ali");
            if (UserType == 2) builder.AppendLine("指定订单ID成功：\t" + url + "ShowOrder=Ali" + "&OrderSuccess=100");
            builder.AppendLine("统计成功金额，是否清空：\t" + url + "PayType=Ali" + "&MoneyAllClear=false");

            Response.Write(ScTool.Pre(builder.ToString()));
        }


        /// <summary>
        /// 确保数据库中，存在订单信息表，若不存在则创建
        /// </summary>
        private void confirmOrderTab()
        {
            // 创建数据表
            if (!DB.ExistTab(TAB))
            {
                Dictionary<string, int> ColumnInfo = new Dictionary<string, int>();
                ColumnInfo.Add("machinCode", 50);
                ColumnInfo.Add("soft", 200);
                ColumnInfo.Add("product", 200);
                ColumnInfo.Add("money", 30);
                ColumnInfo.Add("ext", 200);
                ColumnInfo.Add("dateTime", 20);
                ColumnInfo.Add("isSuccess", 10);
                ColumnInfo.Add("preOrderId", 30);

                DB.CreateTable(TAB, ColumnInfo);
            }
        }

        /// <summary>
        /// 创建订单，在数据库中记录当前订单信息
        /// </summary>
        /// <param name="machinCode"></param>
        /// <param name="soft"></param>
        /// <param name="product"></param>
        /// <param name="money"></param>
        /// <param name="ext"></param>
        /// <returns></returns>
        private string createOrder(string machinCode, string soft, string product, string money, string ext, string preOrderId)
        {
            // 添加新的数据
            List<string> values = new List<string>();

            values.Add(machinCode);
            values.Add(soft);
            values.Add(product);
            values.Add(money);
            values.Add(ext);
            values.Add(DateTime.Now.ToString("yyyy-MM-dd_HH:mm:ss"));
            values.Add("False");
            values.Add(preOrderId);

            string id = DB.InsetValue(TAB, values);
            return id;
        }


        /// <summary>
        /// 删除指定的订单
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        private bool deletOrder(string orderId)
        {
            return DB.DeletValue(TAB, orderId, "ID");
        }

        /// <summary>
        /// 设置对应的订单号为支付成功
        /// </summary>
        /// <param name="DB"></param>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public static string OrderSuccess(DataBase DB, string orderId, String Author = "")
        {
            Dictionary<string, string> Dic = DB.SelectValue(ScTool.ORDER, orderId, "ID").RowDic();
            if (!Dic.ContainsKey("ext")) return "fail";

            string isSuccess = Dic["isSuccess"];        // 是否已支付成功
            if (isSuccess != null && isSuccess.Contains("True")) return "success";  // 已成功则不再操作

            if (!Author.Equals("")) 
            {
                string ext = Dic["ext"];                // ext
                if (!ext.Contains("author(" + Author + ")")) return "fail";
            }

            // 设置当前订单为支付成功
            Dictionary<string, string> datas = new Dictionary<string,string>();
            datas.Add("isSuccess", "True");
            string result = DB.UpdateValue(ScTool.ORDER, orderId, datas, "ID");

            // 记录preDB中的Order表信息为支付成功
            if (result.Equals("success"))
            {
                if (Dic.Count > 0 && Dic.ContainsKey("preOrderId"))
                {
                    string preOrderId = Dic["preOrderId"];  // 预下单订单号
                    string machinCode = Dic["machinCode"];  // 机器码
                    string soft = Dic["soft"];              // 软件名称

                    DataBase preDB = new DataBase(ScTool.DBName("pre"), ScTool.UserName, ScTool.Password);

                    result = OnlineSerial.GenOnlineSerial(machinCode, soft, preDB);
                    OnlineCode.Add(preDB, OnlineCode.TAB, machinCode, soft, Dic["ext"]);     // 记录支付成功的软件信息至数据表中

                    result = Pay.OrderSuccess(preDB, preOrderId);
                }
            }

            return result;
        }


        /// <summary>
        /// 统计所有状态为success=True的订单，的money总额
        /// </summary>
        /// <param name="DB"></param>
        /// <param name="Author"></param>
        public static string MoneyAll(DataBase DB, String Author = "", String SuccessValue = "")
        {
            if (Author == null || Author.Equals("")) return "0";


            //"select * from [Order] where isSuccess='True' and ext like '%author(test852)%'
            string AppendCondition = "and ext like '%" + "author(" + Author + ")" + "%' ";
            if (SuccessValue.Equals("")) SuccessValue = "True";
            List<String> list = DB.SelectValue(ScTool.ORDER, SuccessValue, "isSuccess", null, null, AppendCondition).ColmnList("money");

            double moneyAll = 0;
            foreach(string moneyStr in list)
            {
                try
                {
                    double money = Double.Parse(moneyStr);
                    moneyAll += money;
                }
                catch(Exception ){}
            }
            return moneyAll.ToString();
        }

        /// <summary>
        /// 设置指定的Author为Finish状态
        /// </summary>
        public static string MoneyAllClear(DataBase DB, String Author = "")
        {
            if (Author == null || Author.Equals("")) return "success";

            Dictionary<string, string> datas = new Dictionary<string,string>();
            datas.Add("isSuccess", "TrueFinish");

            string AppendCondition = "and ext like '%" + "author(" + Author + ")" + "%' ";
            string result = DB.UpdateValue(ScTool.ORDER, "True", datas, "isSuccess", AppendCondition);

            return result;
        }

        //protected void Button1_Click(object sender, EventArgs e)
        //{
        //    DataBase DB = new DataBase("myDataBase", "sa", "Sa12345789");   // 连接指定的数据库，若不存在则创建
        //    //Label3.Text = DB.isInitSuccess.ToString();

        //    List<String> names = DB.DataBaseNames();    // 服务器中所有数据库名称
        //    List<String> names2 = DB.TableNames();      // 当前数据库中所有表名称

        //    //newDataBase1.DeletDataBase("newDataBase3");
        //    //DB.DeletDataBase("");

        //    // 创建数据表
        //    if (!DB.ExistTab("Orders"))
        //    {
        //        List<string> list = new List<string>();
        //        list.Add("日期");
        //        list.Add("软件名称");
        //        list.Add("机器码");
        //        list.Add("金额");
        //        DB.CreateTable("Orders", list);
        //    }

        //    // 添加新的数据
        //    List<string> values = new List<string>();
        //    values.Add(DateTime.Now.ToString("yyyy-MM-dd_HH:mm:ss"));
        //    values.Add("软件1");
        //    values.Add("XXXXX");
        //    values.Add("1");
        //    string id = DB.InsetValue("Orders", values);

        //    // 修改数据
        //    Dictionary<string, string> datas = new Dictionary<string, string>();
        //    datas.Add("软件名称", "软件" + id);
        //    datas.Add("机器码", "XXXXX" + id);
        //    DB.UpdateValue("Orders", id, datas);

        //    // 查询数据
        //    string data = DB.SelectValue("Orders", id);
        //    //Label3.Text = data;

        //    // 删除指定数据
        //    DB.DeletValue("Orders", id);


        //    //DB.CreateTable("测试表", new List<string>());
        //    //DB.InsetValue("测试表", (new string[] { "key1", "value1"}).ToList());
        //    //DB.DeletTable("测试表");

        //    //String name = names[0];
        //}

    }
}