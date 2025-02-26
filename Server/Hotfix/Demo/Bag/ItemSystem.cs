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

    [FriendClass(typeof(Item))]
    [FriendClassAttribute(typeof(ET.EquipInfoComponent))]
    public static class ItemSystem
    {
        public static ItemInfo ToMsgData(this Item self)
        {
            ItemInfo item = new ItemInfo();
            item.Uid = self.Id;
            item.ConfigID = self.configID;
            item.Quality = (int)self.quality;

            switch ((ItemType)self.Config.Type)
            {
                case ItemType.Weapon:
                case ItemType.Armor:
                case ItemType.Ring:
                    var equipInfo = self.GetComponent<EquipInfoComponent>();
                    
                    item.EquipInfo = new EquipInfo() { sign = equipInfo.sign };
                    break;
                case ItemType.Prop:

                    break;
            }

            return item;
        }

        /// <summary>
        /// 随机装备质量 
        /// </summary>
        /// <param name="item"></param>
        public static void RandomItemQuality(this Item self)
        {
            int random = RandomHelper.RandomNumber(0, 100_00);

            if (random < 2000)
            {
                self.quality = ItemQulityType.Normal;
            }
            else if (random < 4000)
            {
                self.quality = ItemQulityType.Good;
            }
            else if (random < 6000)
            {
                self.quality = ItemQulityType.Excellent;
            }
            else if (random < 9500)
            {
                self.quality = ItemQulityType.Epic;
            }
            else
            {
                self.quality = ItemQulityType.Legendary;
            }
        }

        /// <summary>
        /// 添加道具类型的专属脚本
        /// </summary>
        /// <param name="item"></param>
        public static void AddItemTypeCpt(this Item self)
        {
            switch ((ItemType)self.Config.Type)
            {
                case ItemType.Weapon:
                case ItemType.Armor:
                case ItemType.Ring:
                    self.AddComponent<EquipInfoComponent>();
                    break;
                case ItemType.Prop:

                    break;
            }
        }
    }
}