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
        public SortedDictionary<long, GameTask> sdicItems = new SortedDictionary<long, GameTask>();
#if !SERVER
        /// <summary>
        /// 任务列表
        /// </summary>
        public List<GameTask> listTasks = new List<GameTask>();
#endif

#if SERVER
       [BsonIgnore]
#endif
        public M2C_UpdateSomeOneItem m2c_bagItem = new M2C_UpdateSomeOneItem() { NetItemPut = (int)NetItemPut.Bag };
    }
}