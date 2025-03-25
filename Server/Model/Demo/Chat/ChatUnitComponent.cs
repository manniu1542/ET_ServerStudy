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

        public   Chat2C_SycChatMsg chat2C_SycChatMsg = new Chat2C_SycChatMsg() { ChatInfoData = new MChatInfo() };
    }
}