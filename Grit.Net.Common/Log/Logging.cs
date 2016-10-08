using Grit.Net.Common.Config;
using Grit.Net.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Text;
using System.Threading;

namespace Grit.Net.Common.Log
{
    /// <summary>
    /// 日志管理类(先入队列后写文件)
    /// </summary>
    public static class Logging
    {
        #region Format
        private const string FORMAT_DATETIME = "yyyy-MM-dd HH:mm:ss.fff";
        private const string FORMAT_LOG_INF = "-------------------{0}---------------------------\r\nInfo:{1}\r\n";
        private const string FORMAT_LOG_ERR = "-------------------{0}---------------------------\r\nError:{1}\r\n";
        private const string FORMAT_LOG_EXC = "-------------------{0}---------------------------\r\nMessage:{1}\r\nStackTrace:{2}\r\n";
        private const string FORMAT_LOG_BIZ_EXC = "-------------------{0}---------------------------\r\nMessage:{1}\r\nStackTrace:{2}\r\n";
        private const string FORMAT_LOG_EXC_INF = "-------------------{0}---------------------------\r\nMessage:{1}\r\nStackTrace:{2}\r\nInfo:{3}\r\n";
        #endregion

        private static string logDir = null;
        private static Queue<Log> logs = null;
        private static ManualResetEventSlim mre = null;
        private static object locker = new object();
        public static void Create()
        {
            string directory = ConfigurationManager.AppSettings["Log.Directory"];
            if (string.IsNullOrEmpty(directory))
                throw new ConfigNotImplementException();
            Create(ConfigurationManager.AppSettings["Log.Directory"]);
        }
        /// <summary>
        /// 初始化日志操作类
        /// </summary>
        /// <param name="_logDir">日志文件根目录</param>
        public static void Create(string _logDir)
        {
            logDir = _logDir + "\\";
            string loginfo = ConfigHelper.GetAppSettings("Log.Info", "infolog");//默认普通信息日志目录
            string errorlog = ConfigHelper.GetAppSettings("Log.Error", "errorlog");//默认错误信息日志目录

            if (!Directory.Exists(_logDir))
                Directory.CreateDirectory(_logDir);
            if (!Directory.Exists(logDir + loginfo))
                Directory.CreateDirectory(logDir + loginfo);
            if (!Directory.Exists(logDir + errorlog))
                Directory.CreateDirectory(logDir + errorlog);
            logs = new Queue<Log>();
            mre = new ManualResetEventSlim(false);
            Thread thread = new Thread(Work);
            thread.Start();
        }

        public static void BizCreate(string bizFlag)
        {
            string logname = logDir + "\\" + bizFlag; 
            if (!Directory.Exists(logname))
                Directory.CreateDirectory(logname);
        }

        private static void Write(Log log)
        {
            lock (locker)
            {
                logs.Enqueue(log);
                mre.Set();
            }
        }
        /// <summary>
        /// 写入异常
        /// </summary>
        /// <param name="ex">异常</param>
        public static void Error(Exception ex)
        {
            Write(new Log(LogEnum.Error, string.Format(FORMAT_LOG_EXC, DateTime.Now.ToString(FORMAT_DATETIME), ex.Message, ex.StackTrace)));
        }
        /// <summary>
        /// 写入异常
        /// </summary>
        /// <param name="ex">异常</param>
        public static void Error(Exception ex, string info)
        {
            Write(new Log(LogEnum.Error, string.Format(FORMAT_LOG_EXC_INF, DateTime.Now.ToString(FORMAT_DATETIME), ex.Message, ex.StackTrace, info)));
        }
        /// <summary>
        /// 写入错误信息
        /// </summary>
        /// <param name="error">错误信息内容</param>
        public static void Error(string error)
        {
            Write(new Log(LogEnum.Error, string.Format(FORMAT_LOG_ERR, DateTime.Now.ToString(FORMAT_DATETIME), error)));
        }
        /// <summary>
        /// 写入普通信息
        /// </summary>
        /// <param name="info">内容</param>
        public static void Info(string info)
        {
            Write(new Log(LogEnum.Information, string.Format(FORMAT_LOG_INF, DateTime.Now.ToString(FORMAT_DATETIME), info)));
        }
        /// <summary>
        /// 写入日志
        /// </summary>
        /// <param name="logEnum">日志类型</param>
        /// <param name="timeGranularity">时间粒度</param>
        /// <param name="content">内容</param>
        public static void Write(LogEnum logEnum, TimeGranularity timeGranularity, string content)
        {
            Write(new Log(logEnum, timeGranularity, content));
        }
        /// <summary>
        /// 写入自定义业务日志
        /// </summary>
        /// <param name="content">内容</param>
        /// <param name="biz">业务标识</param>
        /// <param name="format">是否格式化,默认true</param>
        public static void BizWrite(string content, string biz, bool format = true)
        {
            if (format)
                Write(new Log(biz, string.Format(FORMAT_LOG_INF, DateTime.Now.ToString(FORMAT_DATETIME), content), TimeGranularity.Daily));
            else
                Write(new Log(biz, content + "\r\n", TimeGranularity.Daily));
        }
        /// <summary>
        /// 写入自定义业务日志
        /// </summary>
        /// <param name="content">内容</param>
        /// <param name="biz">业务标识</param>
        /// <param name="timeGranularity">时间粒度</param>
        /// <param name="format">是否格式化,默认true</param>
        public static void BizWrite(string content, string biz, TimeGranularity timeGranularity, bool format = true)
        {
            if (format)
                Write(new Log(biz, string.Format(FORMAT_LOG_INF, DateTime.Now.ToString(FORMAT_DATETIME), content), timeGranularity));
            else
                Write(new Log(biz, content + "\r\n", timeGranularity));
        }

