using System.Collections.Generic;

namespace ET
{

    
    
    //做冒险
    [ComponentOf(typeof (Scene))]
    public class AdventureComponent: Entity, IAwake, IDestroy
    {
        /// <summary>
        ///  开始冒险的计时器
        /// </summary>
        public long TimerID;

        /// <summary>
        /// 战斗回合数
        /// </summary>
        public int roundCount = 0;

        /// <summary>
        /// 本关的敌人单位的unitID列表
        /// </summary>
        public List<long> listEnemyUnitID = new();
        
        /// <summary>
        /// 活着的敌人单位
        /// </summary>
        public List<long> listAliveEnemyUnitID = new();

        public AdventureBattleRoundState state;

    }
}