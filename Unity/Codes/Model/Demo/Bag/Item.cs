using MongoDB.Bson.Serialization.Attributes;

namespace ET
{
    public enum ItemType
    {
        Weapon,
        Armor,
        Ring,
        Prop
    }

    public enum ItemQulityType
    {
        Normal,
        Good,
        Excellent,
        Epic,
        Legendary,
    }

    [ChildType(typeof (BagComponent))]
    public class Item: Entity, IAwake<int>, IDestroy, ISerializeToEntity
    {
        public int configID;

        public ItemQulityType quality;

        [BsonIgnore]
        public ItemConfig Config => ItemConfigCategory.Instance.Get(this.configID);
    }
}