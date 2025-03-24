using System;
using MongoDB.Driver.Linq;

namespace ET
{
    [FriendClass(typeof (GateMapComponent))]
    [FriendClass(typeof (SessionStateComponent))]
    [FriendClass(typeof (SessionPlayerComponent))]
    [FriendClass(typeof (PlayerComponent))]
    public class G2Chat_LoginRequestHandler: AMActorRpcHandler<Scene, G2Chat_LoginRequest, Chat2G_LoginResponse>
    {
        protected override async ETTask Run(Scene scene, G2Chat_LoginRequest request, Chat2G_LoginResponse response, Action reply)
        {
            var chatUnitCpt = scene.GetComponent<ChatUnitComponent>();
            var chatUnit = chatUnitCpt.AddChatUnit(request.UnitId, request.Name, request.GateSessionId);

            response.ChatUnitInstanceId = chatUnit.InstanceId;
            reply();
            await ETTask.CompletedTask;
        }
    }
}