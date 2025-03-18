using System;

namespace ET
{
    [FriendClassAttribute(typeof (ET.RankComponent))]
    public class C2Rank_GetCurAllRankInfoHandler: AMRpcHandler<C2Rank_GetCurAllRankInfo, Rank2C_GetCurAllRankInfo>
    {
        protected override async ETTask Run(Session session, C2Rank_GetCurAllRankInfo request, Rank2C_GetCurAllRankInfo response, Action reply)
        {
            //请求的服务器类型
            SceneType st = session.DomainScene().SceneType;
            if (st != SceneType.Rank)
            {
                response.Error = ErrorCode.ERR_SwitchSceneSever;
                reply();
                session.Disconnect().Coroutine();
                Log.Error("请求的账号，场景服务器错误！" + st);
                return;
            }
      
            var rankCpt = session.DomainScene().GetComponent<RankComponent>();
            if (rankCpt == null)
            {
                response.Error = ErrorCode.ERR_GetCurAllRankInfo;
                reply();

                Log.Error("获取当前排行榜数据错误！");
                return;
            }

            foreach (RankInfo rankInfo in rankCpt.slRankInfo.Keys)
            {
                response.RankInfos.Add(rankInfo.ToMsgData());
            }

            reply();
        }
    }
}