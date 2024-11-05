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
    public class RoleInfo : Entity, IAwake, IDestroy
    {
 
        public long AccountId;

        public long ServerId;

        public string Name;

        public RoleInfoState State;

        public long LastLoginTime;

        public long CreateRoleTime;


    }
}
