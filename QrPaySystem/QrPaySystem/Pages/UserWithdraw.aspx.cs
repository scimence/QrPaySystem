
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
    /// 记录和查询 用户收益信息
    /// </summary>
    public partial class UserWithdraw : BaseWebPage
    {
        DataBase DB;
        DataBase DB_Ali;
        public static string TAB = "ProductWithdraw";
        string TYPE = "";

        /// <summary>
        /// 载入后执行参数对应的sql请求
        /// </summary>
        public override void Load(object sender, EventArgs e)
        {
            DB = new DataBase(ScTool.DBName("pre"), ScTool.UserName, ScTool.Password);
            DB_Ali = new DataBase(ScTool.DBName(ScTool.PayTypeAli), ScTool.UserName, ScTool.Password);
            confirmTabExist();

            TYPE = Request["TYPE"];

            if (TYPE != null)
            {
                if (TYPE.Equals("Add"))
                {
                    string result = Add(Request["author"], Request["reward"], Request["sum"]);
                    Response.Write(result);
                    return;
                }
                else if (TYPE.Equals("Update")) Update(Request["ID"], Request["author"], Request["reward"], Request["sum"]);
                else if (TYPE.Equals("Delet")) Delet(Request["ID"]);
                else if (TYPE.Equals("Select"))
                {
                    string result = Select(Request["ID"], Request["key"]);      // 查询ID数据项对应的key值
                    Response.Write(result);
                    return;
                }
                else if (TYPE.Equals("DeletThisTab")) DB.DeletTable(TAB);
                else if (TYPE.Equals("Reward"))
                {
                    string result = CountReward(Request["author"]);
                    Response.Write(result);
                    return;
                }
                else if (TYPE.Equals("RewardClear")) RewardClear(Request["author"]);
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
            builder.AppendLine("用户商品信息统计接口，使用说明：");
            builder.AppendLine("");
            builder.AppendLine("添加：\t" + url + "TYPE=Add&author=账号&reward=0&sum=0");
            builder.AppendLine("修改：\t" + url + "TYPE=Update&ID=100&author=scimence&reward=1&sum=0");
            builder.AppendLine("删除：\t" + url + "TYPE=Delet&ID=101");
            builder.AppendLine("查询：\t" + url + "TYPE=Select&ID=100&key=author");
            builder.AppendLine("");
            builder.AppendLine("载入所有用户收益：\t" + url + "TYPE=Reward&author=ALL");
            builder.AppendLine("");
            builder.AppendLine("载入指定用户收益：\t" + url + "TYPE=Reward&author=账号");
            builder.AppendLine("清除指定用户当前收益：\t" + url + "TYPE=RewardClear&author=账号");
            
            builder.AppendLine("");

            Response.Write(ScTool.Pre(builder.ToString()));
        }

        /// <summary>
        /// 确保数据库中，存在订单信息表，若不存在则创建
        /// </summary>
        private void confirmTabExist()
        {
            // 创建数据表
            if (!DB.ExistTab(TAB))
            {
                Dictionary<string, int> ColumnInfo = new Dictionary<string, int>();

                ColumnInfo.Add("author", 300);      // 资源主账号
                ColumnInfo.Add("reward", 30);       // 当前收益
                ColumnInfo.Add("dateTime", 20);     // 创建的日期时间
                ColumnInfo.Add("lastTime", 20);     // 最后修改时间
                ColumnInfo.Add("sum", 30);          // 历史收益
                
                DB.CreateTable(TAB, ColumnInfo);
            }
        }

        /// <summary>
        /// 添加新的数据项
        /// </summary>
        /// <param name="author">用户账号</param>
        /// <param name="sum">历史收益</param>
        /// <param name="reward">当前收益</param>
        private string Add(string author, string reward = "", string sum = "")
        {
            if (reward == null || reward.Equals("")) reward = "0";
            if (sum == null || sum.Equals("")) sum = "0";

            sum = FormatPrice(sum, 2);
            reward = FormatPrice(reward, 2);

            // 查询已有的数据信息
            string ID = DB.SelectValue(TAB, author, "author", new string[] { "ID" }.ToList()).FirstData();
            if (!ID.Equals("")) return ID;

            // 添加新的数据
            List<string> values = new List<string>();

            values.Add(author);
            values.Add(reward);

            values.Add(DateTime.Now.ToString("yyyy-MM-dd_HH:mm:ss"));
            values.Add(DateTime.Now.ToString("yyyy-MM-dd_HH:mm:ss"));

            values.Add(sum);

            string id = DB.InsetValue(TAB, values);
            return id;
        }

        /// <summary>
        /// 更新TAB表中指定项的数据
        /// </summary>
        private string Update(string ID, string author, string reward, string sum)
        {
            sum = FormatPrice(sum, 2);
            reward = FormatPrice(reward, 2);

            Dictionary<string, string> datas = new Dictionary<string, string>();
            if (author != null) datas.Add("author", author);
            if (reward != null) datas.Add("reward", reward);
            if (sum != null) datas.Add("sum", sum);

            datas.Add("lastTime", ScTool.Date());   // 日期时间自动修改

            string result = DB.UpdateValue(TAB, ID, datas, "ID");
            return result;
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
        /// 统计用户当前收益信息
        /// </summary>
        private string CountReward(string author)
        {
            string reward = "0";
            if (author.Equals("ALL") || author.Equals("all"))
            {
                List<string> authors = DB.SelectValue(TAB, "", "", new string[] { "author" }.ToList()).ColmnList();    // 查询所有用户名称

                double sumAll = 0;
                foreach(string name in authors)
                {
                    reward = CountReward(name);             // 对所有用户分别执行信息统计
                    try
                    {
                        sumAll += double.Parse(reward);     // 累计收益信息
                    }
                    catch (Exception ex) { }
                }
                reward = sumAll.ToString("F2");
            }
            else if (AuthorExists(DB_Ali, author))
            {
                // 对应的用户信息若不存在则创建
                string ID = DB.SelectValue(TAB, author, "author", new string[] { "ID" }.ToList()).FirstData();
                if (ID.Equals("") ) Add(author);

                reward = MoneyAll(DB_Ali, author);  // 查询当前收益金额

                Dictionary<string, string> datas = new Dictionary<string, string>();
                datas.Add("reward", reward);
                datas.Add("lastTime", ScTool.Date());       // 日期时间自动修改
                string result = DB.UpdateValue(TAB, author, datas, "author");   // 更新author对应的收益
                if (!result.Equals("success")) reward = "修改" + author + "的收益信息失败！" + "查询到收益：" + reward;
            }

            return reward;
        }

        /// <summary>
        /// 清除指定用户的当前收益信息，并累计到历史收益信息中
        /// </summary>
        private string RewardClear(string author)
        {
            string result = MoneyAllClear(DB_Ali, author);  // 设置为Finish状态
            if (result.ToLower().Equals("success"))
            {
                Dictionary<String, String> rowdic = DB.SelectValue(TAB, author, "author").RowDic();

                double numReward = double.Parse(rowdic["reward"]);
                double numSum = double.Parse(rowdic["sum"]);
                string numStr = (numReward + numSum).ToString("F2");

                result = Update(rowdic["ID"], null, "0", numStr);
                return result;
            }
            return "false";
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
            foreach (string moneyStr in list)
            {
                try
                {
                    double money = Double.Parse(moneyStr);
                    moneyAll += money;
                }
                catch (Exception) { }
            }

            double rewardRate = 0.9;    // 提现比例值
            return (moneyAll * rewardRate).ToString("F2");
        }


        /// <summary>
        /// 判断指定用户的订单是否存在
        /// </summary>
        public static bool AuthorExists(DataBase DB, String Author)
        {
            if (Author == null || Author.Equals("")) return false;

            string sql = "select top 1 * from [" + ScTool.ORDER + "]";
            sql = sql + " where ext like '%" + "author(" + Author + ")" + "%' ";

            bool contains = DB.Execute(sql).RowDic().Count > 0;

            return contains;
        }

        /// <summary>
        /// 设置指定的Author为Finish状态
        /// </summary>
        public static string MoneyAllClear(DataBase DB, String Author = "")
        {
            if (Author == null || Author.Equals("")) return "success";

            Dictionary<string, string> datas = new Dictionary<string, string>();
            datas.Add("isSuccess", "TrueFinish");

            string AppendCondition = "and ext like '%" + "author(" + Author + ")" + "%' ";
            string result = DB.UpdateValue(ScTool.ORDER, "True", datas, "isSuccess", AppendCondition);

            return result;
        }


        //-------------------------------------------


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
        private string FormatPrice(string price, int n)
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