using QrPaySystem.Tool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace QrPaySystem.PayFor
{
    /// <summary>
    /// 用户信息处理工具类
    /// </summary>
    public class UserTool
    {
        private static DataBase DB;
        private static string TAB = "UserInfo";

        /// <summary>
        /// 初始化基础信息
        /// </summary>
        public static void InitTool()
        {
            if (DB == null) DB = new DataBase(ScTool.DBName("pre"), ScTool.UserName, ScTool.Password);
            confirmOrderTab();
        }


        /// <summary>
        /// 确保数据库中，存在订单信息表，若不存在则创建
        /// </summary>
        private static void confirmOrderTab()
        {
            //DB.DeletTable(TAB);

            // 创建数据表
            if (!DB.ExistTab(TAB))
            {
                Dictionary<string, int> ColumnInfo = new Dictionary<string, int>();

                ColumnInfo.Add("Account", 50);          // 用户名称
                ColumnInfo.Add("Password", 50);         // 用户密码
                ColumnInfo.Add("Phone", 30);            // 手机号
                ColumnInfo.Add("IdCard", 30);           // 身份证号
                ColumnInfo.Add("QrUrl", 300);           // 收款二维码
                ColumnInfo.Add("RealName", 30);         // 真实姓名
                ColumnInfo.Add("Address", 30);          // 地址信息
                ColumnInfo.Add("ext", 200);             // 附加信息
                ColumnInfo.Add("createTime", 20);       // 创建时间
                ColumnInfo.Add("dateTime", 20);         // 日期时间
                ColumnInfo.Add("count", 20);            // 登录次数统计

                DB.CreateTable(TAB, ColumnInfo);
            }
        }

        /// <summary>
        /// 添加用户信息
        /// </summary>
        public static string Add(string Account, string Password, string Phone, string IdCard, string QrUrl = null, string RealName = null, string Address = null, string ext = null, string count = null)
        {
            if (Account == null || Account.Equals("") || Exist(Account)) return "fail";

            //Dictionary<String, String> row = Get(Account);
            //if (row.ContainsKey("Account")) return row["Account"];  // 若帐号已存在，则返回对应的Id
            //else
            {
                // 添加新的数据
                List<string> values = new List<string>();

                if (Account == null) Account = "";
                if (Password == null) Password = "";
                if (Phone == null) Phone = "";
                if (IdCard == null) IdCard = "";
                if (QrUrl == null) QrUrl = "";
                if (RealName == null) RealName = "";
                if (Address == null) Address = "";
                if (ext == null) ext = "";
                if (count == null) count = "0";
                
                values.Add(Account);
                values.Add(Password);
                values.Add(Phone);
                values.Add(IdCard);
                values.Add(QrUrl);
                values.Add(RealName);
                values.Add(Address);
                values.Add(ext);
                values.Add(ScTool.Date());
                values.Add(ScTool.Date());
                values.Add(count);

                string id = DB.InsetValue(TAB, values);
                return id;
            }
        }

        /// <summary>
        /// 获取帐号Account对应的数据信息
        /// </summary>
        public static Dictionary<String, String> Get(string Account)
        {
            // 查询当前在线的qrTable
            Dictionary<String, String> Dic = DB.SelectValue(TAB, Account, "Account").RowDic();
            return Dic;
        }

        /// <summary>
        /// 获取帐号Account对应的key数据信息
        /// </summary>
        public static String Get(string Account, string key)
        {
            // 查询当前在线的qrTable
            Dictionary<String, String> Dic = DB.SelectValue(TAB, Account, "Account").RowDic();
            if (Dic.ContainsKey(key)) return Dic[key];
            else return "";
        }

        /// <summary>
        /// 判断帐号是否已存在
        /// </summary>
        public static bool Exist(string Account)
        {
            Dictionary<String, String> row = Get(Account);
            if (row.ContainsKey("Account")) return true;
            else return false;
        }

        /// <summary>
        /// 更新TAB表中指定项的数据
        /// </summary>
        public static string Update(string ID, string Account, string Password, string Phone, string IdCard, string QrUrl, string RealName, string Address, string ext, string count)
        {
            Dictionary<string, string> datas = new Dictionary<string, string>();
            if (Account != null) datas.Add("Account", Account);
            if (Password != null) datas.Add("Password", Password);
            if (Phone != null) datas.Add("Phone", Phone);
            if (IdCard != null) datas.Add("IdCard", IdCard);
            if (QrUrl != null) datas.Add("QrUrl", QrUrl);
            if (RealName != null) datas.Add("RealName", RealName);
            if (Address != null) datas.Add("Address", Address);
            if (ext != null) datas.Add("ext", ext);
            if (count != null) datas.Add("count", count);
            //datas.Add("createTime", ScTool.Date()); // 创建时间
            datas.Add("dateTime", ScTool.Date());   // 日期时间自动修改

            return DB.UpdateValue(TAB, ID, datas, "ID");
        }

        /// <summary>
        /// 统计登录次数
        /// </summary>
        public static string CountLogin(String Account)
        {
            Dictionary<String, String> row = Get(Account);
            if (!row.ContainsKey("Account")) return "fail";

            String ID = row["ID"];
            string count = row["count"];

            if (count != null && !count.Equals(""))
            {
                count = (long.Parse(count) + 1).ToString();   // 启动次数递增
            }

            return Update(ID, null, null, null, null, null, null, null, null, count);
        }

        /// <summary>
        /// 删除指定Id对应项
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public static bool DeletIteam(string Id)
        {
            return DB.DeletValue(TAB, Id, "ID");
        }

        #region 帐号密码信息记录、解析、清除

        /// <summary>
        /// 记录帐号密码信息
        /// </summary>
        public static void SaveAccount(HttpSessionState session, string Account, string Password)
        {
            string EAccount = Locker.Encrypt(Account, "UserInfo");
            string EPassword = Locker.Encrypt(Password, Account);
            session["Password"] = EPassword;
            session["Account"] = EAccount;
        }

        /// <summary>
        /// 解析密码信息
        /// </summary>
        public static string GetPassword(HttpSessionState session)
        {
            string account = GetAccount(session);
            string password = "";
            if (session["Password"] == null || session["Password"].Equals("") || account.Equals("")) password = "";
            else
            {
                password = session["Password"] as string;
                password = Locker.Decrypt(password, account);
            }
            return password;
        }

        /// <summary>
        /// 解析帐号信息
        /// </summary>
        public static string GetAccount(HttpSessionState session)
        {
            string account = "";
            if (session["Account"] == null || session["Account"].Equals("")) account = "";
            else
            {
                account = session["Account"] as string;
                account = Locker.Decrypt(account, "UserInfo");
            }
            return account;
        }

        /// <summary>
        /// 清除Session中的帐号信息
        /// </summary>
        /// <param name="session"></param>
        public static void ClearAccount(HttpSessionState session)
        {
            session["Password"] = "";
            session["Account"] = "";
        }

        /// <summary>
        /// 获取当前登录的用户类型信息
        /// 
        /// 0：未登录   1：普通用户  2：管理员
        /// </summary>
        /// <param name="session"></param>
        /// <returns></returns>
        public static int UserType(HttpSessionState session)
        {
            String account = GetAccount(session);       // 获取Session中保存的帐号信息
            if (account.Equals("")) return 0;

            String password = GetPassword(session);     // 获取Session中保存的密码信息

            string psw = UserTool.Get(account, "Password");     // 查询用户密码信息
            if (!psw.Equals(password)) return 0;        // 不相同则未登录

            if (account.Equals("scimence")) return 2;   
            else return 1;
        }

        #endregion

    }
}