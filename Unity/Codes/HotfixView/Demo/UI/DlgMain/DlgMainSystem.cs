using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace ET
{
    [FriendClass(typeof (DlgMain))]
    public static class DlgMainSystem
    {
        public static void RegisterUIEvent(this DlgMain self)
        {
            EUIHelper.AddListener(self.View.E_RoleButton, async () =>
            {
                var isFinish = await NumericHelper.GetNewNumericSpawn(self.ZoneScene());
                //Gate网关
                Log.Error("获取最新属性是否成功：" + isFinish);
            });
        }

        public static void ShowWindow(this DlgMain self, Entity contextData = null)
        {
            self.RefreshUI();
        }

        public static void RefreshUI(this DlgMain self)
        {
            //客户端发送 获取 玩家消息 请求, 获取Unit组件 的NumCpt把他身上的属性显示出来
            Unit unit = UnitHelper.GetMyUnitFromCurrentScene(self.ZoneScene().CurrentScene());
            NumericComponent numCpt = unit.GetComponent<NumericComponent>();

            self.View.E_ExpText.SetText(numCpt.GetAsInt(NumericType.Exp).ToString());
            self.View.E_GoldText.SetText(numCpt.GetAsInt(NumericType.Gold).ToString());
        }
    }
}