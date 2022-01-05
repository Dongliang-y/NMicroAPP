/*********************************************************** 
**项目名称:	 NPlatform.Repositories                                                             				   
**功能描述:	角色分类  
**作    者: 	初版由CodeSmith生成。                                         			    
**版 本 号:	1.0                                           			
**修改历史： 
************************************************************/ 
namespace NPlatform.Repositories.Sys
{ 
	using System; 
	using System.Linq; 
	using NPlatform.Domains.IRepositories.Sys;
    using NPlatform.Repositories.IRepositories;
    using NPlatform.Domains.Entity.Sys;
    
    /// <summary> 
    /// Role_Type仓储操作
    /// </summary> 
    public partial class Role_TypeRepository : RepositoryBase<Role_Type, string>, IRole_TypeRepository
    { 
        /// <summary>
        /// Initializes a new instance of the <see cref="Role_TypeRepository"/> class.
        /// </summary>
        /// <param name="options">
        /// The options.
        /// </param>
        public Role_TypeRepository(IRepositoryOptions options)
            : base(options)
        {
        }
    } 
}
