namespace ET
{
    [MessageHandler]
    public class G2C_DisconnectHandler: AMHandler<G2C_ForcePlayerDisconnect>
    {
        protected override void Run(Session session, G2C_ForcePlayerDisconnect message)
        {
            
  
            Log.Info("自己被踢下线，重置到重新登录的界面！");
            UIComponent ui = session.DomainScene().GetComponent<UIComponent>();
            ui.CloseAllWindow();
            ui.ShowWindow(WindowID.WindowID_Login);
            session.Dispose();
        }
    }
}