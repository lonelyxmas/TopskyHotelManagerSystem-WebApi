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
 */
using EOM.TSHotelManagement.Common.Contract;
using EOM.TSHotelManagement.Common.Core;
using EOM.TSHotelManagement.Common.Util;
using EOM.TSHotelManagement.EntityFramework;
using SqlSugar;

namespace EOM.TSHotelManagement.Application
{
    /// <summary>
    /// 资产信息接口实现类
    /// </summary>
    public class AssetService : IAssetService
    {
        /// <summary>
        /// 资产信息
        /// </summary>
        private readonly GenericRepository<Asset> assetRepository;

        /// <summary>
        /// 部门
        /// </summary>
        private readonly GenericRepository<Department> deptRepository;

        /// <summary>
        /// 员工
        /// </summary>
        private readonly GenericRepository<Employee> employeeRepository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="assetRepository"></param>
        /// <param name="deptRepository"></param>
        /// <param name="employeeRepository"></param>
        public AssetService(GenericRepository<Asset> assetRepository, GenericRepository<Department> deptRepository, GenericRepository<Employee> employeeRepository)
        {
            this.assetRepository = assetRepository;
            this.deptRepository = deptRepository;
            this.employeeRepository = employeeRepository;
        }

        /// <summary>
        /// 添加资产信息
        /// </summary>
        /// <param name="asset"></param>
        /// <returns></returns>
        public BaseOutputDto AddAssetInfo(CreateAssetInputDto asset)
        {
            try
            {
                if (assetRepository.IsAny(a => a.AssetNumber == asset.AssetNumber))
                {
                    return new BaseOutputDto() { Message = LocalizationHelper.GetLocalizedString("the asset number already exists.", "资产编号已存在"), StatusCode = StatusCodeConstants.InternalServerError };
                }
                var result = assetRepository.Insert(EntityMapper.Map<CreateAssetInputDto, Asset>(asset));
            }
            catch (Exception ex)
            {
                return new BaseOutputDto() { Message = LocalizationHelper.GetLocalizedString(ex.Message, ex.Message), StatusCode = StatusCodeConstants.InternalServerError };
            }
            return new BaseOutputDto();
        }

        /// <summary>
        /// 查询资产信息
        /// </summary>
        /// <returns></returns>
        public ListOutputDto<ReadAssetOutputDto> SelectAssetInfoAll(ReadAssetInputDto asset)
        {
            //查询所有部门信息
            List<Department> depts = new List<Department>();
            depts = deptRepository.GetList(a => a.IsDelete != 1);
            //查询所有员工信息
            List<Employee> employees = new List<Employee>();
            employees = employeeRepository.GetList(a => a.IsDelete != 1);

            ListOutputDto<ReadAssetOutputDto> cs = new ListOutputDto<ReadAssetOutputDto>();

            var where = Expressionable.Create<Asset>();

            where = where.And(a => a.IsDelete == asset.IsDelete);

            int totalCount = 0;
            if (asset.Page != 0 && asset.PageSize != 0)
            {
                cs.listSource = EntityMapper.MapList<Asset, ReadAssetOutputDto>(assetRepository.AsQueryable().Where(where.ToExpression()).OrderBy(a => a.AssetNumber)
                    .ToPageList((int)asset.Page, (int)asset.PageSize, ref totalCount));
                cs.total = totalCount;
            }

            cs.listSource.ForEach(source =>
            {
                var dept = depts.SingleOrDefault(a => a.DepartmentNumber.Equals(source.DepartmentCode));
                source.DepartmentName = dept == null ? "" : dept.DepartmentName;
                var worker = employees.SingleOrDefault(a => a.EmployeeId.Equals(source.AcquiredByEmployeeId));
                source.AcquiredByEmployeeName = worker == null ? "" : worker.EmployeeName;

                source.AssetValueFormatted = source.AssetValue == 0 ? "" : Decimal.Parse(source.AssetValue.ToString()).ToString("#,##0.00").ToString();

            });

            return cs;
        }

        /// <summary>
        /// 更新资产信息
        /// </summary>
        /// <param name="asset"></param>
        /// <returns></returns>
        public BaseOutputDto UpdAssetInfo(UpdateAssetInputDto asset)
        {
            try
            {
                if (!assetRepository.IsAny(a => a.AssetNumber == asset.AssetNumber))
                {
                    return new BaseOutputDto() { Message = LocalizationHelper.GetLocalizedString("asset number does not exist.", "资产编号不存在"), StatusCode = StatusCodeConstants.InternalServerError };
                }
                assetRepository.Update(EntityMapper.Map<UpdateAssetInputDto, Asset>(asset));
            }
            catch (Exception ex)
            {
                return new BaseOutputDto() { Message = LocalizationHelper.GetLocalizedString(ex.Message, ex.Message), StatusCode = StatusCodeConstants.InternalServerError };
            }
            return new BaseOutputDto();
        }

        /// <summary>
        /// 删除资产信息
        /// </summary>
        /// <param name="asset"></param>
        /// <returns></returns>
        public BaseOutputDto DelAssetInfo(DeleteAssetInputDto asset)
        {
            try
            {
                if (!assetRepository.IsAny(a => a.AssetNumber == asset.AssetNumber))
                {
                    return new BaseOutputDto() { Message = LocalizationHelper.GetLocalizedString("asset number does not exist.", "资产编号不存在"), StatusCode = StatusCodeConstants.InternalServerError };
                }
                assetRepository.Update(a=>new Asset { IsDelete = asset.IsDelete},a=> a.AssetNumber == asset.AssetNumber);
            }
            catch (Exception ex)
            {
                return new BaseOutputDto() { Message = LocalizationHelper.GetLocalizedString(ex.Message, ex.Message), StatusCode = StatusCodeConstants.InternalServerError };
            }
            return new BaseOutputDto();
        }
    }
}
