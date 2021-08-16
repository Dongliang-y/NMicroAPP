
### 产品名称：公司基础框架
### 公司：长沙中交京纬
### 作者：Dongliang Yi
### 基本说明：
API/工具类清单


### 更新记录：
* 2021-2-3 V 5.0.1-alpha  在3.0版本上 增加对 .NET5 ,.NET 2.2的支持,实现 2.2,3.1 5.0多目标框架支持.
* 2020-11-4：V3.0.0.16  http client 工具类增加文件上传
* V3.0.0.14  报表工具类的序列化方法增加 bool descriptionAsHeard = false 参数，作用：“是否使用中文名作为序列化后的报表数据源表头。“  值为 true，则尝试读取所序列化对象属性上的 [System.ComponentModel.Description]特性里指定的中文描述作为表头。
ReportHelper.ObjectToTabString(IEnumerable<T> datas, bool descriptionAsHeard = false)  
ReportHelper.EnumerableToTabString(IEnumerable<T> datas, bool descriptionAsHeard = false)
* 2020-10-26 V 3.0.0.13 增加 ReportHelper 
``` C#
            var rpt = new TestReport();
            rpt.ID = Guid.NewGuid().ToString();
            rpt.Name = new IdGenerator().GenerateId().ToString();
            rpt.Ct = DateTime.Now;
            rpt.Count = 109999;

            var rst=ReportHelper.ObjectToTabString(rpt);
            Console.WriteLine(rst);

            var listRst = ReportHelper.EnumerableToTabString(new TestReport[] { rpt, rpt, rpt, rpt, rpt });
            Console.WriteLine(listRst);
```
结果是帆软支持的“以逗号作为分隔符分隔数据”的格式。
```
ID,Name,Ct,Count,
e9f8622e-1fd8-4d7c-9333-5f4af8415114,1320690536964820992,2020/10/26 19:35:35,109999,

ID,Name,Ct,Count,
e9f8622e-1fd8-4d7c-9333-5f4af8415114,1320690536964820992,2020/10/26 19:35:35,109999,
e9f8622e-1fd8-4d7c-9333-5f4af8415114,1320690536964820992,2020/10/26 19:35:35,109999,
e9f8622e-1fd8-4d7c-9333-5f4af8415114,1320690536964820992,2020/10/26 19:35:35,109999,
e9f8622e-1fd8-4d7c-9333-5f4af8415114,1320690536964820992,2020/10/26 19:35:35,109999,
e9f8622e-1fd8-4d7c-9333-5f4af8415114,1320690536964820992,2020/10/26 19:35:35,109999,
```

* 2020-12-26 V3.0.0.21 修复一个报表格式化的bug
* 2020-12-18 V3.0.0.20 redis 緩存更新
* 2020-11-4: V3.0.0.18  excel 导出时数据长度超长时处理的BUG。
* 2020-11-4：V3.0.0.18  http client 工具类处理heards配置bug
* 2020-11-4：V3.0.0.17  http client 工具类增加文件上传,文件上传放开heards配置
* 2020-10-06 V 3.0.0.12 调整Redis帮助类。
* 2020-8-26 v3.0.0.11 新增ID生成类IdGenerator，作为 统一的配置类。
* 2020-8-26 v3.0.0.9 增加日志  LogLocation LogBrowser（user-agent) 字段
* 2020-8-25 V 3.0.0.7
设置  loghelper 工具类为过期类型。使用服务替代。
* 2020-8-22
修复log4net 配置文件的错误
* 2020-8-6
缓存增加 config.MachineID+ config.ServiceID.ToString(); 作为前缀。
* 2020-7-29
完善编码规范内容
* 2020-7-22 
增加  logerHelper error重载
* 2020-7-16 3.0.0.2
SnowFlake 更换 SnowFlake.core包，正式启用。
* 2020-7-10
升级为 .net core 3.1
* 2020-05-09 增加SnowFlake算法生成id.
```            
它的结果是一个64bit大小的整数。
1bit，不用，因为二进制中最高位是符号位，1表示负数，0表示正数。生成的id一般都是用整数，所以最高位固定为0。
41bit-时间戳，用来记录时间戳，毫秒级。
- 41位可以表示个数字，
- 如果只用来表示正整数（计算机中正数包含0），可以表示的数值范围是：0 至 ，减1是因为可表示的数值范围是从0开始算的，而不是1。
- 也就是说41位可以表示个毫秒的值，转化成单位年则是年

10bit-工作机器id，用来记录工作机器id。
- 可以部署在个节点，包括5位datacenterId和5位workerId
- 5位（bit）可以表示的最大正整数是，即可以用0、1、2、3、....31这32个数字，来表示不同的datecenterId或workerId

12bit-序列号，序列号，用来记录同毫秒内产生的不同id。
- 12位（bit）可以表示的最大正整数是，即可以用0、1、2、3、....4094这4095个数字，来表示同一机器同一时间截（毫秒)内产生的4095个ID序号。

由于在Java中64bit的整数是long类型，所以在Java中SnowFlake算法生成的id就是long来存储的。

SnowFlake可以保证：

所有生成的id按时间趋势递增
整个分布式系统内不会产生重复id（因为有datacenterId和workerId来做区分）
```
* 2020-04-28
调整EPlatformConfig 结构

* 2020-06-01
log4net 工具类跳转，push 消息推送调整。
* 2020-06-02
log 枚举命名空间调整等
* 2020-06-03
集成 log4net, adonetappender 包
* 2020-06-16
配置路径中多了两个// 不是很美观。
* 2020-7-1
累积性更新