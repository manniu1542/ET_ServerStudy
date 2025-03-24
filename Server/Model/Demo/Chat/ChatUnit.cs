using MongoDB.Bson.Serialization.Attributes;

namespace ET
{
    [ChildType(typeof (ChatUnitComponent))]
    public class ChatUnit: Entity, IAwake, IDestroy
    {
   
        public string name;

        public long gateSessionId;
    }
}