using EOM.TSHotelManagement.Common.Contract;
using EOM.TSHotelManagement.Common.Core;
using EOM.TSHotelManagement.Common.Util;
using EOM.TSHotelManagement.EntityFramework;
using jvncorelib.EntityLib;
using Microsoft.AspNetCore.Http;

namespace EOM.TSHotelManagement.Application
{
    /// <summary>
    /// 员工照片接口实现类
    /// </summary>
    public class EmployeePhotoService : IEmployeePhotoService
    {
        /// <summary>
        /// 员工照片
        /// </summary>
        private readonly GenericRepository<EmployeePhoto> workerPicRepository;

        /// <summary>
        /// 兰空图床帮助类
        /// </summary>
        private readonly LskyHelper lskyHelper;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="workerPicRepository"></param>
        /// <param name="lskyHelper"></param>
        public EmployeePhotoService(GenericRepository<EmployeePhoto> workerPicRepository, LskyHelper lskyHelper)
        {
            this.workerPicRepository = workerPicRepository;
            this.lskyHelper = lskyHelper;
        }

        /// <summary>
        /// 查询员工照片
        /// </summary>
        /// <param name="readEmployeePhotoInputDto"></param>
        /// <returns></returns>
        public SingleOutputDto<ReadEmployeePhotoOutputDto> EmployeePhoto(ReadEmployeePhotoInputDto readEmployeePhotoInputDto)
        {
            var workerPicSource = new EmployeePhoto();

            workerPicSource = workerPicRepository.GetSingle(a => a.EmployeeId.Equals(readEmployeePhotoInputDto.EmployeeId));

            if (workerPicSource.IsNullOrEmpty())
            {
                return new SingleOutputDto<ReadEmployeePhotoOutputDto>
                {
                    Source = new ReadEmployeePhotoOutputDto
                    {
                        PhotoId = 0,
                        PhotoPath = "",
                        EmployeeId = readEmployeePhotoInputDto.EmployeeId
                    }
                };
            }

            return new SingleOutputDto<ReadEmployeePhotoOutputDto>
            {
                Source = EntityMapper.Map<EmployeePhoto, ReadEmployeePhotoOutputDto>(workerPicSource)
            };
        }
        /// <summary>
        /// 添加员工照片
        /// </summary>
        /// <param name="createEmployeePhotoInputDto"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        public SingleOutputDto<ReadEmployeePhotoOutputDto> InsertWorkerPhoto(CreateEmployeePhotoInputDto createEmployeePhotoInputDto, IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    return new SingleOutputDto<ReadEmployeePhotoOutputDto> { Message = LocalizationHelper.GetLocalizedString("File cannot null", "文件不能为空"), StatusCode = StatusCodeConstants.BadRequest };
                }

                if (file.Length > 1048576)
                {
                    return new SingleOutputDto<ReadEmployeePhotoOutputDto>
                    {
                        Message = LocalizationHelper.GetLocalizedString("Image size exceeds 1MB limit", "图片大小不能超过1MB"),
                        StatusCode = StatusCodeConstants.BadRequest
                    };
                }
                if (file.ContentType != "image/jpeg" && file.ContentType != "image/png")
                {
                    return new SingleOutputDto<ReadEmployeePhotoOutputDto>
                    {
                        Message = LocalizationHelper.GetLocalizedString("Invalid image format", "图片格式不正确"),
                        StatusCode = StatusCodeConstants.BadRequest
                    };
                }

                var token = lskyHelper.GetImageStorageTokenAsync().Result;
                if (string.IsNullOrEmpty(token))
                {
                    return new SingleOutputDto<ReadEmployeePhotoOutputDto> { Message = LocalizationHelper.GetLocalizedString("Get Token Fail", "获取Token失败"), StatusCode = StatusCodeConstants.InternalServerError };
                }

                using var stream = file.OpenReadStream();
                stream.Seek(0, SeekOrigin.Begin);
                string imageUrl = lskyHelper.UploadImageAsync(
                    fileStream: stream,
                    fileName: file.FileName,
                    contentType: file.ContentType,
                    token: token
                ).Result;
                if (string.IsNullOrEmpty(imageUrl))
                {
                    return new SingleOutputDto<ReadEmployeePhotoOutputDto> { Message = "图片上传失败", StatusCode = StatusCodeConstants.InternalServerError };
                }

                var workerPicSource = workerPicRepository.GetSingle(a => a.EmployeeId.Equals(createEmployeePhotoInputDto.EmployeeId));
                if (workerPicSource.IsNullOrEmpty())
                {
                    workerPicRepository.Insert(new EmployeePhoto
                    {
                        EmployeeId = createEmployeePhotoInputDto.EmployeeId,
                        PhotoPath = imageUrl
                    });
                }
                else
                {
                    workerPicRepository.Update(a => new EmployeePhoto
                    {
                        PhotoPath = imageUrl
                    }, a => a.EmployeeId.Equals(createEmployeePhotoInputDto.EmployeeId));
                }

                return new SingleOutputDto<ReadEmployeePhotoOutputDto>
                {
                    Source = new ReadEmployeePhotoOutputDto
                    {
                        PhotoId = 0,
                        PhotoPath = imageUrl,
                        EmployeeId = createEmployeePhotoInputDto.EmployeeId
                    }
                };
            }
            catch (Exception ex)
            {
                return new SingleOutputDto<ReadEmployeePhotoOutputDto> { Message = LocalizationHelper.GetLocalizedString(ex.Message, ex.Message), StatusCode = StatusCodeConstants.InternalServerError };
            }
        }

        /// <summary>
        /// 删除员工照片
        /// </summary>
        /// <param name="deleteEmployeePhotoInputDto"></param>
        /// <returns></returns>
        public BaseOutputDto DeleteWorkerPhoto(DeleteEmployeePhotoInputDto deleteEmployeePhotoInputDto)
        {
            try
            {
                workerPicRepository.Delete(a => a.EmployeeId.Equals(deleteEmployeePhotoInputDto.EmployeeId));
            }
            catch (Exception ex)
            {
                return new BaseOutputDto { Message = LocalizationHelper.GetLocalizedString(ex.Message, ex.Message), StatusCode = StatusCodeConstants.InternalServerError };
            }
            return new BaseOutputDto();
        }

        /// <summary>
        /// 更新员工照片
        /// </summary>
        /// <param name="updateEmployeePhotoInputDto"></param>
        /// <returns></returns>
        public BaseOutputDto UpdateWorkerPhoto(UpdateEmployeePhotoInputDto updateEmployeePhotoInputDto)
        {
            try
            {
                workerPicRepository.Update(a => new EmployeePhoto
                {
                    PhotoPath = updateEmployeePhotoInputDto.PhotoUrl
                }, a => a.EmployeeId.Equals(updateEmployeePhotoInputDto.EmployeeId));
            }
            catch (Exception ex)
            {
                return new BaseOutputDto { Message = LocalizationHelper.GetLocalizedString(ex.Message, ex.Message), StatusCode = StatusCodeConstants.InternalServerError };
            }
            return new BaseOutputDto();
        }
    }
}
