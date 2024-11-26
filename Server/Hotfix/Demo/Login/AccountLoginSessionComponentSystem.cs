using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET
{
    [ObjectSystem]
    public class AccountLoginCenterComponentAwakeSystem : AwakeSystem<AccountLoginSessionComponent>
    {
        public override void Awake(AccountLoginSessionComponent self)
        {
            self.dicLoginAccountSessionId = new Dictionary<long, Session>();

        }
    }
    [FriendClass(typeof(AccountLoginSessionComponent))]
    public static class AccountLoginCenterComponentSystem
    {
   
        public static Session Get(this AccountLoginSessionComponent self, long id)
        {
            if (!self.dicLoginAccountSessionId.TryGetValue(id, out Session session))
            {
                session = null;
            }
            return session;
        }
        public static void Add(this AccountLoginSessionComponent self, long id, Session session)
        {
            if (self.dicLoginAccountSessionId.ContainsKey(id))
            {
                self.dicLoginAccountSessionId[id] = session;
            }
            else
            {
                self.dicLoginAccountSessionId.Add(id, session);
            }
      
        }

        public static void Remove(this AccountLoginSessionComponent self, long id)
        {
            if (self.dicLoginAccountSessionId.ContainsKey(id))
            {
                self.dicLoginAccountSessionId.Remove(id);
            }


        }

    }
}
