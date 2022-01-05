/*********************************************************** 
**项目名称:	 NPlatform.Repositories                                                             				   
**功能描述:	单位  
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
    /// Unit仓储操作
    /// </summary> 
    public partial class UnitRepository : RepositoryBase<Unit, string>, IUnitRepository
    { 
        /// <summary>
        /// Initializes a new instance of the <see cref="UnitRepository"/> class.
        /// </summary>
        /// <param name="options">
        /// The options.
        /// </param>
        public UnitRepository(IRepositoryOptions options)
            : base(options)
        {
        }
    } 
}
