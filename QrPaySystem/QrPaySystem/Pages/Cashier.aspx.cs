using QrPaySystem.Tool;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace QrPaySystem.Pages
{
    public partial class Cashier : BaseWebPage
    {
        public string PayType = "Ali";   // 创建订单请求类型Ali、Wechat
        DataBase DB = null;
        public static string CASHER = ScTool.CASHER;

        string TYPE = "";   // 自定义操作类型

        ///// <summary>
        ///// 获取请求参数信息
        ///// </summary>
        //private String getParam(String LogName = "")
        //{
        //    TYPE = Request["TYPE"];


        //    String Url = Request.Url.ToString();
        //    String param = "";
        //    if (Url.Contains("?"))
        //    {
        //        param = Url.Substring(Url.IndexOf("?") + 1);                // 获取参数信息

        //        if (LogName.Equals("")) LogName = this.GetType().Name;
        //        LogTool log = new LogTool(LogName);                         // 记录至log中
        //        log.WriteLine(param);
        //    }
        //    return param;
        //}

        /// <summary>
        /// 接口使用说明信息
        /// </summary>
        private void NoteInfo()
        {
            String url = "http://" + Request.Params.Get("HTTP_HOST") + "/" + this.GetType().Name.Replace("_", "/").Replace("/aspx", ".aspx") + "?";
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("接口使用说明：");
            builder.AppendLine("支付类型参数：PayType = Ali、Wechat");
            builder.AppendLine("");
            builder.AppendLine("添加、修改qrTab：\t" + url + "TYPE=SetQrTable&ID=100&TabName=qrTab100");
            builder.AppendLine("向qrTab添加数据项：\t" + url + "TYPE=AddToQrTable&TabName=qrTab100&price=0.1&qrLink=http://www.baidu.com");
            builder.AppendLine("修改qrTab中的数据：\t" + url + "TYPE=UpdateQrTable&TabName=qrTab100&ID=100&price=0.1&qrLink=http://www.w3school.com.cn&orderId=100&isUsing=True");
            builder.AppendLine("删除表中指定值对应行：\t" + url + "TYPE=DeletInTable&TabName=qrTab100&KeyValue=104&KeyName=ID");
            builder.AppendLine("删除指定的数据表：\t" + url + "TYPE=DeletTable&TabName=qrTab110");
            builder.AppendLine("");
            builder.AppendLine("请求获取可用的qrLink：\t" + url + "TYPE=GetQrLink&price=2&orderId=1001");
            builder.AppendLine("通知指定price回调完成：\t" + url + "TYPE=PriceFinish&phoneId=681190477515911&price=2");

            Response.Write(ScTool.Pre(builder.ToString()));

            //print("接口使用说明：");
            //print("");
            //print("添加、修改qrTab：\t" + url + "TYPE=SetQrTable&ID=100&TabName=qrTab100");
            //print("向qrTab添加数据项：\t" + url + "TYPE=AddToQrTable&TabName=qrTab100&price=0.1&qrLink=http://www.baidu.com");
            //print("在qrTab中修改数据：\t" + url + "TYPE=UpdateQrTable&TabName=qrTab100&ID=100&price=0.1&qrLink=http://www.w3school.com.cn&orderId=100&isUsing=True");
            //print("删除表中指定值所有行：\t" + url + "TYPE=DeletInTable&TabName=qrTab100&KeyValue=104&KeyName=ID");

        }

        public override void Load(object sender, EventArgs e)
        {
            if (UserType == 0) Response.Redirect("../PayFor/UserLogin.aspx");
            else if (UserType == 1) Response.Redirect("../PayFor/SDK.aspx");

            if (!Page.IsPostBack) ScTool.RecordUserAgent(Request);    // 记录客户端信息

            string type = Request["PayType"];
            if (type != null && !type.Equals("")) PayType = type;

            DB = new DataBase(ScTool.DBName(PayType), ScTool.UserName, ScTool.Password);

            //getParam();     //获取参数记录log信息
            TYPE = Request["TYPE"];
            if (TYPE != null)
            {
                if (TYPE.Equals("SetQrTable")) SetQrTable(Request["ID"], Request["TabName"]);
                else if (TYPE.Equals("AddToQrTable")) AddToQrTable(DB, Request["TabName"], Request["price"], Request["qrLink"]);
                else if (TYPE.Equals("UpdateQrTable")) UpdateQrTable(Request["TabName"], Request["ID"], Request["price"], Request["qrLink"], Request["orderId"], Request["isUsing"]);
                else if (TYPE.Equals("DeletInTable")) DeletInTable(Request["TabName"], Request["KeyValue"], Request["KeyName"]);
                else if (TYPE.Equals("DeletTable")) DeletTable(Request["TabName"]);

                else if (TYPE.Equals("GetQrLink"))
                {
                    string link = GetQrLink(DB, Request["price"], Request["orderId"]);
                    Response.Write(link);
                    return;
                }
                else if (TYPE.Equals("PriceFinish"))
                {
                    string orderId = PriceFinish(DB, Request["phoneId"], Request["price"]);
                    Response.Write(orderId);
                    return;
                }
            }

            NoteInfo();


            if (!DB.ExistTab(CASHER))
            {
                Response.Write(ScTool.Pre("数据表" + CASHER + "不存在！请使用NotificationSender创建"));
            }
            else
            {
                ScTool.showTable(this.Controls, DB, CASHER);

                List<string> qrTableList = DB.ColumnList(CASHER, "qrTabName");
                foreach (string table in qrTableList)
                {
                    if (!table.Equals("")) ScTool.showTable(this.Controls, DB, table, "", false);
                }

                //Table table = DB.ExecuteTable("select * from Cahsier");
                //this.Controls.Add(table);
                //PlaceHolder_Tab.Controls.Add(table);
            }
        }


        /// <summary>
        /// 消息弹窗
        /// </summary>
        /// <param name="msg"></param>
        private void showAlert(string msg)
        {
            Response.Write("<script>alert('" + msg + "')</script>");
        }



        /// <summary>
        /// 设置二维码映射表，若不存在则创建
        /// </summary>
        /// <param name="casherId">与Carsh表对应Id关联</param>
        /// <param name="TAB">表名称</param>
        private void SetQrTable(string casherId, string TAB)
        {
            if (TAB == null || TAB.Trim().Equals(""))
            {
                showAlert("TabName与ID需一一对应，且不为空");
                return;
            }

            CreatQrTable(DB, TAB);

            // 若表已存在，则添加至Cashier中
            if (!casherId.Equals("") && DB.ExistTab(TAB))
            {
                Dictionary<string, string> datas = new Dictionary<string, string>();
                datas.Add("qrTabName", TAB);

                DB.UpdateValue(CASHER, casherId, datas, "ID");
            }
        }

        /// <summary>
        /// 创建二维码映射表
        /// </summary>
        private static void CreatQrTable(DataBase DB, string TAB)
        {
            // 若映射表不存在，则创建
            if (!DB.ExistTab(TAB))
            {
                Dictionary<string, int> ColumnInfo = new Dictionary<string, int>();

                ColumnInfo.Add("price", 50);
                ColumnInfo.Add("qrLink", 100);
                ColumnInfo.Add("orderId", 10);
                ColumnInfo.Add("isUsing", 300);     //是否正在使用
                ColumnInfo.Add("dateTime", 300);    //是否正在使用

                DB.CreateTable(TAB, ColumnInfo);
            }
        }

        /// <summary>
        /// 向TAB中添加新的数据项
        /// </summary>
        /// <param name="TAB"></param>
        /// <param name="price"></param>
        /// <param name="qrLink"></param>
        private static string AddToQrTable(DataBase DB, string TAB, string price, string qrLink, string orderId = "", string isUsing = "False")
        {
            CreatQrTable(DB, TAB);

            List<string> values = new string[] { price, qrLink, orderId, isUsing, ScTool.Date() }.ToList();
            string result = DB.InsetValue(TAB, values);
            return result;
        }

        /// <summary>
        /// 更新TAB表中指定项的数据
        /// </summary>
        /// <param name="TAB"></param>
        /// <param name="ID"></param>
        /// <param name="price"></param>
        /// <param name="qrLink"></param>
        /// <param name="orderId"></param>
        /// <param name="isUsing"></param>
        private void UpdateQrTable(string TAB, string ID, string price, string qrLink, string orderId, string isUsing)
        {
            Dictionary<string, string> datas = new Dictionary<string, string>();
            if (price != null) datas.Add("price", price);
            if (qrLink != null) datas.Add("qrLink", qrLink);
            if (orderId != null) datas.Add("orderId", orderId);
            if (isUsing != null) datas.Add("isUsing", isUsing);
            datas.Add("dateTime", ScTool.Date());   // 日期时间自动修改

            DB.UpdateValue(TAB, ID, datas, "ID");
        }


        /// <summary>
        /// 从TAB表中删除KeyName=KeyValue的所有项
        /// </summary>
        /// <param name="TAB"></param>
        /// <param name="KeyValue"></param>
        /// <param name="KeyName"></param>
        private void DeletInTable(string TAB, string KeyValue, string KeyName = "ID")
        {
            if (KeyName == null) KeyName = "ID";

            DB.DeletValue(TAB, KeyValue, KeyName);
        }

        /// <summary>
        /// 删除指定的数据表
        /// </summary>
        private void DeletTable(string TAB)
        {
            if (DB.ExistTab(TAB)) DB.DeletTable(TAB);
        }

        /// <summary>
        /// 根据price获取可用的QrLink。若获取到可用的link地址，则标记对应项为已使用，并记录orderId
        /// </summary>
        /// <param name="price"></param>
        /// <param name="orderId"></param>
        public static string GetQrLink(DataBase DB, string price, string orderId)
        {
            if (orderId == null || orderId.Equals("")) return "";

            //List<string> qrTableList = DB.ColumnList(CASHER, "qrTabName");
            // 查询当前在线的qrTable
            List<string> qrTableList = DB.SelectValue(CASHER, "True", "isOnline", new string[] { "qrTabName" }.ToList()).ColmnList();

            // 判断当前金额对应的链接地址若不存在，则添加任意金额
            foreach (string table in qrTableList)
            {
                string curPriceLink = getPriceQrlink(DB, table, price);     // 获取当前金额的链接地址
                if (curPriceLink == null)   // 若不存在，则添加任意金额
                {
                    string anyPriceLink = getPriceQrlink(DB, table, "any"); // 获取当前表中任意金额的链接地址
                    if (anyPriceLink == null) continue;
                    else
                    {
                        string result = AddToQrTable(DB, table, price, anyPriceLink, orderId);  // 若添加成功，则返回当前对应的链接
                        if (!result.Equals("")) return anyPriceLink;
                    }
                }
            }

            foreach (string table in qrTableList)
            {
                // 查询价格为price,isUsing=false的数据项
                Dictionary<string, string> condition = new Dictionary<string, string>();
                condition.Add("isUsing", "False");

                Dictionary<string, string> Dic = DB.SelectValue(table, price, "price", null, condition).RowDic();
                if (Dic.Keys.Count > 0 && Dic.ContainsKey("qrLink") && Dic.ContainsKey("ID"))
                {
                    string qrLink = Dic["qrLink"];
                    string ID = Dic["ID"];

                    // 若链接可用，则在表中标记为已使用，返回链接信息
                    if (!qrLink.Equals(""))
                    {
                        Dictionary<string, string> datas = new Dictionary<string, string>();
                        datas.Add("isUsing", "True");
                        datas.Add("orderId", orderId);
                        datas.Add("dateTime", ScTool.Date());

                        DB.UpdateValue(table, ID, datas, "ID");

                        return qrLink;
                    }
                }
            }

            //return "";
            return GetQrLink_InTimeOut(DB, price, orderId);
        }


        /// <summary>
        /// 根据price获取可用的QrLink。若获取到可用的link地址，则标记对应项为已使用，并记录orderId
        /// </summary>
        /// <param name="price"></param>
        /// <param name="orderId"></param>
        private static string GetQrLink_InTimeOut(DataBase DB, string price, string orderId)
        {
            if (orderId == null || orderId.Equals("")) return "";

            //List<string> qrTableList = DB.ColumnList(CASHER, "qrTabName");
            // 查询当前在线的qrTable
            List<string> qrTableList = DB.SelectValue(CASHER, "True", "isOnline", new string[] { "qrTabName" }.ToList()).ColmnList();

            foreach (string table in qrTableList)
            {
                // 查询价格为price,isUsing=false的数据项
                Dictionary<string, string> condition = new Dictionary<string, string>();
                condition.Add("isUsing", "True");

                double addSecond = -(ScTool.CashierWaittingTime);
                string AppendCondition = "and dateTime" + "<'" + ScTool.Date(0, 0, 0, 0, addSecond) + "'";

                Dictionary<string, string> Dic = DB.SelectValue(table, price, "price", null, condition, AppendCondition).RowDic();
                if (Dic.Keys.Count > 0 && Dic.ContainsKey("qrLink") && Dic.ContainsKey("ID"))
                {
                    string qrLink = Dic["qrLink"];
                    string ID = Dic["ID"];

                    // 若链接可用，则在表中标记为已使用，返回链接信息
                    if (!qrLink.Equals(""))
                    {
                        Dictionary<string, string> datas = new Dictionary<string, string>();
                        datas.Add("isUsing", "True");
                        datas.Add("orderId", orderId);
                        datas.Add("dateTime", ScTool.Date());

                        DB.UpdateValue(table, ID, datas, "ID");

                        return qrLink;
                    }
                }
            }
            
            return "";
        }


        /// <summary>
        /// 获取DB中数据表table上金额为price的数据项，对应的Qr地址;
        /// 返回值为null，则没有该金额
        /// </summary>
        private static string getPriceQrlink(DataBase DB, string table, string price)
        {
            Dictionary<string, string> Dic = DB.SelectValue(table, price, "price").RowDic();
            if (Dic.Keys.Count > 0 && Dic.ContainsKey("qrLink") && Dic.ContainsKey("ID"))
            {
                string qrLink = Dic["qrLink"];
                string ID = Dic["ID"];

                return qrLink;
            }
            return null;
        }

        /// <summary>
        /// 清除phoneId对应的QrTable中对金额price的占用;
        /// 返回对应的订单号
        /// </summary>
        /// <param name="phoneId"></param>
        /// <param name="price"></param>
        public static string PriceFinish(DataBase DB, string phoneId, string price)
        {
            if (phoneId == null || phoneId.Equals("")) return "";

            // 查询phoneId对应的qrTable
            List<string> qrTableList = DB.SelectValue(CASHER, phoneId, "phoneId", new string[] { "qrTabName" }.ToList()).ColmnList();
            if (qrTableList.Count > 0)
            {
                string table = qrTableList[0];

                // 查询价格为price的数据项
                Dictionary<string, string> Dic = DB.SelectValue(table, price, "price", null, null).RowDic();
                if (Dic.Keys.Count > 0 && Dic.ContainsKey("orderId") && Dic.ContainsKey("ID"))
                {
                    string ID = Dic["ID"];
                    string orderId = Dic["orderId"];

                    Dictionary<string, string> datas = new Dictionary<string, string>();
                    datas.Add("isUsing", "False");
                    datas.Add("orderId", "");
                    datas.Add("dateTime", ScTool.Date());

                    DB.UpdateValue(table, ID, datas, "ID");

                    return orderId;
                }
            }

            return "";
        }

    }
}