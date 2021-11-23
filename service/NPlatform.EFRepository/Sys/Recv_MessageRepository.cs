/*********************************************************** 
**项目名称:	 NPlatform.Repositories                                                             				   
**功能描述:	接收消息  
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
    /// Recv_Message仓储操作
    /// </summary> 
    public partial class Recv_MessageRepository : RepositoryBase<Recv_Message, string>, IRecv_MessageRepository
    { 
        /// <summary>
        /// Initializes a new instance of the <see cref="Recv_MessageRepository"/> class.
        /// </summary>
        /// <param name="options">
        /// The options.
        /// </param>
        public Recv_MessageRepository(IRepositoryOptions options)
            : base(options)
        {
        }
    } 
}
