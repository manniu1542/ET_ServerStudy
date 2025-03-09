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
        public static ForgeProductionInfo ToMsgData(this Production self)
        {
            return new ForgeProductionInfo()
            {
                ProdictionId = self.Id,
                ForgeProductionConfigID = self.configID,
                StartForgeTime = self.startTime,
                EndForgeTime = self.endTime,
                ProductionReceiveState = (int)self.state,
            };
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