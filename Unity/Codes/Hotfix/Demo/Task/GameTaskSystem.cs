using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ET
{
    [ObjectSystem]
    public class GameTaskAwakeSystem: AwakeSystem<GameTask, int>
    {
        public override void Awake(GameTask self, int config)
        {
            self.configID = config;
            self.progress = 0;
            self.state = (int)GameTaskState.OnGoing;
        }
    }

    [ObjectSystem]
    public class GameTaskDestroySystem: DestroySystem<GameTask>
    {
        public override void Destroy(GameTask self)
        {
        }
    }

    [FriendClass(typeof (GameTask))]
    [FriendClassAttribute(typeof (ET.EquipInfoComponent))]
    public static class GameTaskSystem
    {
        public static void ResetFormGameTaskInfo(this GameTask self, GameTaskInfo gameTaskInfo)
        {
            self.configID = gameTaskInfo.TaskConfigID;
            self.progress = gameTaskInfo.TaskProgress;
            self.state = gameTaskInfo.TaskState;
        }

        public static GameTaskInfo ToMsgData(this GameTask self)
        {
            return new GameTaskInfo() { TaskConfigID = self.configID, TaskProgress = self.progress, TaskState = self.state, };
        }

        public static bool IsTaskState(this GameTask self, GameTaskState state)
        {
            return self.state == (int)state;
        }

        public static void TryFinishTask(this GameTask self)
        {
            if (self.state != (int)GameTaskState.OnGoing) return;

            bool isFinish = false;
            var taskActionConfig = TaskActionConfigCategory.Instance.Get(self.Config.TaskActionType);

            switch ((GameTaskAdvancingType)taskActionConfig.TaskProgressType)
            {
                case GameTaskAdvancingType.Add:
                    isFinish = self.progress >= self.Config.TaskTargetCount;
                    break;
                case GameTaskAdvancingType.Reduce:
                    isFinish = self.progress <= 0;
                    break;
                case GameTaskAdvancingType.Update:
                    isFinish = self.progress >= self.Config.TaskTargetCount;
                    break;
            }

            if (isFinish)
                self.state = (int)GameTaskState.Finish;
        }

        public static void UpdateTaskProgressCount(this GameTask self, int count)
        {
            var taskActionConfig = TaskActionConfigCategory.Instance.Get(self.Config.TaskActionType);

            switch ((GameTaskAdvancingType)taskActionConfig.TaskProgressType)
            {
                case GameTaskAdvancingType.Add:
                    self.progress += count;
                    self.progress = Math.Min(self.progress, self.Config.TaskTargetCount);
                    break;
                case GameTaskAdvancingType.Reduce:
                    self.progress -= count;
                    self.progress = Math.Max(self.progress, self.Config.TaskTargetCount);
                    break;
                case GameTaskAdvancingType.Update:
                    self.progress = count;
                    break;
            }
        }
    }
}