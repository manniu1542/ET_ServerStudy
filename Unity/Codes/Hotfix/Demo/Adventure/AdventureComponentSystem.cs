using System;
using UnityEngine;

namespace ET
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
                self.listEnemyUnitID.Add(unitM.Id);
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
         
            var unit = UnitHelper.GetMyUnitFromZoneScene(self.ZoneScene());
            self.ResetCurRoundAliveEnemy();

            if (self.roundCount % 2 == 0)
            {
                if (unit.IsAlive())
                {
                    await Game.EventSystem.PublishAsync(new EventType.AdventureBattleRole
                    {
                        ZoneScene = self.ZoneScene(), AttackerUnitID = unit.Id, TartgetUnitID = self.listAliveEnemyUnitID[0]
                    });
                }
            }
            else
            {
                long myId = unit.Id;
                foreach (var monsterId in self.listEnemyUnitID)
                {
                    var unitM = self.DomainScene().GetComponent<UnitComponent>().Get(monsterId);
                    if (unitM.IsAlive())
                    {
                        await Game.EventSystem.PublishAsync(new EventType.AdventureBattleRole
                        {
                            ZoneScene = self.ZoneScene(), AttackerUnitID = unitM.Id, TartgetUnitID = myId
                        });
                    }
                }
            }
            
            self.CheckCurRoundEnd();
            await ETTask.CompletedTask;
        }

        public static void CheckCurRoundEnd(this AdventureComponent self)
        {
            self.roundCount++;
            var unitSelf = UnitHelper.GetMyUnitFromZoneScene(self.ZoneScene());

            AdventureBattleRoundState state;
            //检查 玩家是否死亡 
            if (!unitSelf.IsAlive())
            {
                state = AdventureBattleRoundState.Lose;
            }
            else
            {
                //检查 敌人是否全部死亡  
                self.ResetCurRoundAliveEnemy();

                if (self.listAliveEnemyUnitID.Count <= 0)
                {
                    state = AdventureBattleRoundState.Win;
                }
                else
                {
                    state = AdventureBattleRoundState.Keep;
                }
            }

  
            Game.EventSystem.PublishAsync(new EventType.AdventureBattleRoundEnd { ZoneScene = self.ZoneScene(), state = state }).Coroutine();
        }

        public static void ResetCurRoundAliveEnemy(this AdventureComponent self)
        {
            self.listAliveEnemyUnitID.Clear();
            UnitComponent unitCpt = self.DomainScene().GetComponent<UnitComponent>();
            foreach (var monsterId in self.listEnemyUnitID)
            {
                var unitM = unitCpt.Get(monsterId);

                if (unitM.IsAlive())
                {
                    self.listAliveEnemyUnitID.Add(monsterId);
                }
            }
        }
    }
}