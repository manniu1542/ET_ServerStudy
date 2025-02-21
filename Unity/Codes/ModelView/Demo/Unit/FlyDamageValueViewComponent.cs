using System.Collections.Generic;
using UnityEngine;

namespace ET
{
    //血条  飘伤害
    [ComponentOf(typeof(Scene))]
    public class FlyDamageValueViewComponent : Entity,IAwake,IDestroy
    {
        public HashSet<GameObject> FlyingDamageSet = new HashSet<GameObject>();
   
       
    }
}