/*********************************************************** 
**项目名称:	 NPlatform.Repositories                                                             				   
**功能描述:	用户组-用户  
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
    /// Group_User仓储操作
    /// </summary> 
    public partial class Group_UserRepository : RepositoryBase<Group_User, string>, IGroup_UserRepository
    { 
        /// <summary>
        /// Initializes a new instance of the <see cref="Group_UserRepository"/> class.
        /// </summary>
        /// <param name="options">
        /// The options.
        /// </param>
        public Group_UserRepository(IRepositoryOptions options)
            : base(options)
        {
        }
    } 
}
