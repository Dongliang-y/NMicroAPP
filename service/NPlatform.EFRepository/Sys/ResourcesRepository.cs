/*********************************************************** 
**项目名称:	 NPlatform.Repositories                                                             				   
**功能描述:	资源  
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
    /// Resources仓储操作
    /// </summary> 
    public partial class ResourcesRepository : RepositoryBase<Resources, string>, IResourcesRepository
    { 
        /// <summary>
        /// Initializes a new instance of the <see cref="ResourcesRepository"/> class.
        /// </summary>
        /// <param name="options">
        /// The options.
        /// </param>
        public ResourcesRepository(IRepositoryOptions options)
            : base(options)
        {
        }
    } 
}
