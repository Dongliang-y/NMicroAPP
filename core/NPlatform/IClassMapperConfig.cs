﻿using AutoMapper;

namespace ZJJWEPlatform
{
    /// <summary>
    /// class映射接口
    /// </summary>
    public interface IClassMapperConfig
    {
        /// <summary>
        /// config 
        /// </summary>
        /// <param name="cfg"></param>
        void Config(IMapperConfigurationExpression cfg);
    }
}