using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ET
{
    [ObjectSystem]
    public class RankComponentAwakeSystem: AwakeSystem<RankComponent>
    {
        public override void Awake(RankComponent self)
        {
            
        }
    }

    [ObjectSystem]
    public class RankComponentDestroySystem: DestroySystem<RankComponent>
    {
        public override void Destroy(RankComponent self)
        {
           
        }
    }

    [FriendClass(typeof (RankComponent))]
    public static class RankComponentSystem
    {
       
        
    }
}