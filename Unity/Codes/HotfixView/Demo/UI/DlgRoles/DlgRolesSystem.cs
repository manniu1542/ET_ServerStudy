using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace ET
{
    [FriendClass(typeof (AccountInfoComponent))]
    [FriendClass(typeof (RoleInfoComponent))]
    [FriendClass(typeof (RoleInfo))]
    [FriendClass(typeof (DlgRoles))]
    public static class DlgRolesSystem
    {
        public static void RegisterUIEvent(this DlgRoles self)
        {
            if (Define.IsEditor)
            {
                //不可用ILRunTime热更
                self.View.EInputFieldNameInputField.onValueChanged.RemoveAllListeners();
                self.View.EInputFieldNameInputField.onValueChanged.AddListener(str => { self.roleName = str; });
            }
            else
            {
                self.roleName = "tmp";
            }

            EUIHelper.AddListener(self.View.EBackButton, () =>
            {
                self.ZoneScene().GetComponent<UIComponent>().HideWindow(WindowID.WindowID_Roles);
                self.ZoneScene().GetComponent<UIComponent>().ShowWindow(WindowID.WindowID_ServerList);
            });

            EUIHelper.AddListenerAsync(self.View.EEnterGameButton, async () =>
            {
                if (self.ZoneScene().GetComponent<RoleInfoComponent>().IsChooseGameRoleId()) 
                    await self.EnterGame();
            });
            EUIHelper.AddListenerAsync(self.View.ECreateRoleButton, async () =>
            {
                await LoginHelper.CreateRoleInfo(self.ZoneScene(), self.roleName);
                self.UpdateUI();
            });
        }

        public static async ETTask EnterGame(this DlgRoles self)
        {
            int err = await LoginHelper.EnterGameRealmGameToLoginGate(self.ZoneScene());

            if (err != ErrorCode.ERR_Success) return;

            err = await LoginHelper.EnterGame(self.ZoneScene());
            if (err != ErrorCode.ERR_Success) return;
            //客户端也要加载客户端得Unit 获取它上面得NumericComponent 组件获取玩家 属性

            self.ZoneScene().GetComponent<UIComponent>().HideWindow(WindowID.WindowID_Roles);
            self.ZoneScene().GetComponent<UIComponent>().ShowWindow(WindowID.WindowID_Main);
            Log.Info("登录完成！！！");
        }

        public static async void ShowWindow(this DlgRoles self, Entity contextData = null)
        {
            //获取所有角色
            await LoginHelper.GetAllRoleInfo(self.ZoneScene());

            self.UpdateUI();
        }

        public static void UpdateUI(this DlgRoles self)
        {
            for (int i = self.listGORoleInfo.Count - 1; i >= 0; i--)
            {
                GameObject.Destroy(self.listGORoleInfo[i]);
            }

            self.listGORoleInfo.Clear();
            var dic = self.ZoneScene().GetComponent<RoleInfoComponent>().dicRoleInfo;
            foreach (var info in dic)
            {
                GameObject go = GameObject.Instantiate(self.View.EGORoleRectTransform.gameObject, self.View.EGORoleRectTransform.parent);
                self.listGORoleInfo.Add(go);
                go.SetActive(true);

                go.GetComponentInChildren<Text>().text = info.Value.Name;

                EUIHelper.AddListenerAsync(go.transform.Find("btn").GetComponent<Button>(), async () =>
                {
                    //删除角色
                    await LoginHelper.DeleteRoleInfo(self.ZoneScene(), info.Key);
                    self.UpdateUI();
                });

                EUIHelper.AddListener(go.transform.Find("btnClick").GetComponent<Button>(), () =>
                {
                    if (self.imgClick != null)
                        self.imgClick.color = Color.green;
                    self.imgClick = go.GetComponent<Image>();
                    self.imgClick.color = Color.red;
                    self.ZoneScene().GetComponent<RoleInfoComponent>().SetEnterGameRoleId(info.Key);
                });
            }

            self.View.EEnterGameButton.gameObject.SetActive(dic.Count > 0);
        }
    }
}