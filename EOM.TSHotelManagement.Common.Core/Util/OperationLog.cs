/*
 * MIT License
 *Copyright (c) 2021 易开元(Easy-Open-Meta)

 *Permission is hereby granted, free of charge, to any person obtaining a copy
 *of this software and associated documentation files (the "Software"), to deal
 *in the Software without restriction, including without limitation the rights
 *to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 *copies of the Software, and to permit persons to whom the Software is
 *furnished to do so, subject to the following conditions:

 *The above copyright notice and this permission notice shall be included in all
 *copies or substantial portions of the Software.

 *THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 *IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 *FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 *AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 *LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 *OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 *SOFTWARE.
 *
 *模块说明：操作日志类
 */
using SqlSugar;
using System;

namespace EOM.TSHotelManagement.Common.Core
{
    /// <summary>
    /// 日志等级 (Log Level)
    /// </summary>
    public enum LogLevel
    {
        /// <summary>
        /// 普通警告 (Normal Warning)
        /// </summary>
        Normal = 100,

        /// <summary>
        /// 严重警告 (Serious Warning)
        /// </summary>
        Warning = 200,

        /// <summary>
        /// 危险警告 (Critical Warning)
        /// </summary>
        Critical = 300
    }

    /// <summary>
    /// 操作日志表 (Operation Log)
    /// </summary>
    [SugarTable("operation_log", "操作日志表 (Operation Log)")]
    public class OperationLog : BaseEntity
    {
        /// <summary>
        /// 编号 (ID)
        /// </summary>
        [SugarColumn(ColumnName = "id", IsIdentity = true, IsPrimaryKey = true, IsNullable = false, ColumnDescription = "编号 (ID)")]
        public int Id { get; set; }

        /// <summary>
        /// 日志ID (Log ID)
        /// </summary>
        [SugarColumn(
            ColumnName = "operation_number",
            IsPrimaryKey = true,
            ColumnDescription = "日志ID (Log ID)",
            IsNullable = false,                      
            Length = 128                             
        )]
        public string OperationId { get; set; }

        /// <summary>
        /// 操作时间 (Operation Time)
        /// </summary>
        [SugarColumn(
            ColumnName = "operation_time",
            ColumnDescription = "操作时间 (Operation Time)",
            IsNullable = false                       
        )]
        public DateTime OperationTime { get; set; }

        /// <summary>
        /// 操作信息 (Log Content)
        /// </summary>
        [SugarColumn(
            ColumnName = "log_content",
            ColumnDescription = "操作信息 (Log Content)",
            IsNullable = false,                      
            Length = 2000                            
        )]
        public string LogContent { get; set; }

        /// <summary>
        /// 操作账号 (Operation Account)
        /// </summary>
        [SugarColumn(
            ColumnName = "operation_account",
            ColumnDescription = "操作账号 (Operation Account)",
            IsNullable = false,                      
            Length = 50                              
        )]
        public string OperationAccount { get; set; }

        /// <summary>
        /// 日志等级 (Log Level)
        /// </summary>
        [SugarColumn(
            ColumnName = "operation_level",
            ColumnDescription = "日志等级枚举值 (Log Level Enum)",
            IsNullable = false                       
        )]
        public LogLevel LogLevel { get; set; }

        /// <summary>
        /// 软件版本 (Software Version)
        /// </summary>
        [SugarColumn(
            ColumnName = "software_version",
            ColumnDescription = "软件版本号 (Software Version)",
            IsNullable = true,                       
            Length = 50                              
        )]
        public string SoftwareVersion { get; set; }

        /// <summary>
        /// 登录IP (Login IP Address)
        /// </summary>
        [SugarColumn(
            ColumnName = "login_ip",
            ColumnDescription = "登录IP地址 (Login IP Address)",
            IsNullable = false,                       
            Length = 45                               
        )]
        public string LoginIpAddress { get; set; }

        /// <summary>
        /// 日志等级名称 (Log Level Name)（不存储到数据库）
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public string LogLevelName { get; set; }

        /// <summary>
        /// 请求路径 (Request Path)
        /// </summary>
        [SugarColumn(
            ColumnName = "request_path",
            ColumnDescription = "请求路径 (Request Path)",
            IsNullable = false,                      
            Length = 500                             
        )]
        public string RequestPath { get; set; }

        /// <summary>
        /// 查询字符串 (Query String)
        /// </summary>
        [SugarColumn(
            ColumnName = "query_string",
            ColumnDescription = "查询参数 (Query String)",
            IsNullable = true,                       
            Length = 2000                            
        )]
        public string QueryString { get; set; }

        /// <summary>
        /// 响应时间 (Elapsed Time)
        /// </summary>
        [SugarColumn(
            ColumnName = "elapsed_time",
            ColumnDescription = "响应时间（毫秒） (Elapsed Time in ms)",
            IsNullable = false,                      
            DefaultValue = "0"                         
        )]
        public long ElapsedTime { get; set; }

        /// <summary>
        /// 请求方法 (Http Method)
        /// </summary>
        [SugarColumn(
            ColumnName = "http_method",
            ColumnDescription = "HTTP请求方法 (HTTP Method)",
            IsNullable = false,                      
            Length = 10                              
        )]
        public string HttpMethod { get; set; }

        /// <summary>
        /// 状态码 (Status Code)
        /// </summary>
        [SugarColumn(
            ColumnName = "status_code",
            ColumnDescription = "HTTP状态码 (HTTP Status Code)",
            IsNullable = false,                      
            DefaultValue = "200"                       
        )]
        public int StatusCode { get; set; }

        /// <summary>
        /// 异常消息 (Exception Message)
        /// </summary>
        [SugarColumn(
            ColumnName = "exception_message",
            ColumnDescription = "异常消息内容 (Exception Message)",
            IsNullable = true,                       
            Length = 2000                            
        )]
        public string ExceptionMessage { get; set; }

        /// <summary>
        /// 异常堆栈 (Exception Stack Trace)
        /// </summary>
        [SugarColumn(
            ColumnName = "exception_stacktrace",
            ColumnDescription = "异常堆栈信息 (Exception Stack Trace)",
            IsNullable = true,                       
            Length = 4000                            
        )]
        public string ExceptionStackTrace { get; set; }
    }
}
