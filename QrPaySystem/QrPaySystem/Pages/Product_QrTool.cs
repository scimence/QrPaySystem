
using QrPaySystem.Tool;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;

namespace QrPaySystem.Pages
{
    /// <summary>
    /// 付费资源，二维码生成
    /// 从QrTool.exe反射调用二维码解析与生成的方法
    /// </summary>
    public class Product_QrTool
    {
        #region 付费资源二维码,功能接口

        /// <summary>
        /// 生成商品收款码
        /// </summary>
        /// <param name="link_QR">收款码地址</param>
        /// <param name="Tittle">资源名称</param>
        /// <param name="Id">收款Id</param>
        /// <param name="SaveDir">收款码保存路径</param>
        /// <returns></returns>
        public static string genPayPic(string link_QR, string Tittle, string Id, string SaveDir)
        {
            string picName = Id + ".png";
            string picPath = AppDomain.CurrentDomain.BaseDirectory + SaveDir + picName;
            if (!File.Exists(picPath))
            {
                Bitmap pic = ToPayQr(link_QR, Tittle);   // 生成收款码
                picName = Save(pic, SaveDir, Id + ".png");
            }

            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + SaveDir + picName)) return picName;
            else return "";
        }


        /// <summary>
        /// 将二维码图像转化为编码串
        /// </summary>
        public static string ToCode(Bitmap pic)
        {
            try
            {
                startStatic(QrToolAsm(), "QRTool.DependentFiles", "LoadResourceDll", true, new object[] { });
                string code = (string)startStatic(QrToolAsm(), "QRTool.Form_QR", "ToCode", true, new object[] { pic });
                return code;
            }
            catch (Exception ex)
            {
                string error = ex.ToString();
                return "";
            }
        }

        /// <summary>
        /// 获取链接地址对应的二维码图像
        /// </summary>
        public static Bitmap ToQR(String url)
        {
            startStatic(QrToolAsm(), "QRTool.DependentFiles", "LoadResourceDll", true, new object[] { });
            Bitmap pic = (Bitmap)startStatic(QrToolAsm(), "QRTool.Form_QR", "ToQR", true, new object[] { url });
            return pic;
        }

        /// <summary>
        /// 生成二维码
        /// </summary>
        /// <param name="link_QR">二维码地址</param>
        /// <param name="tittle">资源名称</param>
        /// <returns></returns>
        public static Bitmap ToPayQr(string link_QR, string tittle)
        {
            Bitmap QR = ToQR(link_QR);      // 生成二维码
            Bitmap BG = get_BG();           // 获取收款码背景图

            Bitmap HbQr = CombineSK(BG, QR, tittle);

            return HbQr;
        }

        #endregion


        //#region 红包码功能接口


        ///// <summary>
        ///// 生成红包码
        ///// </summary>
        ///// <param name="hbUrl0">红包码</param>
        ///// <param name="HbDir">收款码保存路径</param>
        ///// <returns></returns>
        //public static string genHbPic(string hbUrl0, string HbDir)
        //{
        //    string hbUrl = hbUrl0.Trim();
        //    string picName = "";
        //    if (hbUrl.ToLower().StartsWith("https://qr.alipay.com/"))
        //    {
        //        picName = hbUrl.Substring("https://qr.alipay.com/".Length) + ".png";
        //    }
        //    else
        //    {
        //        picName = MD5.Encrypt(hbUrl) + ".png";
        //    }

        //    string picPath = AppDomain.CurrentDomain.BaseDirectory + HbDir + picName;
        //    if (!File.Exists(picPath))
        //    {
        //        Bitmap pic = ToHbPic(hbUrl);   // 生成红包码
        //        picName = Save(pic, HbDir, picName);
        //    }

        //    if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + HbDir + picName)) return picName;
        //    else return "";
        //}


        ///// <summary>
        ///// 生成红包二维码图像，添加背景图
        ///// </summary>
        ///// <param name="hbUrl">红包二维码链接</param>
        //public static Bitmap ToHbPic(string hbUrl)
        //{
        //    Bitmap QR = ToQR(hbUrl);    // 生成二维码
        //    Bitmap BG = getHB_BG();     // 获取红包收款码背景图

        //    Bitmap HbQr = CombinePic(BG, QR, 146, 346, 280, 280);   // 合并二维码至红包码背景图上

        //    return HbQr;
        //}

        //#endregion



        #region 红包二维码功能实现逻辑

        public static Assembly QrTool = null;

        /// <summary>
        /// 获取QR工具的Assembly
        /// </summary>
        /// <returns></returns>
        private static Assembly QrToolAsm()
        {
            if (QrTool == null)
            {
                String dirPath = AppDomain.CurrentDomain.BaseDirectory + "tools\\";
                String exeFile = dirPath + "QRTool\\QRTool.exe";               // 二维码生成工具路径

                Assembly assembly = getAssembly(exeFile);
                QrTool = assembly;
            }
            return QrTool;
        }


        /// <summary>
        /// 合并生成红包收款码
        /// </summary>
        /// <param name="BG">背景图</param>
        /// <param name="QR">二维码</param>
        /// <param name="tittle">商家名称</param>
        /// <returns></returns>
        private static Bitmap CombineSK(Bitmap BG, Bitmap QR, string tittle)
        {
            //创建图像
            Bitmap tmp = new Bitmap(BG.Width, BG.Height);                //按指定大小创建位图
            Rectangle Rect = new Rectangle(0, 0, BG.Width, BG.Height);   //pic的整个区域

            //创建Graphic
            Graphics g = Graphics.FromImage(tmp);                   //从位图创建Graphics对象
            g.Clear(Color.FromArgb(0, 0, 0, 0));                    //清空

            // 绘制背景图
            g.DrawImage(BG, Rect, Rect, GraphicsUnit.Pixel);       //从pic的给定区域进行绘制

            // 绘制二维码
            Rectangle srcRect = new Rectangle(0, 0, QR.Width, QR.Height);   //pic的整个区域
            Rectangle destRect = new Rectangle(120, 348, 360, 360);         //pic的整个区域
            g.DrawImage(QR, destRect, srcRect, GraphicsUnit.Pixel);

            //256, 804
            // 绘制商家名称
            Font drawFont = new Font("宋体", 20, FontStyle.Bold);
            DrawString(g, drawFont, Color.White, 206, 804, tittle, 0);

            return tmp;     //返回构建的新图像
        }


        /// <summary>
        /// 合并背景图和二维码图像，指定绘制区域
        /// </summary>
        /// <param name="BG">背景图</param>
        /// <param name="QR">二维码</param>
        /// <returns></returns>
        private static Bitmap CombinePic(Bitmap BG, Bitmap QR, int x, int y, int width, int height)
        {
            //创建图像
            Bitmap tmp = new Bitmap(BG.Width, BG.Height);                //按指定大小创建位图
            Rectangle Rect = new Rectangle(0, 0, BG.Width, BG.Height);   //pic的整个区域

            //创建Graphic
            Graphics g = Graphics.FromImage(tmp);                   //从位图创建Graphics对象
            g.Clear(Color.FromArgb(0, 0, 0, 0));                    //清空

            // 绘制背景图
            g.DrawImage(BG, Rect, Rect, GraphicsUnit.Pixel);       //从pic的给定区域进行绘制

            // 绘制二维码
            Rectangle srcRect = new Rectangle(0, 0, QR.Width, QR.Height);   //pic的整个区域
            //Rectangle destRect = new Rectangle(120, 348, 360, 360);         //pic的整个区域
            Rectangle destRect = new Rectangle(x, y, width, height);        //pic的整个区域
            g.DrawImage(QR, destRect, srcRect, GraphicsUnit.Pixel);

            return tmp;     //返回构建的新图像
        }


        /// <summary>  
        /// 绘制根据点旋转文本，一般旋转点给定位文本包围盒中心点  
        /// </summary>  
        /// <param name="s">文本</param>  
        /// <param name="font">字体</param>  
        /// <param name="brush">填充</param>  
        /// <param name="point">旋转点</param>  
        /// <param name="format">布局方式</param>  
        /// <param name="angle">角度</param>  
        private static void DrawString(Graphics g, Font font, Color color, float x, float y, string s, float angle/*, StringAlignment H = StringAlignment.Center, StringAlignment L = StringAlignment.Center*/)
        {
            PointF point = new PointF(x, y);
            SolidBrush brush = new SolidBrush(color);

            // 绘制围绕点旋转的文本  
            StringFormat format = new StringFormat();
            //format.Alignment = L;
            //format.LineAlignment = H;

            // Save the matrix  
            Matrix mtxSave = g.Transform;

            Matrix mtxRotate = g.Transform;
            mtxRotate.RotateAt(angle, point);
            g.Transform = mtxRotate;

            g.DrawString(s, font, brush, point, format);

            // Reset the matrix  
            g.Transform = mtxSave;
        }

        private static Bitmap SK_BG = null;
        /// <summary>
        /// 获取红包收款码背景图
        /// </summary>
        /// <returns></returns>
        private static Bitmap get_BG()
        {
            if (SK_BG == null)
            {
                // 载入收款红包码背景图
                string BG_path = AppDomain.CurrentDomain.BaseDirectory + "tools\\Product_pic\\eaxmple_product.png";
                Image pic = Bitmap.FromFile(BG_path);

                SK_BG = ToBitmap(pic);

                // 释放文件占用
                pic.Dispose();
                pic = null;
            }
            return SK_BG;
        }

        private static Bitmap HB_BG = null;
        /// <summary>
        /// 获取红包码背景图
        /// </summary>
        private static Bitmap getHB_BG()
        {
            if (HB_BG == null)
            {
                // 载入收款红包码背景图
                string BG_path = AppDomain.CurrentDomain.BaseDirectory + "tools\\HB_pic\\example_hb.png";
                Image pic = Bitmap.FromFile(BG_path);

                HB_BG = ToBitmap(pic);

                // 释放文件占用
                pic.Dispose();
                pic = null;
            }
            return HB_BG;
        }

        //Image转化为Bitamap
        private static Bitmap ToBitmap(Image pic)
        {
            //创建图像
            Bitmap tmp = new Bitmap(pic.Width, pic.Height);                //按指定大小创建位图
            Rectangle Rect = new Rectangle(0, 0, pic.Width, pic.Height);   //pic的整个区域

            //绘制
            Graphics g = Graphics.FromImage(tmp);                   //从位图创建Graphics对象
            g.Clear(Color.FromArgb(0, 0, 0, 0));                    //清空

            g.DrawImage(pic, Rect, Rect, GraphicsUnit.Pixel);       //从pic的给定区域进行绘制

            return tmp;     //返回构建的新图像
        }


        #region 其他相关功能函数

        /// <summary>
        /// 保存图像返回文件名
        /// </summary>
        public static String Save(Bitmap pic, string SubDir = "QR_Product\\", string fileName = "")
        {
            String path = curDir() + SubDir;
            checkDir(path);

            if (fileName.Equals("")) fileName = DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".png";

            Save(pic, path + fileName);

            return fileName;
        }

        /// <summary>
        /// 保存图像
        /// </summary>
        public static void Save(Bitmap pic, String filePath)
        {
            if (File.Exists(filePath)) File.Delete(filePath);
            pic.Save(filePath, ImageFormat.Png);
        }

        /// <summary>
        /// 获取当前运行路径
        /// </summary>
        public static string curDir()
        {
            return AppDomain.CurrentDomain.BaseDirectory;
        }

        /// <summary>
        /// 检测目录是否存在，若不存在则创建
        /// </summary>
        public static void checkDir(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        /// <summary>
        /// 在文件浏览器中显示指定的文件
        /// </summary>
        public static void ShowFileInExplorer(String file)
        {
            if (File.Exists(file)) System.Diagnostics.Process.Start("explorer.exe", "/e,/select, " + file);
            else System.Diagnostics.Process.Start("explorer.exe", "/e, " + file);
        }

        #endregion

        //-------------------------------------

        /// <summary>
        ///  从Assebly启动
        /// </summary>
        /// <param name="assembly">Assembly</param>
        /// <param name="args">参数</param>
        /// <returns></returns>
        private static object startStatic(Assembly assembly, string classFullName, string methodName, bool IsPublic, object[] args)
        {
            //string NameSpace = GetNamespace(assembly);
            //string classFullName = NameSpace + ".Program";
            //string methodName = "Main";

            //MethodInfo entryPoint = assembly.EntryPoint;				// 获取入口
            //string classFullName = entryPoint.DeclaringType.FullName;	// 入口所在类
            //string methodName = entryPoint.Name;						// 入口方法名

            Type type = assembly.GetType(classFullName, true, true);	// 获取入口类 或 entryPoint.DeclaringType

            // 调用程序集的静态方法： Type.InvokeMember
            //object tmp = type.InvokeMember(methodName, BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.Static, null, null, args);
            //return type.InvokeMember(methodName, BindingFlags.InvokeMethod | (entryPoint.IsPublic ? BindingFlags.Public : BindingFlags.NonPublic) | BindingFlags.Static, null, null, args);
            try
            {
                object obj = type.InvokeMember(methodName, BindingFlags.InvokeMethod | (IsPublic ? BindingFlags.Public : BindingFlags.NonPublic) | BindingFlags.Static, null, null, args);
                return obj;
            }
            catch (Exception ex)
            {
                string str = ex.ToString();
                string error = str;
            }
            return null;
        }


        /// <summary>
        /// 载入exe文件为Assembly
        /// </summary>
        /// <param name="exeFile"></param>
        /// <returns></returns>
        private static Assembly getAssembly(string exeFile)
        {
            byte[] data = File2Bytes(exeFile);
            Assembly assembly = Assembly.Load(data);
            return assembly;
        }

        /// <summary>  
        /// 将文件转换为byte数组  
        /// </summary>  
        /// <param name="path">文件地址</param>  
        /// <returns>转换后的byte数组</returns>  
        private static byte[] File2Bytes(string path)
        {
            if (!File.Exists(path))
            {
                return new byte[0];
            }

            FileInfo fi = new FileInfo(path);
            byte[] buff = new byte[fi.Length];

            FileStream fs = fi.OpenRead();
            fs.Read(buff, 0, Convert.ToInt32(fs.Length));
            fs.Close();

            return buff;
        }

        #endregion

    }

}