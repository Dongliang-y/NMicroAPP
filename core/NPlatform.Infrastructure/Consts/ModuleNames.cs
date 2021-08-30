namespace NPlatform
{
    using System.ComponentModel;

    /// <summary>
    /// 模块定义
    /// </summary>
    public enum ModuleNames
    {
        /// <summary>
        /// 系统管理
        /// </summary>
        [Description("全局")]
        ALL,

        /// <summary>
        /// 系统管理
        /// </summary>
        [Description("系统管理")]
        SYS,

        /// <summary>
        /// 图纸管理
        /// </summary>
        [Description("图纸管理")]
        DM,

        /// <summary>
        /// 进度计划
        /// </summary>
        [Description("进度计划")]
        DSM,

        /// <summary>
        /// 变更管理
        /// </summary>
        [Description("变更管理")]
        ATM,

        /// <summary>
        /// 配置管理
        /// </summary>
        [Description("配置管理")]
        BASE,

        /// <summary>
        /// 计量支付
        /// </summary>
        [Description("计量支付")]
        MPM,

        /// <summary>
        /// 概预算
        /// </summary>
        [Description("概预算")]
        PJE,

        /// <summary>
        /// 工程决算
        /// </summary>
        [Description("工程决算")]
        FAM,

        /// <summary>
        /// 流程管理
        /// </summary>
        [Description("流程管理")]
        WORKFLOW,

        /// <summary>
        /// 报表设置
        /// </summary>
        [Description("报表设置")]
        RCM,

        /// <summary>
        /// 流程设置
        /// </summary>
        [Description("流程设置")]
        WFM,

        /// <summary>
        /// 后台管理
        /// </summary>
        [Description("后台管理")]
        Admin,

        /// <summary>
        /// 合同管理
        /// </summary>
        [Description("合同管理")]
        CM,

        /// <summary>
        /// 质量管理
        /// </summary>
        [Description("质量管理")]
        QCM,

        /// <summary>
        /// 监理计量
        /// </summary>
        [Description("质量管理")]
        SPM,

        /// <summary>
        /// 会议管理
        /// </summary>
        [Description("会议管理")]
        SAF,

        /// <summary>
        /// 人员管理
        /// </summary>
        [Description("人员管理")]
        LOG,

        /// <summary>
        /// 设备管理
        /// </summary>
        [Description("设备管理")]
        EM,

        /// <summary>
        /// 信誉评价
        /// </summary>
        [Description("信誉评价")]
        CEM,

        /// <summary>
        /// BIM管理
        /// </summary>
        [Description("BIM管理")]
        BIM=1
    }
}