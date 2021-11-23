/*********************************************************** 
**项目名称:	 NPlatform.Repositories                                                             				   
**功能描述:	禁止资源  
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
    /// Forbid_Res仓储操作
    /// </summary> 
    public partial class Forbid_ResRepository : RepositoryBase<Forbid_Res, string>, IForbid_ResRepository
    { 
        /// <summary>
        /// Initializes a new instance of the <see cref="Forbid_ResRepository"/> class.
        /// </summary>
        /// <param name="options">
        /// The options.
        /// </param>
        public Forbid_ResRepository(IRepositoryOptions options)
            : base(options)
        {
        }
    } 
}
