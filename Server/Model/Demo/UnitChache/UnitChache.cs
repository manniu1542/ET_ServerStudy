using System.Collections.Generic;

namespace ET
{
    
    
    public interface IUnitChache
    {
        
        
    }

   /// <summary>
   ///  某个unit组件类型 的所有 Unit该类型组件
   /// </summary>
    public class UnitChache:Entity ,IAwake,IDestroy
    {
        //某个组件类型
        public string key;
        //该组件类型下所有 unit的 该类型 实体 ，同一个类型用UnitId做区分
        public Dictionary<long, Entity> dicChacheComponent = new Dictionary<long, Entity>();


    }
}