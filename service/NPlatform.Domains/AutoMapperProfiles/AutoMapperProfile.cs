using AutoMapper;

namespace NPlatform.Domains
{
    public class AutoMapperProfile : Profile
    {
        /// <summary>
        /// 配置可以互转的类
        /// </summary>
        public AutoMapperProfile()
        {
         //   CreateMap<TodoList, BackLog>().ForMember(x => x.title, z => z.MapFrom(y => y.field0001 + "，" + y.field0002 + "，" + y.field0003));
        }
    }
}
