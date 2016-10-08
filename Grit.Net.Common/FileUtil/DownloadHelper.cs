using Grit.Net.Common.Log;
using System;
using System.IO;
using System.Net;


namespace Grit.Net.Common.FileUtil
{
    public class DownloadHelper
    {
        public static void Download(string url, string savePath)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(new Uri(url));
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream stream = response.GetResponseStream();

                FileStream fs = new FileStream(savePath, FileMode.Create);
                byte[] readdata = new byte[1024];
                int length = stream.Read(readdata, 0, readdata.Length);
                while (length > 0)
                {
                    fs.Write(readdata, 0, length);
                    length = stream.Read(readdata, 0, readdata.Length);
                }
                fs.Close();
                fs.Dispose();
                stream.Close();
                stream.Dispose();
                response.Close();
                response.Dispose();
            }
            catch (WebException ex)
            {
                Logging.Error(ex);
            }
            catch (IOException ex)
            {
                Logging.Error(ex);
            }
            catch (Exception ex)
            {
                Logging.Error(ex);
            }
        }
        public static void DownloadAnyc(string url, string savePath)
        {

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(new Uri(url));
            RequestState requestState = new RequestState();
            requestState.BUFFER_SIZE = 1024;
            requestState.BufferRead = new byte[requestState.BUFFER_SIZE];
            requestState.Request = request;
            requestState.SavePath = savePath;
            requestState.FileStream = new FileStream(requestState.SavePath, FileMode.OpenOrCreate);

            //开始异步请求资源  
            request.BeginGetResponse(new AsyncCallback(ResponseCallback), requestState);
        }

        private static void ResponseCallback(IAsyncResult asyncResult)
        {
            RequestState requestState = (RequestState)asyncResult.AsyncState;
            requestState.Response = (HttpWebResponse)requestState.Request.EndGetResponse(asyncResult);

            Stream responseStream = requestState.Response.GetResponseStream();
            requestState.ResponseStream = responseStream;

            //开始异步读取流  
            responseStream.BeginRead(requestState.BufferRead, 0, requestState.BufferRead.Length, ReadCallback, requestState);
        }

        private static void ReadCallback(IAsyncResult asyncResult)
        {
            RequestState requestState = (RequestState)asyncResult.AsyncState;
            int read = requestState.ResponseStream.EndRead(asyncResult);
            if (read > 0)
            {
                requestState.FileStream.Write(requestState.BufferRead, 0, read);
                requestState.ResponseStream.BeginRead(requestState.BufferRead, 0, requestState.BufferRead.Length, ReadCallback, requestState);
            }
            else
            {
                requestState.Response.Close();
                requestState.FileStream.Close();
            }
        }

    }

    public class RequestState
    {
        /// <summary>
        /// 缓冲区大小
        /// </summary>
        public int BUFFER_SIZE { get; set; }

        /// <summary>
        /// 缓冲区
        /// </summary>
        public byte[] BufferRead { get; set; }

        /// <summary>
        /// 保存路径
        /// </summary>
        public string SavePath { get; set; }

        /// <summary>
        /// 请求流
        /// </summary>
        public HttpWebRequest Request { get; set; }

        /// <summary>
        /// 响应流
        /// </summary>
        public HttpWebResponse Response { get; set; }

        /// <summary>
        /// 流对象
        /// </summary>
        public Stream ResponseStream { get; set; }

        /// <summary>
        /// 文件流
        /// </summary>
        public FileStream FileStream { get; set; }

    }
}
