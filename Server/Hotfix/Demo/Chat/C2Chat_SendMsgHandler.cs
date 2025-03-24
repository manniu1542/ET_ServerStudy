using System;

namespace ET
{
    [FriendClass(typeof(GateMapComponent))]
    [FriendClass(typeof(SessionStateComponent))]
    [FriendClass(typeof(SessionPlayerComponent))]
    [FriendClass(typeof(PlayerComponent))]
    [FriendClassAttribute(typeof(ET.ChatUnit))]
    public class C2Chat_SendMsgHandler : AMActorRpcHandler<ChatUnit, C2Chat_SendMsg, Chat2C_SendMsg>
    {
        protected override async ETTask Run(ChatUnit chatUnit, C2Chat_SendMsg request, Chat2C_SendMsg response, Action reply)
        {
            //检查消息 是否合格（todo: 脏话。敏感词 等。。）
            if (string.IsNullOrEmpty(request.msg))
            {
                response.Error = ErrorCode.ERR_ChatSendMsgCheckFail;
                reply();
               
                return;
            }
            ChatUnitComponent chatUnitComponent = chatUnit.GetParent<ChatUnitComponent>();
            chatUnitComponent.SendBoardMsg(chatUnit.name, request.msg);

            reply();
            await ETTask.CompletedTask;
        }
    }
}