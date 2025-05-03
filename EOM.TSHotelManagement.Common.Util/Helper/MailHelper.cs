using EOM.TSHotelManagement.Shared;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;

namespace EOM.TSHotelManagement.Common.Util
{
    public class MailHelper
    {
        private readonly IMailConfigFactory mailConfigFactory;

        public MailHelper(IMailConfigFactory mailConfigFactory)
        {
            this.mailConfigFactory = mailConfigFactory;
        }

        public void SendMail(
            List<string> toEmails,
            string subject,
            string body,
            List<string> ccEmails = null,
            List<string> bccEmails = null,
            List<string> attachments = null,
            bool isBodyHtml = true)
        {
            var mailConfig = mailConfigFactory.GetMailConfig();

            var message = new MimeMessage();

            message.From.Add(new MailboxAddress(mailConfig.DisplayName, mailConfig.UserName));

            AddRecipients(message.To, toEmails);

            AddRecipients(message.Cc, ccEmails);

            AddRecipients(message.Bcc, bccEmails);

            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = isBodyHtml ? body : null,
                TextBody = isBodyHtml ? null : body
            };

            AddAttachments(bodyBuilder, attachments);

            message.Subject = subject;
            message.Body = bodyBuilder.ToMessageBody();

            using var client = new SmtpClient();
            try
            {
                client.Connect(
                    mailConfig.Host,
                    mailConfig.Port,
                    GetSecureSocketOptions()
                );

                client.Authenticate(mailConfig.UserName, mailConfig.Password);

                client.Send(message);
            }
            catch (AuthenticationException ex)
            {
                throw new ApplicationException(LocalizationHelper.GetLocalizedString($"Email verification failed: {ex.Message}", $"邮件认证失败: {ex.Message}"), ex);
            }
            catch (SmtpCommandException ex)
            {
                throw new ApplicationException(LocalizationHelper.GetLocalizedString($"SMTP command error ({ex.StatusCode}): {ex.Message}", $"SMTP命令错误 ({ex.StatusCode}): {ex.Message}"), ex);
            }
            catch (SmtpProtocolException ex)
            {
                throw new ApplicationException(LocalizationHelper.GetLocalizedString($"SMTP protocol error: {ex.Message}", $"SMTP协议错误: {ex.Message}"), ex);
            }
            finally
            {
                client.Disconnect(true);
            }
        }

        #region Private Methods

        private void AddRecipients(InternetAddressList list, List<string> emails)
        {
            emails?.Where(email => !string.IsNullOrWhiteSpace(email))
                   .ToList()
                   .ForEach(email => list.Add(MailboxAddress.Parse(email)));
        }

        private void AddAttachments(BodyBuilder builder, List<string> attachments)
        {
            attachments?.Where(File.Exists)
                       .ToList()
                       .ForEach(filePath =>
                       {
                           using var stream = File.OpenRead(filePath);
                           builder.Attachments.Add(
                               Path.GetFileName(filePath),
                               stream,
                               MimeKit.ContentType.Parse(MediaTypeNames.Application.Octet)
                           );
                       });
        }

        private SecureSocketOptions GetSecureSocketOptions()
        {
            var mailConfig = mailConfigFactory.GetMailConfig();
            return mailConfig.EnableSsl switch
            {
                true when mailConfig.Port == 465 => SecureSocketOptions.SslOnConnect,
                true when mailConfig.Port == 587 => SecureSocketOptions.StartTls,
                true => SecureSocketOptions.Auto,
                false => SecureSocketOptions.None
            };
        }

        #endregion
    }
}