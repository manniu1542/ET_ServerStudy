using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET
{
    [ObjectSystem]
    public class ItemAwakeSystem : AwakeSystem<Item,int>
    {
        public override void Awake(Item self,int config)
        {
            self.configID = config; 
        
            
        }
    }

    [ObjectSystem]
    public class ItemDestroySystem : DestroySystem<Item>
    {
        public override void Destroy(Item self)
        {
          
        }
    }

    [FriendClass(typeof(Item))]
    public static class ItemSystem
    {

        public static void ReadAccountInfo(this Item self,long accountId,string token)
        {

          

        }



    }
}
