using System;
using System.ComponentModel;
using System.Diagnostics;

namespace Grit.Net.Common.ProcessManager
{
    /// <summary>
    /// 进程退出
    /// </summary>
    /// <param name="ex">异常</param>
    public delegate void ProcessExitedEventHandler(Exception ex, object userState);

    /// <summary>
    /// 启动进程帮助类
    /// </summary>
    public class ProcessWraper
    {
        /// <summary>
        ///  进程退出事件
        /// </summary>
        public event ProcessExitedEventHandler ProcessExited;

        /// <summary>
        /// 启动进程(等待进程退出)
        /// </summary>
        /// <param name="filename">进程完整名</param>
        /// <param name="args">参数</param>
        public void Start(string filename, string args)
        {
            Start(filename, args, ProcessWindowStyle.Hidden, null);
        }
        /// <summary>
        /// 启动进程(等待进程退出)
        /// </summary>
        /// <param name="filename">进程完整名</param>
        /// <param name="args">参数</param>
        public void Start(string filename, string args, object userState)
        {
            Start(filename, args, ProcessWindowStyle.Hidden, userState);
        }
        /// <summary>
        /// 启动进程(等待进程退出)
        /// </summary>
        /// <param name="filename">进程完整名</param>
        /// <param name="args">参数</param>
        /// <param name="windowStyle">ProcessWindowStyle</param>
        public void Start(string filename, string args, ProcessWindowStyle windowStyle, object userState)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = filename;
            startInfo.Arguments = args;
            startInfo.WindowStyle = windowStyle;
            Process process = new Process();
            process.StartInfo = startInfo;
            try
            {
                process.Start();
                process.WaitForExit();
                if (ProcessExited != null)
                    ProcessExited(null, userState);
            }
            catch (Win32Exception ex)
            {
                if (ProcessExited != null)
                    ProcessExited(ex, userState);
            }
            catch (Exception ex)
            {
                if (ProcessExited != null)
                    ProcessExited(ex, userState);
            }
            finally
            {
                process.Close();
                process.Dispose();
            }
        }
        /// <summary>
        /// 启动进程(不等待进程退出)
        /// </summary>
        /// <param name="filename">进程完整名</param>
        /// <param name="args">参数</param>
        public void StartAnyc(string filename, string args)
        {
            StartAnyc(filename, args, ProcessWindowStyle.Hidden);
        }
        /// <summary>
        /// 启动进程(不等待进程退出)
        /// </summary>
        /// <param name="filename">进程完整名</param>
        /// <param name="args">参数</param>
        /// <param name="windowStyle">ProcessWindowStyle</param>
        public void StartAnyc(string filename, string args, ProcessWindowStyle windowStyle)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = filename;
            startInfo.Arguments = args;
            startInfo.WindowStyle = windowStyle;
            Process process = new Process();
            process.StartInfo = startInfo;
            process.EnableRaisingEvents = true;
            process.Exited += Process_Exited;
            try
            {
                process.Start();
            }
            catch (Win32Exception ex)
            {
                process.Close();
                process.Dispose();
                if (ProcessExited != null)
                    ProcessExited(ex, null);
            }
            catch (Exception ex)
            {
                process.Close();
                process.Dispose();
                if (ProcessExited != null)
                    ProcessExited(ex, null);
            }

        }

        private void Process_Exited(object sender, EventArgs e)
        {
            Process process = sender as Process;
            if (process != null)
            {
                process.Close();
                process.Dispose();
            }
            if (ProcessExited != null)
                ProcessExited(null, null);
        }
    }
}
