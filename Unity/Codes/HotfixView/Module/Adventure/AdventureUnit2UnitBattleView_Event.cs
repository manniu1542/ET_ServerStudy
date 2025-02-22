using UnityEditor.UI;
using UnityEngine;

namespace ET
{
    public class AdventureUnit2UnitBattleView_Event: AEventAsync<EventType.AdventureUnit2UnitBattleView>
    {
        protected override async ETTask Run(EventType.AdventureUnit2UnitBattleView args)
        {
            var curScene = args.ZoneScene.GetComponent<CurrentScenesComponent>().Scene;
            var unitCpt = curScene.GetComponent<UnitComponent>();

            var attacker = unitCpt.Get(args.AttackerUnitID);
            var target = unitCpt.Get(args.TartgetUnitID);
           
            
            
            var attNumCpt = attacker.GetComponent<NumericComponent>();
            var targetNumCpt = target.GetComponent<NumericComponent>();
            int damValue = attNumCpt.GetAsInt(NumericType.DamageValue);
            int HpValue = targetNumCpt.GetAsInt(NumericType.Hp);
           
            
            
            attacker.GetComponent<AnimatorComponent>().Play(MotionType.Attack);
            target.GetComponent<AnimatorComponent>().Play(MotionType.Hurt);
            //会自动回Idle 动画 状态下
         
            target.GetComponent<GameObjectComponent>().SpriteRenderer.color = Color.red;
            await TimerComponent.Instance.WaitAsync(300);
            //目标 Unit还没被 回收 ，他的颜色重置
            if (args.TartgetUnitID == target.Id)
            {
                target.GetComponent<GameObjectComponent>().SpriteRenderer.color = Color.white;
            }

            await ETTask.CompletedTask;
        }
    }
}