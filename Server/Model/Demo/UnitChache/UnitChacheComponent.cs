using System.Collections.Generic;

namespace ET
{

    [ComponentOf(typeof (Scene))]
    [ChildType(typeof (UnitChache))]
    public class UnitChacheComponent: Entity, IAwake, IDestroy
    {
        /// <summary>
        /// 缓存的Unit组件
        /// </summary>
        public Dictionary<string, Entity> dicUnitChache = new Dictionary<string, Entity>();

        /// <summary>
        /// 已经保存的UnitId
        /// </summary>
        public List<string> listChacheUnitKey = new List<string>();
    }
}