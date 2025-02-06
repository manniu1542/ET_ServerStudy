using System.Collections.Generic;

namespace ET
{
    public interface IUnitChache
    {
    }

    [ComponentOf(typeof (Scene))]
    [ChildType(typeof (UnitChache))]
    public class UnitChacheComponent: Entity, IAwake, IDestroy
    {
        /// <summary>
        /// 缓存的Unit组件
        /// </summary>
        public Dictionary<long, Entity> dicUnitChache;

        /// <summary>
        /// 已经保存的UnitId
        /// </summary>
        public List<long> listUnitID;
    }
}