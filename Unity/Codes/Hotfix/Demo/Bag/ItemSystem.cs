using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET
{
    [ObjectSystem]
    public class ItemAwakeSystem: AwakeSystem<Item, int>
    {
        public override void Awake(Item self, int config)
        {
            self.configID = config;
        }
    }

    [ObjectSystem]
    public class ItemDestroySystem: DestroySystem<Item>
    {
        public override void Destroy(Item self)
        {
        }
    }

    [FriendClass(typeof (Item))]
    [FriendClassAttribute(typeof (ET.EquipInfoComponent))]
    public static class ItemSystem
    {
        public static void ResetFormItemInfo(this Item self, ItemInfo info)
        {
            self.quality = (ItemQulityType)info.Quality;
            self.configID = info.ConfigID;

            switch ((ItemType)self.Config.Type)
            {
                case ItemType.Weapon:
                case ItemType.Armor:
                case ItemType.Ring:
                    var equipInfo = self.AddComponent<EquipInfoComponent>();
                    equipInfo.ResetDataFormMsg(info.EquipInfo);
                    break;
                case ItemType.Prop:

                    break;
            }
        }
    }
}