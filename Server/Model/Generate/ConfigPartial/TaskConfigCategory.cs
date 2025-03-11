using System.Collections.Generic;

namespace ET
{
    public partial class TaskConfigCategory
    {
        /// <summary>
        /// 根据任务类型分类 好的任务。
        /// </summary>
        public MultiMap<int, TaskConfig> mmTasks = new MultiMap<int, TaskConfig>();

        public override void AfterEndInit()
        {
            mmTasks.Clear();
            foreach (var item in this.list)
            {
                mmTasks.Add(item.TaskBeforeId, item);
            }
        }

        /// <summary>
        /// 获取相同前置任务id的 所有任务 
        /// </summary>
        /// <returns></returns>
        public List<TaskConfig> GetAllConfigByBeforeId(int beforeId)
        {
            this.mmTasks.TryGetValue(beforeId, out var list);
            return list;
        }
    }
}