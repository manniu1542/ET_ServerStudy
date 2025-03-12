using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Schema;
using UnityEngine;

namespace ET
{
    [ObjectSystem]
    public class TaskComponentAwakeSystem: AwakeSystem<TaskComponent>
    {
        public override void Awake(TaskComponent self)
        {
        }
    }

    [ObjectSystem]
    public class TaskComponentDestroySystem: DestroySystem<TaskComponent>
    {
        public override void Destroy(TaskComponent self)
        {
            self.Clear();
        }
    }

    [FriendClass(typeof (TaskComponent))]
    [FriendClassAttribute(typeof (ET.GameTask))]
    public static class TaskComponentSystem
    {
        public static void Clear(this TaskComponent self)
        {
            foreach (GameTask selfListTask in self.listTasks)
            {
                selfListTask.Dispose();
            }

            self.listTasks.Clear();
            self.sdicItems.Clear();
        }

        public static void AddOrUpdateGameTask(this TaskComponent self, GameTaskInfo gameTaskInfo)
        {
            var gameTask = self.GetGameTask(gameTaskInfo.TaskConfigID);
            if (gameTask == null)
            {
                gameTask = self.AddChild<GameTask, int>(gameTaskInfo.TaskConfigID);
                self.sdicItems.Add(gameTaskInfo.TaskConfigID, gameTask);
                self.listTasks.Add(gameTask);
            }

            gameTask.ResetFormGameTaskInfo(gameTaskInfo);

            Game.EventSystem.Publish(new EventType.UpdateGameTaskInfo() { ZoneScene = self.ZoneScene() });
        }

        public static GameTask GetGameTask(this TaskComponent self, int taskConfigID)
        {
            self.sdicItems.TryGetValue(taskConfigID, out GameTask gameTask);
            return gameTask;
        }

        /// <summary>
        /// 获取刷新当前任务 数量
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static int GetRefreshCurTaskCount(this TaskComponent self)
        {
            //去除已经领取的
            self.listTasks = self.listTasks.Where(x => (x.state != (int)GameTaskState.Get)).ToList();

            self.listTasks.Sort((x, y) => y.state - x.state);

            return self.listTasks.Count;
        }

        /// <summary>
        /// 获取任务通过索引
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static GameTask GetTaskByIdx(this TaskComponent self, int idx)
        {
            if (idx >= 0 && self.listTasks.Count > idx)
                return self.listTasks[idx];

            return null;
        }

        /// <summary>
        /// 当前有没有领取奖励的任务
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static bool IsCurCanReceivedAnyTaskRewards(this TaskComponent self)
        {
            foreach (GameTask selfListTask in self.listTasks)
            {
                if (selfListTask.IsTaskState(GameTaskState.Finish))
                {
                    return true;
                }
            }

            return false;
        }
    }
}