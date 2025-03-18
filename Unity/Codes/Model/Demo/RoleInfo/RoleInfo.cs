using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET
{
    public enum RoleInfoState
    {
        Normal,
        Freeze,
    }

    [ComponentOf()]
#if SERVER
    public class RoleInfo : Entity, IAwake, IDestroy, ITransfer, IUnitChache
#else
    public class RoleInfo : Entity, IAwake, IDestroy
#endif
    {
 
        public long AccountId;

        public int ServerId;

        public string Name;

        public RoleInfoState State;

        public long LastLoginTime;

        public long CreateRoleTime;


    }
}
