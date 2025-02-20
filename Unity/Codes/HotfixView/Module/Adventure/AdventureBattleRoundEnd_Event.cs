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
                    adCpt.EnterAdventureRound().Coroutine();
                    return;
                case AdventureBattleRoundState.Win:
                    UnitHelper.GetMyUnitFromZoneScene(args.ZoneScene).GetComponent<AnimatorComponent>()?.Play(MotionType.Win);
                    await TimerComponent.Instance.WaitAsync(1000);
                    break;
                case AdventureBattleRoundState.Lose:

                    adCpt.ResetCurRoundAliveEnemy();

                    foreach (var mosterId in adCpt.listAliveEnemyUnitID)
                    {
                        var unit = unitCpt.Get(mosterId);
                        unit.GetComponent<AnimatorComponent>()?.Play(MotionType.Win);
                    }

                    await TimerComponent.Instance.WaitAsync(1000);
                    //回收所有敌人
                    for (int i = adCpt.listAliveEnemyUnitID.Count - 1; i >= 0; i--)
                    {
                        var unit = unitCpt.Get(adCpt.listAliveEnemyUnitID[i]);
                        unit?.Dispose();
                    }
             
                    break;
            }
 
            bool isFinish = await AdventureHelper.OnEndGameCheck(args.ZoneScene, adCpt.roundCount);
            if (isFinish)
            {
                Log.Error("战斗结束！ 处理发放奖励道具。");
                args.ZoneScene.GetComponent<UIComponent>().ShowWindow<DlgAdventure>();
                UnitHelper.GetMyUnitFromZoneScene(args.ZoneScene).GetComponent<AnimatorComponent>()?.Play(MotionType.Idle);
                
            }

            await ETTask.CompletedTask;
        }
    }
}