/*********************************************************** 
**项目名称:	 NPlatform.Repositories                                                             				   
**功能描述:	用户登录记录  
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
    /// Login_Record仓储操作
    /// </summary> 
    public partial class Login_RecordRepository : RepositoryBase<Login_Record, string>, ILogin_RecordRepository
    { 
        /// <summary>
        /// Initializes a new instance of the <see cref="Login_RecordRepository"/> class.
        /// </summary>
        /// <param name="options">
        /// The options.
        /// </param>
        public Login_RecordRepository(IRepositoryOptions options)
            : base(options)
        {
        }
    } 
}
