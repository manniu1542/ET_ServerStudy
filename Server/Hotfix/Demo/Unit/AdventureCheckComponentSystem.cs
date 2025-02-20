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
        }
    }

    [FriendClassAttribute(typeof (ET.AdventureCheckComponent))]
    public static class AdventureCheckComponentSystem
    {
        public static bool CheckWinBattle(this AdventureCheckComponent self, int roundCount, int levelId)
        {
            self.ResetAdventureInfo();
            //验证对局是否能够完成

            if (!self.SimulationAdventure(roundCount, levelId))
            {
                Log.Error("战斗的次数有问题");
                return false;
            }

            //检查玩家总血量 与 敌人伤害
            var numCpt = self.Parent.GetComponent<NumericComponent>();
            
            if (numCpt[NumericType.Hp] <= self.totalMonsterDamge)
            {
                Log.Error("玩家无法存活！");
                return false;
            }
            //检查怪物的总伤害 与怪物的总血量

            if (self.totalMonsterHp > self.totalUnitDamge)
            {
                Log.Error("怪物根本没有死完！");
                return false;
            }

            //动画时间。和战斗 时间
            long intervalTime = TimeHelper.ServerNow() - numCpt[NumericType.AdventureStartTime];
            if (intervalTime < self.battleAnimAllTime)
            {
                Log.Error("战斗时长，有问题！");
                return false;
            }

            return true;
        }

        public static bool SimulationAdventure(this AdventureCheckComponent self, int roundCount, int levelId)
        {
            //初始化 ，怪物的血量，
            var config = BattleLevelConfigCategory.Instance.Get(levelId);
            for (int i = 0; i < config.MonsterIds.Length; i++)
            {
                var mo = UnitConfigCategory.Instance.Get(config.MonsterIds[i]);
                self.dicEnemyHp.Add(i, mo.MaxHP);
                self.totalMonsterHp += mo.MaxHP;
            }

            var numCpt = self.Parent.GetComponent<NumericComponent>();
            int playerDamge = (int)numCpt[NumericType.DamageValue];
            //roundCount  客户端 最后一回合 其实没有战斗的 ，
            for (int i = 0; i < roundCount; i++)
            {
                //玩家出手
                if (i % 2 == 0)
                {
                    self.battleAnimAllTime += 1000;
                    for (int j = 0; j < self.dicEnemyHp.Count; j++)
                    {
                        if (self.dicEnemyHp[j] <= 0)
                        {
                            if (j == self.dicEnemyHp.Count - 1)
                            {
                                return false;
                            }
                            continue;
                        }

                        self.dicEnemyHp[j] -= playerDamge;
                        self.totalUnitDamge += playerDamge;
                        break;
                    }
                }
                else
                {
                    for (int j = 0; j < self.dicEnemyHp.Count; j++)
                    {
                        if (self.dicEnemyHp[j] <= 0)
                        {
                            if (j == self.dicEnemyHp.Count - 1)
                            {
                                return false;
                            }

                            continue;
                        }

                        self.battleAnimAllTime += 1000;
                        var mo = UnitConfigCategory.Instance.Get(config.MonsterIds[j]);
                        self.totalMonsterDamge += mo.DamageValue;
                    }
                }
            }

            return true;
        }

        public static void ResetAdventureInfo(this AdventureCheckComponent self)
        {
            self.battleAnimAllTime = 0;
            self.totalMonsterHp = 0;
            self.totalUnitDamge = 0;
            self.totalMonsterDamge = 0;

            //记录怪物的血量
            self.dicEnemyHp.Clear();
        }
    }
}