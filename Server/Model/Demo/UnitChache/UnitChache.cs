using System.Collections.Generic;

namespace ET
{
    
    
    public interface IUnitChache
    {
        
        
    }


    public class UnitChache:Entity ,IAwake,IDestroy
    {

        public string key;

        public Dictionary<long, Entity> dicChacheComponent = new Dictionary<long, Entity>();


    }
}