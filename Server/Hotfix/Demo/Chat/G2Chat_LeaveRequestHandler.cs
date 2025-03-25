using System;

namespace ET
{
   
    public class G2Chat_LeaveRequestHandler: AMActorRpcHandler<ChatUnit, G2Chat_LeaveRequest, Chat2G_LeaveResponse>
    {
        protected override async ETTask Run(ChatUnit chatUnit, G2Chat_LeaveRequest request, Chat2G_LeaveResponse response, Action reply)
        {
            // var chatUnit = Game.EventSystem.Get(request.ChatUnitInstanceId) as ChatUnit;
            if (chatUnit == null)
            {
                response.Error = ErrorCode.ERR_ChatLeaveChatUnitNotExist;
                Log.Error("聊天服务器退出失败，（没有找到chatunit）");
                reply();
                return;
            }

            var chatUnitCpt = chatUnit.GetParent<ChatUnitComponent>();
            chatUnitCpt.RemoveChatUnit(chatUnit.Id);
            reply();
            await ETTask.CompletedTask;
        }
    }
}