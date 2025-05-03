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

namespace EOM.TSHotelManagement.Application
{
    /// <summary>
    /// 商品接口
    /// </summary>
    public interface ISellService
    {
        /// <summary>
        /// 查询所有商品
        /// </summary>
        /// <returns></returns>
        ListOutputDto<ReadSellThingOutputDto> SelectSellThingAll(ReadSellThingInputDto sellThing);

        /// <summary>
        /// 查询所有商品(包括库存为0/已删除)
        /// </summary>
        /// <returns></returns>
        ListOutputDto<ReadSellThingOutputDto> GetSellThings(ReadSellThingInputDto sellThing);

        /// <summary>
        /// 修改商品
        /// </summary>
        /// <param name="updateSellThingInputDto"></param>
        /// <returns></returns>
        BaseOutputDto UpdateSellThing(UpdateSellThingInputDto updateSellThingInputDto);

        /// <summary>
        /// 修改商品信息
        /// </summary>
        /// <param name="sellThing"></param>
        /// <returns></returns>
        BaseOutputDto UpdateSellthingInfo(UpdateSellThingInputDto sellThing);

        /// <summary>
        /// 撤回客户消费信息
        /// </summary>
        /// <param name="deleteSellThingInputDto"></param>
        /// <returns></returns>
        BaseOutputDto DeleteSellthing(DeleteSellThingInputDto deleteSellThingInputDto);

        /// <summary>
        /// 根据商品编号删除商品信息
        /// </summary>
        /// <param name="deleteSellThingInputDto"></param>
        /// <returns></returns>
        BaseOutputDto DeleteSellThingBySellNo(DeleteSellThingInputDto deleteSellThingInputDto);

        /// <summary>
        /// 根据商品名称和价格查询商品编号
        /// </summary>
        /// <param name="readSellThingInputDto"></param>
        /// <returns></returns>
        SingleOutputDto<ReadSellThingOutputDto> SelectSellThingByNameAndPrice(ReadSellThingInputDto readSellThingInputDto);


        /// <summary>
        /// 根据商品编号查询商品信息
        /// </summary>
        /// <param name="readSellThingInputDto"></param>
        /// <returns></returns>
        ReadSellThingOutputDto SelectSellInfoBySellNo(ReadSellThingInputDto readSellThingInputDto);

        /// <summary>
        /// 添加商品
        /// </summary>
        /// <param name="st"></param>
        /// <returns></returns>
        BaseOutputDto InsertSellThing(CreateSellThingInputDto st);
    }
}