using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ET
{
    [ObjectSystem]
    public class BagComponentAwakeSystem: AwakeSystem<BagComponent>
    {
        public override void Awake(BagComponent self)
        {
        }
    }

    [ObjectSystem]
    public class BagComponentDestroySystem: DestroySystem<BagComponent>
    {
        public override void Destroy(BagComponent self)
        {
            self.Clear();
        }
    }

    [FriendClass(typeof (BagComponent))]
    public static class BagComponentSystem
    {
        public static void Clear(this BagComponent self)
        {
            //判断背包容量
            foreach (var item in self.dicItems)
            {
                item.Value.Dispose();
            }

            self.dicItems.Clear();
            self.mlItem.Clear();
        }

  

        /// <summary>
        /// 背包容量，是否可以添加该道具
        /// </summary>
        public static bool IsMaxCapacity(this BagComponent self, int count = 0)
        {
            //判断背包容量
            var numCpt = UnitHelper.GetMyUnitNumericComponent(self.ZoneScene().CurrentScene());
            long maxCap = numCpt[NumericType.BagCapacity];
            return maxCap <= self.dicItems.Count + count;
        }

        public static int GetCurBagItemCountByType(this BagComponent self, ItemType type)
        {
            return self.mlItem[(int)type].Count;
        }

        /// <summary>
        /// 添加背包容器
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static bool AddBagContent(this BagComponent self, Item item)
        {
            //TODO:查看背包里面该类型 是否存在，存在的话数量是多少限制。没有超过限制的话把这个Item释放掉，然后背包里面的item数量+1。
            //当前就是 道具不合并，背包里面没有该道具则可添加
            if (self.dicItems.ContainsKey(item.Id))
            {
                Log.Error("添加道具失败 背包容量不足！");
                return false;
            }

            self.AddChild(item);
            self.mlItem.Add(item.Config.Type, item);
            self.dicItems.Add(item.Id, item);
            return true;
        }

        public static bool RemoveItem(this BagComponent self, long id)
        {
            //当前就是 道具不合并，背包里面没有该道具则可添加
            if (self.dicItems.ContainsKey(id))
            {
                var item = self.dicItems[id];
                self.mlItem.Remove(item.Config.Type, item);
                self.dicItems.Remove(id);
                item.Dispose();
                return true;
            }

            return false;
        }

        /// <summary>
        ///可以添加该道具
        /// </summary>
        /// <summary>
        /// 是否可以添加该道具
        /// </summary>
        public static bool AddItem(this BagComponent self, Item item)
        {
            if (item == null)
            {
                Log.Error("添加道具失败 道具是空的！");
                return false;
            }

            //判断是否可以添加（配置里面虽大数量）
            if (self.IsMaxCapacity())
                return false;

            //添加背包的容器
            if (!self.AddBagContent(item))
                return false;

            return true;
        }
    }
}