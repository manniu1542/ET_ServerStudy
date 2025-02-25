using MongoDB.Bson.Serialization.Attributes;

namespace ET
{
    public enum ItemType
    {
        Weapon = 0,
        Armor = 1,
        Ring = 2,
        Prop = 3,
    }

    public enum ItemQulityType
    {
        Normal = 0,
        Good = 1,
        Excellent = 2,
        Epic = 3,
        Legendary = 4,
    }

    /// <summary>
    /// 请求Item时候的操作
    /// </summary>
    public enum NetItemOp
    {
        Add = 0,
        Remove = 1
    }

    [ChildType(typeof (BagComponent))]
#if SERVER
    public class Item: Entity, IAwake<int>, IDestroy, ISerializeToEntity
#else
    public class Item: Entity, IAwake<int>, IAwake<ItemInfo>, IDestroy
#endif
    {
        public int configID;

        public ItemQulityType quality;

        [BsonIgnore]
        public ItemConfig Config => ItemConfigCategory.Instance.Get(this.configID);
    }
}