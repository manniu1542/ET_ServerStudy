namespace ET
{
	[ComponentOf(typeof(Session))]
	public class SessionPlayerComponent : Entity, IAwake, IDestroy
	{ 
		public long PlayerID;
         //现在玩家id就是 玩家账号，
		public long AccountID;
		public long PlayerInstaceId;
	}
}