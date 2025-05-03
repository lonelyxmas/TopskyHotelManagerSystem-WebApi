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
 *模块说明：房间类
 */
using EOM.TSHotelManagement.Common.Util;
using SqlSugar;
using System;

namespace EOM.TSHotelManagement.Common.Core
{
    /// <summary>
    /// 酒店房间信息表 (Hotel Room Information)
    /// </summary>
    [SugarTable("room", "酒店房间信息表 (Hotel Room Information)")]
    public class Room : BaseEntity
    {
        /// <summary>
        /// 编号 (ID)
        /// </summary>
        [SugarColumn(ColumnName = "id", IsIdentity = true, IsPrimaryKey = true, IsNullable = false, ColumnDescription = "编号 (ID)")]
        public int Id { get; set; }

        /// <summary>
        /// 房间编号 (Room Number)
        /// </summary>
        [SugarColumn(
            ColumnName = "room_no",
            IsPrimaryKey = true,
            ColumnDescription = "房间唯一编号 (Unique Room Number)",
            IsNullable = false,                     
            Length = 128                            
        )]
        [NeedValid]  // 假设此特性用于业务验证
        public string RoomNumber { get; set; }

        /// <summary>
        /// 房间类型ID (Room Type ID)
        /// </summary>
        [SugarColumn(
            ColumnName = "room_type",
            ColumnDescription = "房间类型ID (关联房间类型表)",
            IsNullable = false                  
        )]
        [NeedValid]
        public int RoomTypeId { get; set; }

        /// <summary>
        /// 客户编号 (Customer Number)
        /// </summary>
        [SugarColumn(
            ColumnName = "custo_no",
            ColumnDescription = "当前入住客户编号 (Linked Customer ID)",
            IsNullable = true,                       
            Length = 128                            
        )]
        public string CustomerNumber { get; set; }

        /// <summary>
        /// 客户姓名 (Customer Name)（不存储到数据库）
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public string CustomerName { get; set; }

        /// <summary>
        /// 最后一次入住时间 (Last Check-In Time)
        /// </summary>
        [SugarColumn(
            ColumnName = "check_in_time",
            ColumnDescription = "最后一次入住时间 (Last Check-In Time)",
            IsNullable = true                       
        )]
        public DateTime? LastCheckInTime { get; set; }

        /// <summary>
        /// 最后一次退房时间 (Last Check-Out Time)
        /// </summary>
        [SugarColumn(
            ColumnName = "check_out_time",
            ColumnDescription = "最后一次退房时间 (Last Check-Out Time)",
            IsNullable = true                        
        )]
        public DateTime? LastCheckOutTime { get; set; }

        /// <summary>
        /// 房间状态ID (Room State ID)
        /// </summary>
        [SugarColumn(
            ColumnName = "room_state_id",
            ColumnDescription = "房间状态ID (如0-空闲/1-已入住)",
            IsNullable = false                      
        )]
        [NeedValid]
        public int RoomStateId { get; set; }

        /// <summary>
        /// 房间状态名称 (Room State Name)（不存储到数据库）
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public string RoomState { get; set; }

        /// <summary>
        /// 房间单价 (Room Rent)
        /// </summary>
        [SugarColumn(
            ColumnName = "room_rent",
            ColumnDescription = "房间单价（单位：元） (Price per Night in CNY)",
            IsNullable = false,                      
            DecimalDigits = 2                        
        )]
        [NeedValid]
        public decimal RoomRent { get; set; }

        /// <summary>
        /// 房间押金 (Room Deposit)
        /// </summary>
        [SugarColumn(
            ColumnName = "room_deposit",
            ColumnDescription = "房间押金（单位：元） (Deposit Amount in CNY)",
            IsNullable = false,                      
            DecimalDigits = 2,                       
            DefaultValue = "0.00"                      
        )]
        [NeedValid]
        public decimal RoomDeposit { get; set; }

        /// <summary>
        /// 房间位置 (Room Location)
        /// </summary>
        [SugarColumn(
            ColumnName = "room_position",
            ColumnDescription = "房间位置描述 (如楼层+门牌号)",
            IsNullable = false,                      
            Length = 200                             
        )]
        [NeedValid]
        public string RoomLocation { get; set; }

        /// <summary>
        /// 客户类型名称 (Customer Type Name)（不存储到数据库）
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public string CustomerTypeName { get; set; }

        /// <summary>
        /// 房间名称 (Room Name)（不存储到数据库）
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public string RoomName { get; set; }

        /// <summary>
        /// 最后一次入住时间（格式化字符串） (Last Check-In Time Formatted)
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public string LastCheckInTimeFormatted { get; set; }
    }

}
