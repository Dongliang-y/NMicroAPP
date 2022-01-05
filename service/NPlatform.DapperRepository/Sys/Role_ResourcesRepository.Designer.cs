/*********************************************************** 
**项目名称:	 NPlatform.Repositories                                                             				   
**功能描述:	角色-资源  
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
    /// Role_Resources仓储操作
    /// </summary> 
    public partial class Role_ResourcesRepository : RepositoryBase<Role_Resources, string>, IRole_ResourcesRepository
    { 
        /// <summary>
        /// Initializes a new instance of the <see cref="Role_ResourcesRepository"/> class.
        /// </summary>
        /// <param name="options">
        /// The options.
        /// </param>
        public Role_ResourcesRepository(IRepositoryOptions options)
            : base(options)
        {
        }
    } 
}
