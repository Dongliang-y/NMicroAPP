/*********************************************************** 
**项目名称:	 NPlatform.Repositories                                                             				   
**功能描述:	行政区划  
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
    /// Region仓储操作
    /// </summary> 
    public partial class RegionRepository : RepositoryBase<Region, string>, IRegionRepository
    { 
        /// <summary>
        /// Initializes a new instance of the <see cref="RegionRepository"/> class.
        /// </summary>
        /// <param name="options">
        /// The options.
        /// </param>
        public RegionRepository(IRepositoryOptions options)
            : base(options)
        {
        }
    } 
}
