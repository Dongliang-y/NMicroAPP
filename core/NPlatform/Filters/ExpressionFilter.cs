using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ZJJWEPlatform.Domains.Entity;

namespace ZJJWEPlatform.Filters
{
    /// <summary>
    /// 表达式过滤器，
    /// 1.属于事后过滤器，对已有的集合进行统一的过滤。
    /// 集合对象类型属于
    /// </summary>
    public class ExpressionFilter : BaseFilter, IResultFilter
    {
        private Expression<Func<string, IEntity, bool>> Expression;
        /// <summary>
        /// 条件过滤器
        /// </summary>
        /// <param name="expression">Lambda表达式条件的字符串格式</param>
        public ExpressionFilter(Expression<Func<string,IEntity, bool>> expression)
        {
            Expression = expression;
        }

        /// <summary>
        /// 过滤筛选表达式
        /// </summary>
        /// <typeparam name="T">处理对象的类型</typeparam>
        /// <returns></returns>
        public Expression<Func<string, IEntity, bool>> GetFilter<T>()
        {
            return Expression;
        }

        /// <summary>
        /// 此方法在事后过滤器中不适用
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        public override void SetFilterProperty<T>(T item)
        {
            throw new Exception("此方法在事后过滤器中不适用");
        }
    }
}
