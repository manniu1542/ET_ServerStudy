using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET
{
    [ObjectSystem]
    public class ChatInfoAwakeSystem: AwakeSystem<ChatInfo>
    {
        public override void Awake(ChatInfo self)
        {
        }
    }

    [ObjectSystem]
    public class ChatInfoDestroySystem: DestroySystem<ChatInfo>
    {
        public override void Destroy(ChatInfo self)
        {
        }
    }

    [FriendClass(typeof (ChatInfo))]
    [FriendClassAttribute(typeof (ET.EquipInfoComponent))]
    public static class ChatInfoSystem
    {
        public static void ResetFormMChatInfo(this ChatInfo self, MChatInfo info)
        {
            self.name = info.name;
            self.content = info.content;
        }
    }
}