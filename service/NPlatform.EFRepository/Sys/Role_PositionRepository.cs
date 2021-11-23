/*********************************************************** 
**项目名称:	 NPlatform.Repositories                                                             				   
**功能描述:	角色_ 岗位  
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
    /// Role_Position仓储操作
    /// </summary> 
    public partial class Role_PositionRepository : RepositoryBase<Role_Position, string>, IRole_PositionRepository
    { 
        /// <summary>
        /// Initializes a new instance of the <see cref="Role_PositionRepository"/> class.
        /// </summary>
        /// <param name="options">
        /// The options.
        /// </param>
        public Role_PositionRepository(IRepositoryOptions options)
            : base(options)
        {
        }
    } 
}
