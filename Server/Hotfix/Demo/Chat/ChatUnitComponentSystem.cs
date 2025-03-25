using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ET
{
    [ObjectSystem]
    public class ChatUnitComponentAwakeSystem: AwakeSystem<ChatUnitComponent>
    {
        public override void Awake(ChatUnitComponent self)
        {
        }
    }

    [ObjectSystem]
    public class ChatUnitComponentDestroySystem: DestroySystem<ChatUnitComponent>
    {
        public override void Destroy(ChatUnitComponent self)
        {
        }
    }

    [FriendClass(typeof (ChatUnitComponent))]
    [FriendClassAttribute(typeof (ET.ChatUnit))]
    public static class ChatUnitComponentSystem
    {
        public static ChatUnit AddChatUnit(this ChatUnitComponent self, long unitId, string name, long gateSessionId)
        {
            //服务端还没来得及剔除的 直接剔除
            self.RemoveChatUnit(unitId);

            var chatUnit = self.AddChild<ChatUnit>();
            chatUnit.name = name;
            chatUnit.gateSessionId = gateSessionId;
            chatUnit.AddComponent<MailBoxComponent>();

            self.dicChatUnit.Add(chatUnit.Id, chatUnit);
            return chatUnit;
        }

        public static void RemoveChatUnit(this ChatUnitComponent self, long unitId)
        {
            if (self.dicChatUnit.ContainsKey(unitId))
            {
                self.dicChatUnit[unitId]?.Dispose();
                self.dicChatUnit.Remove(unitId);
            }
        }

        public static void SendBoardMsg(this ChatUnitComponent self, string name, string msg)
        {
            self.chat2C_SycChatMsg.ChatInfoData.name = name;
            self.chat2C_SycChatMsg.ChatInfoData.content = msg;
            foreach (var cu in self.dicChatUnit)
            {
                MessageHelper.SendActor(cu.Value.gateSessionId, self.chat2C_SycChatMsg);
            }
        }
    }
}