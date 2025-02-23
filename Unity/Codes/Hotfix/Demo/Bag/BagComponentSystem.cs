using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Pool;

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
        }
    }

    [FriendClass(typeof (BagComponent))]
    public static class BagComponentSystem
    {
        /// <summary>
        /// 背包容量，是否可以添加该道具
        /// </summary>
        public static bool IsMaxCapacity(this BagComponent self, int count = 0)
        {
            //判断背包容量
            var numCpt = self.Parent.GetComponent<NumericComponent>();
            long maxCap = numCpt[NumericType.BagCapacity];
            return maxCap <= self.dicItems.Count + count;
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

            self.mlItem.Add(item.Config.Type, item);
            self.dicItems.Add(item.Id, item);
            return true;
        }
        /// <summary>
        /// 添加背包容器
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static void RemoveBagContent(this BagComponent self, Item item)
        {
            self.dicItems.Remove(item.Id);
            self.mlItem.Remove(item.Config.Type, item);
    
        }
        /// <summary>
        /// 是否可以添加该道具
        /// </summary>
        public static bool AddItem(this BagComponent self, Item item)
        {
            if (item == null || item.IsDisposed)
            {
                Log.Error("添加道具失败 道具是空的！");
                return false;
            }

            //判断是否可以添加（配置里面虽大数量）
            if (self.IsMaxCapacity())
                return false;

            //添加背包的容器
            if (self.AddBagContent(item))
                return false;

            //添加道具进入背包 数据库db
            if (item.Parent != self)
                self.AddChild(item);

            //TODO：发送同步道具消息到客户端

            return true;
        }

        /// <summary>
        /// 按配置表添加该道具
        /// </summary>
        public static bool AddItemByConfigID(this BagComponent self, int configId, int count = 1)
        {
            if (count <= 0) return false;

            //判断是否可以添加（配置里面虽大数量）
            if (self.IsMaxCapacity(count - 1))
                return false;
            
            using (ListComponent<Item> list = ListComponent<Item>.Create())
            {
                for (int i = 0; i < count; i++)
                {
                    Item item = ItemFactory.CreateItem(self, configId);
                    list.Add(item);
                    if (!self.AddItem(item))
                    {
                        foreach (var tmp in list)
                        {
                            self.RemoveBagContent(tmp);
                        }

                        return false;
                    }
                }
                
            }
            
            return true;
        }
    }
}