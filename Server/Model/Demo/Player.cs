namespace ET
{
	public enum PlayerState
	{
		//断线
		Disconect,
		//网关
		Gate,
		//游戏
		Game,
	}

	public sealed class Player : Entity, IAwake<long,long>,IDestroy
	{
		//账号id
		public long Account { get; set; }
		
		//角色id
		public long UintId { get; set; }
		//会话实体id
        public long SessionInstanceId { get; set; }
        public PlayerState State { get; set; }
        //聊天服务器上的用户id
        public long ChatUnitInstanceId	{ get; set;}
    }
}