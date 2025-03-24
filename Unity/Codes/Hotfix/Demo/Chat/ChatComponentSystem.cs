using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ET
{
    [ObjectSystem]
    public class ChatComponentAwakeSystem: AwakeSystem<ChatComponent>
    {
        public override void Awake(ChatComponent self)
        {
        }
    }

    [ObjectSystem]
    public class ChatComponentDestroySystem: DestroySystem<ChatComponent>
    {
        public override void Destroy(ChatComponent self)
        {
        }
    }

    [FriendClass(typeof (ChatComponent))]
    public static class ChatComponentSystem
    {
        public static int GetCurCount(this ChatComponent self)
        {
            return self.queMsg.Count;
        }

        public static void AddMsg(this ChatComponent self, MChatInfo mChatInfo)
        {
            if (self.GetCurCount() > 100)
            {
                var msg = self.queMsg.Dequeue();
                msg.Dispose();
            }
            var chatInfo = self.AddChild<ChatInfo>();
            chatInfo.ResetFormMChatInfo(mChatInfo);
            self.queMsg.Enqueue(chatInfo);
        }

        public static ChatInfo GetChatMessageByIndex(this ChatComponent self, int index)
        {
            int tempIndex = 0;
            foreach (var chatInfo in self.queMsg)
            {
                if (tempIndex == index)
                {
                    return chatInfo;
                }

                ++tempIndex;
            }

            return null;
        }
    }
}