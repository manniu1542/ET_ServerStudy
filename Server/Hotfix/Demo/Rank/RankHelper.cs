namespace ET
{
    [FriendClassAttribute(typeof (ET.RoleInfo))]
    public static class RankHelper
    {
        /// <summary>
        /// 更新 当前玩家在排行榜 等级数据
        /// </summary>
        /// <param name="self"></param>
        public static void SendUpdateRankOfUnitLevel(Unit unit)
        {
            int zone = unit.DomainZone();
            var rankConfig = StartSceneConfigCategory.Instance.GetBySceneName(unit.DomainZone(), "Rank");
            var numCpt = unit.GetComponent<NumericComponent>();
            int Level = numCpt.GetAsInt(NumericType.Level);

            //怎么 给unit上绑定roleinfo呢

            RoleInfo roleInfo = unit.GetComponent<RoleInfo>();
            MessageHelper.SendActor(rankConfig.InstanceId,
                new M2Rank_UpdateRankInfo { RankInfo = new MRankInfo { UnitID = unit.Id, Count = Level, Name = roleInfo.Name } });
        }
    }
}