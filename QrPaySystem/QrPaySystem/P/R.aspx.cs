using QrPaySystem.Pages;
using QrPaySystem.Tool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace QrPaySystem.P
{
    /// <summary>
    /// 创建订单过度页面
    /// </summary>
    public partial class R : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // 获取订单号
            string payType = "";
            string preOrderId = "";
            if (Request["a"] != null && !Request["a"].Equals("")) 
            {
                payType = "Ali";
                preOrderId = Request["a"];
            }

            if (Request["w"] != null && !Request["w"].Equals(""))
            {
                payType = "Wechat";
                preOrderId = Request["w"];
            }

            if (preOrderId == null || preOrderId.Equals("")) return;


            DataBase DB = new DataBase(ScTool.DBName("pre"), ScTool.UserName, ScTool.Password);

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
                    //string agent = Request.Params.Get("HTTP_USER_AGENT");
                    //string payType = ScTool.payType(agent);

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
                    //Server.Transfer("Order.aspx");
                    Server.Transfer("../Pages/Order.aspx");
                }
            }
            else Response.Write(ScTool.Alert("订单" + preOrderId + "不存在！请重新下单"));

            //{
            //    String param = "TYPE=CreateOrder&preOrderId=" + p + "&PayType=Ali";
            //    Server.Transfer("../pages/pay.aspx?" + param);
            //}

            //p = Request["w"];
            //if (p != null && !p.Equals(""))
            //{
            //    String param = "TYPE=CreateOrder&preOrderId=" + p + "&PayType=Wechat";
            //    Server.Transfer("../pages/pay.aspx?" + param);
            //}

        }
    }
}