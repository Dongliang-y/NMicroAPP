namespace NPlatform.Domains
{
    /// <summary>
    /// Dapper ORM 忽略映射此列
    /// </summary>
    public class ORMIgnored : System.Attribute
    {
        /// <summary>
        /// 类型Id
        /// </summary>
        public override object TypeId
        {
            get
            {
                return "NPlatformEntityORMIgnored";
            }
        }
    }
}