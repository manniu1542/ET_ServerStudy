using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace ET
{
    [FriendClass(typeof (DlgTask))]
    [FriendClassAttribute(typeof (ET.GameTask))]
    public static class DlgTaskSystem
    {
        public static void RegisterUIEvent(this DlgTask self)
        {
            self.RegisterCloseEvent<DlgTask>(self.View.E_CloseButton);

            self.View.E_TasksLoopVerticalScrollRect.AddItemRefreshListener(self.OnLoopItemRefreshHandler);
        }

        public static void ShowWindow(this DlgTask self, Entity contextData = null)
        {
            self.RefreshUI();
        }

        public static void RefreshUI(this DlgTask self)
        {
            int count = self.ZoneScene().GetComponent<TaskComponent>().GetRefreshCurTaskCount();
            self.AddUIScrollItems(ref self.ScrollItemTasks, count);
            self.View.E_TasksLoopVerticalScrollRect.SetVisible(true, count);
        }

        public static void OnLoopItemRefreshHandler(this DlgTask self, Transform transform, int index)
        {
            Scroll_Item_task scrollItemTask = self.ScrollItemTasks[index].BindTrans(transform);
            GameTask taskInfo = self.ZoneScene().GetComponent<TaskComponent>().GetTaskByIdx(index);

            scrollItemTask.E_TaskNameText.SetText(taskInfo.Config.TaskName);
            scrollItemTask.E_TaskDescText.SetText(taskInfo.Config.TaskDesc);
            scrollItemTask.E_TaskProgressText.SetText($"{taskInfo.progress} / {taskInfo.Config.TaskTargetCount}");
            scrollItemTask.E_TaskRewardCountText.SetText(taskInfo.Config.RewardGoldCount.ToString());
            scrollItemTask.E_ReceiveTipText.SetText(taskInfo.IsTaskState(GameTaskState.Finish)? "领取奖励" : "未完成");
            scrollItemTask.E_ReceiveButton.interactable = taskInfo.IsTaskState(GameTaskState.Finish);
            scrollItemTask.E_ReceiveButton.AddListenerAsyncWithId(self.OnReceiveRewardHandler, taskInfo.configID);
        }

        public static async ETTask OnReceiveRewardHandler(this DlgTask self, int configID)
        {
            //TODO:发送服务端 要领取 任务奖励。

            await ETTask.CompletedTask;
        }
    }
}