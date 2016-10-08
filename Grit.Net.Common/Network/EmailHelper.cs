using System;
using System.ComponentModel;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;

namespace Grit.Net.Common.Network
{
    /// <summary>
    /// 发送邮件完成后调用的委托
    /// </summary>
    /// <param name="ex"></param>
    public delegate void SendCompletedEventHandler(Exception ex);
    /// <summary>
    /// Email帮助类
    /// </summary>
    public class EmailHelper
    {
        private static int SmtpPort = 25;//SMTP 默认端口
        private static string SmtpHost;//SMTP服务器主机
        private static string FromEmailAddress;//发件人地址
        private static string FromEmailPassword;//发件人邮箱地址
        public static string[] DefaultAddressee;//默认收件人地址

        /// <summary>
        /// 发送邮件完成事件
        /// </summary>
        public event SendCompletedEventHandler SendCompleted;
        /// <summary>
        /// 设置邮箱信息
        /// </summary>
        /// <param name="_SmtpHost">SMTP主机</param>
        /// <param name="_FromEmailAddr">发件人邮箱</param>
        /// <param name="_FromEmailPwd">发件人邮箱密码</param>
        public static void Settings(string _SmtpHost, string _FromEmailAddr, string _FromEmailPwd)
        {
            SmtpHost = _SmtpHost;
            FromEmailAddress = _FromEmailAddr;
            FromEmailPassword = _FromEmailPwd;
        }
        /// <summary>
        /// 设置邮箱信息
        /// </summary>
        /// <param name="_SmtpHost">SMTP主机</param>
        /// <param name="_FromEmailAddr">发件人邮箱</param>
        /// <param name="_FromEmailPwd">发件人邮箱密码</param>
        /// <param name="_DefaultAddressee">默认收件人地址</param>
        public static void Settings(string _SmtpHost, string _FromEmailAddr, string _FromEmailPwd, string _DefaultAddressee)
        {
            Settings(_SmtpHost, _FromEmailAddr, _FromEmailPwd, new string[] { _DefaultAddressee });
        }
        /// <summary>
        /// 设置邮箱信息
        /// </summary>
        /// <param name="_SmtpHost">SMTP主机</param>
        /// <param name="_FromEmailAddr">发件人邮箱</param>
        /// <param name="_FromEmailPwd">发件人密码</param>
        /// <param name="_FromEmailPwd">发件人邮箱密码</param>
        /// <param name="_DefaultAddressee">默认收件人地址</param>
        public static void Settings(string _SmtpHost, string _FromEmailAddr, string _FromEmailPwd, string[] _DefaultAddressee)
        {
            SmtpHost = _SmtpHost;
            FromEmailAddress = _FromEmailAddr;
            FromEmailPassword = _FromEmailPwd;
            DefaultAddressee = _DefaultAddressee;
        }

        /// <summary>
        /// 发送邮件(异步)
        /// </summary>
        /// <param name="Subject">主题</param>
        /// <param name="Content">内容</param>
        public void SendAnyc(string Subject, string Content)
        {
            SendAnyc(Subject, Content, DefaultAddressee, null);

        }
        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="Subject">主题</param>
        /// <param name="Content">内容</param>
        /// <param name="Attachments">附件</param>
        public void SendAnyc(string Subject, string Content, string[] Attachments)
        {
            SendAnyc(Subject, Content, DefaultAddressee, Attachments);
        }
        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="Subject">主题</param>
        /// <param name="Content">内容</param>
        /// <param name="Addressee">收件人邮箱</param>
        public void SendAnyc(string Subject, string Content, string Addressee)
        {
            SendAnyc(Subject, Content, new string[] { Addressee }, null);
        }
        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="Subject">主题</param>
        /// <param name="Content">内容</param>
        /// <param name="Addressee">收件人邮箱</param>
        /// <param name="Attachments">附件</param>
        public void SendAnyc(string Subject, string Content, string Addressee, string[] Attachments)
        {
            SendAnyc(Subject, Content, new string[] { Addressee }, Attachments);
        }
        /// <summary>
        /// 发送邮件(异步)
        /// </summary>
        /// <param name="Subject">主题</param>
        /// <param name="Content">内容</param>
        /// <param name="Addressee">收件人邮箱</param>
        /// <param name="Attachments">附件</param>
        public void SendAnyc(string Subject, string Content, string[] Addressee, string[] Attachments)
        {
            if (Addressee == null && Addressee.Length == 0)
                throw new ArgumentNullException("缺少收件人地址");

            SmtpClient client = new SmtpClient(SmtpHost, SmtpPort);

            MailMessage message = new MailMessage();
            message.From = new MailAddress(FromEmailAddress);
            message.Sender = new MailAddress(FromEmailAddress);

            //设置收件人
            SetAddressee(message, Addressee);
            message.SubjectEncoding = Encoding.UTF8;
            message.BodyEncoding = Encoding.UTF8;
            message.Subject = Subject;
            message.Body = Content;
            message.IsBodyHtml = true;
            message.Priority = MailPriority.Normal;

            //添加附件
            SetAttachment(message, Attachments);

            client.UseDefaultCredentials = false;
            NetworkCredential credential = new NetworkCredential(FromEmailAddress, FromEmailPassword);
            client.Credentials = credential;
            client.EnableSsl = false;
            client.SendCompleted += SendCompletedCallback;
            try
            {
                client.SendAsync(message, message);
            }
            catch (SmtpException ex)
            {
                SendCompleted(ex);
                client.Dispose();
            }
            catch (Exception ex)
            {
                SendCompleted(ex);
                client.Dispose();
            }
        }

