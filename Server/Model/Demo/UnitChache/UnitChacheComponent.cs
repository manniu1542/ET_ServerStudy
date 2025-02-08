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
        public Dictionary<string, UnitChache> dicUnitChache = new Dictionary<string, UnitChache>();

        /// <summary>
        /// 已经继承了IChache的所有类型组件
        /// </summary>
        public List<string> listUnitChacheKey = new List<string>();
    }
}