using UnityEditor.UI;

namespace ET
{
    [MessageHandler]
    public class A2C_DisconnectHandler : AMHandler<A2C_Disconnect>
    {
        protected override void Run(Session session, A2C_Disconnect message)
        {
            //顶号被踢下线
            if (message.Error == 0)
            {

                Log.Info("自己被踢下线，重置到重新登录的界面！");
            }//登录 不进入游戏超时处理
            else if (message.Error == 1)
            {

                Log.Info("登录 不进入游戏超时 被踢下线，重置到重新登录的界面！");
            }
            else
            {
                Log.Error("未知原因的下线：" + message.Error);
                return;

            }

            session.DomainScene().RemoveComponent<SessionComponent>();
            session.Dispose();


        }
    }
}
