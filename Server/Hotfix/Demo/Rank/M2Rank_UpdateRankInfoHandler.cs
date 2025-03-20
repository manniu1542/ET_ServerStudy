namespace ET
{
    [ActorMessageHandler]
    public class M2Rank_UpdateRankInfoHandler: AMActorHandler<Scene,M2Rank_UpdateRankInfo>
    {
        protected override async ETTask Run(Scene scene, M2Rank_UpdateRankInfo message)
        {
            await ETTask.CompletedTask;
            //请求的服务器类型
            SceneType st = scene.DomainScene().SceneType;
            if (st != SceneType.Rank)
            {
                Log.Error("请求的账号，场景服务器错误！" + st);
                return;
            }

            var rankCpt = scene.DomainScene().GetComponent<RankComponent>();
            if (rankCpt == null)
            {
                Log.Error("获取当前排行榜数据错误！");
                return;
            }

            rankCpt.AddOrUpdateRankInfo(message.RankInfo).Coroutine();
        }
    }
}