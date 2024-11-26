using System;


namespace ET
{
	[MessageHandler]
	[FriendClass(typeof(SessionPlayerComponent))]
	public class C2G_LoginGateHandler : AMRpcHandler<C2G_LoginGate, G2C_LoginGate>
	{
		protected override async ETTask Run(Session session, C2G_LoginGate request, G2C_LoginGate response, Action reply)
		{
            // 移除超时 还未响应的 组件 (只在这里移除，LoginAccount是Gate服务器的第一个消息，需要移除session超时)
            session.RemoveComponent<SessionAcceptTimeoutComponent>();


            Scene scene = session.DomainScene();
			string account = scene.GetComponent<GateSessionKeyComponent>().Get(request.Key);
			if (account == null)
			{
				response.Error = ErrorCore.ERR_ConnectGateKeyError;
				response.Message = "Gate key验证失败!";
				reply();
				return;
			}
	

			PlayerComponent playerComponent = scene.GetComponent<PlayerComponent>();
			Player player = playerComponent.AddChild<Player, long,long>(1,1);
			playerComponent.Add(player);
			session.AddComponent<SessionPlayerComponent>().PlayerId = player.Id;
			session.AddComponent<MailBoxComponent, MailboxType>(MailboxType.GateSession);

			response.PlayerId = player.Id;
			reply();
			await ETTask.CompletedTask;
		}
	}
}