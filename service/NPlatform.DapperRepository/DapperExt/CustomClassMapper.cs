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
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using DapperExtensions.Mapper;
    using NPlatform.Domains;

    /// <summary>
    /// Dapper Class Mapper
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
            var val = type.CustomAttributes.FirstOrDefault(t => t.AttributeType == typeof(System.ComponentModel.DataAnnotations.Schema.TableAttribute));

            // set db schema 
            // set tableName
            if (val != null)
            {
                var schema = val.NamedArguments.FirstOrDefault(t => t.MemberName == "Schema").TypedValue.ToString();
                var tableName = val.NamedArguments.FirstOrDefault(t => t.MemberName == "Name").TypedValue.ToString();
                Schema(schema);
                Table(tableName);
            }
            // 3.自动映射GUID类型ID 成Varchar2,并把 IdString 设置为主键
            // Map(x => x.Id).Column("Id").Key(KeyType.Assigned);
            AutoMap();

            // 2.通过特性ORMIgnored忽略映射某复属性
            foreach (var pro in this.Properties)
            {
                var Ig = pro.PropertyInfo.CustomAttributes.FirstOrDefault(
                    t => t.AttributeType == typeof(ORMIgnored));
                if (Ig != null)
                {
                    ((PropertyMap)pro).Ignore();
                }
            }
        }
    }
}