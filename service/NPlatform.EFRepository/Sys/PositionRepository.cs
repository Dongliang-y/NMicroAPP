﻿/*********************************************************** 
**项目名称:	 NPlatform.Repositories                                                             				   
**功能描述:	岗位  
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
    /// Position仓储操作
    /// </summary> 
    public partial class PositionRepository : RepositoryBase<Position, string>, IPositionRepository
    { 
        /// <summary>
        /// Initializes a new instance of the <see cref="PositionRepository"/> class.
        /// </summary>
        /// <param name="options">
        /// The options.
        /// </param>
        public PositionRepository(IRepositoryOptions options)
            : base(options)
        {
        }
    } 
}
