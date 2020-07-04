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
    /// 记录和查询 商品信息
    /// </summary>
    public partial class ProductInfo : BaseWebPage
    {
        DataBase DB;
        string TAB = "ProductInfo";
        string TYPE = "";

        /// <summary>
        /// 载入后执行参数对应的sql请求
        /// </summary>
        public override void Load(object sender, EventArgs e)
        {
            DB = new DataBase(ScTool.DBName("pre"), ScTool.UserName, ScTool.Password);
            confirmOrderTab();

            TYPE = Request["TYPE"];

            if (TYPE != null)
            {
                if (TYPE.Equals("Add"))
                {
                    string result = Add(Request["name"], Request["price"], Request["author"], Request["data"], Request["ext"]);
                    Response.Write(result);
                    return;
                }
                else if (TYPE.Equals("Update")) Update(Request["ID"], Request["name"], Request["price"], Request["author"], Request["data"], Request["ext"]);
                else if (TYPE.Equals("Delet")) Delet(Request["ID"]);
                else if (TYPE.Equals("Select"))
                {
                    string result = Select(Request["ID"], Request["key"]);      // 查询ID数据项对应的key值
                    //if (Request["key"].Equals("price") || Request["key"].Equals("ext")) // 若查询金额信息，则进行加密
                    //{
                    //    result = Request["key"] + "(" + result + ")" + Request["key"];
                    //    result = Locker.Encrypt(result, Request["name"]);
                    //}
                    Response.Write(result);
                    return;
                }
                else if (TYPE.Equals("DeletThisTab")) DB.DeletTable(TAB);
                else if(TYPE.Equals("StaticIp"))
                {
                    Product.UseStaticIpMode = !Product.UseStaticIpMode;
                    Response.Write("设置付费资源，网址模式 StaticIp=" + Product.UseStaticIpMode);
                }
            }

            //--------------------------------------------------------

            string resId = Request["ID"];   // 数据项对应的资源ID
            // 1、查询当前用户对应的对于该资源是否已创建订单号，若未创建则创建
            // 2、查询该订单是否已支付，未支付则跳转支付，已支付则显示资源对应密码
            if (TYPE == null && resId != null)
            {
                if (Session.Timeout != 60 * 24) Session.Timeout = 60 * 24;   // 设置Session有效时间为24小时

                string key = "ProductInfo" + resId + "_payId";              // 按资源ID记录,对应的支付预下单号
                // string date = DateTime.Now.ToString("yyyyMMdd");        // 每天可领取一次，首次跳转
                // (Session[key] as string).Equals("");                     // 获取之前的session值

                // 1、
                string value = "";
                if (Session[key] == null )  // 若key对应的订单号不存在
                {
                    Dictionary<String, String> iteam = DB.SelectValue(TAB, resId, "ID").RowDic();           // 查询资源ID对应的数据
                    string preOrderId = CreateOrder(resId, iteam["name"], iteam["price"], iteam["author"]); // 生成预下单号
                    Session[key] = preOrderId;  // 记录至session中
                }
                value = Session[key] as string;

                // 2、
                bool OrderIsPayed = OrderSuccess(value);
                //OrderIsPayed = true;
                if (!OrderIsPayed)
                {   // 跳转支付
                    //GotoPay(value);

                    // 展示资源支付页
                    TipPay(resId, value);
                }
                else
                {   // 显示资源信息
                    Dictionary<String, String> iteam = DB.SelectValue(TAB, resId, "ID").RowDic();           // 查询资源ID对应的数据
                    ShowInfo(iteam["data"]);
                }
            }

            //--------------------------------------------------------

            // 显示接口使用说明
            NoteInfo();

            // 显示指定类型的订单信息
            ScTool.showTable(this.Controls, DB, TAB);
        }

        /// <summary>
        /// 接口使用说明信息
        /// </summary>
        private void NoteInfo()
        {
            String url = "http://" + Request.Params.Get("HTTP_HOST") + "/" + this.GetType().Name.Replace("_", "/").Replace("/aspx", ".aspx") + "?";
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("商品信息接口，使用说明：");
            builder.AppendLine("");
            builder.AppendLine("设置付费资源，网址模式：\t" + url + "TYPE=StaticIp");
            builder.AppendLine("");
            builder.AppendLine("添加：\t" + url + "TYPE=Add&name=资源xxx&price=1&author=您的支付宝账号&data=资源解压密码xxx&ext=拓展参数");
            builder.AppendLine("修改：\t" + url + "TYPE=Update&ID=100&name=资源xx1&author=scimence&price=1&data=密码321&ext=");
            builder.AppendLine("删除：\t" + url + "TYPE=Delet&ID=101");
            builder.AppendLine("查询：\t" + url + "TYPE=Select&ID=100&key=data");
            builder.AppendLine("");
            builder.AppendLine("待支付、显示密码：\t" + url + "ID=100");
            builder.AppendLine("");

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

                ColumnInfo.Add("name", 200);        // 资源名称
                ColumnInfo.Add("price", 30);        // 资源价格
                ColumnInfo.Add("author", 300);      // 资源主账号
                ColumnInfo.Add("data", 300);        // 资源解压密码 或 链接网址
                ColumnInfo.Add("ext", 200);         // 拓展参数

                ColumnInfo.Add("dateTime", 20);     // 创建的日期时间
                ColumnInfo.Add("lastTime", 20);     // 最后修改时间

                DB.CreateTable(TAB, ColumnInfo);
            }
        }

        /// <summary>
        /// 添加资源信息
        /// </summary>
        /// <param name="name">资源名称</param>
        /// <param name="price">金额</param>
        /// <param name="author">账号</param>
        /// <param name="author">资源数据</param>
        /// <param name="ext">拓展参数</param>
        /// <returns></returns>
        private string Add(string name, string price, string author, string data, string ext)
        {
            price = FormatPrice(price, 2);

            // 查询已有的数据信息
            Dictionary<string, string> ortherConditions = new Dictionary<string, string>();
            ortherConditions.Add("name", name);
            ortherConditions.Add("price", price);
            ortherConditions.Add("data", data);

            string ID = DB.SelectValue(TAB, author, "author", new string[] { "ID" }.ToList(), ortherConditions).FirstData();
            if (!ID.Equals("")) return ID;

            // 添加新的数据
            List<string> values = new List<string>();

            values.Add(name);
            values.Add(price);
            values.Add(author);
            values.Add(data);
            values.Add(ext);

            values.Add(DateTime.Now.ToString("yyyy-MM-dd_HH:mm:ss"));
            values.Add(DateTime.Now.ToString("yyyy-MM-dd_HH:mm:ss"));

            string id = DB.InsetValue(TAB, values);
            return id;
        }


        /// <summary>
        /// 更新TAB表中指定项的数据
        /// </summary>
        private void Update(string ID, string softName, string price, string linkUrl, string recomondUrl, string ext)
        {
            price = FormatPrice(price, 2);

            Dictionary<string, string> datas = new Dictionary<string, string>();
            if (softName != null) datas.Add("name", softName);
            if (price != null) datas.Add("price", price);
            if (linkUrl != null) datas.Add("author", linkUrl);
            if (recomondUrl != null) datas.Add("data", recomondUrl);
            if (ext != null) datas.Add("ext", ext);

            datas.Add("lastTime", ScTool.Date());   // 日期时间自动修改

            DB.UpdateValue(TAB, ID, datas, "ID");
        }

        /// <summary>
        /// 删除指定项
        /// </summary>
        private bool Delet(string ID)
        {
            return DB.DeletValue(TAB, ID, "ID");
        }

        /// <summary>
        /// 查询指定软件名称，对应的Key列数据
        /// </summary>
        /// <param name="name"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        private string Select(string ID, string key)
        {
            string value = DB.SelectValue(TAB, ID, "ID", new string[] { key }.ToList()).FirstData();
            return value;
        }

        //-------------------------------------------

        /// <summary>
        /// 从资源信息创建预下单号
        /// </summary>
        private string CreateOrder(string resId, string name, string price, string author)
        {
            // 从网页接口Sql.aspx获取数据
            string commond = "TYPE=PreOrder&machinCode=ProductInfo" + resId + "&soft=付费资源&product=" + name + "&money=" + price + "&ext=author(" + author + ")author;";

            string InfoUrl = "http://" + Request.Params.Get("HTTP_HOST") + "/Pages/Pay.aspx";
            String url = InfoUrl + "?" + commond;
            string Id = ScTool.getWebData(url);     // Result(100)Result
            Id = getNodeData(Id, "Result");

            return Id;
        }

        /// <summary>
        /// 在预下单中，查询订单是否成功
        /// </summary>
        private bool OrderSuccess(string preOrderId)
        {
            string commond = "TYPE=OrderResultX&preOrderId=" + preOrderId;
            string InfoUrl = "http://" + Request.Params.Get("HTTP_HOST") + "/Pages/Pay.aspx";
            String url = InfoUrl + "?" + commond;

            string data = ScTool.getWebData(url);
            if (data.Contains("Result(true)Result")) return true;
            else return false;
        }

        /// <summary>
        /// 在网页中显示数据data
        /// </summary>
        private void ShowInfo(string data)
        {
            if (!data.ToLower().StartsWith("http")) data = "资源密码：" + data;

            //string ShowInfoUrl = "http://" + Request.Params.Get("HTTP_HOST") + "/" + "PageHB/ShowInfo.aspx?p=" + data;
            string ShowInfoUrl = "../PageHB/ShowInfo.aspx?p=" + System.Web.HttpUtility.UrlEncode(data);
            if (data.ToLower().StartsWith("http://") || data.ToLower().StartsWith("https://")) ShowInfoUrl = System.Web.HttpUtility.UrlEncode(data);

            Response.Redirect(ShowInfoUrl);
        }

        /// <summary>
        /// 跳转至支付
        /// </summary>
        private void GotoPay(string preOrderId)
        {
            string url = "../Pages/Pay.aspx" + "?" + "TYPE=" + "CreateOrder" + "&" + "preOrderId=" + preOrderId;
            Response.Redirect(url);
        }

        /// <summary>
        /// 展示商品名称和待支付金额
        /// </summary>
        private void TipPay(string ProductId, string preOrderId)
        {
            Dictionary<String, String> iteam = DB.SelectValue(TAB, ProductId, "ID").RowDic();           // 查询资源ID对应的数据

            string tittle = iteam["name"];
            string price = iteam["price"];
            string link =  "http://" + Request.Params.Get("HTTP_HOST") + "/Pages/Pay.aspx" + "?" + "TYPE=" + "CreateOrder" + "&" + "preOrderId=" + preOrderId;
            link = System.Web.HttpUtility.UrlEncode(link);

            TipPay(tittle, price, link);
        }

        /// <summary>
        /// 展示商品名称和待支付金额
        /// </summary>
        private void TipPay(string tittle, string price, string link)
        {
            string url = "../Pages/ProductPay.aspx" + "?" + "tittle=" + tittle + "&price=" + price + "&link=" + link;
            Response.Redirect(url);
        }

        
        //-------------------------------------------

        /// <summary>
        /// 格式化金额信息, 为小数点后n位
        /// </summary>
        public static string FormatPrice(string price, int n)
        {
            double num = 0;
            try
            {
                num = double.Parse(price);
            }
            catch(Exception ex)
            {
            }

            string fprice = num.ToString("F" + n);
            return fprice;
        }

        /// <summary>
        /// 获取节点中的数据信息，如：key(100)key -> 100
        /// </summary>
        private string getNodeData(string data, string key)
        {
            string keyS = key + "(";
            string keyE = ")" + key;

            int S = 0;
            int E = 0;
            if (data.Contains(keyS)) S = data.IndexOf(keyS) + keyS.Length;
            if (data.Contains(keyE)) E = data.IndexOf(keyE, S);

            string sub = "";
            if (E > S) sub = data.Substring(S, E - S);
            return sub;
        }
    }

}