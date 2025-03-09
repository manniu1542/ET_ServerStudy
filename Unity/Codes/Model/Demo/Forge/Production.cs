using MongoDB.Bson.Serialization.Attributes;

namespace ET
{
    public enum ProductionReceiveState
    {
        Making = 1,
        NeedReceive = 2,
    }

    [ChildType(typeof (BagComponent))]
#if SERVER
    public class Production: Entity, IAwake, IDestroy, ISerializeToEntity
#else
    public class Production: Entity, IAwake, IDestroy
#endif
    {
        public int configID;

        public ProductionReceiveState state;

        public long startTime;

        public long endTime;
         
        
        [BsonIgnore]
        public ForgeProductionConfig Config => ForgeProductionConfigCategory.Instance.Get(this.configID);
    }
}