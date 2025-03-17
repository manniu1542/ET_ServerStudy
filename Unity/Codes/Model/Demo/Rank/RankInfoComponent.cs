using System.Collections.Generic;

namespace ET
{
    [ChildType(typeof (RankInfo))]
    [ComponentOf(typeof (Scene))]
    public class RankInfoComponent: Entity, IAwake, IDestroy
    {
        public List<RankInfo> listRankInfo = new List<RankInfo>();
        
    }
}