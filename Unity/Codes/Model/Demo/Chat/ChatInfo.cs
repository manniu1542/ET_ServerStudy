using MongoDB.Bson.Serialization.Attributes;

namespace ET
{
 
    [ChildType(typeof (ChatComponent))]
    public class ChatInfo: Entity, IAwake,IDestroy
    {
        public string name;
        
        public string content;
        
    }
}