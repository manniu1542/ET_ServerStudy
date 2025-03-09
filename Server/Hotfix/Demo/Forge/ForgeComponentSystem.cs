using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ET
{
    [ObjectSystem]
    public class ForgeComponentAwakeSystem: AwakeSystem<ForgeComponent>
    {
        public override void Awake(ForgeComponent self)
        {
        }
    }

    [ObjectSystem]
    public class ForgeComponentDestroySystem: DestroySystem<ForgeComponent>
    {
        public override void Destroy(ForgeComponent self)
        {
            self.Clear();
        }
    }

    [ObjectSystem]
    public class ForgeComponentDeserializeSystem: DeserializeSystem<ForgeComponent>
    {
        public override void Deserialize(ForgeComponent self)
        {
            self.dicProductions.Clear();
            foreach (var production in self.Children)
            {
                if (production.Value is Production)
                    self.dicProductions.Add(production.Key, production.Value as Production);
            }
        }
    }

    [FriendClass(typeof (ForgeComponent))]
    [FriendClassAttribute(typeof (ET.Production))]
    public static class ForgeComponentSystem
    {
        public static void Clear(this ForgeComponent self)
        {
            //判断背包容量
            foreach (var item in self.dicProductions)
            {
                item.Value.Dispose();
            }

            self.dicProductions.Clear();
        }

        public static Production GetProductionById(this ForgeComponent self, long id)
        {
            self.dicProductions.TryGetValue(id, out Production production);
            return production;
        }

        public static bool IsHasProductionFinishById(this ForgeComponent self, long id)
        {
            var pro = self.GetProductionById(id);

            if (pro == null) return false;
            return pro.IsNeedReceive();
      
        }

        public static Production AddProductionByConfig(this ForgeComponent self, int productionConfigId)
        {
            var production = self.AddChild<Production>();
            production.configID = productionConfigId;
            production.state = ProductionReceiveState.Making;
            production.startTime = TimeHelper.ServerNow();
            production.endTime = production.Config.ProductionTime * 1000 + production.startTime;

            self.dicProductions.Add(production.Id, production);
            return production;
        }
        public static void RemoveProductionByid(this ForgeComponent self, long id)
        {
            var pro = self.GetProductionById(id);
            pro.Dispose();
            self.dicProductions.Remove(id);
        }
        
   
    }
}