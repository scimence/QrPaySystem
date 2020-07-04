using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Threading;


 
namespace QrPaySystem.Tool
{
    public class example
    {
        public static void 本地刷新ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            object arg = null;

            // 在新的线程中执行逻辑
            ThreadTool.ThreadRun(UpdateLocal, arg);	
        }
 
        private static void UpdateLocal(object arg)
        {
            // 自定义处理逻辑
        }
    }
 
    public class ThreadTool
    {
        /// <summary>
        /// 定义委托接口处理函数，用于实时处理cmd输出信息
        /// </summary>
        public delegate void Method(object arg);
 
        /// <summary>
        /// 在新的线程中执行method逻辑
        /// </summary>
        public static void ThreadRun(Method method, object arg)
        {
            Thread thread = new Thread(delegate()
            {
                // 允许不同线程间的调用
                //Control.CheckForIllegalCrossThreadCalls = false;
 
                // 执行method逻辑
                try
                {
                    if (method != null) method(arg);
                }
                catch (Exception ex)
                { }
            });
 
            thread.Priority = ThreadPriority.BelowNormal;               // 设置子线程优先级
            Thread.CurrentThread.Priority = ThreadPriority.Normal;      // 设置当前线程为最高优先级
            thread.Start();
        }
 
    }
}
