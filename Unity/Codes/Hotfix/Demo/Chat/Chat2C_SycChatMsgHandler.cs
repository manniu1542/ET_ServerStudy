using UnityEngine;

namespace ET
{
    [MessageHandler]
    public class Chat2C_SycChatMsgHandler: AMHandler<Chat2C_SycChatMsg>
    {
        protected override void Run(Session session, Chat2C_SycChatMsg message)
        {
            var chatCpt = session.ZoneScene().GetComponent<ChatComponent>();
            chatCpt.AddMsg(message.ChatInfoData);
            Game.EventSystem.Publish(new EventType.RefreshChatUI(){ZoneScene =  session.ZoneScene()});
        }
    }
}