using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET
{
    [ObjectSystem]
    public class RoleEquipComponentAwakeSystem: AwakeSystem<RoleEquipComponent>
    {
        public override void Awake(RoleEquipComponent self)
        {
        }
    }

    [ObjectSystem]
    public class RoleEquipComponentDestroySystem: DestroySystem<RoleEquipComponent>
    {
        public override void Destroy(RoleEquipComponent self)
        {
            self.Clear();
        }
    }

    [FriendClass(typeof (RoleInfo))]
    [FriendClass(typeof (RoleEquipComponent))]
    public static class RoleEquipComponentSystem
    {
        public static void Clear(this RoleEquipComponent self)
        {
            foreach (var equip in self.dicEquips)
            {
                equip.Value.Dispose();
            }

            self.dicEquips.Clear();
        }

        public static Item GetItemByPos(this RoleEquipComponent self, int pos)
        {
            self.dicEquips.TryGetValue(pos, out var item);

            return item;
        }

        public static Item GetItemById(this RoleEquipComponent self, long id)
        {
            Item item = null;
            foreach (var itemTmp in self.dicEquips.Values)
            {
                if (itemTmp.Id == id)
                {
                    item = itemTmp;
                    break;
                }
            }

            return item;
        }

        /// <summary>
        /// 穿衣
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static bool DressUpItem(this RoleEquipComponent self, Item item)
        {
            if (self.GetItemByPos(item.Config.EquipPosition) != null)
            {
                return false;
            }

            self.dicEquips.Add(item.Config.EquipPosition, item);

            self.AddChild(item);

            return true;
        }

        /// <summary>
        /// 拖衣
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static bool UnloadItem(this RoleEquipComponent self, int pos)
        {
            Item item = self.GetItemByPos(pos);
            if (item == null)
            {
                return false;
            }

            self.dicEquips.Remove(pos);
            item.Dispose();

            return true;
        }
    }
}