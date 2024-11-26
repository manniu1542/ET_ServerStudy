using System;
using System.Linq;
using System.Numerics;

namespace ET
{
    [Timer(TimerType.PlayerLineOffComponentTimeCheck)]
    public class PlayerLineOffComponentTimeCheck : ATimer<PlayerLineOffComponent>
    {
        public override void Run(PlayerLineOffComponent self)
        {
            try
            {
                self.LineOffEvent();
            }
            catch (Exception e)
            {
                Log.Error($"move timer error: {self.Id}\n{e}");
            }
        }
    }
    [FriendClass(typeof(PlayerLineOffComponent))]
    public static class PlayerLineOffComponentSystem
    {
        public class AwakeSystem : AwakeSystem<PlayerLineOffComponent>
        {
            public override void Awake(PlayerLineOffComponent self)
            {
                TimerComponent.Instance.Remove(ref self.TimeId);
                self.TimeId = TimerComponent.Instance.NewOnceTimer(TimeHelper.ServerNow() + 10 * 1000, TimerType.PlayerLineOffComponentTimeCheck, self);
            }
        }

        [ObjectSystem]
        public class PlayerLineOffComponentDestroySystem : DestroySystem<PlayerLineOffComponent>
        {
            public override void Destroy(PlayerLineOffComponent self)
            {

                TimerComponent.Instance.Remove(ref self.TimeId);
            }
        }


        public static void LineOffEvent(this PlayerLineOffComponent self)
        {

            Player player = self.GetParent<Player>();

            self.DomainScene().GetComponent<PlayerComponent>().Remove(player.Account);

            player.Dispose();


            Game.EventSystem.Get(player.SessionInstanceId)?.Dispose();



        }

    }
}