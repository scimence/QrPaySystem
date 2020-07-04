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
    public partial class UserWithdrawView : System.Web.UI.Page
    {
        DataBase DB;

        protected void Page_Load(object sender, EventArgs e)
        {
            DB = new DataBase(ScTool.DBName("pre"), ScTool.UserName, ScTool.Password);

            // 显示指定类型的订单信息
            //ScTool.showTable(DivTable.Controls, DB, UserWithdraw.TAB);
            showTable();
        }

        protected void ButtonRefresh_Click(object sender, EventArgs e)
        {
            string author = TextBox_Account1.Text.Trim();
            if (author.Equals("")) author = "ALL";

            string url = "http://" + Request.Params.Get("HTTP_HOST") + "/Pages/UserWithdraw.aspx" + "?" + "TYPE=Reward&author=" + author;
            string reward = ScTool.getWebData(url);

            //ScTool.showTable(DivTable.Controls, DB, UserWithdraw.TAB);
            showTable();

            if(author.Equals("ALL")) Label_tip.Text = "所有用户" + "，当前收益" + reward;
            else Label_tip.Text = "用户" + author + "，当前收益" + reward;
        }

        protected void ButtonClear_Click(object sender, EventArgs e)
        {
            string author = TextBox_Account2.Text.Trim();
            if (!author.Equals(""))
            {
                string url = "http://" + Request.Params.Get("HTTP_HOST") + "/Pages/UserWithdraw.aspx" + "?" + "TYPE=RewardClear&author=" + author;
                string reward = ScTool.getWebData(url);

                //ScTool.showTable(DivTable.Controls, DB, UserWithdraw.TAB);
                showTable();

                Label_tip.Text = "用户" + author + "已发放，当前收益已清空！";
            }
        }

        private void showTable()
        {
            showTable(DivTable.Controls, DB, UserWithdraw.TAB, false);
            showTable(DivTableFinish.Controls, DB, UserWithdraw.TAB, true);
        }

        /// <summary>
        /// 查询Author支付成功的订单信息
        /// </summary>
        /// <param name="DB">数据库</param>
        /// <param name="TAB">表名称</param>
        /// <param name="Author">用户名</param>
        public void showTable(ControlCollection root, DataBase DB, string TAB, bool reward0)
        {
            if (DB.ExistTab(TAB))
            {
                //string sql = "select * from [" + TAB + "]";
                string sql = "select top 1000 * from [" + TAB + "]";
                if(reward0) sql = sql + " where reward='0.00'";
                else sql = sql + " where reward <> '0.00'";
                //sql = sql + " and ext like '%" + "author(" + Author + ")" + "%' ";
                sql = sql + " order by ID desc";

                Table table = DB.Execute(sql).Table();

                Label label = new Label();
                label.ForeColor = Color.BlueViolet;

                if (reward0) label.Text = "\r\n" + "已发放" + "：";
                else label.Text = "\r\n" + "待发放" + "：";

                root.Clear();   // 清除原有控件

                root.Add(label);

                //print("数据表" + TAB +"：");
                root.Add(table);
            }
        }

    }
}
