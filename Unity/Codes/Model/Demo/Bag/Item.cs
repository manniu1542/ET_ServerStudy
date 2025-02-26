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
        Normal = 0, // 2
        Good = 1, //4
        Excellent = 2,//6000
        Epic = 3,//8500
        Legendary = 4,//9500
    }

    /// <summary>
    /// 请求Item时候的操作
    /// </summary>
    public enum NetItemOp
    {
        Add = 0,
        Remove = 1
    }

    /// <summary>
    /// 请求Item放置到哪里（背包/人物装备上）
    /// </summary>
    public enum NetItemPut
    {
        Bag = 0,
        Role = 1
    }

    [ChildType(typeof (BagComponent))]
#if SERVER
    public class Item: Entity, IAwake<int>, IDestroy, ISerializeToEntity
#else
    public class Item: Entity, IAwake<int>,IDestroy
#endif
    {
        public int configID;

        public ItemQulityType quality;

        [BsonIgnore]
        public ItemConfig Config => ItemConfigCategory.Instance.Get(this.configID);
    }
}