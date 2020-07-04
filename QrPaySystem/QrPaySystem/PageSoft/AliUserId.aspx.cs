using QrPaySystem.Tool;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace QrPaySystem.PageSoft
{
    public partial class AliUserId : System.Web.UI.Page
    {

        LogTool log = null;
        String url = "";
        String host = "";
        String TYPE = "";   // 自定义操作类型
        

        protected void Page_Load(object sender, EventArgs e)
        {
            //User_AddAliUser("20880007539361534120577141514747", "nickname", "TB1LKpub6NrDuNkUuBSXXcGaFXa");

            // 获取支付宝用户id
            if (Request["authcode"] != null || Request["avatar"] != null) TYPE = "GetAliUid";

            String LogName = this.GetType().Name;   // 当前页面名称
            log = new LogTool(LogName);             // 记录至log中

            // 显示接口信息
            url = "http://" + Request.Params.Get("HTTP_HOST") + "/" + this.GetType().Name.Replace("_", "/").Replace("/aspx", ".aspx");
            host = "http://" + Request.Params.Get("HTTP_HOST") + "/";

            if (TYPE.Equals("GetAliUid"))
            {
                this.Controls.Clear();      // 清除当前页所有控件
                //DivBody.InnerHtml = "";
                //DivBody.Style.Add("display", "none");   // 隐藏控件
                //Response.Clear();
                //HtmlBody.Style.Add("display", "none");   // 隐藏控件

                string avator = Request["avatar"];
                if (avator == null) avator = "";

                string auth_code = Request["authcode"];
                if (auth_code == null) auth_code = "";

                string app_id = Request["appid"];
                if (app_id == null) app_id = "";

                String aliUid = "";

                bool appIsPayed = IsPayed(app_id, "AliUserId_aspx");
                if (!appIsPayed)
                {
                    aliUid = "{\"error_response\":\"需开通在线接口，请点击‘立即开通’\"}";
                }

                // 查询avator对应的支付宝用户id
                if (aliUid.Equals("") && !avator.Equals(""))
                {
                    aliUid = User_GetIteam("avatar", avator, "aliUid", true);
                }

                // 从授权码，查询支付宝用户id
                if (aliUid.Equals("") && !app_id.Equals(""))
                {
                    string private_key = App_ReadData(app_id, "PrivateKey");
                    string alipay_public_key = App_ReadData(app_id, "AliPublicKey");

                    aliUid = GetUserId(auth_code, app_id, private_key, alipay_public_key);
                    saveAliUserInfo(app_id, aliUid, private_key, alipay_public_key, Request["nickname"], Request["avatar"]);
                }

                if (aliUid.Equals(""))   
                {
                    // 返回支付宝用户id获取异常信息
                    Response.Write(rawUerIdData);
                }
                else
                {
                    if (Request["all"] == null) Response.Write(aliUid);
                    else
                    {
                        // 获取支付宝用户id对应的数据项
                        string data = User_GetIteamJson("aliUid", aliUid);
                        Response.Write(data);
                    }
                }
            }

            //Response.Write(ScTool.Reslut(id));
        }

        /// <summary>
        /// 当输入的 小程序AppId变动时，查询小程序对应私钥公钥参数
        /// </summary>
        protected void TextBox_AppId_TextChanged(object sender, EventArgs e)
        {
            // 载入appid对应的 应用私钥、支付宝公钥信息
            string app_id = text_AppId.Text.Trim();
            if (app_id.Length > 15)
            {
                if (text_privateKey.Text.Trim().Equals(""))     // 没有设置私钥，则载入本地值
                {
                    string private_key = App_ReadData(app_id, "PrivateKey");
                    if (!private_key.Equals("")) text_privateKey.Text = private_key;
                }

                if (text_aliPublicKey.Text.Trim().Equals(""))   // 没有设置公钥，则载入本地值
                {
                    string alipay_public_key = App_ReadData(app_id, "AliPublicKey");
                    if (!alipay_public_key.Equals("")) text_aliPublicKey.Text = alipay_public_key;
                }
            }
        }

        /// <summary>
        /// 获取支付宝用户id
        /// </summary>
        protected void Button_GetUserId_Click(object sender, EventArgs e)
        {
            string app_id = text_AppId.Text.Trim();
            string private_key = text_privateKey.Text.Trim();
            string alipay_public_key = text_aliPublicKey.Text.Trim();
            string auth_code = text_authCode.Text.Trim();

            string userId = "";
            if (checkRaw.Checked) userId = Sci.AliTool.GetUserIdRaw(auth_code, app_id, private_key, alipay_public_key);
            else userId = GetUserId(auth_code, app_id, private_key, alipay_public_key);

            text_aliUserId.Text = (!userId.Equals("") ? userId : rawUerIdData);

            //{
            //    "error_response": {
            //        "code": "40002",
            //        "msg": "Invalid Arguments",
            //        "sub_code": "isv.code-invalid",
            //        "sub_msg": "授权码code无效"
            //    },
            //    "sign": ""
            //}

            // 若成功获取到了用户id，则保存参数信息到文件中
            if (!checkRaw.Checked)
            {
                saveAliUserInfo(app_id, userId, private_key, alipay_public_key);
            }

            SetLinkInfo(app_id, auth_code);
        }


        /// <summary>
        /// 开通在线请求接口
        /// </summary>
        protected void HrefPay2_Click(object sender, EventArgs e)
        {
            if (HrefPay2.Text.Equals("立即开通"))
            {
                string app_id = text_AppId.Text.Trim();
                //string HrefPay2_HRef = "../" + getPayUrl(app_id, "AliUserId_aspx", true);
                //Server.Transfer(HrefPay2_HRef);     //"../Pages/Pay.aspx"

                //string HrefPay2_HRef = getPayUrl(app_id, "AliUserId_aspx");
                string HrefPay2_HRef = getPayUrl_OnPreOrder(app_id, "AliUserId_aspx");
                Response.Redirect(HrefPay2_HRef);
            }
        }

        //string HrefPay2_HRef = "";
        /// <summary>
        /// 设置在线接口信息
        /// </summary>
        private void SetLinkInfo(string app_id, string auth_code, string nickname = null, string avatar = null)
        {
            if (app_id.Equals("")) DivTipInfo.InnerText = "";
            else
            {
                //app_id = "Ali" + app_id;
                bool appIsPayed = IsPayed(app_id, "AliUserId_aspx");

                // 设置在线支付链接信息
                if (appIsPayed)
                {
                    HrefPay2.Text = "接口已开通";
                }
                else
                {
                    HrefPay2.Text = "立即开通";
                    //HrefPay2_HRef = getPayUrl(app_id, "AliUserId_aspx");
                }

                //if (appIsPayed)
                //{
                //    HrefPay.HRef = url;
                //    HrefPay.InnerText = "接口已开通";
                //}
                //else
                //{
                //    HrefPay.HRef = getPayUrl(app_id, "AliUserId_aspx");
                //    HrefPay.InnerText = "立即开通";
                //}

                // 设置在线请求url
                String tipInfo = url + "?" + "appid=" + app_id + "&authcode=" + auth_code;
                if (nickname != null) tipInfo += "&nickname=" + nickname;
                if (avatar != null) tipInfo += "&avatar=" + avatar;

                HrefLink.HRef = tipInfo;
                HrefLink.InnerText = tipInfo;
            }
        }

        private static DataBase DB_pre = null;                      // 本地数据库连接对象

        /// <summary>
        /// 判断指定的machinCode/soft是否已完成支付
        /// </summary>
        private bool IsPayed(string machinCode, string soft = null)
        {
            if(machinCode == null || machinCode.Equals("")) return false;

            if(DB_pre == null) DB_pre = new DataBase(ScTool.DBName("pre"), ScTool.UserName, ScTool.Password);
            string result = QrPaySystem.Pages.OnlineCode.Check(DB_pre, ScTool.UserCode, machinCode, soft);
            return result.Equals("success");
        }

        /// <summary>
        /// 判断指定的machinCode/soft是否已完成支付
        /// </summary>
        private bool IsPayed_Web(string machinCode, string soft = null)
        {
            if (machinCode == null || machinCode.Equals("")) return false;

            // TYPE=OrderResultX&machinCode=机器码1&soft=可为空
            if (soft == null) soft = "";
            string payUrl = host + "Pages/Pay.aspx";
            string commond = "TYPE=OrderResultX&machinCode=" + machinCode + "&soft=" + soft;
            string url = payUrl + "?" + commond;
            string value = ScTool.getWebData(url);
            return value.Contains("(success)");
        }

        /// <summary>
        /// 生成支付请求url
        /// </summary>
        private string getPayUrl(string machinCode, string soft = null, bool noHost = false)
        {
            if(machinCode == null || machinCode.Equals("")) return url;

            if (soft == null) soft = "";
            string payUrl = (noHost ? "" : host) + "Pages/Pay.aspx";
            string param = QrPaySystem.Pages.Pay.PayParams(machinCode, soft, "5.00", null, "支付宝用户id接口");
            string urlP = payUrl + "?" + param;
            //urlP = System.Web.HttpUtility.UrlEncode(urlP);

            return urlP;
        }

        /// <summary>
        /// 创建待支付订单
        /// </summary>
        private string CreatePreOrder(string machinCode, string soft = null)
        {
            if(machinCode == null || machinCode.Equals("")) return url;

            if (soft == null) soft = "";
            string payUrl = host + "Pages/Pay.aspx";
            string param = QrPaySystem.Pages.Pay.PayParams(machinCode, soft, "5.00", null, "支付宝用户id接口");
            string urlP = payUrl + "?" + "TYPE=PreOrder&" + param;

            string Id = ScTool.getWebData(urlP);   // 创建订单
            Id = ScTool.getNodeData(Id, "Result"); // Result(100)Result

            return Id;
        }

        /// <summary>
        /// 通过预下单的方式，创建待支付url
        /// </summary>
        private string getPayUrl_OnPreOrder(string machinCode, string soft = null)
        {
            string preOrder = CreatePreOrder(machinCode,  soft);
            string payUrl = host + "Pages/Pay.aspx" + "?TYPE=ShowPreOrder&preOrderId=" + preOrder;
            return payUrl;
        }

        private string rawUerIdData = "";

        /// <summary>
        /// 重写获取支付宝用户id信息
        /// </summary>
        private string GetUserId(string auth_code, string app_id, string private_key, string alipay_public_key)
        {
            string userId = Sci.AliTool.GetUserId(auth_code, app_id, private_key, alipay_public_key);
            string aliKey = "\"error_response\":";
            if (userId.Contains(aliKey))
            {
                rawUerIdData = userId;
                userId = "";
            }
            return userId;
        }

        /// <summary>
        /// 保存支付宝用户参数信息
        /// </summary>
        /// <param name="app_id"></param>
        /// <param name="userId"></param>
        /// <param name="private_key"></param>
        /// <param name="alipay_public_key"></param>
        private void saveAliUserInfo(string app_id, string userId, string private_key, string alipay_public_key, string nickname = null, string avator = null)
        {
            if (!app_id.Equals("") && !userId.Equals(""))
            {
                App_SaveData(app_id, "PrivateKey", private_key);
                App_SaveData(app_id, "AliPublicKey", alipay_public_key);

                APP_AddIteam(app_id);   // 添加支付宝小程序信息
                User_AddAliUser(userId, nickname, avator);
            }
        }


        //----------------------------------------------------------------

        #region 数据参数，文件保存逻辑

        /// <summary>
        /// 生成本地文件保存路径
        /// </summary>
        private string AliDataPath(string app_id, string fileName)
        {
            string AliData = AppDomain.CurrentDomain.BaseDirectory + "tools/AliData";
            string path = AliData + "/" + app_id + "_" + fileName + ".txt";
            return path;
        }

        /// <summary>
        /// 保存数据content到文件
        /// </summary>
        /// <param name="app_id">小程序appId</param>
        /// <param name="fileName">文件名称</param>
        /// <param name="content">文件内容</param>
        private void App_SaveData(string app_id, string fileName, string content)
        {
            string path = AliDataPath(app_id, fileName);
            SaveData(path, content);
        }

        
        /// <summary>
        /// 读取指定文件的内容
        /// </summary>
        /// <param name="app_id">小程序appId</param>
        /// <param name="fileName">文件名称</param>
        /// <returns></returns>
        private string App_ReadData(string app_id, string fileName)
        {
            string path = AliDataPath(app_id, fileName);
            return ReadData(path);
        }


        //----------------------------------------------------------------

        /// <summary>
        /// 保存contents到path
        /// </summary>
        private void SaveData(string path, string content)
        {
            if (content.Equals("")) return; // 文件内容为空则不保存

            // 创建路径
            String dir = System.IO.Path.GetDirectoryName(path);
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

            // 文件内容变动，则重写
            String preContent = ReadData(path);
            if (!preContent.Equals(content))
            {
                File.WriteAllText(path, content);
            }
        }

        /// <summary>
        /// 读取指定文件的内容
        /// </summary>
        private string ReadData(string path)
        {
            string contents = "";
            if (File.Exists(path)) contents = File.ReadAllText(path);
            return contents;
        }

        # endregion

        //----------------------------------------------------------------


        private static String DBName = "DataBase_Mini";
        private static DataBase DB = null;                      // 本地数据库连接对象
        private static string TAB_AliApps = "AliApps";          // 记录支付宝小程序应用相关信息
        private static string TAB_User = "User";                // 记录用户信息

        private static void Init_DB()
        {
            if (DB == null)
            {
                // 连接指定的数据库，若不存在则创建
                DB = new DataBase(DBName, ScTool.UserName, ScTool.Password);
                confirmAliAppsTab();
                confirmUserTab();
            }
        }


        #region 用户信息表操作逻辑


        /// <summary>
        /// 确保数据库中，存在User信息表，若不存在则创建
        /// </summary>
        private static void confirmUserTab()
        {
            // 创建数据表
            if (!DB.ExistTab(TAB_User))
            {
                Dictionary<string, int> ColumnInfo = new Dictionary<string, int>();
                ColumnInfo.Add("nickname", 50);
                ColumnInfo.Add("avatar", 50);

                ColumnInfo.Add("creatDateTime", 20);
                ColumnInfo.Add("lastDateTime", 20);
                ColumnInfo.Add("count", 20);

                ColumnInfo.Add("aliUid", 50);
                ColumnInfo.Add("wxUid", 50);
                ColumnInfo.Add("phone", 20);
                ColumnInfo.Add("idNumber", 30);
                ColumnInfo.Add("realName", 20);
                ColumnInfo.Add("ext", 200);

                DB.CreateTable(TAB_User, ColumnInfo);
            }
        }

        /// <summary>
        /// 添加支付宝用户id，若nickname、avator变动，则更新数据
        /// </summary>
        /// <param name="aliUid">支付宝用户id</param>
        /// <param name="nickname">用户昵称</param>
        /// <param name="avatar">用户头像</param>
        /// <returns></returns>
        public static string User_AddAliUser(string aliUid, string nickname, string avatar)
        {
            // 查询支付宝用户id对应的数据项信息
            Dictionary<string, string> Iteam = User_GetIteamDic("aliUid", aliUid);

            string id = "";
            if(Iteam.ContainsKey("ID")) id = Iteam["ID"];
            
            string avator0 = "";
            if (Iteam.ContainsKey("avatar")) avator0 = Iteam["avatar"];

            string nickname0 = "";
            if(Iteam.ContainsKey("nickname")) nickname0 = Iteam["nickname"];

            // 若数据项不存在，则添加
            if (id.Equals("")) id = User_AddIteam(nickname, avatar, aliUid);

            // 数据变动，则更新数据
            else if(!avator0.Equals(avatar) || !nickname0.Equals(nickname))
            {
                User_UpdateIteam("aliUid", aliUid, "avatar", avatar, false, "nickname", nickname);   
            }

            return id;
        }

        /// <summary>
        /// 创建新的User信息，在数据库中添加新的数据项
        /// </summary>
        public static string User_AddIteam(string nickname, string avator = null, string aliUid = null, string wxUid = null, string phone = null, string idNumber = null, string realName = null, string ext = null)
        {
            Init_DB();
            string id = "fail";
            if (nickname == null) nickname = "";

            if (avator == null) avator = "";

            if (aliUid == null) aliUid = "";
            if (wxUid == null) wxUid = "";
            if (phone == null) phone = "";
            if (idNumber == null) idNumber = "";
            if (realName == null) realName = "";

            if (ext == null) ext = "";

            {
                // 添加新的数据
                List<string> values = new List<string>();

                values.Add(nickname);
                values.Add(avator);

                values.Add(ScTool.Date());
                values.Add(ScTool.Date());
                values.Add("0");

                values.Add(aliUid);
                values.Add(wxUid);
                values.Add(phone);
                values.Add(idNumber);
                values.Add(realName);

                values.Add(ext);

                id = DB.InsetValue(TAB_User, values);
            }

            return id;
        }

        /// <summary>
        /// 获取列名称为ColumnName，列值为ColumnValue的首个数据项
        /// </summary>
        /// <param name="ColumnName"></param>
        /// <param name="ColumnValue"></param>
        public static Dictionary<string, string> User_GetIteamDic(String ColumnName, String ColumnValue)
        {
            Init_DB();
            Dictionary<string, string> Iteam = DB.SelectValue(TAB_User, ColumnValue, ColumnName, null, null).RowDic();  // 查询ID指定的行信息
            return Iteam;
        }

        /// <summary>
        /// 查询IteamName列对应的数据。查询条件ColumnName=ColumnValue
        /// </summary>
        public static string User_GetIteam(String ColumnName, String ColumnValue, String IteamName = null, bool countAdd = false)
        {
            if (IteamName == null || IteamName.Equals("")) IteamName = "ID";

            String data = "";
            Dictionary<string, string> Iteam = User_GetIteamDic(ColumnName, ColumnValue);
            if (Iteam.ContainsKey(IteamName)) data = Iteam[IteamName];

            // 查询次数计数
            if (!data.Equals("") && countAdd)
            {
                if (Iteam.ContainsKey("count"))
                {
                    Dictionary<string, string> datas = new Dictionary<string, string>();

                    long count = long.Parse(Iteam["count"]) + 1;
                    datas.Add("count", count + "");
                    if (Iteam.ContainsKey("lastDateTime")) datas.Add("lastDateTime", ScTool.Date()); // 日期时间自动修改
                    
                    DB.UpdateValue(TAB_User, ColumnValue, datas, ColumnName);
                }
            }

            return data;
        }

        /// <summary>
        /// 获取列名称为ColumnName，列值为ColumnValue的首个数据项
        /// </summary>
        /// <param name="ColumnName"></param>
        /// <param name="ColumnValue"></param>
        public static string User_GetIteamJson(String ColumnName, String ColumnValue)
        {
            Init_DB();
            string iteamData = DB.SelectValue(TAB_User, ColumnValue, ColumnName, null, null).ToString();  // 查询ID指定的行信息
            return iteamData;
        }

        /// <summary>
        /// 删除指定Id对应项
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        private static bool User_DeletIteam(string Id)
        {
            return DB.DeletValue(TAB_User, Id, "ID");
        }

        /// <summary>
        /// 更新TAB表中指定项的数据
        /// </summary>
        public static string User_UpdateIteam(String ColumnName, String ColumnValue, String IteamName, String IteamValue, bool countAdd = false, String IteamName2=null, String IteamValue2=null)
        {
            Init_DB();

            Dictionary<string, string> datas = new Dictionary<string, string>();

            if (IteamValue == null) IteamValue = "";
            if (IteamName != null && !IteamName.Equals(""))
            {
                datas.Add(IteamName, IteamValue);
                datas.Add("lastDateTime", ScTool.Date()); // 日期时间自动修改

                if (IteamName2 != null && !IteamName2.Equals(""))
                {
                    if (IteamValue2 == null) IteamValue2 = "";
                    datas.Add(IteamName2, IteamValue2);
                }

                // 数据项计数
                if (countAdd)
                {
                    Dictionary<string, string> Iteam = DB.SelectValue(TAB_User, ColumnValue, ColumnName).RowDic();  // 查询ID指定的行信息
                    if (Iteam.ContainsKey("count"))
                    {
                        long count = long.Parse(Iteam["count"]) + 1;
                        datas.Add("count", count + "");
                    }
                }
            }

            return DB.UpdateValue(TAB_User, ColumnValue, datas, ColumnName);
        }

        #endregion



        #region AliApps数据表操作逻辑

        /// <summary>
        /// 确保数据库中，存在订单信息表，若不存在则创建
        /// </summary>
        private static void confirmAliAppsTab()
        {
            // 创建数据表
            if (!DB.ExistTab(TAB_AliApps))
            {
                Dictionary<string, int> ColumnInfo = new Dictionary<string, int>();
                ColumnInfo.Add("appId", 100);
                ColumnInfo.Add("reg", 20);
                ColumnInfo.Add("creatDateTime", 20);
                ColumnInfo.Add("lastDateTime", 20);
                ColumnInfo.Add("ext", 200);

                DB.CreateTable(TAB_AliApps, ColumnInfo);
            }
        }

        /// <summary>
        /// 在支付宝小程序数据表中记录数据项
        /// </summary>
        public static string APP_AddIteam(string appId, string reg = null, string ext = null)
        {
            Init_DB();
            string id = "fail";
            if (appId == null || appId.Equals("")) return id;
            if (reg == null) reg = "";
            if (ext == null) ext = "";

            // 查询已存在的数据信息对应Id,若无则添加新的
            //conditions.Add("Tittle", Tittle);
            Dictionary<string, string> Iteam = DB.SelectValue(TAB_AliApps, appId, "appId", null, null).RowDic();  // 查询ID指定的行信息
            if (Iteam.ContainsKey("ID")) id = Iteam["ID"];
            else
            {
                // 添加新的数据
                List<string> values = new List<string>();

                values.Add(appId);
                values.Add(reg);
                values.Add(ScTool.Date());
                values.Add(ScTool.Date());
                values.Add(ext);

                id = DB.InsetValue(TAB_AliApps, values);
            }

            return id;
        }

        /// <summary>
        /// 删除指定Id对应项
        /// </summary>
        private static bool APP_DeletIteam(string Id)
        {
            return DB.DeletValue(TAB_AliApps, Id, "ID");
        }

        /// <summary>
        /// 更新TAB表中指定项的数据
        /// </summary>
        public static string APP_UpdateIteam(string appId, string reg, string ext)
        {
            Init_DB();

            if (appId == null || appId.Equals("")) return "fail";

            Dictionary<string, string> datas = new Dictionary<string, string>();
            if (reg != null && !reg.Equals("")) datas.Add("reg", reg);
            if (ext != null && !ext.Equals("")) datas.Add("ext", ext);
            datas.Add("lastDateTime", ScTool.Date()); // 日期时间自动修改

            return DB.UpdateValue(TAB_AliApps, appId, datas, "appId");
        }

        /// <summary>
        /// 查询指定列的数据
        /// </summary>
        /// <param name="ColumnName"></param>
        /// <param name="ColumnValue"></param>
        /// <param name="IteamName"></param>
        /// <returns></returns>
        private static string APP_GetIteam(String ColumnName, String ColumnValue, String IteamName = null)
        {
            Init_DB();

            if (IteamName == null || IteamName.Equals("")) IteamName = "ID";

            List<string> columns = new List<string>();
            columns.Add(IteamName);
            string data = DB.SelectValue(TAB_AliApps, ColumnValue, ColumnName, columns, null).FirstData();  // 查询ID指定的行信息

            return data;
        }

        /// <summary>
        /// 判断指定的appId是否，reg项为true
        /// </summary>
        /// <param name="appId"></param>
        /// <returns></returns>
        private static bool App_IsReg(string appId)
        {
            string reg = APP_GetIteam("appId", appId, "reg");
            if (reg.Trim().Equals("")) return false;
            else return reg.Trim().ToLower().Contains("true");
        }
        

        #endregion

    }

}