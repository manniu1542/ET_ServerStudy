using ET;
using UnityEngine;

namespace ET
{
    [FriendClassAttribute(typeof (ET.AdventureComponent))]
    public class AdventureBattleRoundEnd_Event: AEventAsync<EventType.AdventureBattleRoundEnd>
    {
        protected override async ETTask Run(EventType.AdventureBattleRoundEnd args)
        {
            var curScene = args.ZoneScene.GetComponent<CurrentScenesComponent>().Scene;
            var adCpt = curScene.GetComponent<AdventureComponent>();
            var unitCpt = curScene.GetComponent<UnitComponent>();
            //继续战斗
            switch (args.state)
            {
                case AdventureBattleRoundState.Keep:
                    adCpt.StartNewRoundOneBattle();
                    return;

                case AdventureBattleRoundState.Win:
                    UnitHelper.GetMyUnitFromZoneScene(args.ZoneScene).GetComponent<AnimatorComponent>()?.Play(MotionType.Win);
       
                    break;
                case AdventureBattleRoundState.Lose:
                    adCpt.ResetCurRoundAliveEnemy();
                    foreach (var mosterId in adCpt.listAliveEnemyUnitID)
                    {
                        var unit = unitCpt.Get(mosterId);
                        unit.GetComponent<AnimatorComponent>()?.Play(MotionType.Win);
                    }
                    break;
            }
            
            
            
            bool isFinish = await AdventureHelper.OnEndGameCheck(args.ZoneScene, adCpt.roundCount, args.state);
            if (isFinish)
            {
                Log.Error("战斗结束！ 处理发放奖励道具。");
            }
            //因为角色被杀死1秒后自动销毁。 如果我也延迟1000中会导致，播放动画途中。角色被销毁，或者 两个地方同时销毁一个角色
            await TimerComponent.Instance.WaitAsync(1500);
            //重置 战斗场景
            adCpt.SetUpdateAllUnitHeadHp(false);
            await adCpt.ResetAdventure();
         

            args.ZoneScene.GetComponent<UIComponent>().ShowWindow<DlgAdventure>();
            UnitHelper.GetMyUnitFromZoneScene(args.ZoneScene).GetComponent<AnimatorComponent>()?.Play(MotionType.Idle);

            await ETTask.CompletedTask;
        }
    }
}