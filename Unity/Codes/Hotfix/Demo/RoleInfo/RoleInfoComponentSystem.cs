using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET
{
    [ObjectSystem]
    public class RoleInfoComponentAwakeSystem: AwakeSystem<RoleInfoComponent>
    {
        public override void Awake(RoleInfoComponent self)
        {
            self.roleInfo = null;
        }
    }

    [ObjectSystem]
    public class RoleInfoComponentDestroySystem: DestroySystem<RoleInfoComponent>
    {
        public override void Destroy(RoleInfoComponent self)
        {
            self.roleInfo = null;
        }
    }

    [FriendClass(typeof (RoleInfo))]
    [FriendClass(typeof (RoleInfoComponent))]
    public static class RoleInfoComponentSystem
    {
        public static void SetRoleInfo(this RoleInfoComponent self, RoleInfo ri)
        {
            self.roleInfo = ri;
        }

        public static void SetRoleInfo(this RoleInfoComponent self, MRoleInfo ri)
        {
            self.Remove();
            RoleInfo roleInfo = null;
            if (ri != null)
            {
                roleInfo = self.AddChildWithId<RoleInfo>(ri.AccountId);
                roleInfo.FromMessage(ri);
            }

            self.SetRoleInfo(roleInfo);
        }

        public static void Remove(this RoleInfoComponent self)
        {
            
            self.roleInfo?.Dispose();
            self.roleInfo = null;
        }

        public static RoleInfo Get(this RoleInfoComponent self)
        {
            return self.roleInfo;
        }

        public static bool Exit(this RoleInfoComponent self)
        {
            return self.roleInfo != null;
        }
    }
}