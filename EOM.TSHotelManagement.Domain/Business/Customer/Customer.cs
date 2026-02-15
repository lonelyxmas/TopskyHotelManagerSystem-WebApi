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
 *模块说明：客户信息类
 */
using SqlSugar;
using System;

namespace EOM.TSHotelManagement.Domain
{
    /// <summary>
    /// 客户信息
    /// </summary>
    [SqlSugar.SugarTable("customer")]
    public class Customer : BaseEntity
    {
        /// <summary>
        /// 编号 (ID)
        /// </summary>
        [SugarColumn(ColumnName = "id", IsIdentity = true, IsPrimaryKey = true, IsNullable = false, ColumnDescription = "编号 (ID)")]
        public int Id { get; set; }

        /// <summary>
        /// 客户编号 (Customer Number)
        /// </summary>
        [SqlSugar.SugarColumn(ColumnName = "custo_no", IsPrimaryKey = true, IsNullable = false, Length = 128, ColumnDescription = "客户编号 (Customer Number)")]
        public string CustomerNumber { get; set; }

        /// <summary>
        /// 客户名称 (Customer Name)
        /// </summary>
        [SqlSugar.SugarColumn(ColumnName = "custo_name", IsNullable = false, Length = 250, ColumnDescription = "客户名称 (Customer Name)")]
        public string CustomerName { get; set; }

        /// <summary>
        /// 客户性别 (Customer Gender)
        /// </summary>
        [SqlSugar.SugarColumn(ColumnName = "custo_gender", IsNullable = false, ColumnDescription = "客户性别 (Customer Gender)")]
        public int? CustomerGender { get; set; }

        /// <summary>
        /// 证件类型 (Passport Type)
        /// </summary>
        [SqlSugar.SugarColumn(ColumnName = "passport_type", IsNullable = false, ColumnDescription = "客户性别 (Customer Gender)")]
        public int PassportId { get; set; }

        /// <summary>
        /// 性别 (Gender)
        /// </summary>
        [SqlSugar.SugarColumn(IsIgnore = true)]
        public string GenderName { get; set; }

        /// <summary>
        /// 客户电话 (Customer Phone Number)
        /// </summary>
        [SqlSugar.SugarColumn(ColumnName = "custo_tel", IsNullable = false, Length = 256, ColumnDescription = "客户电话 (Customer Phone Number)")]
        public string CustomerPhoneNumber { get; set; }

        /// <summary>
        /// 出生日期 (Date of Birth)
        /// </summary>
        [SqlSugar.SugarColumn(ColumnName = "custo_birth", IsNullable = false, ColumnDescription = "出生日期 (Date of Birth)")]
        public DateOnly DateOfBirth { get; set; }

        /// <summary>
        /// 客户类型名称 (Customer Type Name)
        /// </summary>
        [SqlSugar.SugarColumn(IsIgnore = true)]
        public string CustomerTypeName { get; set; }

        /// <summary>
        /// 证件类型名称 (Passport Type Name)
        /// </summary>
        [SqlSugar.SugarColumn(IsIgnore = true)]
        public string PassportName { get; set; }

        /// <summary>
        /// 证件号码 (Passport ID)
        /// </summary>
        [SqlSugar.SugarColumn(ColumnName = "passport_id", IsNullable = false, Length = 256, ColumnDescription = "证件号码 (Passport ID)")]
        public string IdCardNumber { get; set; }

        /// <summary>
        /// 居住地址 (Customer Address)
        /// </summary>
        [SqlSugar.SugarColumn(ColumnName = "custo_address", IsNullable = true, Length = 256, ColumnDescription = "居住地址 (Customer Address)")]
        public string CustomerAddress { get; set; }

        /// <summary>
        /// 客户类型 (Customer Type)
        /// </summary>
        [SqlSugar.SugarColumn(ColumnName = "custo_type", IsNullable = false, ColumnDescription = "客户类型 (Customer Type)")]
        public int CustomerType { get; set; }
    }
}
