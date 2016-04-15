using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Threading;

namespace Grit.Net.Common.Internal
{
    ///   <summary>   
    ///   计时类
    ///   </summary>   
    public class TrackTime
    {
        [DllImport("Kernel32.dll")]
        private static extern bool QueryPerformanceFrequency(out long lpm_Frequency);

        [DllImport("Kernel32.dll")]
        private static extern bool QueryPerformanceCounter(out long lpPerformanceCount);

        private long m_StartTime;
        private long m_StopTime;
        private long m_Freq;

        ///<summary>   
        ///TrackTime
        ///</summary>   
        public TrackTime()
        {
            this.m_StartTime = 0;
            this.m_StopTime = 0;

            if (QueryPerformanceFrequency(out m_Freq) == false)
            {
                throw new Win32Exception();
            }
        }
        ///<summary>   
        ///Start  
        ///</summary>   
        public void Start()
        {
            Thread.Sleep(0);
            QueryPerformanceCounter(out m_StartTime);
        }
        ///<summary>   
        ///Stop
        ///</summary>   
        public void Stop()
        {
            QueryPerformanceCounter(out m_StopTime);
        }
        ///<summary>   
        ///持续时间秒
        ///</summary>   
        public double DurationSecs
        {
            get
            {
                //   判断是否执行了开始和结束   
                if (m_StartTime == 0 || m_StopTime == 0)
                {
                    throw new InvalidOperationException("Must execute 'Start'!");
                }
                return (double)(m_StopTime - m_StartTime) / (double)m_Freq;
            }
        }

        ///<summary>   
        ///持续时间毫秒 float .###
        ///</summary>   
        public float DurationMsel
        {
            get
            {
                return float.Parse((this.DurationSecs * 1000).ToString("##0.###"));
            }
        }
        ///<summary>   
        ///持续时间毫秒 int
        ///</summary> 
        public int DurationMillisecond
        {
            get
            {  //   判断是否执行了开始和结束   
                if (m_StartTime == 0 || m_StopTime == 0)
                {
                    throw new InvalidOperationException("Must execute 'Start'!");
                }
                return (int)((m_StopTime - m_StartTime) * 1000 / m_Freq);
            }
        }
    }
}
