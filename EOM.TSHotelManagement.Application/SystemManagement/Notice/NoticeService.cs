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
using jvncorelib.EntityLib;
using SqlSugar;

namespace EOM.TSHotelManagement.Application
{
    /// <summary>
    /// 公告信息接口实现类
    /// </summary>
    public class NoticeService : INoticeService
    {
        /// <summary>
        /// 公告
        /// </summary>
        private readonly GenericRepository<AppointmentNotice> noticeRepository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="noticeRepository"></param>
        public NoticeService(GenericRepository<AppointmentNotice> noticeRepository)
        {
            this.noticeRepository = noticeRepository;
        }

        /// <summary>
        /// 获取所有公告信息
        /// </summary>
        /// <returns></returns>
        public ListOutputDto<ReadAppointmentNoticeOutputDto> SelectNoticeAll(ReadAppointmentNoticeInputDto readAppointmentNoticeInputDto)
        {
            List<AppointmentNotice> ntc = new List<AppointmentNotice>();
            var where = Expressionable.Create<AppointmentNotice>();
            if (!string.IsNullOrEmpty(readAppointmentNoticeInputDto.NoticeTheme))
            {
                where = where.And(a => a.NoticeTheme.Contains(readAppointmentNoticeInputDto.NoticeTheme));
            }
            if (!string.IsNullOrEmpty(readAppointmentNoticeInputDto.NoticeType))
            {
                where = where.And(a => a.NoticeType == readAppointmentNoticeInputDto.NoticeType);
            }
            if (!readAppointmentNoticeInputDto.IsDelete.IsNullOrEmpty())
            {
                where = where.And(a => a.IsDelete == readAppointmentNoticeInputDto.IsDelete);
            }
            var count = 0;
            ntc = noticeRepository.AsQueryable().Where(where.ToExpression()).ToPageList(readAppointmentNoticeInputDto.Page, readAppointmentNoticeInputDto.PageSize, ref count);
            ntc.ForEach(source =>
            {
                switch (source.NoticeType)
                {
                    case "PersonnelChanges":
                        source.NoticeTypeDescription = "人事变动";
                        break;
                    case "GeneralNotice":
                        source.NoticeTypeDescription = "普通公告";
                        break;
                }
            });

            var listSource = EntityMapper.MapList<AppointmentNotice, ReadAppointmentNoticeOutputDto>(ntc);

            return new ListOutputDto<ReadAppointmentNoticeOutputDto>
            {
                listSource = listSource,
                total = listSource.Count
            };
        }

        /// <summary>
        /// 根据公告编号查找公告信息
        /// </summary>
        /// <param name="readAppointmentNoticeInputDto"></param>
        /// <returns></returns>
        public ReadAppointmentNoticeOutputDto SelectNoticeByNoticeNo(ReadAppointmentNoticeInputDto readAppointmentNoticeInputDto)
        {
            AppointmentNotice notice = new AppointmentNotice();
            notice = noticeRepository.GetSingle(a => a.NoticeNumber == readAppointmentNoticeInputDto.NoticeId);
            switch (notice.NoticeType)
            {
                case "PersonnelChanges":
                    notice.NoticeTypeDescription = "人事变动";
                    break;
                case "GeneralNotice":
                    notice.NoticeTypeDescription = "普通公告";
                    break;
            }
            var source = EntityMapper.Map<AppointmentNotice, ReadAppointmentNoticeOutputDto>(notice);

            return source;
        }

        /// <summary>
        /// 上传公告信息
        /// </summary>
        /// <param name="notice"></param>
        /// <returns></returns>
        public BaseOutputDto InsertNotice(CreateAppointmentNoticeInputDto notice)
        {
            try
            {
                noticeRepository.Insert(EntityMapper.Map<CreateAppointmentNoticeInputDto, AppointmentNotice>(notice));
            }
            catch (Exception ex)
            {
                return new BaseOutputDto { Message = LocalizationHelper.GetLocalizedString(ex.Message, ex.Message), StatusCode = StatusCodeConstants.InternalServerError };
            }
            return new BaseOutputDto();
        }

        /// <summary>
        /// 删除公告信息
        /// </summary>
        /// <param name="deleteAppointmentNoticeInputDto"></param>
        /// <returns></returns>
        public BaseOutputDto DeleteNotice(DeleteAppointmentNoticeInputDto deleteAppointmentNoticeInputDto)
        {
            try
            {
                noticeRepository.Update(EntityMapper.Map<DeleteAppointmentNoticeInputDto, AppointmentNotice>(deleteAppointmentNoticeInputDto));
            }
            catch (Exception ex)
            {
                LogHelper.LogError(LocalizationHelper.GetLocalizedString("delete appointment notice failed","删除公告失败"),ex);
                return new BaseOutputDto { Message = LocalizationHelper.GetLocalizedString("delete appointment notice failed", "删除公告失败"), StatusCode = StatusCodeConstants.InternalServerError };
            }
            return new BaseOutputDto();
        }

        /// <summary>
        /// 更新公告信息
        /// </summary>
        /// <param name="updateAppointmentNoticeInputDto"></param>
        /// <returns></returns>
        public BaseOutputDto UpdateNotice(UpdateAppointmentNoticeInputDto updateAppointmentNoticeInputDto)
        {
            try
            {
                noticeRepository.Update(EntityMapper.Map<UpdateAppointmentNoticeInputDto, AppointmentNotice>(updateAppointmentNoticeInputDto));
            }
            catch (Exception ex)
            {
                LogHelper.LogError(LocalizationHelper.GetLocalizedString("update appointment notice failed", "更新公告失败"), ex);
                return new BaseOutputDto { Message = LocalizationHelper.GetLocalizedString("update appointment notice failed", "更新公告失败"), StatusCode = StatusCodeConstants.InternalServerError };
            }
            return new BaseOutputDto();
        }

    }
}
