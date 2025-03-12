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

            //客户端先判断 有没有 位置了(当前 正在打造的 数量，当前等级可以打造的 数量)
            var gameTask = self.ZoneScene().GetComponent<TaskComponent>().GetGameTask(configID);
            if (gameTask == null)
            {
                Log.Error("没有该任务id：" + configID);
                return;
            }

            if (!gameTask.IsTaskState(GameTaskState.Finish))
            {
                Log.Error("任务没有完成！" + configID);
                return;
            }

     
            //做请求
            M2C_ReceiveGameTaskReward m2c_gameTaskReward;
            var gateSession = self.ZoneScene().GetComponent<SessionComponent>().Session;
            try
            {
                m2c_gameTaskReward = await gateSession.Call(new C2M_ReceiveGameTaskReward() { TaskConfigId = configID }) as M2C_ReceiveGameTaskReward;
                if (m2c_gameTaskReward.Error != ErrorCode.ERR_Success)
                {
                    Log.Error("请求 获取任务奖励 错误码是：" + m2c_gameTaskReward.Error);
                    return;
                }
                else
                {
                    self.RefreshUI();
                }
            }
            catch (Exception e)
            {
                Log.Error("请求出错：" + e.ToString());
            }

            await ETTask.CompletedTask;
        }
    }
}