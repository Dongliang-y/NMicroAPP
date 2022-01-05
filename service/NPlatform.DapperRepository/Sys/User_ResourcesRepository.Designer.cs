/*********************************************************** 
**项目名称:	 NPlatform.Repositories                                                             				   
**功能描述:	用户-资源  
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
    /// User_Resources仓储操作
    /// </summary> 
    public partial class User_ResourcesRepository : RepositoryBase<User_Resources, string>, IUser_ResourcesRepository
    { 
        /// <summary>
        /// Initializes a new instance of the <see cref="User_ResourcesRepository"/> class.
        /// </summary>
        /// <param name="options">
        /// The options.
        /// </param>
        public User_ResourcesRepository(IRepositoryOptions options)
            : base(options)
        {
        }
    } 
}
