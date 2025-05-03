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

namespace EOM.TSHotelManagement.Application
{
    /// <summary>
    /// 酒店宣传联动内容接口实现类
    /// </summary>
    public class PromotionContentService : IPromotionContentService
    {
        /// <summary>
        /// 跑马灯
        /// </summary>
        private readonly GenericRepository<PromotionContent> fontsRepository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fontsRepository"></param>
        public PromotionContentService(GenericRepository<PromotionContent> fontsRepository)
        {
            this.fontsRepository = fontsRepository;
        }

        /// <summary>
        /// 查询所有宣传联动内容
        /// </summary>
        /// <returns></returns>
        public ListOutputDto<ReadPromotionContentOutputDto> SelectPromotionContentAll(ReadPromotionContentInputDto readPromotionContentInputDto)
        {
            ListOutputDto<ReadPromotionContentOutputDto> fonts = new ListOutputDto<ReadPromotionContentOutputDto>();
            var count = 0;

            var listSource = fontsRepository.AsQueryable().ToPageList(readPromotionContentInputDto.Page, readPromotionContentInputDto.PageSize, ref count);

            fonts.listSource = EntityMapper.MapList<PromotionContent, ReadPromotionContentOutputDto>(listSource);
            fonts.total = count;
            return fonts;
        }

        /// <summary>
        /// 查询所有宣传联动内容(跑马灯)
        /// </summary>
        /// <returns></returns>
        public ListOutputDto<ReadPromotionContentOutputDto> SelectPromotionContents()
        {
            ListOutputDto<ReadPromotionContentOutputDto> fonts = new ListOutputDto<ReadPromotionContentOutputDto>();
            var listSource = fontsRepository.AsQueryable().ToList();
            fonts.listSource = EntityMapper.MapList<PromotionContent, ReadPromotionContentOutputDto>(listSource);
            return fonts;
        }

        /// <summary>
        /// 添加宣传联动内容
        /// </summary>
        /// <param name="createPromotionContentInputDto"></param>
        /// <returns></returns>
        public BaseOutputDto AddPromotionContent(CreatePromotionContentInputDto createPromotionContentInputDto)
        {
            try
            {
                fontsRepository.Insert(EntityMapper.Map<CreatePromotionContentInputDto, PromotionContent>(createPromotionContentInputDto));
            }
            catch (Exception ex)
            {
                LogHelper.LogError(LocalizationHelper.GetLocalizedString("Insert Promotion Content Failed", "宣传联动内容添加失败"), ex);
                return new BaseOutputDto(StatusCodeConstants.InternalServerError, LocalizationHelper.GetLocalizedString("Insert Promotion Content Failed", "宣传联动内容添加失败"));
            }

            return new BaseOutputDto(StatusCodeConstants.Success, LocalizationHelper.GetLocalizedString("Insert Promotion Content Success", "宣传联动内容添加成功"));
        }

        /// <summary>
        /// 删除宣传联动内容
        /// </summary>
        /// <param name="deletePromotionContentInputDto"></param>
        /// <returns></returns>
        public BaseOutputDto DeletePromotionContent(DeletePromotionContentInputDto deletePromotionContentInputDto)
        {
            try
            {
                fontsRepository.Update(EntityMapper.Map<DeletePromotionContentInputDto, PromotionContent>(deletePromotionContentInputDto));
            }
            catch (Exception ex)
            {
                LogHelper.LogError(LocalizationHelper.GetLocalizedString("Delete Promotion Content Failed", "宣传联动内容删除失败"), ex);
                return new BaseOutputDto(StatusCodeConstants.InternalServerError, LocalizationHelper.GetLocalizedString("Delete Promotion Content Failed", "宣传联动内容删除失败"));
            }

            return new BaseOutputDto(StatusCodeConstants.Success, LocalizationHelper.GetLocalizedString("Delete Promotion Content Success", "宣传联动内容删除成功"));
        }

        /// <summary>
        /// 更新宣传联动内容
        /// </summary>
        /// <param name="updatePromotionContentInputDto"></param>
        /// <returns></returns>
        public BaseOutputDto UpdatePromotionContent(UpdatePromotionContentInputDto updatePromotionContentInputDto)
        {
            try
            {
                fontsRepository.Update(EntityMapper.Map<UpdatePromotionContentInputDto, PromotionContent>(updatePromotionContentInputDto));
            }
            catch (Exception ex)
            {
                LogHelper.LogError(LocalizationHelper.GetLocalizedString("Update Promotion Content Failed", "宣传联动内容更新失败"), ex);
                return new BaseOutputDto(StatusCodeConstants.InternalServerError, LocalizationHelper.GetLocalizedString("Update Promotion Content Failed", "宣传联动内容更新失败"));
            }

            return new BaseOutputDto(StatusCodeConstants.Success, LocalizationHelper.GetLocalizedString("Update Promotion Content Success", "宣传联动内容更新成功"));

        }
    }
}
