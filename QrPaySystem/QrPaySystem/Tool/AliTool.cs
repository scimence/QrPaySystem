using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Sci
{
    /// <summary>
    /// 获取支付宝用户id示例：
    /// string userId = Sci.AliTool.GetUserId(auth_code, app_id, private_key, alipay_public_key);
    /// </summary>
    class AliTool
    {
        /// <summary>
        /// 调用assembly中的静态方法
        /// </summary>
        private static object RunStatic(Assembly assembly, string classFullName, string methodName, object[] args)
        {
            if (assembly == null) return null;

            Type type = assembly.GetType(classFullName, true, true);

            //object[] arg = new object[] { "参数1", "参数2" };
            object tmp = type.InvokeMember(methodName, BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.Static, null, null, args);
            return tmp;
        }

        private static Assembly asm = null;


        /// <summary>
        /// 根据授权码，获取支付宝用户id
        /// </summary>
        /// <param name="auth_code">支付宝小程序,授权码</param>
        /// <param name="app_id">支付宝小程序,应用id</param>
        /// <param name="private_key">支付宝小程序,使用的私钥</param>
        /// <param name="alipay_public_key">支付宝后台配置的公钥</param>
        /// <returns></returns>
        public static string GetUserId(string auth_code, string app_id, string private_key, string alipay_public_key)
        {
            if (asm == null)
            {
                asm = Assembly.Load(GetByte());
            }

            if (asm != null)
            {
                RunStatic(asm, "SciAli.DllTool", "LoadResourceDll", new object[] { });

                object[] args = new object[] { auth_code, app_id, private_key, alipay_public_key };
                Object obj = RunStatic(asm, "SciAli.AliTool", "GetUserId", args);
                return obj.ToString();
            }

            return "";

        }

        /// <summary>
        /// 根据授权码，获取支付宝用户id。返回原始支付宝数据。
        /// </summary>
        /// <param name="auth_code">支付宝小程序,授权码</param>
        /// <param name="app_id">支付宝小程序,应用id</param>
        /// <param name="private_key">支付宝小程序,使用的私钥</param>
        /// <param name="alipay_public_key">支付宝后台配置的公钥</param>
        /// <returns></returns>
        public static string GetUserIdRaw(string auth_code, string app_id, string private_key, string alipay_public_key)
        {
            if (asm == null)
            {
                asm = Assembly.Load(GetByte());
            }

            if (asm != null)
            {
                RunStatic(asm, "SciAli.DllTool", "LoadResourceDll", new object[] { });

                object[] args = new object[] { auth_code, app_id, private_key, alipay_public_key };
                Object obj = RunStatic(asm, "SciAli.AliTool", "GetUserIdRaw", args);
                return obj.ToString();
            }

            return "";
        }


        #region 获取支付宝用户id插件

        private static byte[] GetByte()
        {
            string data_run = getData("https://scimence.gitee.io/aliuserid/AliTool.data");
            byte[] bytes = ToBytes(data_run);
            return bytes;
        }

        /// <summary>  
        /// 解析字符串为Bytes数组
        /// </summary>  
        private static byte[] ToBytes(string data)
        {
            byte[] B = new byte[data.Length / 2];
            char[] C = data.ToCharArray();

            for (int i = 0; i < C.Length; i += 2)
            {
                byte b = ToByte(C[i], C[i + 1]);
                B[i / 2] = b;
            }

            return B;
        }

        /// <summary>  
        /// 每两个字母还原为一个字节  
        /// </summary>  
        private static byte ToByte(char a1, char a2)
        {
            return (byte)((a1 - 'a') * 16 + (a2 - 'a'));
        }

        /// <summary>
        /// 从指定dataUrl载入数据，并在本地缓存
        /// </summary>
        /// <param name="dataUrl"></param>
        /// <returns></returns>
        private static string getData(string dataUrl)
        {
            string data = "";
            try
            {
                string fileName = dataUrl.Substring(dataUrl.LastIndexOf("/") + 1);
                string localPath = AppDomain.CurrentDomain.BaseDirectory + fileName;

                // 优先从本地载入数据
                if (File.Exists(localPath))
                {
                    data = File.ReadAllText(localPath).Trim();
                    if (data.Trim().Equals("")) File.Delete(localPath);
                }

                // 若本地无数据，则从网址加载
                if (!File.Exists(localPath))
                {
                    System.Net.WebClient client = new System.Net.WebClient();
                    data = client.DownloadString(dataUrl).Trim();

                    File.WriteAllText(localPath, data);     // 本地缓存
                }

            }
            catch (Exception) { }
            return data;
        }

        #endregion

    }
}
