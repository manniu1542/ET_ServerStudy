using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace ET
{

    [FriendClass(typeof(ServerInfo))]
    [FriendClass(typeof(ServerInfoComponent))]
    [FriendClass(typeof(DlgServerList))]
    public static class DlgServerListSystem
    {

        public static void RegisterUIEvent(this DlgServerList self)
        {
            EUIHelper.AddListener(self.View.E_EnterGameButton, () =>
            {
                self.ZoneScene().GetComponent<UIComponent>().HideWindow(WindowID.WindowID_ServerList);
                self.ZoneScene().GetComponent<UIComponent>().ShowWindow(WindowID.WindowID_RoleInfo);

            });
        }
        public static void OnHideWindow(this DlgServerList self)
        {

        }


        public static async void ShowWindow(this DlgServerList self, Entity contextData = null)
        {
            for (int i = self.listGOServerList.Count - 1; i >= 0; i--)
            {
                GameObject.Destroy(self.listGOServerList[i]);
            }
            self.listGOServerList.Clear();
            self.imgCurChoose = null;
            int err = await LoginHelper.GetServerInfo(self.ZoneScene());

            if (err == ErrorCode.ERR_Success)
            {
                var scrServerInfo = Game.Scene.GetComponent<ServerInfoComponent>();
                foreach (ServerInfo info in scrServerInfo.ServerInfoList)
                {

                    GameObject go = GameObject.Instantiate(self.View.EGoServerInfoTmpRectTransform.gameObject, self.View.EGoServerInfoTmpRectTransform.parent);
                    self.listGOServerList.Add(go);
                    go.SetActive(true);
                    go.GetComponent<Image>().color = info.State == ServerInfoState.Normal ? Color.green : Color.grey;
                    go.GetComponentInChildren<Text>().text = info.Name;
                    if (info.State == ServerInfoState.Normal)
                        go.GetComponent<Button>().onClick.AddListener(() =>
                        {
                            if (self.imgCurChoose != null)
                                self.imgCurChoose.color = Color.green;

                            self.imgCurChoose = go.GetComponent<Image>();
                            self.imgCurChoose.color = Color.red;
                            Game.Scene.GetComponent<ServerInfoComponent>().SetCurServerId(info.Id);
                            Log.Info($"当前所选大区ID{info.Id}");
                        });


                }


            }
        }


    }
}
