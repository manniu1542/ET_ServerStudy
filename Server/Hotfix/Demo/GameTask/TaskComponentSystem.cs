using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ET
{
    [ObjectSystem]
    public class TaskComponentAwakeSystem: AwakeSystem<TaskComponent>
    {
        public override void Awake(TaskComponent self)
        {
            self.AddOrUpdateNewTask(0, false);
        }
    }

    [ObjectSystem]
    public class TaskComponentDestroySystem: DestroySystem<TaskComponent>
    {
        public override void Destroy(TaskComponent self)
        {
        }
    }

    [ObjectSystem]
    public class TaskComponentDeserializeSystem: DeserializeSystem<TaskComponent>
    {
        public override void Deserialize(TaskComponent self)
        {
            foreach (var tmp in self.Children)
            {
                var item = tmp.Value as GameTask;
                if (item != null)
                {
                    self.sdicItems.Add(item.Config.Id, item);
                    if (!item.IsTaskState(GameTaskState.Get))
                    {
                        self.hsCurTasks.Add(item.Config.Id);
                    }
                }
            }
        }
    }

    [FriendClass(typeof (TaskComponent))]
    [FriendClassAttribute(typeof (ET.Item))]
    [FriendClassAttribute(typeof (ET.GameTask))]
    public static class TaskComponentSystem
    {
        /// <summary>
        /// 当前正在处理的游戏任务
        /// </summary>
        /// <param name="self"></param>
        /// <param name="taskConfigID"></param>
        /// <returns></returns>
        public static bool IsCurHandlerGameTask(this TaskComponent self, int taskConfigID)
        {
            return self.hsCurTasks.Contains(taskConfigID);
        }

        /// <summary>
        /// 获取游戏任务是否完成。待领取
        /// </summary>
        /// <param name="self"></param>
        /// <param name="taskConfigID"></param>
        /// <returns></returns>
        public static bool GetGameTaskIsFinish(this TaskComponent self, int taskConfigID)
        {
            self.sdicItems.TryGetValue(taskConfigID, out GameTask gameTask);
            if (gameTask != null)
                return gameTask.IsTaskState(GameTaskState.Finish);
            return false;
        }

        public static bool TryReceiveTaskReward(this TaskComponent self, int taskConfigID)
        {
            if (!self.IsCurHandlerGameTask(taskConfigID))
            {
                Log.Error($"不存在当前执行的任务id:" + taskConfigID);
                return false;
            }

            if (!self.GetGameTaskIsFinish(taskConfigID))
            {
                Log.Error($"当前任务不是待领取的状态:" + taskConfigID);

                return false;
            }

            self.sdicItems.TryGetValue(taskConfigID, out GameTask gameTask);
            int beforeTaskId = gameTask.Config.TaskBeforeId;
            //该任务的前置任务是否已经领取！
            if (beforeTaskId != 0 && !self.sdicItems[beforeTaskId].IsTaskState(GameTaskState.Get))
            {
                Log.Error($"该任务{taskConfigID}的前置任务{beforeTaskId}没有领取:");
                return false;
            }

            return true;
        }

        /// <summary>
        /// 领取任务奖励时的处理
        /// </summary>
        /// <param name="self"></param>
        /// <param name="curFinishTaskId"></param>
        /// <param name="isSendClient"></param>
        public static void ReceiveGameTaskHandler(this TaskComponent self, int curFinishTaskId, bool isSendClient = true)
        {
            //修改当前任务状态
            self.sdicItems.TryGetValue(curFinishTaskId, out GameTask gameTask);
            gameTask.state = (int)GameTaskState.Get;

            if (isSendClient)
            {
                TaskComponentHelper.AsyncGameTask(self.GetParent<Unit>(), gameTask);
            }
            //开启关联任务的下个任务

            self.AddOrUpdateNewTask(curFinishTaskId, isSendClient);
        }

        /// <summary>
        /// 更新新任务。
        /// </summary>
        /// <param name="self"></param>
        /// <param name="beforeId"></param>
        /// <param name="isSendClient"></param>
        public static void AddOrUpdateNewTask(this TaskComponent self, int curGetTaskId, bool isSendClient = true)
        {
            self.hsCurTasks.Remove(curGetTaskId);
            var listTask = TaskConfigCategory.Instance.GetAllConfigByBeforeId(curGetTaskId);
            //这个类型的任务已经做完了
            if (listTask == null)
                return;

            foreach (TaskConfig taskConfig in listTask)
            {
                self.hsCurTasks.Add(taskConfig.Id);
                int initCount = self.GetInitTaskProgressCount(taskConfig);
                self.AddOrUpdateCurrentTask(taskConfig.Id, initCount, isSendClient);
            }
        }

        /// <summary>
        /// 更新当前的任务
        /// </summary>
        /// <param name="self"></param>
        /// <param name="beforeId"></param>
        /// <param name="cu"></param>
        /// <param name="isSendClient"></param>
        public static void AddOrUpdateCurrentTask(this TaskComponent self, int taskConfigId, int gtCount, bool isSendClient = true)
        {
            var config = TaskConfigCategory.Instance.Get(taskConfigId);
            //这个类型的任务已经做完了
            if (config == null)
            {
                Log.Error("没有找到该任务在配置表里：" + taskConfigId);
                return;
            }

            self.sdicItems.TryGetValue(taskConfigId, out GameTask gameTask);
            if (gameTask == null)
            {
                gameTask = self.AddChild<GameTask, int>(taskConfigId);
                self.sdicItems.Add(taskConfigId, gameTask);
            }

            gameTask.UpdateTaskProgressCount(gtCount);
            gameTask.TryFinishTask();

            if (isSendClient)
            {
                TaskComponentHelper.AsyncGameTask(self.GetParent<Unit>(), gameTask);
            }
        }

        public static int GetInitTaskProgressCount(this TaskComponent self, TaskConfig taskConfig)
        {
            ///升级任务的初始化。是当前的玩家等级，其余的都是次数是否完成
            if (taskConfig.TaskActionType == (int)GameTaskActionType.UpdateLevel)
            {
                var numCpt = self.GetParent<Unit>().GetComponent<NumericComponent>();

                return numCpt.GetAsInt(NumericType.Level);
            }

            return 0;
        }

        /// <summary>
        /// 触发更新符合条件的当前游戏任务
        /// </summary>
        /// <param name="self"></param>
        /// <param name="taskConfig"></param>
        /// <returns></returns>
        public static int TrrigerUpdateEligibleGameTask(this TaskComponent self, GameTaskActionType taskActionType, int targetParm = 0,
        int targetProgress = 1)
        {
            ///升级任务的初始化。是当前的玩家等级，其余的都是次数是否完成
            foreach (int selfHsCurTask in self.hsCurTasks)
            {
                self.sdicItems.TryGetValue(selfHsCurTask, out GameTask gameTask);
                if (gameTask.IsTaskState(GameTaskState.OnGoing) && gameTask.Config.TaskActionType == (int)taskActionType &&
                    gameTask.Config.TaskTargetParm == targetParm)
                {
                    self.AddOrUpdateCurrentTask(gameTask.Config.Id, targetProgress);
                }
            }

            return 0;
        }
    }
}