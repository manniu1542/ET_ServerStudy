using System.Collections.Generic;
using System.Runtime.Serialization;
using MongoDB.Bson.Serialization.Attributes;

namespace ET
{
    [ChildType(typeof (GameTask))]
#if SERVER
       [ComponentOf(typeof (Unit))]
       public class TaskComponent: Entity, IAwake, IDestroy,IDeserialize, ITransfer, IUnitChache
#else
    [ComponentOf(typeof (Scene))]
    public class TaskComponent: Entity, IAwake, IDestroy
#endif
    {
#if SERVER
   [BsonIgnore]
#endif
        /// <summary>
        /// 所有任务的字典 
        /// </summary>
        public SortedDictionary<int, GameTask> sdicItems = new SortedDictionary<int, GameTask>();
#if !SERVER
        /// <summary>
        /// 任务列表
        /// </summary>
        public List<GameTask> listTasks = new List<GameTask>();
#endif

#if SERVER
       [BsonIgnore]
        public M2C_UpdateGameTaskProgress m2c_bagItem = new M2C_UpdateGameTaskProgress() ;
#endif
    }
}