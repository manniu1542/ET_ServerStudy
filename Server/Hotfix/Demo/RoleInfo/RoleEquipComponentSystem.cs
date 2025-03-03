using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver.Linq;
using UnityEngine;

namespace ET
{
    [ObjectSystem]
    public class RoleEquipComponentAwakeSystem: AwakeSystem<RoleEquipComponent>
    {
        public override void Awake(RoleEquipComponent self)
        {
            for (int i = (int)RoleEuipPosType.None + 1; i < (int)RoleEuipPosType.Count; i++)
            {
                self.dicEquips.Add(i, null);
            }
        }
    }

    [ObjectSystem]
    public class RoleEquipComponentDestroySystem: DestroySystem<RoleEquipComponent>
    {
        public override void Destroy(RoleEquipComponent self)
        {
        }
    }

    [ObjectSystem]
    public class RoleEquipComponentDeserializeSystem: DeserializeSystem<RoleEquipComponent>
    {
        public override void Deserialize(RoleEquipComponent self)
        {
            foreach (var tmp in self.Children)
            {
                var item = tmp.Value as Item;
                if (item != null)
                {
                }
            }
        }
    }

    [FriendClass(typeof (RoleEquipComponent))]
    [FriendClassAttribute(typeof (ET.Item))]
    public static class RoleEquipComponentSystem
    {
        public static Item GetItem(this RoleEquipComponent self, int pos)
        {
            self.dicEquips.TryGetValue(pos, out var item);

            return item;
        }

        public static bool IsContainItem(this RoleEquipComponent self, int pos)
        {
            return self.GetItem(pos) != null;
        }

        /// <summary>
        /// 穿衣
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static bool DressUpItem(this RoleEquipComponent self, Item item, bool isSendToClientMsg = true)
        {
            if (self.GetItem(item.Config.EquipPosition) != null)
            {
                return false;
            }

            self.dicEquips[item.Config.EquipPosition] = item;

            self.AddChild(item);
            
            Game.EventSystem.Publish(new EventType.NumCpt_RoleEquipChange() { Unit = self.GetParent<Unit>(), item = item, op = RoleItemOp.DressUp });

            if (isSendToClientMsg)
            {
                ItemHelper.AsyncAddItemData(self.GetParent<Unit>(), item, self.m2c_roleEqpItem);
            }
            return true;
        }

        /// <summary>
        /// 拖衣
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static Item UnloadItem(this RoleEquipComponent self, int pos, bool isSendToClientMsg = true)
        {
            Item item = self.GetItem(pos);
            if (item == null)
            {
                return null;
            }

            Game.EventSystem.Publish(new EventType.NumCpt_RoleEquipChange() { Unit = self.GetParent<Unit>(), item = item, op = RoleItemOp.Unload });

            if (isSendToClientMsg)
            {
                ItemHelper.AsyncRemoveItemData(self.GetParent<Unit>(), item, self.m2c_roleEqpItem);
            }
            
            self.dicEquips[pos] = null;
            return item;
        }
    }
}