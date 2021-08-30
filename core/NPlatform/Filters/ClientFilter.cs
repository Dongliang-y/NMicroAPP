namespace NPlatform.Filters
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    /// <summary>
    /// 客户端过滤器
    /// </summary>
    public class ClientFilter : BaseFilter, IQueryFilter
    {
        /// <summary>
        /// 过滤表达式
        /// </summary>
        /// <typeparam name="T">要过滤的实体类型</typeparam>
        /// <returns>返回过滤表达式</returns>
        public override Expression<Func<T, bool>> GetFilter<T>()
        {
            var clientId = string.Empty;
            if (this.FilterParameters.ContainsKey(DataFilterParameters.ClientId))
            {
                clientId = this.FilterParameters[DataFilterParameters.ClientId].ToString().TrimNull();
            }

            if (typeof(IClient).IsAssignableFrom(typeof(T)) && !clientId.IsNullOrEmpty())
            {
                Expression<Func<T, bool>> filter = t =>
                    (t as IClient).ClientId == clientId;
                return filter;
            }

            return null;
        }

        /// <summary>
        /// 设置参数
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="item">实体</param>
        public override void SetFilterProperty<T>(T item)
        {
            var clientId = string.Empty;
            if (this.FilterParameters.ContainsKey(DataFilterParameters.ClientId))
            {
                clientId = this.FilterParameters[DataFilterParameters.ClientId].ToString().TrimNull();
            }

            if (typeof(IClient).IsAssignableFrom(typeof(T)) && !clientId.IsNullOrEmpty())
            {
                (item as IClient).ClientId = this.FilterParameters[DataFilterParameters.ClientId].ToString();
            }
        }
    }
}