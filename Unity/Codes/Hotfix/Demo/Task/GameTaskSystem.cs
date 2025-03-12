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
            return new GameTaskInfo()
            {
                TaskConfigID = self.configID,
                TaskProgress = self.progress,
                TaskState = self.state,
            };
        }
        
        public static bool IsTaskState(this GameTask self, GameTaskState state)
        {
            return self.state == (int)state;
        }
     
        public static void UpdateTaskProgressCount(this GameTask self, int count)
        {
            var taskActionConfig = TaskActionConfigCategory.Instance.Get(self.Config.TaskActionType);

            switch ((GameTaskTaskAdvancingType)taskActionConfig.TaskProgressType)
            {
                case GameTaskTaskAdvancingType.Add:
                    self.progress += count;
                    break;
                case GameTaskTaskAdvancingType.Reduce:
                    self.progress -= count;
                    break;
                case GameTaskTaskAdvancingType.Update:
                    self.progress = count;
                    break;
            }
        }
    }
}