using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace ET
{
    [FriendClass(typeof(ServerInfoComponent))]
    [FriendClass(typeof(DlgLobby))]
    [FriendClass(typeof(ServerInfo))]

    public static class DlgLobbySystem
    {

        public static void RegisterUIEvent(this DlgLobby self)
        {
            self.View.E_EnterMapButton.AddListener(() =>
            {
                self.OnEnterMapClickHandler().Coroutine();
            });

        }

        public static async void ShowWindow(this DlgLobby self, Entity contextData = null)
        {
            for (int i = self.listGOServerList.Count - 1; i >= 0; i--)
            {
                GameObject.Destroy(self.listGOServerList[i]);
            }
            self.imgCurChoose = null;
            int err = await LoginHelper.GetServerInfo(self.ZoneScene());

            if (err == ErrorCode.ERR_Success)
            {
                var scrServerInfo = Game.Scene.GetComponent<ServerInfoComponent>();
                foreach (ServerInfo info in scrServerInfo.ServerInfoList)
                {

                    GameObject go = GameObject.Instantiate(self.View.EGoServerInfoTmpRectTransform.gameObject, self.View.EGoServerInfoTmpRectTransform.parent);
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
                            self.serverId = info.Id;
                            Log.Info($"当前所选大区ID{info.Id}");
                        });


                }


            }
        }

        public static async ETTask OnEnterMapClickHandler(this DlgLobby self)
        {




            await EnterMapHelper.EnterMapAsync(self.ZoneScene());
        }
    }
}
