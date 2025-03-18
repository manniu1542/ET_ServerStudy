namespace ET
{
    [MessageHandler]
    public class M2Rank_UpdateRankInfoHandler : AMHandler<M2Rank_UpdateRankInfo>
    {
        protected override void Run(Session session, M2Rank_UpdateRankInfo message)
        {
            //请求的服务器类型
            SceneType st = session.DomainScene().SceneType;
            if (st != SceneType.Rank)
            {
     
                Log.Error("请求的账号，场景服务器错误！" + st);
                return;
            }
            var rankCpt = session.DomainScene().GetComponent<RankComponent>();
            if (rankCpt == null)
            {
                Log.Error("获取当前排行榜数据错误！");
                return;
            }

            rankCpt.AddOrUpdateRankInfo(message.RankInfo).Coroutine();


        }
    }
}