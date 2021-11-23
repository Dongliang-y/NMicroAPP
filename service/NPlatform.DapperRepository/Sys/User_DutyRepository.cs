﻿/*********************************************************** 
**项目名称:	 NPlatform.Repositories                                                             				   
**功能描述:	用户-职务  
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
    /// User_Duty仓储操作
    /// </summary> 
    public partial class User_DutyRepository : RepositoryBase<User_Duty, string>, IUser_DutyRepository
    { 
        /// <summary>
        /// Initializes a new instance of the <see cref="User_DutyRepository"/> class.
        /// </summary>
        /// <param name="options">
        /// The options.
        /// </param>
        public User_DutyRepository(IRepositoryOptions options)
            : base(options)
        {
        }
    } 
}
