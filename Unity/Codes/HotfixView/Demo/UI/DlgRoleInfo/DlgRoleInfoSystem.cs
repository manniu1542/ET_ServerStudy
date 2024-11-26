using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace ET
{
    [FriendClass(typeof (AccountInfoComponent))]
    [FriendClass(typeof (RoleInfoComponent))]
    [FriendClass(typeof (RoleInfo))]
    [FriendClass(typeof (DlgRoleInfo))]
    public static class DlgRoleInfoSystem
    {
        public static void RegisterUIEvent(this DlgRoleInfo self)
        {
            //不可用ILRunTime热更
            // self.View.EInputFieldNameInputField.onValueChanged.RemoveAllListeners();
            // self.View.EInputFieldNameInputField.onValueChanged.AddListener(str => { self.roleName = str; });
            self.roleName = "tmp";
            EUIHelper.AddListener(self.View.EBackButton, () =>
            {
                self.ZoneScene().GetComponent<UIComponent>().HideWindow(WindowID.WindowID_RoleInfo);
                self.ZoneScene().GetComponent<UIComponent>().ShowWindow(WindowID.WindowID_ServerList);
            });

            EUIHelper.AddListenerAsync(self.View.EEnterGameButton, async () => { await self.EnterGame(); });
            EUIHelper.AddListenerAsync(self.View.ECreateRoleButton, async () =>
            {
                if (self.roleInfoType == UIRoleInfoType.Enter)
                {
                    Log.Warning("已有角色不可创建角色了！");
                    return;
                }

                await LoginHelper.CreateRoleInfo(self.ZoneScene(), self.roleName);
                self.UpdateUI();
            });
        }

        public static async ETTask EnterGame(this DlgRoleInfo self)
        {
            int err = await LoginHelper.EnterGameRealmGameToLoginGate(self.ZoneScene());

            if (err != ErrorCode.ERR_Success) return;
            self.ZoneScene().GetComponent<UIComponent>().HideWindow(WindowID.WindowID_RoleInfo);
            self.ZoneScene().GetComponent<UIComponent>().ShowWindow(WindowID.WindowID_Lobby);
            Log.Info("登录完成！！！");
        }

        public static void UpdateRole(this DlgRoleInfo self)
        {
            if (self.roleInfoType == UIRoleInfoType.Enter)
            {
                var info = self.ZoneScene().GetComponent<RoleInfoComponent>().Get();
                self.View.EGORoleRectTransform.GetComponentInChildren<Text>().text = info.Name;
                Log.Info("---" + info.Name);
                EUIHelper.AddListenerAsync(self.View.EGORoleRectTransform.GetComponentInChildren<Button>(), async () =>
                {
                    //删除角色
                    await LoginHelper.DeleteRoleInfo(self.ZoneScene());
                    self.UpdateUI();
                });
            }
        }

        public static async void ShowWindow(this DlgRoleInfo self, Entity contextData = null)
        {
            //获取所有角色
            await LoginHelper.GetRoleInfo(self.ZoneScene());
        
            self.UpdateUI();
        }

        public static async void UpdateUI(this DlgRoleInfo self)
        {
            self.roleInfoType = self.ZoneScene().GetComponent<RoleInfoComponent>().Exit()? UIRoleInfoType.Enter : UIRoleInfoType.Create;

            self.View.EGORoleRectTransform.gameObject.SetActive(self.roleInfoType == UIRoleInfoType.Enter);
            self.View.EEnterGameButton.gameObject.SetActive(self.roleInfoType == UIRoleInfoType.Enter);

            self.View.EInputFieldNameInputField.gameObject.SetActive(self.roleInfoType == UIRoleInfoType.Create);
            self.View.ECreateRoleButton.gameObject.SetActive(self.roleInfoType == UIRoleInfoType.Create);
            self.UpdateRole();
        }
    }
}