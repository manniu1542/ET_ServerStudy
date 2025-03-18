using System;
using UnityEngine;

namespace ET
{
    [FriendClass(typeof (Unit))]
    public static class UnitFactory
    {
        public static Unit Create(Scene scene, long id, UnitType unitType)
        {
            UnitComponent unitComponent = scene.GetComponent<UnitComponent>();
            switch (unitType)
            {
                case UnitType.Player:
                {
                    Unit unit = unitComponent.AddChildWithId<Unit, int>(id, 1001);
      
                    NumericComponent numericComponent = unit.AddComponent<NumericComponent>();
             
                    var playerNumericConfigs = PlayerNumericConfigCategory.Instance.GetAll();
                    foreach (var attribute in playerNumericConfigs)
                    {
                        //初始属性跳过
                        if(attribute.Value.BaseValue==0)continue;;
                        
                        if (attribute.Key < 3000)//有加成推导的最终属性
                        {
                            int baseKey = attribute.Key * 10 + 1;
                            numericComponent.SetNoEvent(baseKey,attribute.Value.BaseValue);

                        }
                        else//直接使用没有加成的属性
                        {
                            numericComponent.SetNoEvent(attribute.Key,attribute.Value.BaseValue);
                        }
                        
                        
                    }

                    unit.AddComponent<BagComponent>();
                    
                    unit.AddComponent<RoleEquipComponent>();
                    unit.AddComponent<ForgeComponent>();
                    unit.AddComponent<TaskComponent>();
                    
                    // 加入aoi
                    // unit.AddComponent<AOIEntity, int, Vector3>(9 * 1000, unit.Position);
                    return unit;
                }
                default:
                    throw new Exception($"not such unit type: {unitType}");
            }
        }
        
        
        
        public static  Unit CreateMonster(Scene currentScene, int monsterId)
        {
            var monsterConfig = UnitConfigCategory.Instance.Get(monsterId);
            UnitComponent unitComponent = currentScene.GetComponent<UnitComponent>();
            Unit unit = unitComponent.AddChildWithId<Unit, int>(IdGenerater.Instance.GenerateId(), monsterConfig.Id);

            var numCpt = unit.AddComponent<NumericComponent>();
            numCpt.SetNoEvent(NumericType.IsAlive, 0);
            numCpt.SetNoEvent(NumericType.Hp, monsterConfig.MaxHP);
            numCpt.SetNoEvent(NumericType.MaxHp, monsterConfig.MaxHP);
            numCpt.SetNoEvent(NumericType.DamageValue, monsterConfig.DamageValue);
            
            return unit;
        }
        
    }
}