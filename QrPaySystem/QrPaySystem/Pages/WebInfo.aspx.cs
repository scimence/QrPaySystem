using QrPaySystem.Tool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace QrPaySystem.Pages
{
    /// <summary>
    /// 在数据库中记录和查询日志信息
    /// 
    /// 添加信息：http://localhost:5517/WebInfo.aspx?TAB=测试表&KEY=TAG4&VALUE=添加log信息
    /// 修改信息：http://localhost:5517/WebInfo.aspx?TAB=测试表&KEY=TAG4&VALUE_M=修改log信息
    /// 删除信息：http://localhost:5517/WebInfo.aspx?TAB=测试表&KEY_D=TAG4
    /// 查询所有信息： http://localhost:5517/WebInfo.aspx?TAB=测试表
    /// 查询指定信息： http://localhost:5517/WebInfo.aspx?TAB=测试表&KEY=TAG1
    /// </summary>
    public partial class WebInfo : BaseWebPage
    {

        String TAB = "default";     // 表名称
        String KEY = "";            // 数据标签
        String KEY_D = "";          // 删除数据标签行
        String VALUE = "";          // 数据信息
        String VALUE_M = "";        // 修改数据信息


        /// <summary>
        /// 载入后执行参数对应的sql请求
        /// </summary>
        public override void Load(object sender, EventArgs e)
        {
            TAB = Request["TAB"];
            KEY = Request["KEY"];
            KEY_D = Request["KEY_D"];
            VALUE = Request["VALUE"];
            VALUE_M = Request["VALUE_M"];


            String data = "";
            if (TAB == null || TAB.Equals(""))          // 无参数时显示说明信息
            {
                data = NoteInfo();
            }
            else
            {
                if (KEY_D != null && !KEY_D.Equals(""))
                {
                    data = Delet(TAB, KEY_D);           // 删除数据
                }
                else if (VALUE == null && VALUE_M == null)
                {
                    data = Get(TAB, KEY);               // 查询数据信息
                }
                else if (VALUE != null)
                {
                    data = Set(TAB, KEY, VALUE);        // 保存为新的数据项
                }
                else if (VALUE_M != null)
                {
                    data = Update(TAB, KEY, VALUE_M);   // 执行数据修改
                }
            }
            Response.Write(data);
        }


        /// <summary>
        /// 接口使用说明信息
        /// </summary>
        private String NoteInfo()
        {
            //String url = "http://" + Request.Params.Get("HTTP_HOST") + "/WebInfo.aspx";

            List<String> list = new List<string>();
            list.Add("接口使用说明：");
            list.Add("");
            list.Add("添加信息：" + url + "?" + "TAB=测试表&KEY=TAG4&VALUE=添加log信息");
            list.Add("修改信息：" + url + "?" + "TAB=测试表&KEY=TAG4&VALUE_M=修改log信息");
            list.Add("删除信息：" + url + "?" + "TAB=测试表&KEY_D=TAG4");
            list.Add("查询所有信息：" + url + "?" + "TAB=测试表");
            list.Add("查询指定信息：" + url + "?" + "TAB=测试表&KEY=TAG1");

            return NoteInfo("WebInfo保存与查询", list);
        }

        /// <summary>
        /// 页面使用说明信息
        /// </summary>
        private String NoteInfo(String tittle, List<String> list)
        {
            StringBuilder Str = new StringBuilder();
            Str.AppendLine("<!DOCTYPE html>");
            Str.AppendLine("<html lang=\"zh-CN\">");
            Str.AppendLine("");
            Str.AppendLine("<head><meta http-equiv=\"Content-Type\" content=\"text/html; charset=UTF-8\">");
            Str.AppendLine("<title>" + tittle + "</title>");
            Str.AppendLine("</head>");
            Str.AppendLine("");
            Str.AppendLine("<body>");
            Str.AppendLine("<div>");
            foreach (String str in list)
            {
                Str.AppendLine("<p>" + str + "</p>");
            }
            Str.AppendLine("</div>");
            Str.AppendLine("</body>");
            Str.AppendLine("");
            Str.AppendLine("</html>");

            return Str.ToString();
        }


        #region 数据表处理逻辑

        /// <summary>
        /// 删除TAB表标签为KEY的所有行
        /// </summary>
        public static String Delet(String TAB, String KEY_D)
        {
            String sql = "delete from " + TAB + " where 标签='" + KEY_D + "'"; ;
            String data = Execute(sql);
            return data;
        }

        /// <summary>
        /// 修改TAB表所有标签为KEY的所有数据
        /// </summary>
        public static String Update(String TAB, String KEY, String VALUE_M)
        {
            String sql = "update " + TAB + " set 信息 = '" + VALUE_M + "'" + " where 标签='" + KEY + "'";
            String data = Execute(sql);
            return data;
        }

        /// <summary>
        /// 查询TAB表标签为KEY的所有数据，KEY为null时返回所有表数据
        /// </summary>
        public static String Get(String TAB, String KEY)
        {
            String sql = "select * from [" + TAB + "]" + (KEY == null ? "" : " where 标签='" + KEY + "'");
            String data = Execute(sql);
            return data;
        }

        /// <summary>
        /// 保存log信息到数据表中
        /// </summary>
        public static String Set(String TAB, String KEY, String VALUE)
        {
            Boolean result = false;
            if (!TabExist(TAB))
            {
                if (CreateTable(TAB)) result = InsetValue(TAB, KEY, VALUE);
            }
            else result = InsetValue(TAB, KEY, VALUE);

            return (result ? "success" : "fail");
        }

        /// <summary>
        /// 判断数据库中是否存在指定名称的表
        /// </summary>
        public static Boolean TabExist(String TAB)
        {
            // 查询表是否存在： select name from sys.tables where name='数据表1'
            String sql = "select name from sys.tables where name='" + TAB + "'";
            String data = Execute(sql);

            return (data.Contains(TAB));
        }

        /// <summary>
        /// 创建指定名称的表
        /// </summary>
        public static Boolean CreateTable(String TAB)
        {
            //CREATE TABLE [dbo].[Log_All]
            //(
            //    [ID] INT NOT NULL, 
            //    [日期] NCHAR(30) NULL, 
            //    [信息] NCHAR(300) NULL, 
            //    CONSTRAINT [PK_Log_All] PRIMARY KEY ([ID]) 
            //)

            // 查询表是否存在： select name from sys.tables where name='数据表1'
            String sql = "CREATE TABLE [dbo].[" + TAB + "] ( [日期] NCHAR(30) NOT NULL,[标签] NCHAR(100) NULL,[信息] NCHAR(300) NULL,CONSTRAINT [PK_" + TAB + "] PRIMARY KEY ([日期]) )";
            String data = Execute(sql);

            return (data.Trim().ToLower().Equals("success"));
        }

        /// <summary>
        /// 向表中插入新的数据
        /// </summary>
        public static Boolean InsetValue(String TAB, String KEY, String VALUE)
        {
            // insert into tb_stu(num, name, sex, age, class) Values('1042163', '', '', '', '',)
            String Date = DateTime.Now.ToString("yyyy-MM-dd_HH:mm:ss_ffffff");
            String sql = "insert into " + TAB + "(日期, 标签, 信息) Values('" + Date + "', '" + KEY + "', '" + VALUE + "')";
            String data = Execute(sql);

            return (data.Trim().ToLower().Equals("success"));
        }

        /// <summary>
        /// 执行sql语句，获取数据库信息
        /// </summary>
        public static String Execute(String sql)
        {
            String data = "";

            //// 从网页接口Sql.aspx获取数据
            //String url = "http://" + Request.Params.Get("HTTP_HOST") + "/Sql.aspx" + "?" + sql;
            //data = getWebData(url);

            // 从代码接口直接获取数据
            data = Sql.Execute(sql);

            return data;
        }

        /// <summary>
        /// 获取指定url的网页数据
        /// </summary>
        public static String getWebData(String url)
        {
            String data = "";

            WebClient client = new WebClient();
            client.Encoding = System.Text.Encoding.UTF8;
            data = client.DownloadString(url);

            return data;
        }

        #endregion


    }
}