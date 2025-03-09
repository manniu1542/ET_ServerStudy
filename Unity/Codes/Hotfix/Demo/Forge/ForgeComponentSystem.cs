using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Schema;
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

    [FriendClass(typeof (ForgeComponent))]
    public static class ForgeComponentSystem
    {
        public static void Clear(this ForgeComponent self)
        {
            //判断背包容量
            foreach (var item in self.dicProductions)
            {
                item.Value.Dispose();
            }

            self.listProductions.Clear();
            self.dicProductions.Clear();
        }

        public static Production GetProductionById(this ForgeComponent self, long id)
        {
            self.dicProductions.TryGetValue(id, out Production production);
            return production;
        }

        public static void RemoveProduction(this ForgeComponent self, long id)
        {
            var pro = self.GetProductionById(id);
            if (pro == null) return;

            self.dicProductions.Remove(id);
            self.listProductions.Remove(pro);
            pro.Dispose();

            Game.EventSystem.Publish(new EventType.RefreshForgeRedPoint() { ZoneScene = self.ZoneScene() });
        }

        public static void ResetFormData(this ForgeComponent self, ref List<ForgeProductionInfo> data)
        {
            self.Clear();

            data.ForEach(x => { self.AddProduction(x); });
            Game.EventSystem.Publish(new EventType.RefreshForgeRedPoint() { ZoneScene = self.ZoneScene() });
        }

        public static bool IsCanForgeNewItem(this ForgeComponent self)
        {
            var numCpt = UnitHelper.GetMyUnitNumericComponent(self.ZoneScene().CurrentScene());
            int level = numCpt.GetAsInt(NumericType.Level);
            int canForgeCount = PlayerLevelConfigCategory.Instance.Get(level).ForgeTheNumOfQueues;

            return canForgeCount > self.dicProductions.Count;
        }

        public static bool AddProduction(this ForgeComponent self, ForgeProductionInfo data)
        {
            if (self.dicProductions.ContainsKey(data.ProdictionId))
            {
                Log.Error("添加失败有重复id" + data.ProdictionId);
                return false;
            }

            var production = self.AddChildWithId<Production>(data.ProdictionId);
            production.ResetFormData(data);
            self.dicProductions.Add(data.ProdictionId, production);
            self.listProductions.Add(production);
            return true;
        }
        
        public static  bool  IsNeedSpwanMakingTime(this ForgeComponent self)
        {
            foreach (Production selfListProduction in self.listProductions)
            {
                if (!selfListProduction.IsNeedReceive())
                    return true;
            }
            return false;
        }
    }
}