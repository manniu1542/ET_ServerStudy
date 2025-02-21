using System.Collections.Generic;

namespace ET
{
    /// <summary>
    /// 验证玩家冒险胜利的情况
    /// </summary>
    [ComponentOf(typeof(Unit))]
    public class AdventureCheckComponent:Entity,IAwake,IDestroy
    {
        
        //记录 战斗的时间(一个unit打斗需要1秒钟时间，如果 战斗开始的时间 到 结算的时候 》 打斗的时间 ，这是正常的)
        public long battleAnimAllTime;

        //怪物总伤害 》 人物总血量。不正常
        public long totalMonsterHp;
        public long totalUnitDamge;
        public long totalMonsterDamge;
        
        //记录怪物的血量
        public Dictionary<int, int> dicEnemyHp = new Dictionary<int, int>();

    }
}