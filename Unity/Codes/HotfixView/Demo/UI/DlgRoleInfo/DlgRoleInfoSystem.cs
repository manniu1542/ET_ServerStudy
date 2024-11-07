using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace ET
{
    [FriendClass(typeof(AccountInfoComponent))]
    [FriendClass(typeof(RoleInfoComponent))]
    [FriendClass(typeof(RoleInfo))]
    [FriendClass(typeof(DlgRoleInfo))]
    public static class DlgRoleInfoSystem
    {

        public static void RegisterUIEvent(this DlgRoleInfo self)
        {
            self.View.EInputFieldNameInputField.onValueChanged.RemoveAllListeners();
            self.View.EInputFieldNameInputField.onValueChanged.AddListener(str =>
            {
                self.roleName = str;
            });
            EUIHelper.AddListener(self.View.EBackButton, () =>
            {

                self.ZoneScene().GetComponent<UIComponent>().HideWindow(WindowID.WindowID_RoleInfo);
                self.ZoneScene().GetComponent<UIComponent>().ShowWindow(WindowID.WindowID_ServerList);

            });

            EUIHelper.AddListenerAsync(self.View.ECreateRoleButton, async () =>
            {
                //生成角色
                long id = self.ZoneScene().GetComponent<AccountInfoComponent>().AccountId;
                if (self.ZoneScene().GetComponent<RoleInfoComponent>().Exit(id))
                {
                    Log.Warning("已有角色不可创建角色了！");
                    return;
                }


                await LoginHelper.CreateRoleInfo(self.ZoneScene(), self.roleName);
                self.UpdateRole();

            });


        }
        public static void UpdateRole(this DlgRoleInfo self)
        {

            long userId = self.ZoneScene().GetComponent<AccountInfoComponent>().AccountId;
            for (int i = self.listGORoleInfo.Count - 1; i >= 0; i--)
            {
                GameObject.Destroy(self.listGORoleInfo[i]);
            }
            self.listGORoleInfo.Clear();
            foreach (var info in self.ZoneScene().GetComponent<RoleInfoComponent>().dicRoleInfo)
            {

                GameObject go = GameObject.Instantiate(self.View.EGoRoleTmpRectTransform.gameObject, self.View.EGoRoleTmpRectTransform.parent);
                self.listGORoleInfo.Add(go);
                go.SetActive(true);

                go.GetComponentInChildren<Text>().text = info.Value.Name;
                go.GetComponentInChildren<Button>().gameObject.SetActive(userId == info.Key);
                EUIHelper.AddListenerAsync(go.GetComponentInChildren<Button>(), async () =>
                {
                    //删除角色
                    await LoginHelper.DeleteRoleInfo(self.ZoneScene());
                    self.UpdateRole();
                });



            }

        }

        public static async void ShowWindow(this DlgRoleInfo self, Entity contextData = null)
        {



            //获取所有角色
            await LoginHelper.GetAllRoleInfo(self.ZoneScene());
            self.UpdateRole();



        }



    }
}
