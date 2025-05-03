using System;
using System.IO;

namespace EOM.TSHotelManagement.Common.Util
{
    public static class LogHelper
    {
        private static readonly string logFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs", "ErrorLog.txt");

        static LogHelper()
        {
            var logDirectory = Path.GetDirectoryName(logFilePath);
            if (!Directory.Exists(logDirectory))
            {
                Directory.CreateDirectory(logDirectory);
            }
        }

        /// <summary>
        /// 记录CRUD错误日志
        /// </summary>
        /// <param name="message">错误消息</param>
        /// <param name="exception">异常对象</param>
        public static void LogError(string message, Exception exception)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(logFilePath, true))
                {
                    writer.WriteLine("--------------------------------------------------");
                    writer.WriteLine($"时间: {DateTime.Now}");
                    writer.WriteLine($"消息: {message}");
                    writer.WriteLine($"异常: {exception}");
                    writer.WriteLine("--------------------------------------------------");
                    writer.WriteLine();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"记录日志时发生异常: {ex}");
            }
        }
    }
}
