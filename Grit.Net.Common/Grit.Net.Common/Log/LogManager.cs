using Grit.Net.Common.Exceptions;
using System.Configuration;
using System.IO;
using System.Text;

namespace Grit.Net.Common.Log
{
    public static class LogManager
    {
        private const string FORMAT_TRACE_TIME = "-------------------{0}---------------------------";
        private static object locker = new object();
        private delegate void OnWriteDelegate(Log log);
        private static OnWriteDelegate OnWriteHandling = null;
        private static string logDir = null;
        //private static Queue<Log> logs;
        //private static Thread writelogThread;

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
            string loginfo = ConfigurationManager.AppSettings["Log.Info"];
            if (string.IsNullOrEmpty(loginfo))
                loginfo = "log";
            string errorlog = ConfigurationManager.AppSettings["Log.Error"];
            if (string.IsNullOrEmpty(errorlog))
                errorlog = "errorlog";
            if (!Directory.Exists(_logDir))
                Directory.CreateDirectory(_logDir);
            if (!Directory.Exists(logDir + loginfo))
                Directory.CreateDirectory(logDir + loginfo);
            if (!Directory.Exists(logDir + errorlog))
                Directory.CreateDirectory(logDir + errorlog);
            OnWriteHandling = new OnWriteDelegate(OnWrite);
        }
        /// <summary>
        /// 写入异常信息
        /// </summary>
        /// <param name="message"></param>
        public static void Write(string message)
        {
            if (OnWriteHandling == null)
                throw new NotInitializedException();
            OnWriteHandling.BeginInvoke(new Log(message), null, null);
        }
        /// <summary>
        /// 写入指定类型的信息，默认日志时间粒度为天
        /// </summary>
        /// <param name="message"></param>
        /// <param name="logType"></param>
        public static void Write(string message, LogEnum logType)
        {
            if (OnWriteHandling == null)
                throw new NotInitializedException();
            OnWriteHandling.BeginInvoke(new Log(logType, message), null, null);
        }
        /// <summary>
        /// 写入指定类型的信息，指定日志时间粒度
        /// </summary>
        /// <param name="message"></param>
        /// <param name="logType"></param>
        /// <param name="_timeGranularity"></param>
        public static void Write(string message, LogEnum logType, TimeGranularity _timeGranularity)
        {
            if (OnWriteHandling == null)
                throw new NotInitializedException();
            OnWriteHandling.BeginInvoke(new Log(logType, _timeGranularity, message), null, null);
        }

        private static void OnWrite(Log log)
        {
            lock (locker)
            {
                StreamWriter sw = new StreamWriter(log.GetFilename(logDir), true, Encoding.UTF8);
                sw.WriteLine(FORMAT_TRACE_TIME, log.LastWriteTime);
                sw.WriteLine(log.Content);
                sw.Close();
                sw.Dispose();
            }
        }
    }
}
