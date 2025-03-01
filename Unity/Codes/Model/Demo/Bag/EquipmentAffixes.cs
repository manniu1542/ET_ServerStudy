namespace ET
{
    /// <summary>
    /// 词条类型
    /// </summary>
    public enum EquipmentAffixesType
    {
        Normal = 1,
        Special = 2,
    }


#if SERVER
    public class EquipmentAffixes: Entity, IAwake, IDestroy, ISerializeToEntity
#else
    public class EquipmentAffixes: Entity, IAwake,IDestroy
#endif

    {
        
        /// <summary>
        /// 词条所属的数值类型（加血或加护甲 等等属性类型）
        /// </summary>
        public int numType;

        /// <summary>
        /// 词条所属的数值类型的值
        /// </summary>
        public long numValue;
        
        /// <summary>
        /// 词条自己的类型
        /// </summary>
        public EquipmentAffixesType type;

    }
}