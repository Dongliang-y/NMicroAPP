namespace NPlatform
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    /// <summary>
    /// 统一ParameterExpression
    /// </summary>
    internal class ParameterReplacer : ExpressionVisitor
    {
        public ParameterReplacer(ParameterExpression paramExpr)
        {
            ParameterExpression = paramExpr;
        }

        public ParameterExpression ParameterExpression { get; private set; }

        public Expression Replace(Expression expr)
        {
            return Visit(expr);
        }

        protected override Expression VisitParameter(ParameterExpression p)
        {
            return ParameterExpression;
        }
    }

    /// <summary>
    /// Predicate扩展
    /// </summary>
    public static class PredicateExtensionses
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expLeft"></param>
        /// <param name="expRight"></param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> AndAlso<T>(
            this Expression<Func<T, bool>> expLeft,
            Expression<Func<T, bool>> expRight)
        {
            var candidateExpr = Expression.Parameter(typeof(T), "candidate");
            var parameterReplacer = new ParameterReplacer(candidateExpr);

            var left = parameterReplacer.Replace(expLeft.Body);
            var right = parameterReplacer.Replace(expRight.Body);
            var body = Expression.AndAlso(left, right);

            return Expression.Lambda<Func<T, bool>>(body, candidateExpr);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Expression<Func<T, bool>> False<T>()
        {
            return f => false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expLeft"></param>
        /// <param name="expRight"></param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> OrElse<T>(
            this Expression<Func<T, bool>> expLeft,
            Expression<Func<T, bool>> expRight)
        {
            var candidateExpr = Expression.Parameter(typeof(T), "candidate");
            var parameterReplacer = new ParameterReplacer(candidateExpr);

            var left = parameterReplacer.Replace(expLeft.Body);
            var right = parameterReplacer.Replace(expRight.Body);
            var body = Expression.OrElse(left, right);

            return Expression.Lambda<Func<T, bool>>(body, candidateExpr);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Expression<Func<T, bool>> True<T>()
        {
            return f => true;
        }
    }

    /// <summary>
    /// Queryable扩展
    /// </summary>
    public static class QueryableExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queryable"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static IQueryable<T> OrderBy<T>(this IQueryable<T> queryable, string propertyName)
        {
            return OrderBy(queryable, propertyName, false);
        }

        /// <summary>
        /// OrderBy
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="queryable">条件</param>
        /// <param name="propertyName">属性名称</param>
        /// <param name="desc">是否降序</param>
        /// <returns></returns>
        public static IQueryable<T> OrderBy<T>(this IQueryable<T> queryable, string propertyName, bool desc)
        {
            var param = Expression.Parameter(typeof(T));
            var body = Expression.Property(param, propertyName);
            dynamic keySelector = Expression.Lambda(body, param);

            return desc
                       ? Queryable.OrderByDescending(queryable, keySelector)
                       : Queryable.OrderBy(queryable, keySelector);
        }
    }
    /// <summary>
    /// CommonEqualityComparer
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="V"></typeparam>
    public class CommonEqualityComparer<T, V> : IEqualityComparer<T>
    {
        private IEqualityComparer<V> comparer;

        private Func<T, V> keySelector;
        /// <summary>
        /// CommonEqualityComparer
        /// </summary>
        /// <param name="keySelector"></param>
        /// <param name="comparer"></param>
        public CommonEqualityComparer(Func<T, V> keySelector, IEqualityComparer<V> comparer)
        {
            this.keySelector = keySelector;
            this.comparer = comparer;
        }
        /// <summary>
        /// CommonEqualityComparer
        /// </summary>
        /// <param name="keySelector"></param>
        public CommonEqualityComparer(Func<T, V> keySelector)
            : this(keySelector, EqualityComparer<V>.Default)
        {
        }
        /// <summary>
        /// Equals
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public bool Equals(T x, T y)
        {
            return comparer.Equals(keySelector(x), keySelector(y));
        }
        /// <summary>
        /// GetHashCode
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int GetHashCode(T obj)
        {
            return comparer.GetHashCode(keySelector(obj));
        }
    }
    /// <summary>
    /// CommonFunction
    /// </summary>
    public static class CommonFunction
    {
        /// <summary>
        /// 扩展Distinct方法
        /// </summary>
        /// <typeparam name="T">源类型</typeparam>
        /// <typeparam name="V">委托返回类型（根据V类型，排除重复项）</typeparam>
        /// <param name="source">扩展源</param>
        /// <param name="keySelector">委托（执行操作）</param>
        /// <returns></returns>
        public static IEnumerable<T> Distinct<T, V>(this IEnumerable<T> source, Func<T, V> keySelector)
        {
            return source.Distinct(new CommonEqualityComparer<T, V>(keySelector));
        }
    }
}