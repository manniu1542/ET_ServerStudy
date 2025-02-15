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
              
                    // 加入aoi
                    // unit.AddComponent<AOIEntity, int, Vector3>(9 * 1000, unit.Position);
                    return unit;
                }
                default:
                    throw new Exception($"not such unit type: {unitType}");
            }
        }
    }
}