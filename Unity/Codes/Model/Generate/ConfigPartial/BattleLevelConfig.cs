using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace ET
{
  
    public partial class BattleLevelConfigCategory 
    {

        public BattleLevelConfig GetConfigByIndex(int idx)
        {
            if (idx < this.list.Count)
            {
                return this.list[idx];
            }

            return null;
         
        }
        
    }

}
