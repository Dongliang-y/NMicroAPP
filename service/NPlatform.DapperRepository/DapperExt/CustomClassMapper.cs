/********************************************************************************

** auth： DongliangYi

** date： 2017/11/3 14:47:18

** desc：/// 1.通过特性移除表名中的下划线
*        /// 2.通过特性忽略映射某复杂列
*        /// 3.自动映射GUID类型成Varchar2
** Ver.:  V1.0.0

*********************************************************************************/

namespace NPlatform.Repositories.DapperExt
{
    using System;
    using System.Linq;

    using DapperExtensions.Mapper;

    /// <summary>
    /// Dapper Class Mapper
    /// 1.通过特性移除表名中的下划线
    /// 2.通过特性忽略映射某复属性
    /// </summary>
    public class CustomClassMapper<TEntity> : ClassMapper<TEntity>
        where TEntity : class
    {
        /// <summary>
        /// Dapper Class Mapper
        /// </summary>
        public CustomClassMapper()
        {
            Type type = typeof(TEntity);
            var val = type.CustomAttributes.FirstOrDefault(t => t.AttributeType == typeof(Domains.TableName));

            // 1.通过特性移除表名中的下划线val.NamedArguments[0].TypedValue
            if (val != null)
                Table(val.NamedArguments.First().TypedValue.Value.ToString());

            // 3.自动映射GUID类型ID 成Varchar2,并把 IdString 设置为主键
            // Map(x => x.Id).Column("Id").Key(KeyType.Assigned);
            AutoMap();

            // 2.通过特性ORMIgnored忽略映射某复属性
            foreach (var pro in this.Properties)
            {
                var Ig = pro.PropertyInfo.CustomAttributes.FirstOrDefault(
                    t => t.AttributeType == typeof(Domains.ORMIgnored));
                if (Ig != null)
                {
                    ((PropertyMap)pro).Ignore();
                }
            }
        }
    }
}