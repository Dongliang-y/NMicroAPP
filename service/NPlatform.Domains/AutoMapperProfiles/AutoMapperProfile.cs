using AutoMapper;
using NPlatform.AutoMap;
using NPlatform.Domains.Entity;
using NPlatform.Domains.Entity.Sys;
using NPlatform.Dto.Sys;
using NPlatform.Result;
using System.Collections;
using System.Collections.Generic;

namespace NPlatform.Domains
{
    public class AutoMapperProfile : Profile, IProfile
    {
        /// <summary>
        /// 配置可以互转的类
        /// </summary>
        public AutoMapperProfile()
        {
            CreateMap<User, UserDto>().ReverseMap();
        }
    }
}
