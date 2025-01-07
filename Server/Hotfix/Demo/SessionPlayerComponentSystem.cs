

namespace ET
{
	[FriendClass(typeof(SessionPlayerComponent))]
	public static class SessionPlayerComponentSystem
	{
		public class SessionPlayerComponentDestroySystem: DestroySystem<SessionPlayerComponent>
		{
			public override void Destroy(SessionPlayerComponent self)
			{
				
				// 清理服务器上角色信息
				if (!self.IsLoginAgain && self.PlayerInstaceId != 0)
				{
					Player player = Game.EventSystem.Get(self.PlayerInstaceId) as Player;
					SessionDisconnectHelp.LetPlayLeave(player).Coroutine();
				}


				self.AccountID = 0;
				self.PlayerInstaceId = 0;
				self.IsLoginAgain = false;
				self.PlayerID = 0;


			}
		}

		public static Player GetMyPlayer(this SessionPlayerComponent self)
		{
			return self.Domain.GetComponent<PlayerComponent>().Get(self.AccountID);
		}
	}
}
