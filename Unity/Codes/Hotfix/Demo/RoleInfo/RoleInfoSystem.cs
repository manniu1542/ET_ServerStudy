using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET
{
    [ObjectSystem]
    public class RoleInfoAwakeSystem : AwakeSystem<RoleInfo>
    {
        public override void Awake(RoleInfo self)
        {

        }
    }

    [ObjectSystem]
    public class RoleInfoDestroySystem : DestroySystem<RoleInfo>
    {
        public override void Destroy(RoleInfo self)
        {

        }
    }

    [FriendClass(typeof(RoleInfo))]
    public static class RoleInfoSystem
    {

        public static void FromMessage(this RoleInfo self, MRoleInfo m)
        {

            self.AccountId = m.AccountId;
            self.Name = m.Name;
            self.ServerId = m.ServerId;
            self.State = (RoleInfoState)m.State;
            self.LastLoginTime = m.LastLoginTime;
            self.CreateRoleTime = m.CreateRoleTime;


        }

        public static MRoleInfo ToMessage(this RoleInfo self)
        {
            return new MRoleInfo()
            {
                AccountId = self.AccountId,
                Name = self.Name,
                ServerId = self.ServerId,
                State = (int)self.State,
                LastLoginTime = self.LastLoginTime,
                CreateRoleTime = self.CreateRoleTime,
            };
        }



    }
}
