using System.Collections.Generic;
using System.Runtime.Serialization;
using MongoDB.Bson.Serialization.Attributes;

namespace ET
{
    [ChildType(typeof (ChatInfo))]
    [ComponentOf(typeof (Scene))]
    public class ChatComponent: Entity, IAwake, IDestroy
    {
        public Queue<ChatInfo> queMsg = new Queue<ChatInfo>(100);
    }
}