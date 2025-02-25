using System.Collections.Generic;
using System.Runtime.Serialization;
using MongoDB.Bson.Serialization.Attributes;

namespace ET
{
    [ChildType(typeof (Item))]

#if SERVER
       [ComponentOf(typeof (Unit))]
       public class BagComponent: Entity, IAwake, IDestroy,IDeserialize, ITransfer, IUnitChache
#else
    [ComponentOf(typeof (Scene))]
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

#if SERVER
       [BsonIgnore]
        public M2C_BagUpdateItem m2c_bagItem = new M2C_BagUpdateItem();
#endif
    }
}