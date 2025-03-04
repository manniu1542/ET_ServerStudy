using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

namespace ET
{
    [ObjectSystem]
    public class ES_EquipItemAwakeSystem: AwakeSystem<ES_EquipItem, Transform>
    {
        public override void Awake(ES_EquipItem self, Transform transform)
        {
            self.uiTransform = transform;
        }
    }

    [ObjectSystem]
    public class ES_EquipItemDestroySystem: DestroySystem<ES_EquipItem>
    {
        public override void Destroy(ES_EquipItem self)
        {
            self.DestroyWidget();
        }
    }

    public static class ES_EquipItemSystem
    {
        public static void RefreshUI(this ES_EquipItem self, Item item,SpriteAtlas sa)
        {
            if (item == null)
            {
                self.E_IconImage.overrideSprite = null;
                self.E_QualityImage.color = Color.black;
            }
            else
            {
                self.E_IconImage.overrideSprite = sa.GetSprite(item.Config.Icon);
                self.E_QualityImage.color = item.ItemQualityColor();
            }

          
            EUIHelper.AddListenerAsync(self.E_SelecteButton, async () =>
            {
                if(item==null)return;
                
                var uiCpt = self.ZoneScene().GetComponent<UIComponent>();
                await uiCpt.ShowWindowAsync(WindowID.WindowID_ItemPopUp);
                uiCpt.GetDlgLogic<DlgItemPopUp>().RefreshUI(item.Id, NetItemPut.Role);
            });
        }
    }
}