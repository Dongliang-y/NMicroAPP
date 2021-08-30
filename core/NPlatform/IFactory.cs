/********************************************************************************

** auth： DongliangYi

** date： 2016/8/29 10:08:46

** desc：工厂接口

** Ver.:  V1.0.0

*********************************************************************************/

namespace NPlatform
{
    /// <summary>
    /// 工厂 接口
    /// </summary>
    public interface IFactory<T>
    {
        /// <summary>
        /// 创建对象
        /// </summary>
        /// <returns></returns>
        T Build();
    }
}