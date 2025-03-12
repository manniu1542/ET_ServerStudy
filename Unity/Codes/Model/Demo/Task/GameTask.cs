using MongoDB.Bson.Serialization.Attributes;

namespace ET
{
    /// <summary>
    /// 任务完成的进度
    /// </summary>
    public enum GameTaskState
    {
        None = 0,

        /// <summary>
        /// 正在进行
        /// </summary>
        OnGoing = 1,

        /// <summary>
        /// 已完成
        /// </summary>
        Finish =2,

        /// <summary>
        /// 已领取
        /// </summary>
        Get = 3,
    }

    /// <summary>
    /// 任务推进类型
    /// </summary>
    public enum GameTaskAdvancingType
    {
        Add = 1,
        Reduce = 2,
        Update = 3,
    }

    /// <summary>
    /// 任务 执行的类型
    /// </summary>
    public enum GameTaskActionType
    {
        UpdateLevel = 1,
        Forge = 2,
        Adventure = 3,
    }

    [ChildType(typeof (TaskComponent))]
#if SERVER
    public class GameTask: Entity, IAwake<int>, IDestroy, ISerializeToEntity
#else
    public class GameTask: Entity, IAwake, IAwake<int>, IDestroy
#endif
    {
        public int configID;

        /// <summary>
        /// 任务自身状态
        /// </summary>
        public int state;

        /// <summary>
        /// 任务进度
        /// </summary>
        public int progress;

        [BsonIgnore]
        public TaskConfig Config => TaskConfigCategory.Instance.Get(this.configID);
    }
}