/********************************************************************************

** auth： DongliangYi

** date： 2016/8/29 10:08:46

** desc：Dto接口

** Ver.:  V1.0.0

*********************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using NPlatform.Result;

namespace NPlatform.DTO
{

    /// <summary>
    /// 分页查询条件提交用的DTO
    /// </summary>
    public class PageParamDTO:IDTO
    {

        /// <summary>
        /// 页码
        /// </summary>
        [Required(ErrorMessage = "Page 不能为空！")]
        [Range(1,int.MaxValue,ErrorMessage ="页码大小必须大于1")]
        public int Page { get; set; }

        /// <summary>
        /// 排序字段
        /// </summary>
        public string SortField { get; set; }
        /// <summary>
        /// 排序方式 ascend  ,descend
        /// </summary>
        public string SortOrder { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "页大小必须大于1")]
        [Required(ErrorMessage ="页大小不能为空!")]
        /// <summary>
        /// 页大小
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 校验数据
        /// </summary>
        /// <returns></returns>
        public ValidateResult Validate()
        {
            return this.Validates();
        }
    }
}
