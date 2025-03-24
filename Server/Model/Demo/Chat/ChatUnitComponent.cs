using System.Collections.Generic;
using System.Runtime.Serialization;
using MongoDB.Bson.Serialization.Attributes;

namespace ET
{
    [ChildType(typeof (ChatUnit))]
    [ComponentOf(typeof (Scene))]
    public class ChatUnitComponent: Entity, IAwake, IDestroy
    {
        public Dictionary<long, ChatUnit> dicChatUnit = new Dictionary<long, ChatUnit>();
    }
}