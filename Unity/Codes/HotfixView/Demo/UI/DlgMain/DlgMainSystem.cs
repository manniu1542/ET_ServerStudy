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
            EUIHelper.AddListener(self.View.E_RoleButton, () =>
            {
                self.ZoneScene().GetComponent<UIComponent>().ShowWindow(WindowID.WindowID_RoleInfo);
            });
            EUIHelper.AddListener(self.View.E_BagButton, () =>
            {
                self.ZoneScene().GetComponent<UIComponent>().ShowWindow(WindowID.WindowID_Bag);
            });
            EUIHelper.AddListener(self.View.E_MakeButton, () =>
            {
                self.ZoneScene().GetComponent<UIComponent>().ShowWindow(WindowID.WindowID_Forge);
            });
            EUIHelper.AddListener(self.View.E_TaskButton, () =>
            {
                self.ZoneScene().GetComponent<UIComponent>().ShowWindow(WindowID.WindowID_Task);
            });
            EUIHelper.AddListener(self.View.E_BattleButton,
                () => { self.ZoneScene().GetComponent<UIComponent>().ShowWindow(WindowID.WindowID_Adventure); });
            
            RedDotHelper.AddRedDotNodeView(self.ZoneScene(),RedDotType.Role, self.View.E_RoleButton.gameObject, Vector3.one, new Vector3(75,55,0));
            RedDotHelper.AddRedDotNodeView(self.ZoneScene(),RedDotType.Forge, self.View.E_MakeButton.gameObject, Vector3.one, new Vector3(75,55,0));
            RedDotHelper.AddRedDotNodeView(self.ZoneScene(),RedDotType.GameTask, self.View.E_TaskButton.gameObject, Vector3.one, new Vector3(75,55,0));
        }
        public static void UnloadWindow(this DlgMain self)
        {
            RedDotMonoView redView = self.View.E_RoleButton.GetComponent<RedDotMonoView>();
            RedDotHelper.RemoveRedDotView(self.ZoneScene(), RedDotType.Role,out redView);
            redView = self.View.E_MakeButton.GetComponent<RedDotMonoView>();
            RedDotHelper.RemoveRedDotView(self.ZoneScene(), RedDotType.Forge,out redView);
            redView = self.View.E_TaskButton.GetComponent<RedDotMonoView>();
            RedDotHelper.RemoveRedDotView(self.ZoneScene(), RedDotType.GameTask,out redView);
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
            self.View.E_RoleLevelText.SetText(numCpt.GetAsInt(NumericType.Level).ToString());
        }
        
        
        
    }
}