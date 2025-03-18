using System.Collections.Generic;

namespace ET
{
    /// <summary>
    /// 排行榜服务器的组件
    /// </summary>
    [ComponentOf(typeof (Scene))]
    [ChildType(typeof (RankInfo))]
    public class RankComponent: Entity, IAwake, IDestroy
    {
        public readonly string saveDBName = "RankComponent";
        public SortedList<RankInfo, long> slRankInfo = new SortedList<RankInfo, long>(new CompareRankInfo());

        public Dictionary<long, RankInfo> dicRankInfo = new Dictionary<long, RankInfo>();
    }

    /// <summary>
    /// 比较rankinfo的类
    /// </summary>
    [FriendClassAttribute(typeof (ET.RankInfo))]
    public class CompareRankInfo: IComparer<RankInfo>
    {
        public int Compare(RankInfo x, RankInfo y)
        {
            if (x.count != y.count)
            {
                return (int)(y.count - x.count);
            }

            if (x.unitId > y.unitId)
            {
                return 1;
            }

            if (y.unitId > x.unitId)
            {
                return -1;
            }

            return 0;
        }
    }
}