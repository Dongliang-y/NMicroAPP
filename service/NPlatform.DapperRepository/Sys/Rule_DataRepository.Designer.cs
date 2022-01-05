/*********************************************************** 
**项目名称:	 NPlatform.Repositories                                                             				   
**功能描述:	sys_Rule_Data  
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
    /// Rule_Data仓储操作
    /// </summary> 
    public partial class Rule_DataRepository : RepositoryBase<Rule_Data, string>, IRule_DataRepository
    { 
        /// <summary>
        /// Initializes a new instance of the <see cref="Rule_DataRepository"/> class.
        /// </summary>
        /// <param name="options">
        /// The options.
        /// </param>
        public Rule_DataRepository(IRepositoryOptions options)
            : base(options)
        {
        }
    } 
}