        /// <summary>
        /// 写入自定义业务异常
        /// </summary>
        /// <param name="content">内容</param>
        /// <param name="biz">业务标识</param>
        /// <param name="format">是否格式化,默认true</param>
        public static void BizError(Exception ex, string biz)
        {
            Write(new Log(biz, string.Format(FORMAT_LOG_BIZ_EXC, DateTime.Now.ToString(FORMAT_DATETIME), ex.Message, ex.StackTrace), TimeGranularity.Daily));
        }
        /// <summary>
        /// 写入自定义业务日志异常
        /// </summary>
        /// <param name="content">内容</param>
        /// <param name="biz">业务标识</param>
        /// <param name="timeGranularity">时间粒度</param>
        /// <param name="format">是否格式化,默认true</param>
        public static void BizError(Exception ex, string biz, TimeGranularity timeGranularity)
        {
            Write(new Log(biz, string.Format(FORMAT_LOG_BIZ_EXC, DateTime.Now.ToString(FORMAT_DATETIME), ex.Message, ex.StackTrace), timeGranularity));
        }
        //循环等待写入
        private static void Work()
        {
            while (true)
            {
                if (logs.Count > 0)
                    FileWrite();
                else
                {
                    //等待写入信号
                    mre.Reset();
                    mre.Wait();
                }
            }
        }
        //写入日志
        private static void FileWrite()
        {
            Log log = null;

            lock (locker)
            {
                if (logs.Count > 0)
                    log = logs.Dequeue();
            }
            if (log != null)
            {
                TextWrite(log);
            }
        }
        /// <summary>
        /// 写入文件
        /// </summary>
        /// <param name="log"></param>
        private static void TextWrite(Log log)
        {
            FileStream fs = null;
            try
            {
                fs = File.Open(log.GetFilename(logDir), FileMode.Append, FileAccess.Write);
                fs.Position = fs.Length;
                byte[] bytes = Encoding.UTF8.GetBytes(log.Content);
                fs.Write(bytes, 0, bytes.Length);
            }
            catch (UnauthorizedAccessException) { }
            catch (DirectoryNotFoundException)
            {
                DirecotoryNotFound(log);
            }
            catch (IOException) { }
            catch (Exception) { }
            finally
            {
                if (fs != null)
                {
                    //fs.Flush();
                    fs.Close();
                    fs.Dispose();
                }

            }
        }

        private static void DirecotoryNotFound(Log log)
        {
            string filename = log.GetFilename(logDir);
            string dir = filename.Substring(0, filename.LastIndexOf('\\'));
            if (!Directory.Exists(dir))
            {
                try
                {
                    Directory.CreateDirectory(dir);
                    TextWrite(log);
                }
                catch (UnauthorizedAccessException) { }
                catch (IOException) { }
                catch (Exception) { }

            }
        }

    }
}
