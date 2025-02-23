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

    
        /// <summary>
        /// 缓存怪物的Unit的id
        /// </summary>
        public List<long> listCacheEnemyId = new List<long>();
        /// <summary>
        /// 本局的怪物Unit的id
        /// </summary>
        public List<long> listEnemyUnitID = new List<long>();
        
        //战斗
        public SRandom battleRandom;

    }
}