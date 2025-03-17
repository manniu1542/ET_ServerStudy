using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Schema;
using UnityEngine;

namespace ET
{
    [ObjectSystem]
    public class RankInfoComponentAwakeSystem: AwakeSystem<RankInfoComponent>
    {
        public override void Awake(RankInfoComponent self)
        {
        }
    }

    [ObjectSystem]
    public class RankInfoComponentDestroySystem: DestroySystem<RankInfoComponent>
    {
        public override void Destroy(RankInfoComponent self)
        {
            self.Clear();
        }
    }

    [FriendClass(typeof (RankInfoComponent))]
    public static class RankInfoComponentSystem
    {
        public static void Clear(this RankInfoComponent self)
        {
            //判断背包容量
            foreach (var item in self.listRankInfo)
            {
                item?.Dispose();
            }

            self.listRankInfo.Clear();
        }

        public static int GetCurRankCount(this RankInfoComponent self)
        {
            return self.listRankInfo.Count;
        }

        public static RankInfo GetCurRankInfoByIdx(this RankInfoComponent self, int idx)
        {
            if (idx >= 0 && self.listRankInfo.Count > idx)
            {
                return self.listRankInfo[idx];
            }

            return null;
        }

        public static void ResetRankInfo(this RankInfoComponent self, List<MRankInfo> listRI)
        {
            self.Clear();
            listRI.ForEach(x =>
            {
                var ri = self.AddChild<RankInfo>(true);
                ri.ResetFormData(x);
                self.listRankInfo.Add(ri);
            });
        }
    }
}