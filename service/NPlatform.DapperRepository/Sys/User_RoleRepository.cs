/*********************************************************** 
**项目名称:	 NPlatform.Repositories                                                             				   
**功能描述:	用户-角色  
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
    /// User_Role仓储操作
    /// </summary> 
    public partial class User_RoleRepository : RepositoryBase<User_Role, string>, IUser_RoleRepository
    { 
        /// <summary>
        /// Initializes a new instance of the <see cref="User_RoleRepository"/> class.
        /// </summary>
        /// <param name="options">
        /// The options.
        /// </param>
        public User_RoleRepository(IRepositoryOptions options)
            : base(options)
        {
        }
    } 
}
