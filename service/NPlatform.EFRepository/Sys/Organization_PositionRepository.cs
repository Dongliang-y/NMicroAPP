/*********************************************************** 
**项目名称:	 NPlatform.Repositories                                                             				   
**功能描述:	机构岗位设定  
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
    /// Organization_Position仓储操作
    /// </summary> 
    public partial class Organization_PositionRepository : RepositoryBase<Organization_Position, string>, IOrganization_PositionRepository
    { 
        /// <summary>
        /// Initializes a new instance of the <see cref="Organization_PositionRepository"/> class.
        /// </summary>
        /// <param name="options">
        /// The options.
        /// </param>
        public Organization_PositionRepository(IRepositoryOptions options)
            : base(options)
        {
        }
    } 
}
