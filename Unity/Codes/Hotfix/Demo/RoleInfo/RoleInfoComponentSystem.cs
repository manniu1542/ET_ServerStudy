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
            self.ClearAll();
            self.dicRoleInfo = null;
        }
    }
    [FriendClass(typeof(RoleInfo))]
    [FriendClass(typeof(RoleInfoComponent))]
    public static class RoleInfoComponentSystem
    {

        public static void Add(this RoleInfoComponent self, RoleInfo ri)
        {
            if (self.dicRoleInfo.ContainsKey(ri.Id))
            {
                Log.Error("添加重复角色元素！" + ri.Name);
                return;
            }
            self.dicRoleInfo.Add(ri.Id, ri);


        }

        public static void Add(this RoleInfoComponent self, MRoleInfo ri)
        {
            if (self.dicRoleInfo.ContainsKey(ri.RoleId))
            {
                Log.Error("添加重复角色元素！" + ri.Name);
                return;
            }
            var roleInfo = self.AddChildWithId<RoleInfo>(ri.RoleId);
            roleInfo.FromMessage(ri);


            self.dicRoleInfo.Add(roleInfo.Id, roleInfo);


        }
        public static void Remove(this RoleInfoComponent self, long roleID)
        {
            self.dicRoleInfo.Remove(roleID);
        }
        public static void ClearAll(this RoleInfoComponent self)
        {
            self.dicRoleInfo.Foreach((x,v) => v?.Dispose());
            self.dicRoleInfo.Clear();
        }
        public static RoleInfo Get(this RoleInfoComponent self, long roleid)
        {
            self.dicRoleInfo.TryGetValue(roleid, out RoleInfo ri);
            return ri;
        }
        public static bool Exit(this RoleInfoComponent self, long roleid)
        {


            return self.dicRoleInfo.ContainsKey(roleid); ;

        }
        public static void SetEnterGameRoleId(this RoleInfoComponent self, long roleid)
        {


            self.curEnterGameRoleId = roleid;

        }
        public static bool IsChooseGameRoleId(this RoleInfoComponent self)
        {
           return self.curEnterGameRoleId != 0;
        }
    }
}
