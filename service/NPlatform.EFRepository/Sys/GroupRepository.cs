/*********************************************************** 
**项目名称:	 NPlatform.Repositories                                                             				   
**功能描述:	用户组  
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
    /// Group仓储操作
    /// </summary> 
    public partial class GroupRepository : RepositoryBase<Group, string>, IGroupRepository
    { 
        /// <summary>
        /// Initializes a new instance of the <see cref="GroupRepository"/> class.
        /// </summary>
        /// <param name="options">
        /// The options.
        /// </param>
        public GroupRepository(IRepositoryOptions options)
            : base(options)
        {
        }
    } 
}
