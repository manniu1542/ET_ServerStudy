using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ET
{
    [ObjectSystem]
    public class RankComponentAwakeSystem: AwakeSystem<RankComponent>
    {
        public override void Awake(RankComponent self)
        {
         
        }
    }

    [ObjectSystem]
    public class RankComponentDestroySystem: DestroySystem<RankComponent>
    {
        public override void Destroy(RankComponent self)
        {
        }
    }

    [FriendClass(typeof (RankComponent))]
    [FriendClassAttribute(typeof (ET.RankInfo))]
    public static class RankComponentSystem
    {
        /// <summary>
        /// 重置排行榜信息
        /// </summary>
        /// <param name="self"></param>
        public static async ETTask InitRankInfo(this RankComponent self)
        {
            //从数据库中拿出 rankinfo
            var dbc = DBManagerComponent.Instance.GetZoneDB(self.DomainZone());
            var roleInfos = await dbc.Query<RankInfo>(e => true, self.saveDBName);

            //重新加入 这个 容器
            foreach (RankInfo rankInfo in roleInfos)
            {
                self.AddChild(rankInfo);
                self.slRankInfo.Add(rankInfo, rankInfo.unitId);
                self.dicRankInfo.Add(rankInfo.unitId, rankInfo);
            }
        }

        /// <summary>
        /// 添加新的排行榜信息
        /// </summary>
        /// <param name="self"></param>
        /// <param name="mrankInfo"></param>
        public static async ETTask AddOrUpdateRankInfo(this RankComponent self, MRankInfo mrankInfo)
        {
            var dbCpt = DBManagerComponent.Instance.GetZoneDB(self.DomainZone());
            //存在该数据， 那就把它移除了。重新添加到排行榜里
            if (self.dicRankInfo.ContainsKey(mrankInfo.UnitID))
            {
                var oldRankInfo = self.dicRankInfo[mrankInfo.UnitID];
                if (oldRankInfo.count == mrankInfo.Count) return;
                self.slRankInfo.Remove(oldRankInfo);
                self.dicRankInfo.Remove(mrankInfo.UnitID);
                await dbCpt.Remove<RankInfo>(oldRankInfo.unitId,oldRankInfo.Id, self.saveDBName);
                oldRankInfo?.Dispose();
            }

            //重新加入 这个 容器
            var rankInfo = self.AddChild<RankInfo>(true);
            self.slRankInfo.Add(rankInfo, mrankInfo.UnitID);
            self.dicRankInfo.Add(mrankInfo.UnitID, rankInfo);

            //找到 数据库更新下当前排行榜数据

            await dbCpt.Save(rankInfo.unitId,rankInfo, self.saveDBName);
        }
    }
}