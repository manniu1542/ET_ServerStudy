using System;

namespace ET
{

    [Timer(TimerType.AccountLoginOutTimeCheck)]
    public class AccountLoginOutTimeCheck : ATimer<AccountLoginOutTimeCheckComponent>
    {
        public override void Run(AccountLoginOutTimeCheckComponent self)
        {
            try
            {
                self.DisconnectOutTimeLoginSession();
            }
            catch (Exception e)
            {
                Log.Error($"move timer error: {self.Id}\n{e}");
            }
        }
    }

    [ObjectSystem]
    public class AccountLoginOutTimeCheckComponentAwakeSystem : AwakeSystem<AccountLoginOutTimeCheckComponent, long>
    {
        public override void Awake(AccountLoginOutTimeCheckComponent self, long account)
        {
            self.accountId = account;

            TimerComponent.Instance.Remove(ref self.timerId);
            self.timerId = TimerComponent.Instance.NewOnceTimer(TimeHelper.ServerNow() + self.autoCheckOutTime, TimerType.AccountLoginOutTimeCheck, self);

        }
    }
    [ObjectSystem]
    public class AccountLoginOutTimeCheckComponentDestroySystem : DestroySystem<AccountLoginOutTimeCheckComponent>
    {
        public override void Destroy(AccountLoginOutTimeCheckComponent self)
        {
            self.accountId = 0;

            TimerComponent.Instance.Remove(ref self.timerId);


        }
    }
    [FriendClass(typeof(AccountLoginOutTimeCheckComponent))]
    public static class AccountLoginOutTimeCheckComponentSystem
    {

        public static void DisconnectOutTimeLoginSession(this AccountLoginOutTimeCheckComponent self)
        {
            var scrAls = self.DomainScene().GetComponent<AccountLoginSessionComponent>();
            var session = scrAls.Get(self.accountId);

            if (session != null && !session.IsDisposed && session.Id == self.Parent.Id)
            {
                session.Send(new A2C_Disconnect() { Error = 1 });
                scrAls.Remove(self.accountId);
            }
            session.Disconnect().Coroutine();


            self.Dispose();


        }


    }
}
