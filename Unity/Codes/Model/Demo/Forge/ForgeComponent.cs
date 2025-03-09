using System.Collections.Generic;
using System.Runtime.Serialization;
using MongoDB.Bson.Serialization.Attributes;

namespace ET
{
    [ChildType(typeof (Production))]
#if SERVER
       [ComponentOf(typeof (Unit))]
       public class ForgeComponent: Entity, IAwake, IDestroy,IDeserialize, ITransfer, IUnitChache
#else
    [ComponentOf(typeof (Scene))]
    public class ForgeComponent: Entity, IAwake, IDestroy
#endif
    {
#if SERVER
   [BsonIgnore]
#endif
        public Dictionary<long, Production> dicProductions = new Dictionary<long, Production>();

#if !SERVER
        [BsonIgnore]
        public List<Production> listProductions = new List<Production>();
#endif
    }
}