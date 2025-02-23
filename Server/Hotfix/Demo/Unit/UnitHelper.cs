using System.Collections.Generic;
using UnityEngine;

namespace ET
{
    [FriendClass(typeof (GateMapComponent))]
    [FriendClass(typeof (Unit))]
    [FriendClass(typeof (MoveComponent))]
    [FriendClass(typeof (NumericComponent))]
    public static class UnitHelper
    {
        public static UnitInfo CreateUnitInfo(Unit unit)
        {
            UnitInfo unitInfo = new UnitInfo();
            NumericComponent nc = unit.GetComponent<NumericComponent>();
            unitInfo.UnitId = unit.Id;
            unitInfo.ConfigId = unit.ConfigId;
            unitInfo.Type = (int)unit.Type;
            // Vector3 position = unit.Position;
            // unitInfo.X = position.x;
            // unitInfo.Y = position.y;
            // unitInfo.Z = position.z;
            // Vector3 forward = unit.Forward;
            // unitInfo.ForwardX = forward.x;
            // unitInfo.ForwardY = forward.y;
            // unitInfo.ForwardZ = forward.z;
            //
            // MoveComponent moveComponent = unit.GetComponent<MoveComponent>();
            // if (moveComponent != null)
            // {
            //     if (!moveComponent.IsArrived())
            //     {
            //         unitInfo.MoveInfo = new MoveInfo();
            //         for (int i = moveComponent.N; i < moveComponent.Targets.Count; ++i)
            //         {
            //             Vector3 pos = moveComponent.Targets[i];
            //             unitInfo.MoveInfo.X.Add(pos.x);
            //             unitInfo.MoveInfo.Y.Add(pos.y);
            //             unitInfo.MoveInfo.Z.Add(pos.z);
            //         }
            //     }
            // }

            foreach ((int key, long value) in nc.NumericDic)
            {
                unitInfo.Ks.Add(key);
                unitInfo.Vs.Add(value);
            }

            return unitInfo;
        }

        // 获取看见unit的玩家，主要用于广播
        public static Dictionary<long, AOIEntity> GetBeSeePlayers(this Unit self)
        {
            return self.GetComponent<AOIEntity>().GetBeSeePlayers();
        }

        public static void NoticeUnitAdd(Unit unit, Unit sendUnit)
        {
            M2C_CreateUnits createUnits = new M2C_CreateUnits();
            createUnits.Units.Add(CreateUnitInfo(sendUnit));
            MessageHelper.SendToClient(unit, createUnits);
        }

        public static void NoticeUnitRemove(Unit unit, Unit sendUnit)
        {
            M2C_RemoveUnits removeUnits = new M2C_RemoveUnits();
            removeUnits.Units.Add(sendUnit.Id);
            MessageHelper.SendToClient(unit, removeUnits);
        }

        /// <summary>
        /// 加载数据库中的Unit(先从数据库中拿取 对应的Unit,如果有拿，没有则创建再写入数据库)
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public static async ETTask<(bool,Unit)> LoadUnit(Player player)
        {
            GateMapComponent gateMapComponent = player.AddComponent<GateMapComponent>();
            //创建一个动态场景（就是为了创建Unit时，用的 逻辑场景）
            gateMapComponent.Scene = await SceneFactory.Create(gateMapComponent, "GateMap", SceneType.Map);

            Unit unit = await UnitChacheHelper.GetUnitChache(gateMapComponent.Scene, player.UintId);
            bool isNewUnit = unit == null;
            if (isNewUnit)
            {
                unit = UnitFactory.Create(gateMapComponent.Scene, player.UintId, UnitType.Player);
                UnitChacheHelper.AddOrUpdateAllUnitChache(unit).Coroutine();
            }

           
            return (isNewUnit,unit);
        }

       /// <summary>
       /// 初始化Unit
       /// </summary>
       /// <param name="unit"></param>
        public static async ETTask InitUnit(Unit unit,bool isNew)
        {
            await ETTask.CompletedTask;
        }
       
       
        /// <summary>
        /// 是否活着
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static bool IsAlive(this Unit self)
        {
            if (self == null || self.IsDisposed) return false;

            var numCpt = self.GetComponent<NumericComponent>();
            if (numCpt == null) return false;
            
            return numCpt[NumericType.IsAlive] == 0;
        }

        /// <summary>
        /// 设置是否活着
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static void SetAliveNoEvent(this Unit self, bool isAlive)
        {
            if (self == null || self.IsDisposed) return;

            var numCpt = self.GetComponent<NumericComponent>();
            if (numCpt == null) return;

            numCpt.SetNoEvent(NumericType.IsAlive,isAlive? 0 : 1);
        }
   
    }
}