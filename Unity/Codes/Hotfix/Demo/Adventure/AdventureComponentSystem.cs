using System;
using UnityEngine;

namespace ET.Adventure
{
    [Timer(TimerType.AdventureStartEnterRound)]
    public class AdventureStartEnterRoundTimer: ATimer<AdventureComponent>
    {
        public override void Run(AdventureComponent self)
        {
            try
            {
                self?.EnterAdventureRound().Coroutine();
            }
            catch (Exception e)
            {
                Log.Error($"move timer error: {self.Id}\n{e}");
            }
        }
    }

    [ObjectSystem]
    public class AdventureComponentAwakeSystem: AwakeSystem<AdventureComponent>
    {
        public override void Awake(AdventureComponent self)
        {
        }
    }

    [ObjectSystem]
    public class AdventureComponentDestroySystem: DestroySystem<AdventureComponent>
    {
        public override void Destroy(AdventureComponent self)
        {
        }
    }

    [FriendClass(typeof (AdventureComponent))]
    public static class AdventureComponentSystem
    {
        public static async ETTask ResetAdventure(this AdventureComponent self)
        {
            //回收之前的敌人
            var unitCpt = self.DomainScene().GetComponent<UnitComponent>();
            for (int i = self.listEnemyUnitID.Count - 1; i >= 0; i--)
            {
                unitCpt.Remove(self.listEnemyUnitID[i]);
            }

            //属性重置
            self.listEnemyUnitID.Clear();
            self.roundCount = 0;
            self.TimerID = 0;
            //人物的动作重置

            await Game.EventSystem.PublishAsync(new EventType.AdventureStartReset() { ZoneScene = self.ZoneScene() });
            await ETTask.CompletedTask;
        }

        public static async ETTask CreateEnemy(this AdventureComponent self)
        {
            var numCpt = UnitHelper.GetMyUnitNumericComponent(self.DomainScene());
            long levelId = numCpt[NumericType.AdventureState];
            levelId -= 1;
    
            BattleLevelConfig config = BattleLevelConfigCategory.Instance.GetConfigByIndex((int)levelId);

            for (int i = 0; i < config.MonsterIds.Length; i++)
            {
                Unit unitM = await UnitFactory.CreateMonster(self.DomainScene(), config.MonsterIds[i]);
                unitM.Position = new Vector3(1.5f, -2 + i, 0);
                self.listEnemyUnitID.Add(config.MonsterIds[i]);
            }

            await ETTask.CompletedTask;
        }

        /// <summary>
        /// 开始冒险
        /// </summary>
        /// <param name="self"></param>
        /// <param name="zoneScene"></param>
        public static async ETTask StartAdventure(this AdventureComponent self)
        {
            Log.Error("开始冒险!!");
            //重置冒险，
            await self.ResetAdventure();
            //生成冒险的敌人
            await self.CreateEnemy();

            self.TimerID = TimerComponent.Instance.NewOnceTimer(500, TimerType.AdventureStartEnterRound, self);

            await ETTask.CompletedTask;
        }

        public static async ETTask EnterAdventureRound(this AdventureComponent self)
        {
            Log.Error("进入战斗回合！");

            await ETTask.CompletedTask;
        }
    }
}