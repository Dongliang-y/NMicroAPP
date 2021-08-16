# NoName
    NoName 是一个旨在提供快速开发基于.NET CORE 的微服务系统的框架。
    基于现有的.NET CORE生态中，框架比较少，ABP的庞大，高昂的学习成本和开发成本，让中小项目望而却步，自建框架或者使用不成熟的框架不仅开发成本高，对核心技术团队要求高，而且稳定性差。因此约各位志同道合之人，开发此框架，目标是面向中小项目以及各位自己业余项目（此处应有个表情）。



## 文档的管理约定
* 尽量使用markdown 编写文档 ，最终使用 doctoc 插件生成目录


## 技术栈约定
* Mysql\redis\mongodb\InfluxDB
* .NET CORE 5.0
* EFCore
* MVC5 + Razor + VUE
* vant 做APP端
* 网关ocelot + Consul服务管理
* 事务
* 全套使用 异步到底
* 引入 ddd 部分理论， 引入事件驱动。
* 仓储层实现读写分离。
* 引入grpc、 web api

## 