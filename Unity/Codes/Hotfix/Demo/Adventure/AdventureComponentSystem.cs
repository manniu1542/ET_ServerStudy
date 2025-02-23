using System;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

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
            TimerComponent.Instance.Remove(ref self.TimerID);
       
        }
    }

    [FriendClass(typeof (AdventureComponent))]
    public static class AdventureComponentSystem
    {
        /// <summary>
        /// 重置冒险参数
        /// </summary>
        /// <param name="self"></param>
        public static  void ResetBattleRandom(this AdventureComponent self)
        {
            var numCpt = UnitHelper.GetMyUnitNumericComponent(self.DomainScene());
            uint seed = (uint)numCpt[NumericType.AdventureRandomSeed];
            if(self.battleRandom == null)
                self.battleRandom = new SRandom(seed);
            else
            {
                self.battleRandom.SetRandomSeed(seed);
            }
            
        }

     
        /// <summary>
        /// 重置冒险参数
        /// </summary>
        /// <param name="self"></param>
        public static async ETTask ResetAdventure(this AdventureComponent self)
        {
            //回收之前的敌人
            var unitCpt = self.DomainScene().GetComponent<UnitComponent>();
            for (int i = self.listEnemyUnitID.Count - 1; i >= 0; i--)
            {
                unitCpt.Remove(self.listEnemyUnitID[i]);
            }

            //属性重置
            self.listAliveEnemyUnitID.Clear();
            self.listEnemyUnitID.Clear();
            self.roundCount = 0;
            self.TimerID = 0;
            self.ResetBattleRandom();
            //人物的动作重置

            await Game.EventSystem.PublishAsync(new EventType.AdventureStartReset() { ZoneScene = self.ZoneScene() });
            await ETTask.CompletedTask;
        }

        public static async ETTask CreateEnemy(this AdventureComponent self)
        {
            var numCpt = UnitHelper.GetMyUnitNumericComponent(self.DomainScene());
            long levelId = numCpt[NumericType.AdventureState];
       
            BattleLevelConfig config = BattleLevelConfigCategory.Instance.Get((int)levelId);

            for (int i = 0; i < config.MonsterIds.Length; i++)
            {
                Unit unitM = await UnitFactory.CreateMonster(self.DomainScene(), config.MonsterIds[i]);
                unitM.Position = new Vector3(1.5f, -2 + i, 0);
                self.listEnemyUnitID.Add(unitM.Id);
            }

            await ETTask.CompletedTask;
        }

        /// <summary>
        /// 启动一个回合的战斗
        /// </summary>
        public static void StartNewRoundOneBattle(this AdventureComponent self)
        {
            self.TimerID = TimerComponent.Instance.NewOnceTimer(TimeHelper.ServerNow() + 500, TimerType.AdventureStartEnterRound, self);
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
            //更新显示所有血条
            self.SetUpdateAllUnitHeadHp(true);

            self.StartNewRoundOneBattle();
            await ETTask.CompletedTask;
        }

        //设置角色血条
        public static void SetUpdateAllUnitHeadHp(this AdventureComponent self, bool isShow)
        {
            //玩家自己
            Unit unitSelf = UnitHelper.GetMyUnitFromCurrentScene(self.DomainScene());
            Game.EventSystem.Publish(new EventType.SetUpdateHeadHp { ZoneScene = self.ZoneScene(), isShow = isShow, unitId = unitSelf.Id });
            for (int i = 0; i < self.listEnemyUnitID.Count; i++)
            {
                Game.EventSystem.Publish(new EventType.SetUpdateHeadHp
                {
                    ZoneScene = self.ZoneScene(), isShow = isShow, unitId = self.listEnemyUnitID[i]
                });
            }
        }

        public static async ETTask EnterAdventureRound(this AdventureComponent self)
        {
            var unit = UnitHelper.GetMyUnitFromZoneScene(self.ZoneScene());
            self.ResetCurRoundAliveEnemy();
 
            if (self.roundCount % 2 == 0)
            {
                if (unit.IsAlive())
                {
                    Game.EventSystem.PublishAsync(new EventType.AdventureUnit2UnitBattleView
                    {
                        ZoneScene = self.ZoneScene(), AttackerUnitID = unit.Id, TartgetUnitID = self.listAliveEnemyUnitID[0]
                    }).Coroutine();
                    Game.EventSystem.Publish(new EventType.AdventureUnit2UnitBattle
                    {
                        ZoneScene = self.ZoneScene(), AttackerUnitID = unit.Id, TartgetUnitID = self.listAliveEnemyUnitID[0]
                    });
                    await TimerComponent.Instance.WaitAsync(1000);
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
                        Game.EventSystem.PublishAsync(new EventType.AdventureUnit2UnitBattleView
                        {
                            ZoneScene = self.ZoneScene(), AttackerUnitID = unitM.Id, TartgetUnitID = myId
                        }).Coroutine();
                        ;
                        Game.EventSystem.Publish(new EventType.AdventureUnit2UnitBattle
                        {
                            ZoneScene = self.ZoneScene(), AttackerUnitID = unitM.Id, TartgetUnitID = myId
                        });
                        await TimerComponent.Instance.WaitAsync(1000);
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