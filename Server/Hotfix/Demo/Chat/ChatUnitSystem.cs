using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET
{
    [ObjectSystem]
    public class ChatUnitAwakeSystem: AwakeSystem<ChatUnit>
    {
        public override void Awake(ChatUnit self)
        {
        }
    }

    [ObjectSystem]
    public class ChatUnitDestroySystem: DestroySystem<ChatUnit>
    {
        public override void Destroy(ChatUnit self)
        {
            
        }
    }

    [FriendClass(typeof (ChatUnit))]
    [FriendClassAttribute(typeof (ET.EquipInfoComponent))]
    public static class ChatUnitSystem
    {
        
    }
}