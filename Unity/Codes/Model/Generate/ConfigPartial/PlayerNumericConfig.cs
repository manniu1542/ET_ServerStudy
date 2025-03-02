using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace ET
{
    public partial class PlayerNumericConfigCategory
    {
        public List<PlayerNumericConfig> listNeedShow = new List<PlayerNumericConfig>();
 
        public override void AfterEndInit()
        {
            listNeedShow.Clear();
            foreach (var item in this.GetAll())
            {
                if (item.Value.isNeedShow == 1)
                    listNeedShow.Add(item.Value);
                
            }
        }

  
    }
}