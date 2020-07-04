using QrPaySystem.PayFor;
using QrPaySystem.Tool;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace QrPaySystem.Pages
{
    public partial class UserRewards : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string author = Request["p"];

            if (author == null || author.Equals(""))
            {
                author = UserTool.GetAccount(Session); // 获取登录的用户名
            }

            if (author != null && !author.Equals(""))
            {
                // 查询收益信息
                string url = "http://" + Request.Params.Get("HTTP_HOST") + "/Pages/UserWithdraw.aspx" + "?" + "TYPE=Reward&author=" + author;
                string reward = ScTool.getWebData(url);

                LabelAccount.Text = author;
                LabelReward.Text = reward;

                DataBase DB_Ali = new DataBase(ScTool.DBName(ScTool.PayTypeAli), ScTool.UserName, ScTool.Password);
                showTable(DivTable.Controls, DB_Ali, ScTool.ORDER, author, "True");

                showTable(DivTableHistory.Controls, DB_Ali, ScTool.ORDER, author, "TrueFinish");
            }
            else
            {
                LabelAccount.Text = "";
                LabelReward.Text = "0";

                DivTable.InnerText = "示例：" + "http://" + Request.Params.Get("HTTP_HOST") + "/Pages/UserRewards.aspx" + "?" + "p=scimence";
            }
        }

        /// <summary>
        /// 查询Author支付成功的订单信息
        /// </summary>
        /// <param name="DB">数据库</param>
        /// <param name="TAB">表名称</param>
        /// <param name="Author">用户名</param>
        public static void showTable(ControlCollection root, DataBase DB, string TAB, string Author, string isSuccess)
        {
            if (DB.ExistTab(TAB))
            {
                //string sql = "select * from [" + TAB + "]";
                string sql = "select top 1000 ID,product,money,dateTime from [" + TAB + "]";
                sql = sql + " where isSuccess='" + isSuccess + "'";
                sql = sql + " and ext like '%" + "author(" + Author + ")" + "%' ";
                sql = sql + " order by ID desc";

                Table table = DB.Execute(sql).Table();

                Label label = new Label();
                label.ForeColor = Color.BlueViolet;

                if (isSuccess.Equals("True")) label.Text = "\r\n" + "收益详情" + "：";
                else if (isSuccess.Equals("TrueFinish")) label.Text = "\r\n" + "历史收益" + "：";

                root.Add(label);

                //print("数据表" + TAB +"：");
                root.Add(table);
            }
        }
    }
}