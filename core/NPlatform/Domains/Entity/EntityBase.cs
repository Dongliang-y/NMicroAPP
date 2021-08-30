/***********************************************************
**项目名称:	                                                                  				   
**功能描述:	  的摘要说明
**作    者: 	易栋梁                                         			   
**版 本 号:	1.0                                             			   
**创建日期： 2015/5/15 11:19:07
**修改历史： 2016/08/24 修改为可支持多种主键类型
************************************************************/

namespace NPlatform.Domains.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Basic implementation of IEntity interface.
    ///     An entity can inherit this class of directly implement to IEntity interface.
    /// </summary>
    /// <typeparam name="TPrimaryKey">主键类型
    /// </typeparam>
    [Serializable]
    public abstract partial class EntityBase<TPrimaryKey> : IEntity
    {
        /// <summary>
        /// Unique identifier for this entity.
        /// </summary>
        [Required]
        public virtual TPrimaryKey Id { get; set; }

        /// <inheritdoc />
        public static bool operator ==(EntityBase<TPrimaryKey> left, EntityBase<TPrimaryKey> right)
        {
            if (Equals(left, null))
            {
                return Equals(right, null);
            }

            return left.Equals(right);
        }

        /// <inheritdoc />
        public static bool operator !=(EntityBase<TPrimaryKey> left, EntityBase<TPrimaryKey> right)
        {
            return !(left == right);
        }

        /// <summary>
        /// The dispose.
        /// </summary>
        public virtual void Dispose()
        {
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            // Same instances must be considered as equal
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            // Transient objects are not considered as equal
            var other = obj as EntityBase<TPrimaryKey>;
            if (other == null)
            {
                return false;
            }

            if (IsTransient() && other.IsTransient())
            {
                return false;
            }

            // Must have a IS-A relation of types or must be same type
            var typeOfThis = GetType();
            var typeOfOther = other.GetType();
            if (!typeOfThis.IsAssignableFrom(typeOfOther) && !typeOfOther.IsAssignableFrom(typeOfThis))
            {
                return false;
            }

            return Id.Equals(other.Id);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            if (Id == null)
            {
                return base.GetHashCode();
            }

            return Id.GetHashCode();
        }

        /// <summary>
        ///     检查该实体是否是暂时的 (刚Create后 ID属性未赋值).
        /// </summary>
        /// <returns>True, if this entity is transient</returns>
        public virtual bool IsTransient()
        {
            return EqualityComparer<TPrimaryKey>.Default.Equals(Id, default(TPrimaryKey));
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return string.Format("[{0} {1}]", GetType().Name, Id);
        }

        /// <summary>
        /// 获取string 类型的 ID
        /// </summary>
        /// <returns></returns>
        public string GetID()
        {
            return this.Id.ToString();
        }
    }
}