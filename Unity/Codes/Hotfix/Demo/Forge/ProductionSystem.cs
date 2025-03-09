using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ET
{
    [ObjectSystem]
    public class ProductionAwakeSystem: AwakeSystem<Production>
    {
        public override void Awake(Production self)
        {
        }
    }

    [ObjectSystem]
    public class ProductionDestroySystem: DestroySystem<Production>
    {
        public override void Destroy(Production self)
        {
           
        }
    }

    [FriendClass(typeof (Production))]
    public static class ProductionSystem
    {
        public static void ResetFormData(this Production self, ForgeProductionInfo info)
        {
            self.configID = info.ForgeProductionConfigID;
            self.startTime = info.StartForgeTime;
            self.endTime = info.EndForgeTime;
            self.state = (ProductionReceiveState)info.ProductionReceiveState;
        }

        /// <summary>
        /// 是否需要领取了 制作完成了
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static bool IsNeedReceive(this Production self)
        {
            if (self.state != ProductionReceiveState.Making)
                return false;
            long cutTime = TimeHelper.ServerNow();
            return cutTime >= self.endTime;
        }
    }
}