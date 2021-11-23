/*********************************************************** 
**项目名称:	 NPlatform.Repositories                                                             				   
**功能描述:	机构  
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
    /// Organization仓储操作
    /// </summary> 
    public partial class OrganizationRepository : RepositoryBase<Organization, string>, IOrganizationRepository
    { 
        /// <summary>
        /// Initializes a new instance of the <see cref="OrganizationRepository"/> class.
        /// </summary>
        /// <param name="options">
        /// The options.
        /// </param>
        public OrganizationRepository(IRepositoryOptions options)
            : base(options)
        {
        }
    } 
}
