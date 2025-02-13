namespace ET
{
    [FriendClass(typeof (Player))]
    public static class PlayerSystem
    {
        [ObjectSystem]
        public class PlayerAwakeSystem: AwakeSystem<Player, long, long>
        {
            public override void Awake(Player self, long account, long roleId)
            {
                self.Account = account;
                self.UintId = roleId;
                self.State = PlayerState.Disconect;
                self.SessionInstanceId = 0;
            }
        }

        [ObjectSystem]
        public class PlayerDestroySystem: DestroySystem<Player>
        {
            public override void Destroy(Player self)
            {
                self.Account = 0;
                self.UintId = 0;
                self.State = PlayerState.Disconect;
                self.SessionInstanceId = 0;
            }
        }
    }
}