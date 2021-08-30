/***************************** Class1 ******************************
**项目名称:	                                     
**类    名:	                                 				   
**功能描述:	                                       					   
**作    者: 	易栋梁                                         			   
**版 本 号:	1.0                                             			   
**创建日期：2015/3/12 10:39:06
**修改历史：
****************************** Class1 ******************************/

namespace NPlatform.Repositories.Exceptions
{
    using System;

    /// <summary>
    /// 仓储数据操作异常
    /// </summary>
    public class RepositoryException : NPlatformException, IRepositoryException
    {

        /// <summary>
        /// 仓储数据操作异常
        /// </summary>
        public RepositoryException(string msg, Exception ex)
            : base($"{msg}[RepositoryException]", ex, "RepositoryException")
        {
        }
    }
}