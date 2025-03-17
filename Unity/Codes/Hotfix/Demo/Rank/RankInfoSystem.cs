using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ET
{
    [ObjectSystem]
    public class RankInfoAwakeSystem: AwakeSystem<RankInfo>
    {
        public override void Awake(RankInfo self)
        {
            self.count = 0;
            self.name = string.Empty;
            self.unitId = 0;
        }
    }

    [ObjectSystem]
    public class RankInfoDestroySystem: DestroySystem<RankInfo>
    {
        public override void Destroy(RankInfo self)
        {
            self.count = 0;
            self.name = string.Empty;
            self.unitId = 0;
        }
    }

    [FriendClass(typeof (RankInfo))]
    public static class RankInfoSystem
    {
        /// <summary>
        /// 重置消息数据
        /// </summary>
        /// <param name="self"></param>
        /// <param name="info"></param>
        public static void ResetFormData(this RankInfo self, MRankInfo info)
        {
            self.count = info.Count;
            self.name = info.Name;
            self.unitId = info.UnitID;
        }

        /// <summary>
        /// 转消息数据
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static MRankInfo ToMsgData(this RankInfo self)
        {
            return new MRankInfo() { Count = self.count, Name = self.name, UnitID = self.unitId, };
        }
    }
}