using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace ET
{
    /// <summary>
    /// 道具中的装备，装备的组件
    /// </summary>
    [ComponentOf(typeof (Item))]
    [ChildType(typeof (EquipmentAffixes))]
#if SERVER
    public class EquipInfoComponent: Entity, IAwake, IDestroy, ISerializeToEntity, IDeserialize
#else
    public class EquipInfoComponent: Entity, IAwake, IDestroy
#endif
    {
        /// <summary>
        /// 生成过词条没有（默认没有生成词条）
        /// </summary>
        public bool isCreateAffixes;

        /// <summary>
        /// 装备得分
        /// </summary>
        public int score;

        /// <summary>
        /// 装备词条
        /// </summary>
#if SERVER
        [BsonIgnore]
#endif
        public List<EquipmentAffixes> listAffixes = new List<EquipmentAffixes>();
    }
}