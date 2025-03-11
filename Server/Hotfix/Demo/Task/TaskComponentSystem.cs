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
                }
            }
        }
    }

    [FriendClass(typeof (TaskComponent))]
    [FriendClassAttribute(typeof (ET.Item))]
    [FriendClassAttribute(typeof (ET.GameTask))]
    public static class TaskComponentSystem
    {
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
                gameTask.InitTaskProgressCount();
                gameTask.state = (int)GameTaskState.OnGoing;
            }

            if (isSendClient)
            {
                MessageHelper.SendToClient(self.GetParent<Unit>(), self.m2c_bagItem);
            }
        }
    }
}