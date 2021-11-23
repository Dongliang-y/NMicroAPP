/*********************************************************** 
**项目名称:	 NPlatform.Repositories                                                             				   
**功能描述:	消息表  
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
    /// Message仓储操作
    /// </summary> 
    public partial class MessageRepository : RepositoryBase<Message, string>, IMessageRepository
    { 
        /// <summary>
        /// Initializes a new instance of the <see cref="MessageRepository"/> class.
        /// </summary>
        /// <param name="options">
        /// The options.
        /// </param>
        public MessageRepository(IRepositoryOptions options)
            : base(options)
        {
        }
    } 
}
