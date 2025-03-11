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
        public static void ResetFormGameTaskInfo(this GameTask self)
        {
        }

        public static bool IsTaskState(this GameTask self, GameTaskState state)
        {
            return self.state == (int)state;
        }
        public static void InitTaskProgressCount(this GameTask self)
        {
            ///升级任务的初始化。是当前的玩家等级，其余的都是次数是否完成
            if (self.Config.TaskActionType == (int)GameTaskTaskActionType.UpdateLevel)
            {
                var numCpt = self.GetParent<Unit>().GetComponent<NumericComponent>();
                self.progress = numCpt.GetAsInt(NumericType.Level);
                return;
            }
            
            self.progress = 0;
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