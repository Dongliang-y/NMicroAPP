using AutoMapper;
using NPlatform.AutoMap;
using NPlatform.Result;
using System.Collections;
using System.Collections.Generic;

namespace NPlatform.Domains
{
    public class ApplicationMapProfile : Profile, IProfile
    {
        /// <summary>
        /// 配置可以互转的类
        /// </summary>
        public ApplicationMapProfile()
        {
           // CreateMap(typeof(IListResult<>), typeof(IListResult<>)).ReverseMap();
           // CreateMap(typeof(NPlatform.Result.INPResult), typeof(INPResult));
        }
    }
}
