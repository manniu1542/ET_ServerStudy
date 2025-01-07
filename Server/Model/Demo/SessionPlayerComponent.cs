namespace ET
{
	[ComponentOf(typeof(Session))]
	public class SessionPlayerComponent : Entity, IAwake, IDestroy
	{ 
		public long PlayerID;
         //现在玩家id就是 玩家账号，
		public long AccountID;
		public long PlayerInstaceId;
		
		/// <summary>
		/// 是否顶号登录的（再顶号的时候不清理服务器上玩家信息，可以实现数据的断线重连）
		/// </summary>
		public bool IsLoginAgain;
	}
}