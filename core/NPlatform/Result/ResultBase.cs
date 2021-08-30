using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NPlatform.Infrastructure.Loger;

namespace NPlatform.Result
{
    /// <summary>
    /// 返回结果的封装
    /// </summary>
    public class ResultBase
    {

        #region success
        /// <summary>
        ///  返回SuccessResult
        /// </summary>
        protected virtual EPResult<bool> Success(string msg)
        {
            return new EPResult<bool>(true,msg,true);
        }

        /// <summary>
        ///  返回SuccessResult
        /// </summary>
        protected virtual EPResult<bool> Success()
        {
            return new EPResult<bool>(true, string.Empty, true);
        }

        /// <summary>
        ///  返回SuccessResult
        /// </summary>
        protected virtual EPResult<T> Success<T>(string msg,T data)
        {
            return new EPResult<T>(true, msg,data);
        }
        /// <summary>
        ///  返回SuccessResult
        /// </summary>
        protected virtual EPResult<T> Success<T>( T data)
        {
            return new EPResult<T>(true, string.Empty, data);
        }
        #endregion

        #region Error

        /// <summary>
        /// 返回错误信息
        /// </summary>
        protected virtual IEPResult<bool> Error(string msg)
        {
            return Error<bool>(msg);
        }

        /// <summary>
        /// 返回错误信息
        /// </summary>
        protected virtual ErrorResult<T> Error<T>(string msg)
        {
            return new ErrorResult<T>(msg);
        }

        /// <summary>
        /// 返回参数不能为空的提示 “{pName}参数不能为空！”
        /// 使用：NotNullResult(nameof(参数名))
        /// </summary>
        /// <param name="pName">参数名</param>
        /// <returns>IEPResult</returns>
        protected virtual IEPResult<bool> NotNullResult(string pName)
        {
            return Error<bool>($"{pName}参数不能为空！");
        }

        /// <summary>
        /// 返回参数不能为空的提示 “{pName}参数不能为空！”
        /// 使用：NotNullResult(nameof(参数名))
        /// </summary>
        /// <param name="pName">参数名</param>
        /// <returns>IEPResult</returns>
        protected virtual ErrorResult<T> NotNullResult<T>(string pName)
        {
            return new ErrorResult<T>($"{pName}参数不能为空！");
        }
        /// <summary>
        /// 返回错误信息
        /// </summary>
        protected virtual IEPResult<bool> Error(string msg,Exception ex)
        {
            return Error<bool>(msg,new NPlatformException(msg,ex, "500"));
        }
        /// <summary>
        /// 返回错误信息
        /// </summary>
        protected virtual IEPResult<bool> Error(Exception ex)
        {
            return Error<bool>(ex.Message, new NPlatformException("", ex, "500"));
        }

        /// <summary>
        /// 返回错误信息
        /// </summary>
        protected virtual ErrorResult<T> Error<T>(NPlatformException ex) 
        {
            return Error<T>(ex.Message, ex);
        }
        /// <summary>
        /// 返回错误信息
        /// </summary>
        protected virtual ErrorResult<T> Error<T>(string msg, NPlatform.NPlatformException ex)
        {
            if (ex.GetType().IsSubclassOf(typeof(LogicException)))
            {
                LogerHelper.Error(ex.Message, "", ex);
                return Error<T>(ex.Message);
            }
            else if (ex.GetType().IsSubclassOf(typeof(ConfigException)))
            {
                LogerHelper.Error("系统配置加载异常!", "", ex);
                return Error<T>("系统配置加载异常");
            }
            else
            {
                LogerHelper.Error(ex.Message, "", ex);
                return Error<T>(ex.Message);
            }
        }

        #endregion

        /// <summary>
        /// 树格式节点
        /// </summary>
        /// <typeparam name="T">TreeNode 类型</typeparam>
        /// <param name="nodes">树节点</param>
        /// <returns></returns>
        protected TreeResult<T> TreeData<T>(IEnumerable<T> nodes)
        {
            var trees = new TreeResult<T>();
            trees.AddRange(nodes);
            return trees;
        }

        /// <summary>
        /// 返回数据集合
        /// </summary>
        [Obsolete("PageData已过期，后续请使用ListResult。", false)]
        protected virtual ListResult<T> PageData<T>(IEnumerable<T> list, long total)
        {
            var content = new ListResult<T>(list, total);
            return content;
        }
        /// <summary>
        /// 返回数据集合
        /// </summary>
        protected virtual ListResult<T> ListData<T>(IEnumerable<T> list)
        {
            var content = new ListResult<T>(list, -1);
            return content;
        }
        /// <summary>
        /// 返回数据集合
        /// </summary>
        protected virtual ListResult<T> ListData<T>(IEnumerable<T> list, long total)
        {
            var content = new ListResult<T>(list, total);
            return content;
        }

    }
}
