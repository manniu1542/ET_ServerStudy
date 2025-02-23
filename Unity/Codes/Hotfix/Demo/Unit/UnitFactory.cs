using UnityEngine;

namespace ET
{
    public static class UnitFactory
    {
        public static  Unit Create(Scene currentScene, UnitInfo unitInfo)
        {
            UnitComponent unitComponent = currentScene.GetComponent<UnitComponent>();
            Unit unit = unitComponent.AddChildWithId<Unit, int>(unitInfo.UnitId, unitInfo.ConfigId);

            NumericComponent numericComponent = unit.AddComponent<NumericComponent>();
            for (int i = 0; i < unitInfo.Ks.Count; ++i)
            {
                numericComponent.Set(unitInfo.Ks[i], unitInfo.Vs[i]);
            }

            unit.AddComponent<ObjectWait>();

             Game.EventSystem.PublishAsync(new EventType.AfterUnitCreate() { Unit = unit }).Coroutine();
            return unit;
        }

        public static async ETTask<Unit> CreateMonster(Scene currentScene, int monsterId)
        {
            
            var monsterConfig = UnitConfigCategory.Instance.Get(monsterId);
            UnitComponent unitComponent = currentScene.GetComponent<UnitComponent>();
            Unit unit = unitComponent.AddChildWithId<Unit, int>(IdGenerater.Instance.GenerateId(), monsterConfig.Id);

            var numCpt = unit.AddComponent<NumericComponent>();
            numCpt.SetNoEvent(NumericType.IsAlive, 0);
            numCpt.SetNoEvent(NumericType.Hp, monsterConfig.MaxHP);
            numCpt.SetNoEvent(NumericType.MaxHp, monsterConfig.MaxHP);
            numCpt.SetNoEvent(NumericType.DamageValue, monsterConfig.DamageValue);

            unit.AddComponent<ObjectWait>();

            await Game.EventSystem.PublishAsync(new EventType.AfterUnitCreate() { Unit = unit });
            return unit;
        }
    }
}