/*********************************************************** 
**项目名称:	 NPlatform.IServices                                                               				   
**功能描述:	  UserServices 的摘要说明 
**作    者: 	此代码由CodeSmith生成。                                         			    
**版 本 号:	1.0                                           			    
**创建日期： 2021-12-15 17:26
**修改历史： 
************************************************************/ 
namespace  NPlatform.Domains.Services.Sys
{ 
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using NPlatform.Domains.Entity.Sys;
    using NPlatform.Domains.IService.Sys;
    using NPlatform.Domains.Service;
    using NPlatform.Dto.Sys;
    using NPlatform.Result;
    using NPlatform;
    using NPlatform.Dto;
    using Microsoft.Extensions.Logging;
    using System.Threading;
    using NPlatform.Domains.IRepositories.Sys;

    /// <summary> 
    ///    User  业务层
    /// </summary> 
    public partial class UserService
    {  
        public ILogger<UserService> Loger { get; set; }
        public IUserRepository Repository { get; set; }

        public async Task<IListResult<UserDto>> GetListAsync(SearchExp exp)
        {
            var vResult=exp.Validates();
            if (vResult.StatusCode==200)
            {
                var users= await Repository.GetListByExpAsync(exp.GetExp<User>(), exp.GetSelectSorts());
                var userDtos=this.MapperObj.Map<IEnumerable <User>, ListResult<UserDto>>(users);
                return userDtos;
            }
            return (IListResult<UserDto>)vResult;
        }
        public async Task<IListResult<UserDto>> GetListAsync(Expression<Func<User, bool>> filter)
        {
            var users = await Repository.GetListByExpAsync(filter);
            var userDtos = MapperObj.Map<IEnumerable<User>, IListResult<UserDto>>(users);
            return userDtos;
        }
        public async Task<IListResult<UserDto>> GetPageAsync(SearchPageExp exp)
        {
            var vResult = exp.Validates();
            if (vResult.StatusCode == 200)
            {
                var users = await Repository.GetPagedAsync(exp.PageIndex,exp.PageSize, exp.GetExp<User>(), exp.GetSelectSorts());
                var userDtos = MapperObj.Map<IListResult<User>, IListResult<UserDto>>(users);
                return userDtos;
            }
            return (IListResult<UserDto>)vResult;
        }

        public async Task<INPResult> PostAsync(UserDto user)
        {
            var vResult = user.Validates();
            if (vResult.StatusCode == 200)
            {
                var userEntity = MapperObj.Map<User>(user);
                userEntity.password = NPlatform.Infrastructure.MD5S.GetSHA256(user.password);

                var userRst = await Repository.AddAsync(userEntity);
                user.id = userRst.Id;
                return Success(user) ;
            }
            return vResult;
        }

        public async Task<INPResult> PutAsync(UserDto user)
        {
            var vResult = user.Validates();
            if (vResult.StatusCode == 200)
            {
                var userPut = MapperObj.Map<User>(user);

                var userExit=await Repository.GetFirstOrDefaultAsync(t=>t.Id==user.id);

                userPut.password = userExit.password;

                var userRst = await Repository.UpdateAsync(userPut);
                return Success(user);
            }
            return vResult;
        }

        /// <summary>
        /// 使用验证码修改密码
        /// </summary>
        /// <param name="userKey">用户ID、账号、手机号、邮箱</param>
        /// <param name="passwordNew1">新密码1</param>
        /// <param name="passwordNew2">新密码2</param>
        /// <param name="smsCode">短信验证码</param>
        /// <param name="emailCode">邮箱验证码</param>
        /// <param name="checkType">验证类型，1任意一个验证码通过即可，2两个验证码同时通过即可。</param>
        /// <returns></returns>
        public async Task<INPResult> ChangePasswordAsync(string userKey,string passwordNew1, string passwordNew2,string smsCode,string emailCode,int checkType=1)
        {
            throw new NotImplementedException();
        }
    } 
} 
 
