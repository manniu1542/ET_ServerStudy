using System;
using System.Collections.Generic;
using System.Linq;

namespace ET
{
    public class AdventureCheckComponentAwakeSystem: AwakeSystem<AdventureCheckComponent>
    {
        public override void Awake(AdventureCheckComponent self)
        {
         
        }
    }

    public class AdventureCheckComponentDestroySystem: DestroySystem<AdventureCheckComponent>
    {
        public override void Destroy(AdventureCheckComponent self)
        {
            UnitComponent unitComponent = self.DomainScene().GetComponent<UnitComponent>();
            for (int i = self.listCacheEnemyId.Count - 1; i >= 0; i--)
            {
                unitComponent.Remove(self.listCacheEnemyId[i]);
            }

            self.listCacheEnemyId.Clear();
            self.listEnemyUnitID.Clear();
            self.battleRandom = null;
        }
    }

    [FriendClassAttribute(typeof (ET.AdventureCheckComponent))]
    [FriendClassAttribute(typeof (ET.Unit))]
    public static class AdventureCheckComponentSystem
    {
        public static bool CheckWinBattle(this AdventureCheckComponent self, int roundCount, int levelId)
        {
            try
            {
                self.ResetAdventureInfo();
                self.ResetBattleRandom();
                self.CreateEnemy();
                //验证对局是否能够完成

                if (!self.SimulationAdventure(roundCount, levelId))
                {
                    Log.Error("战斗的次数有问题");
                    return false;
                }

                //检查玩家总血量 与 敌人伤害

                if (!self.GetParent<Unit>().IsAlive())
                {
                    Log.Error("玩家无法存活！");
                    return false;
                }

                //检查怪物的总伤害 与怪物的总血量
                var unitCpt = self.DomainScene().GetComponent<UnitComponent>();
                foreach (var monsterId in self.listEnemyUnitID)
                {
                    if (unitCpt.Get(monsterId).IsAlive())
                    {
                        Log.Error("怪物根本没有死完！");
                        return false;
                    }
                }

                //动画时间。和战斗 时间
                var numCpt = self.GetParent<Unit>().GetComponent<NumericComponent>();
                long intervalTime = TimeHelper.ServerNow() - numCpt[NumericType.AdventureStartTime];
                if (intervalTime < self.battleAnimAllTime)
                {
                    Log.Error("战斗时长，有问题！");
                    return false;
                }

                return true;
            }
            finally
            {
                self.ResetAdventureInfo();
            }
        }

        public static bool SimulationAdventure(this AdventureCheckComponent self, int roundCount, int levelId)
        {
            var numCpt = self.Parent.GetComponent<NumericComponent>();

            var unitCpt = self.DomainScene().GetComponent<UnitComponent>();
            Unit unitM;
            NumericComponent NumMonster;
            //roundCount  客户端 最后一回合 其实没有战斗的 ，
            for (int i = 0; i < roundCount; i++)
            {
                //玩家出手
                if (i % 2 == 0)
                {
                    self.battleAnimAllTime += 1000;
                    for (int j = 0; j < self.listEnemyUnitID.Count; j++)
                    {
                        unitM = unitCpt.Get(self.listEnemyUnitID[j]);
                        if (!unitM.IsAlive())
                        {
                            continue;
                        }

                        NumMonster = unitM.GetComponent<NumericComponent>();
                        int damage = AdventureDamageHelper.CaclutaionDamage(numCpt, NumMonster, ref self.battleRandom);
                        int newHp = NumMonster.GetAsInt(NumericType.Hp) - damage;
                        if (newHp <= 0)
                        {
                            newHp = 0;
                            unitM.SetAliveNoEvent(false);
                        }

                        NumMonster.SetNoEvent(NumericType.Hp, newHp);
                        break;
                    }
                }
                else
                {
                    for (int j = 0; j < self.listEnemyUnitID.Count; j++)
                    {
                        unitM = unitCpt.Get(self.listEnemyUnitID[j]);
                        if (!unitM.IsAlive())
                        {
                            continue;
                        }

                        self.battleAnimAllTime += 1000;
                        NumMonster = unitM.GetComponent<NumericComponent>();
                        int damage = AdventureDamageHelper.CaclutaionDamage(NumMonster, numCpt, ref self.battleRandom);
                        int newHp = numCpt.GetAsInt(NumericType.Hp);
                        newHp -= damage;
                        if (newHp <= 0)
                        {
                            newHp = 0;
                            self.GetParent<Unit>().SetAliveNoEvent(false);
                            return false;
                        }

                        numCpt.SetNoEvent(NumericType.Hp, newHp);
                    }
                }
            }

            return true;
        }

        public static void ResetAdventureInfo(this AdventureCheckComponent self)
        {
            self.battleAnimAllTime = 0;
            self.listEnemyUnitID.Clear();

            NumericComponent numSlef = self.GetParent<Unit>().GetComponent<NumericComponent>();
            
            numSlef.SetNoEvent(NumericType.Hp, numSlef[NumericType.MaxHp]);
            self.GetParent<Unit>().SetAliveNoEvent(true);
        }

        /// <summary>
        /// 重置冒险参数
        /// </summary>
        /// <param name="self"></param>
        public static void ResetBattleRandom(this AdventureCheckComponent self)
        {
            var numCpt = self.GetParent<Unit>().GetComponent<NumericComponent>();
            uint seed = (uint)numCpt[NumericType.AdventureRandomSeed];
            if (self.battleRandom == null)
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
        public static void CreateEnemy(this AdventureCheckComponent self)
        {
            var numCpt = self.GetParent<Unit>().GetComponent<NumericComponent>();
            long levelId = numCpt[NumericType.AdventureState];

            BattleLevelConfig config = BattleLevelConfigCategory.Instance.Get((int)levelId);

            self.listEnemyUnitID.Clear();
            var unitCpt = self.DomainScene().GetComponent<UnitComponent>();
            for (int i = 0; i < config.MonsterIds.Length; i++)
            {
                Unit unitM;
                if (self.listCacheEnemyId.Count > i)
                {
                    unitM = unitCpt.Get(self.listCacheEnemyId[i]);
                    unitM.ConfigId = config.MonsterIds[i];
                    var numMonsterCpt = unitM.AddComponent<NumericComponent>();
                    numMonsterCpt.SetNoEvent(NumericType.IsAlive, 0);
                    numMonsterCpt.SetNoEvent(NumericType.Hp, unitM.Config.MaxHP);
                    numMonsterCpt.SetNoEvent(NumericType.MaxHp, unitM.Config.MaxHP);
                    numMonsterCpt.SetNoEvent(NumericType.DamageValue, unitM.Config.DamageValue);
                }
                else
                {
                    unitM = UnitFactory.CreateMonster(self.DomainScene(), config.MonsterIds[i]);
                    self.listCacheEnemyId.Add(unitM.Id);
                }

                self.listEnemyUnitID.Add(unitM.Id);
            }
        }
    }
}