using UnityEditor.UI;
using UnityEngine;

namespace ET
{
    public class AdventureBattleRole_Event: AEventAsync<EventType.AdventureBattleRole>
    {
        protected override async ETTask Run(EventType.AdventureBattleRole args)
        {
            var curScene = args.ZoneScene.GetComponent<CurrentScenesComponent>().Scene;
            var unitCpt = curScene.GetComponent<UnitComponent>();

            var attacker = unitCpt.Get(args.AttackerUnitID);
            var target = unitCpt.Get(args.TartgetUnitID);

            var attNumCpt = attacker.GetComponent<NumericComponent>();
            var targetNumCpt = target.GetComponent<NumericComponent>();
            int damValue = attNumCpt.GetAsInt(NumericType.DamageValue);
            int HpValue = targetNumCpt.GetAsInt(NumericType.Hp);

            
            Game.EventSystem.PublishAsync(new EventType.CreateUnitDamageValue
            {
                ZoneScene = args.ZoneScene, unitId = target.Id, damgeValue = damValue
            }).Coroutine();
            
            attacker.GetComponent<AnimatorComponent>().Play(MotionType.Attack);
            //被攻击者活着
            if (HpValue - damValue > 0)
            {
                //播放受伤动画
                targetNumCpt[NumericType.Hp] = HpValue - damValue;
                target.GetComponent<AnimatorComponent>().Play(MotionType.Hurt);
            }
            else
            {
                targetNumCpt[NumericType.Hp] = 0;
                //播放 死亡动画
                target.SetAlive(false);
                args.TartgetUnitID = 0;
            }

    
            //等待300毫秒
            await TimerComponent.Instance.WaitAsync(1000);

            attacker.GetComponent<AnimatorComponent>().Play(MotionType.Idle);
            //证明当前 target还没被回收

            if (args.TartgetUnitID == target.Id)
            {
                target.GetComponent<AnimatorComponent>().Play(MotionType.Idle);
            }

            await ETTask.CompletedTask;
        }
    }
}