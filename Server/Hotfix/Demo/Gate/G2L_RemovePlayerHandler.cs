using System;

namespace ET
{
    [FriendClass(typeof (GateMapComponent))]
    [FriendClass(typeof (SessionStateComponent))]
    [FriendClass(typeof (SessionPlayerComponent))]
    [FriendClass(typeof (PlayerComponent))]
    public class G2L_RemovePlayerHandler: AMActorRpcHandler<Scene, G2L_RemovePlayer, L2G_RemovePlayer>
    {
        protected override async ETTask Run(Scene scene, G2L_RemovePlayer request, L2G_RemovePlayer response, Action reply)
        {
            //请求的服务器类型
            SceneType st = scene.DomainScene().SceneType;
            if (st != SceneType.LoginCenter)
            {
                response.Error = ErrorCode.ERR_LoginSceneSever;
                reply();
                Log.Error("请求的账号，场景服务器错误！" + st);
                return;
            }

            using (await CoroutineLockComponent.Instance.Wait(CoroutineLockType.LoginGate, request.Account.GetHashCode()))
            {
                var laid = scene.GetComponent<LoginAccountInDistrictRecordComponent>();
                if (laid.Get(request.Account) == request.ServerId)
                    laid.Remove(request.Account);
            }

            await ETTask.CompletedTask;
        }
    }
}