using System.Collections.Generic;
using System.ComponentModel;
using System.Collections;

namespace NPlatform
{
    /// <summary>
    /// 组织机构类型（0 业主单位、1 监理单位、2 施工单位、3 设计单位、4 咨询单位 、10其他/归类）
    /// </summary>
    public enum OrgTypes
    {
        /// <summary>
        /// 业主单位
        /// </summary>
        [Description("业主单位")]
        YZ = 0,

        /// <summary>
        /// 监理单位
        /// </summary>
        [Description("监理单位")]
        JL = 1,

        /// <summary>
        /// 施工单位
        /// </summary>
        [Description("施工单位")]
        SG = 2,

        /// <summary>
        /// 设计单位
        /// </summary>
        [Description("设计单位")]
        SJ = 3,

        /// <summary>
        /// 咨询单位
        /// </summary>
        [Description("咨询单位")]
        ZX = 4,

        /// <summary>
        /// 其他
        /// </summary>
        [Description("其他")]
        QT = 10,
    }


    /// <summary>
    /// 获取组织机构级别
    /// （0集团、1 公司、2项目、3 项目参建单位、4 参建单位部门、5 参建单位分部 、10其他/归类）
    /// </summary>
    public enum OrgLevels
    {
        /// <summary>
        /// 集团
        /// </summary>
        [Description("集团")]
        JT = 0,

        /// <summary>
        /// 公司
        /// </summary>
        [Description("公司")]
        GS = 1,

        /// <summary>
        /// 项目
        /// </summary>
        [Description("项目")]
        XM = 2,

        /// <summary>
        /// 项目参建单位
        /// </summary>
        [Description("项目参建单位")]
        CJDW = 3,

        /// <summary>
        /// 参建单位部门
        /// </summary>
        [Description("参建单位部门")]
        CJDWBM = 4,

        /// <summary>
        /// 参建单位分部
        /// </summary>
        [Description("参建单位分部")]
         CJDWFB= 5,

        /// <summary>
        /// 其他/归类
        /// </summary>
        [Description("其他/归类")]
        QT = 10,
    }
}
