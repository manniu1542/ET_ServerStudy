using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET
{
    [ObjectSystem]
    public class RoleInfoComponentAwakeSystem : AwakeSystem<RoleInfoComponent>
    {
        public override void Awake(RoleInfoComponent self)
        {
            self.dicRoleInfo = new Dictionary<long, RoleInfo>();
        }
    }

    [ObjectSystem]
    public class RoleInfoComponentDestroySystem : DestroySystem<RoleInfoComponent>
    {
        public override void Destroy(RoleInfoComponent self)
        {

        }
    }
    [FriendClass(typeof(RoleInfo))]
    [FriendClass(typeof(RoleInfoComponent))]
    public static class RoleInfoComponentSystem
    {

        public static void Add(this RoleInfoComponent self, RoleInfo ri)
        {
            if (self.dicRoleInfo.ContainsKey(ri.AccountId))
            {
                Log.Error("添加重复角色元素！" + ri.Name);
                return;
            }
            self.dicRoleInfo.Add(ri.AccountId, ri);


        }

        public static void Add(this RoleInfoComponent self, MRoleInfo ri)
        {
            if (self.dicRoleInfo.ContainsKey(ri.AccountId))
            {
                Log.Error("添加重复角色元素！" + ri.Name);
                return;
            }
            var roleInfo = self.AddChildWithId<RoleInfo>(ri.AccountId);
            roleInfo.FromMessage(ri);


            self.dicRoleInfo.Add(roleInfo.AccountId, roleInfo);


        }
        public static void Remove(this RoleInfoComponent self, long id)
        {
            self.dicRoleInfo.Remove(id);
        }
        public static void ClearAll(this RoleInfoComponent self)
        {
            self.dicRoleInfo.Clear();
        }
        public static RoleInfo Get(this RoleInfoComponent self, long id)
        {
            self.dicRoleInfo.TryGetValue(id, out RoleInfo ri);
            return ri;
        }
        public static bool Exit(this RoleInfoComponent self, long id)
        {


            return self.dicRoleInfo.ContainsKey(id); ;

        }
    }
}
