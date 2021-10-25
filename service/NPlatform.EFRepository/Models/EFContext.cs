using System;
using System.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;
using PocManage.Models.TestResource;

#nullable disable

namespace PocManage.Repositories
{
    public partial class EFContext : DbContext
    {
        public EFContext(DbContextOptions<EFContext> options)
            : base(options)
        {
        }
        public virtual DbSet<EgineerEvaluate> EgineerEvaluates { get; set; }
        public virtual DbSet<POCTask> POCTasks { get; set; }
        public virtual DbSet<POCTaskResource> POCTaskResources { get; set; }
        public virtual DbSet<TestQuestions> Questions { get; set; }
        public virtual DbSet<TestResource> Resources { get; set; }

        /// <summary>
        /// 获取 数据类型
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <returns></returns>
        public virtual DbSet<T>  GetTDB<T>() where T:class
        {
            return this.Set<T>();
        }
    }
}
