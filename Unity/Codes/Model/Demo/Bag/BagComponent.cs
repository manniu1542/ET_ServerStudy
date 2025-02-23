using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace ET
{
    [ChildType(typeof(Item))]
    [ComponentOf(typeof (Unit))]
#if SERVER
       public class BagComponent: Entity, IAwake, IDestroy, ITransfer, IUnitChache
#else
    public class BagComponent: Entity, IAwake, IDestroy
#endif

    {
#if SERVER
   [BsonIgnore]
#endif
        public Dictionary<long, Item> dicItems = new Dictionary<long, Item>();
#if SERVER
   [BsonIgnore]
#endif
        public MultiMap<int, Item> mlItem = new MultiMap<int, Item>();
        
        
        
    }
}