        private void SendCompletedCallback(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Cancelled)//邮件已取消
            {
            }
            if (e.Error != null)//发送失败
            {
            }
            MailMessage message = e.UserState as MailMessage;
            if (message != null)
            {
                DisposeAttachment(message);
                message.Dispose();
            }
            SmtpClient client = sender as SmtpClient;
            if (client != null)
                client.Dispose();
            if (SendCompleted != null)
                SendCompleted(e.Error);
        }

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="Subject">主题</param>
        /// <param name="Content">内容</param>
        public void Send(string Subject, string Content)
        {
            Send(Subject, Content, DefaultAddressee, null);
        }
        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="Subject">主题</param>
        /// <param name="Content">内容</param>
        /// <param name="Attachments">附件</param>
        public void Send(string Subject, string Content, string[] Attachments)
        {
            Send(Subject, Content, DefaultAddressee, Attachments);
        }
        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="Subject">主题</param>
        /// <param name="Content">内容</param>
        /// <param name="Addressee">收件人邮箱</param>
        public void Send(string Subject, string Content, string Addressee)
        {
            Send(Subject, Content, new string[] { Addressee }, null);
        }
        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="Subject">主题</param>
        /// <param name="Content">内容</param>
        /// <param name="Addressee">收件人邮箱</param>
        /// <param name="Attachments">附件</param>
        public void Send(string Subject, string Content, string Addressee, string[] Attachments)
        {
            Send(Subject, Content, new string[] { Addressee }, Attachments);
        }
        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="Subject">主题</param>
        /// <param name="Content">内容</param>
        /// <param name="Addressee">收件人邮箱</param>
        /// <param name="Attachments">附件</param>
        public bool Send(string Subject, string Content, string[] Addressee, string[] Attachments)
        {
            if (Addressee == null && Addressee.Length == 0)
                throw new ArgumentException("缺少收件人地址");

            SmtpClient client = new SmtpClient(SmtpHost, SmtpPort);

            MailMessage message = new MailMessage();
            message.From = new MailAddress(FromEmailAddress);
            message.Sender = new MailAddress(FromEmailAddress);

            //设置收件人
            SetAddressee(message, Addressee);
            message.SubjectEncoding = Encoding.UTF8;
            message.BodyEncoding = Encoding.UTF8;
            message.Subject = Subject;
            message.Body = Content;
            message.IsBodyHtml = true;
            message.Priority = MailPriority.Normal;

            //添加附件
            SetAttachment(message, Attachments);

            client.UseDefaultCredentials = false;
            NetworkCredential credential = new NetworkCredential(FromEmailAddress, FromEmailPassword);
            client.Credentials = credential;
            client.EnableSsl = false;

            try
            {
                client.Send(message);
                if (SendCompleted != null)
                    SendCompleted(null);
                return true;
            }
            catch (SmtpException ex)
            {
                if (SendCompleted != null)
                    SendCompleted(ex);
                return false;
            }
            catch (Exception ex)
            {
                if (SendCompleted != null)
                    SendCompleted(ex);
                return false;
            }
            finally
            {
                DisposeAttachment(message);
                message.Dispose();
                client.Dispose();
            }
        }
        // 设置收件人
        private void SetAddressee(MailMessage message, string[] addressee)
        {
            if (addressee != null && addressee.Length > 0)
            {
                foreach (string addr in addressee)
                {
                    message.To.Add(addr);
                }
            }
        }
        // 添加附件
        private void SetAttachment(MailMessage message, string[] attachments)
        {
            if (attachments != null && attachments.Length > 0)
            {
                Attachment data = null;
                foreach (string filename in attachments)
                {
                    data = new Attachment(filename, MediaTypeNames.Application.Octet);
                    ContentDisposition disposition = data.ContentDisposition;
                    disposition.CreationDate = System.IO.File.GetCreationTime(filename);
                    disposition.ModificationDate = System.IO.File.GetLastWriteTime(filename);
                    disposition.ReadDate = System.IO.File.GetLastAccessTime(filename);
                    message.Attachments.Add(data);
                }
            }
        }

        private void DisposeAttachment(MailMessage message)
        {
            if (message.Attachments != null && message.Attachments.Count > 0)
            {
                foreach (Attachment attachment in message.Attachments)
                {
                    attachment.Dispose();
                }
            }
        }

    }

}
