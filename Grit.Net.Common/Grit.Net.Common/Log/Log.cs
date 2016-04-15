using System;

namespace Grit.Net.Common.Log
{
    /// <summary>
    /// 日志基本信息
    /// </summary>
    public class Log
    {
        //private string fileName;
        private string content;
        private DateTime lastWriteTime;
        private LogEnum logType;
        private TimeGranularity timeGranularity;

        public string Content
        {
            get { return content; }
            set { content = value; }
        }

        public LogEnum LogType
        {
            get { return logType; }
            set { logType = value; }
        }
        public TimeGranularity TimeGranularity
        {
            get { return timeGranularity; }
            set { timeGranularity = value; }
        }
        public DateTime LastWriteTime
        {
            get { return lastWriteTime; }
            set { lastWriteTime = value; }
        }

        public Log(string _content)
        {
            logType = LogEnum.Error;
            timeGranularity = TimeGranularity.Daily;
            content = _content;
            lastWriteTime = DateTime.Now;
        }

        public Log(LogEnum _logType, string _content)
        {
            logType = _logType;
            timeGranularity = TimeGranularity.Daily;
            content = _content;
            lastWriteTime = DateTime.Now;
        }

        public Log(LogEnum _logType, TimeGranularity _timeGranularity, string _content)
        {
            logType = _logType;
            timeGranularity = _timeGranularity;
            content = _content;
            lastWriteTime = DateTime.Now;
        }

        public string GetFilename(string logDir)
        {
            string dir = logDir;
            string name = "log_";
            switch (logType)
            {
                case LogEnum.Error:
                    dir += "errorlog";
                    break;
                default:
                    dir += "log";
                    break;
            }

            switch (timeGranularity)
            {
                case TimeGranularity.Daily:
                    name += lastWriteTime.ToString("yyyy-MM-dd");
                    break;
                case TimeGranularity.Weekly:
                    name += lastWriteTime.ToString("yyyy-MM-dd");
                    break;
                case TimeGranularity.Monthly:
                    name += lastWriteTime.ToString("yyyy-MM");
                    break;
                case TimeGranularity.Annually:
                    name += lastWriteTime.ToString("yyyy");
                    break;
                default:
                    name += lastWriteTime.ToString("yyyy-MM-dd");
                    break;
            }

            return dir + "\\" + name + ".txt";
        }

    }
    /// <summary>
    /// 日志枚举类型
    /// </summary>
    public enum LogEnum
    {
        /// <summary>
        /// 指示未知信息类型的日志记录
        /// </summary>
        Unknown,

        /// <summary>
        /// 指示普通信息类型的日志记录
        /// </summary>
        Information,

        /// <summary>
        /// 指示警告信息类型的日志记录
        /// </summary>
        Warning,

        /// <summary>
        /// 指示错误信息类型的日志记录
        /// </summary>
        Error,

        /// <summary>
        /// 指示成功信息类型的日志记录
        /// </summary>
        Success
    }
    /// <summary>
    /// 日志时间粒度
    /// </summary>
    public enum TimeGranularity
    {
        /// <summary>
        /// 天
        /// </summary>
        Daily,

        /// <summary>
        /// 周
        /// </summary>
        Weekly,

        /// <summary>
        /// 月
        /// </summary>
        Monthly,

        /// <summary>
        /// 年
        /// </summary>
        Annually
    }

}
