/*********************************************************** 
**项目名称:	 NPlatform.Repositories                                                             				   
**功能描述:	职务  
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
    /// Duty仓储操作
    /// </summary> 
    public partial class DutyRepository : RepositoryBase<Duty, string>, IDutyRepository
    { 
        /// <summary>
        /// Initializes a new instance of the <see cref="DutyRepository"/> class.
        /// </summary>
        /// <param name="options">
        /// The options.
        /// </param>
        public DutyRepository(IRepositoryOptions options)
            : base(options)
        {
        }
    } 
}
