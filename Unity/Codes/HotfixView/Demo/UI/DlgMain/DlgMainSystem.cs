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
        }

        public static void ShowWindow(this DlgMain self, Entity contextData = null)
        {
            //客户端发送 获取 玩家消息 请求, 获取Unit组件 的NumCpt把他身上的属性显示出来
            Unit unit = UnitHelper.GetMyUnitFromCurrentScene(self.ZoneScene().CurrentScene());
            NumericComponent numCpt = unit.GetComponent<NumericComponent>();

            Log.Error("加载成功：--" + numCpt.InstanceId);
            var hp = numCpt.GetAsInt(NumericType.Hp);
        }
    }
}