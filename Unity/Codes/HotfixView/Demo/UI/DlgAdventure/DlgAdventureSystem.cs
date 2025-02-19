using System.Collections;
using System.Collections.Generic;
using System;
using DG.Tweening;
using ET.Adventure;
using UnityEngine;
using UnityEngine.UI;

namespace ET
{
    [FriendClass(typeof (DlgAdventure))]
    public static class DlgAdventureSystem
    {
        public static void RegisterUIEvent(this DlgAdventure self)
        {
            self.View.E_BattleLevelLoopVerticalScrollRect.AddItemRefreshListener((Transform transform, int index) =>
            {
                self.OnBattleLevelItemRefreshHandler(transform, index);
            });

            self.RegisterCloseEvent<DlgAdventure>(self.View.E_CloseButton);
        }

        public static void ShowWindow(this DlgAdventure self, Entity contextData = null)
        {
            self.View.EG_ContentRectTransform.DOScale(new Vector3(0.2f, 0.2f, 0.2f), 0.0f);
            self.View.EG_ContentRectTransform.DOScale(Vector3.one, 0.3f).onComplete += () => { self.RefreshUI(); };
        }

        public static void HideWindow(this DlgAdventure self)
        {
            self.View.E_BattleLevelLoopVerticalScrollRect.SetVisible(false);
            self.RemoveUIScrollItems(ref self.ScrollItemAttributes);
        }

        public static void RefreshUI(this DlgAdventure self)
        {
            int count = BattleLevelConfigCategory.Instance.GetAll().Count;
            self.AddUIScrollItems(ref self.ScrollItemAttributes, count);
            self.View.E_BattleLevelLoopVerticalScrollRect.SetVisible(true, count);
        }

        public static void OnBattleLevelItemRefreshHandler(this DlgAdventure self, Transform transform, int index)
        {
            Scroll_Item_battleLevel scrollItemAttribute = self.ScrollItemAttributes[index].BindTrans(transform);
            BattleLevelConfig config = BattleLevelConfigCategory.Instance.GetConfigByIndex(index);

            var numCpt = UnitHelper.GetMyUnitNumericComponent(self.ZoneScene().CurrentScene());

            int level = numCpt.GetAsInt(NumericType.Level);
            int adventureState = numCpt.GetAsInt(NumericType.AdventureState);

            bool isOpenAdventureLevel = level >= config.MiniEnterLevel[0] && level < config.MiniEnterLevel[1];
            bool isNoOpenAdventure = adventureState == 0;
            //当前玩家的管卡等级， 当前玩家的状态是，按钮响应：
            scrollItemAttribute.E_LevelNameText.SetText($"{config.Name} Lv.{config.MiniEnterLevel[0]}~Lv.{config.MiniEnterLevel[1]}");

            scrollItemAttribute.E_LevelNotEnoughText.SetVisible(!isOpenAdventureLevel);
            //是否可以前往。根据玩家的等级
            scrollItemAttribute.E_GoButton.SetVisible(isOpenAdventureLevel && isNoOpenAdventure);
            scrollItemAttribute.E_InAdventureTipText.SetVisible(isOpenAdventureLevel && (!isNoOpenAdventure));

            EUIHelper.AddListenerAsync(scrollItemAttribute.E_GoButton, async () =>
            {
                bool isFinish = await DlgAdventureHelper.OnStartGameLevelClickHandler(self.ZoneScene(), config.Id);

                if (isFinish)
                {
                    self.ZoneScene().GetComponent<UIComponent>().HideWindow<DlgAdventure>();
               

                    self.ZoneScene().CurrentScene().GetComponent<AdventureComponent>().StartAdventure().Coroutine();
                }
            });
        }
    }
}