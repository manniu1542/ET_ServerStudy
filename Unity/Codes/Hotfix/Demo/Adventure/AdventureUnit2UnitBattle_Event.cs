
using UnityEngine;

namespace ET
{
    [FriendClassAttribute(typeof (ET.AdventureComponent))]
    public class AdventureUnit2UnitBattle_Event: AEvent<EventType.AdventureUnit2UnitBattle>
    {
        protected override void Run(EventType.AdventureUnit2UnitBattle args)
        {
            var curScene = args.ZoneScene.GetComponent<CurrentScenesComponent>().Scene;
            var unitCpt = curScene.GetComponent<UnitComponent>();

            var attacker = unitCpt.Get(args.AttackerUnitID);
            var target = unitCpt.Get(args.TartgetUnitID);

            var attNumCpt = attacker.GetComponent<NumericComponent>();
            var targetNumCpt = target.GetComponent<NumericComponent>();

            int HpValue = targetNumCpt.GetAsInt(NumericType.Hp);

            var random = curScene.GetComponent<AdventureComponent>().battleRandom;
            int damValue = AdventureDamageHelper.CaclutaionDamage(attNumCpt, targetNumCpt, ref random);

            int targetNowHp = HpValue - damValue;
            //被攻击者活着
            if (targetNowHp < 0)
            {
                targetNowHp = 0;
                //播放 死亡动画
                target.SetAlive(false);
            }

            targetNumCpt[NumericType.Hp] = targetNowHp;

            //生成伤害的 显示
            Game.EventSystem.PublishAsync(new EventType.CreateUnitDamageValue
            {
                ZoneScene = args.ZoneScene, unitId = target.Id, damgeValue = damValue
            }).Coroutine();
        }
    }
}