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
            self.AddOrUpdateTask(0, false);
            Log.Error("111111");
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
        {       Log.Error("222222");
            foreach (var tmp in self.Children)
            {
                var item = tmp.Value as GameTask;
                if (item != null)
                {
                    self.AddTaskContainer(item);
                }
            }
        }
    }

    [FriendClass(typeof (TaskComponent))]
    [FriendClassAttribute(typeof (ET.Item))]
    [FriendClassAttribute(typeof (ET.GameTask))]
    public static class TaskComponentSystem
    {
        public static void AddTaskContainer(this TaskComponent self, GameTask gtask)
        {
            self.sdicItems.TryAdd(gtask.Config.Id, gtask);
        }

        public static void AddOrUpdateTask(this TaskComponent self, int beforeId, bool isSendClient = true)
        {
            var listTask = TaskConfigCategory.Instance.GetAllConfigByBeforeId(beforeId);
            //这个类型的任务已经做完了
            if (listTask == null)
                return;
            GameTask gameTask;
            foreach (TaskConfig taskConfig in listTask)
            {
                gameTask = self.AddChild<GameTask, int>(taskConfig.Id);
                self.InitTaskProgressCount(gameTask);
                gameTask.state = (int)GameTaskState.OnGoing;
                self.AddTaskContainer(gameTask);
            }

            if (isSendClient)
            {
                MessageHelper.SendToClient(self.GetParent<Unit>(), self.m2c_bagItem);
            }
        }

        public static void InitTaskProgressCount(this TaskComponent self, GameTask gameTask)
        {
            ///升级任务的初始化。是当前的玩家等级，其余的都是次数是否完成
            if (gameTask.Config.TaskActionType == (int)GameTaskTaskActionType.UpdateLevel)
            {
                var numCpt = self.GetParent<Unit>().GetComponent<NumericComponent>();
                gameTask.progress = numCpt.GetAsInt(NumericType.Level);
                return;
            }

            gameTask.progress = 0;
        }
    }
}