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
    /// <summary>
    /// 注册软件信息
    /// </summary>
    public partial class SoftReg : System.Web.UI.Page
    {
        DataBase DB;
        string TAB = SoftInfo.TAB;
        string author = "";
        public int UserType = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            UserType = UserTool.UserType(Session);      // 获取当前登录的用户类型信息

            author = Request["p"];

            if (author == null || author.Equals(""))
            {
                author = UserTool.GetAccount(Session); // 获取登录的用户名
            }


            if (author != null && !author.Equals(""))
            {
                LabelAccount.Text = author;

                DB = new DataBase(ScTool.DBName("pre"), ScTool.UserName, ScTool.Password);
                SoftInfo.confirmOrderTab(DB);

                showTable(DivTable.Controls, DB, TAB, author, UserType);
            }
        }

        /// <summary>
        /// 查询Author支付成功的订单信息
        /// </summary>
        /// <param name="DB">数据库</param>
        /// <param name="TAB">表名称</param>
        /// <param name="Author">用户名</param>
        public static void showTable(ControlCollection root, DataBase DB, string TAB, string Author, int UserType)
        {
            if (DB.ExistTab(TAB))
            {
                //string sql = "select * from [" + TAB + "]";
                string sql = "select top 1000 ID,softName,price,ext,dateTime from [" + TAB + "]";
                if (UserType == 1)  sql = sql + " where ext like '%" + "author(" + Author + ")" + "%' ";    // 普通用户
                sql = sql + " order by ID desc";

                Table table = DB.Execute(sql).Table();

                Label label = new Label();
                label.ForeColor = Color.BlueViolet;

                label.Text = "\r\n" + "现有注册软件，详情" + "：";

                root.Clear();

                root.Add(label);

                //print("数据表" + TAB +"：");
                root.Add(table);
            }
        }

        protected void ButtonAdd_Click(object sender, EventArgs e)
        {
            string softName = TextBox_Name.Text.Trim();
            string softPrice = TextBox_Price.Text.Trim();
            string freeTime = TextBox_Time.Text.Trim();

            softPrice = ScTool.FormatPrice(softPrice, 2);  // 对金额进行格式化

            string ext = "msg()msg;freeTimes(" + freeTime + ")freeTimes;author(" + LabelAccount.Text + ")author";
            string result = SoftInfo.Add(DB, softName, softPrice, "", "", ext);

            if (!result.Equals("") && !result.StartsWith("fail")) Label_tip.Text = "已添加！" + result;
            else Label_tip.Text = "添加失败！" + result;

            showTable(DivTable.Controls, DB, TAB, author, UserType);
        }

        protected void ButtonUpdate_Click(object sender, EventArgs e)
        {
            string softName = TextBox_Name.Text.Trim();
            string softPrice = TextBox_Price.Text.Trim();
            string freeTime = TextBox_Time.Text.Trim();

            softPrice = ScTool.FormatPrice(softPrice, 2);  // 对金额进行格式化

            bool isAuthor = UserType == 2 ? true : SoftInfo.IsAuthor(DB, softName, author);
            if (isAuthor)
            {
                string ext = "msg()msg;freeTimes(" + freeTime + ")freeTimes;author(" + LabelAccount.Text + ")author";
                string result = SoftInfo.Update2(DB, softName, softPrice, "", "", ext);

                if (result.Equals("success")) Label_tip.Text = "已修改！" + result;
                else Label_tip.Text = "修改失败！" + result;

                showTable(DivTable.Controls, DB, TAB, author, UserType);
            }
            else Label_tip.Text = "修改失败！" + "您当前没有软件:" + softName;
        }

        protected void ButtonDelet_Click(object sender, EventArgs e)
        {
            string softName = TextBox_Name.Text.Trim();
            bool isAuthor = UserType == 2 ? true : SoftInfo.IsAuthor(DB, softName, author);
            if (isAuthor)
            {
                bool result = SoftInfo.Delet2(DB, softName);
                Label_tip.Text = result ? "已删除！" : "删除失败！";

                showTable(DivTable.Controls, DB, TAB, author, UserType);
            }
            else Label_tip.Text = "删除失败！" + "您当前没有软件:" + softName;
        }

    }
}