/*********************************************************** 
**项目名称:	 NPlatform.Repositories                                                             				   
**功能描述:	角色-用户组  
**作    者: 	初版由CodeSmith生成。                                         			    
**版 本 号:	1.0                                           			
**修改历史： 
************************************************************/ 
namespace NPlatform.Repositories.Sys
{ 
	using System; 
	using System.Linq; 
	using NPlatform.Domains.IRepositories.Sys;
    using NPlatform.Domains.IRepositories;
    using NPlatform.Domains.Entity.Sys;
    
    /// <summary> 
    /// Role_Group仓储操作
    /// </summary> 
    public partial class Role_GroupRepository : RepositoryBase<Role_Group, string>, IRole_GroupRepository
    { 
        /// <summary>
        /// Initializes a new instance of the <see cref="Role_GroupRepository"/> class.
        /// </summary>
        /// <param name="options">
        /// The options.
        /// </param>
        public Role_GroupRepository(IRepositoryOptions options)
            : base(options)
        {
        }
    } 
}
