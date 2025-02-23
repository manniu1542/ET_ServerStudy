using System.Threading.Tasks;
using ILRuntime.Runtime;
using UnityEngine;
using UnityEngine.UI;

namespace ET
{
    [FriendClass(typeof (DlgRoleInfo))]
    public static class DlgRoleInfoSystem
    {
        public static void RegisterUIEvent(this DlgRoleInfo self)
        {
            self.View.ES_AttributeItem.RegisterUIEvent(NumericType.Power);
            self.View.ES_AttributeItem1.RegisterUIEvent(NumericType.PhysicalStrength);
            self.View.ES_AttributeItem2.RegisterUIEvent(NumericType.Agile);
            self.View.ES_AttributeItem3.RegisterUIEvent(NumericType.Spirit);
            EUIHelper.AddListenerAsync(self.View.E_UpLevelButton, self.OnUpLevelHandler);

            self.View.E_AttributesLoopVerticalScrollRect.AddItemRefreshListener((Transform transform, int index) =>
            {
                self.OnAttributeItemRefreshHandler(transform, index);
            });

            self.RegisterCloseEvent<DlgRoleInfo>(self.View.E_CloseButton);
           
            RedDotHelper.AddRedDotNodeView(self.ZoneScene(), RedDotType.Role_Level, self.View.E_UpLevelButton.gameObject, Vector3.one,
                new Vector3(75, 55, 0));
            RedDotHelper.AddRedDotNodeView(self.ZoneScene(), RedDotType.Role_AttributePoint, self.View.E_AttributePointText.gameObject, Vector3.one,
                new Vector3(75, 55, 0));
        }

        public static void UnloadWindow(this DlgRoleInfo self)
        {
            RedDotMonoView redView = self.View.E_UpLevelButton.GetComponent<RedDotMonoView>();
            RedDotHelper.RemoveRedDotView(self.ZoneScene(), RedDotType.Role_Level, out redView);
            redView = self.View.E_AttributePointText.GetComponent<RedDotMonoView>();
            RedDotHelper.RemoveRedDotView(self.ZoneScene(), RedDotType.Role_AttributePoint, out redView);
        }

        public static void ShowWindow(this DlgRoleInfo self, Entity contextData = null)
        {
            self.RefreshUI();
        }

        public static async ETTask OnUpLevelHandler(this DlgRoleInfo self)
        {
            bool isFinish = await NumericHelper.ReqUpLevel(self.ZoneScene());
        }

        public static void RefreshUI(this DlgRoleInfo self)
        {
            var unit = UnitHelper.GetMyUnitFromCurrentScene(self.ZoneScene().CurrentScene());
            var numCpt = unit?.GetComponent<NumericComponent>();
            if (numCpt == null) return;

            self.View.E_CombatEffectivenessText.text = "战力值：" + numCpt[NumericType.CombatEffectiveness].ToString();

            self.View.ES_AttributeItem.RefreshUI(NumericType.Power);
            self.View.ES_AttributeItem1.RefreshUI(NumericType.PhysicalStrength);
            self.View.ES_AttributeItem2.RefreshUI(NumericType.Agile);
            self.View.ES_AttributeItem3.RefreshUI(NumericType.Spirit);

            self.View.E_AttributePointText.text = numCpt[NumericType.AttributePoint].ToString();

            int count = PlayerNumericConfigCategory.Instance.listNeedShow.Count;
            self.AddUIScrollItems(ref self.ScrollItemAttributes, count);
            self.View.E_AttributesLoopVerticalScrollRect.SetVisible(true, count);
        }

        public static void OnAttributeItemRefreshHandler(this DlgRoleInfo self, Transform transform, int index)
        {
            Scroll_Item_attribute scrollItemAttribute = self.ScrollItemAttributes[index].BindTrans(transform);
            PlayerNumericConfig config = PlayerNumericConfigCategory.Instance.listNeedShow[index];
            scrollItemAttribute.E_attributeNameText.text = config.Name + ":";
            scrollItemAttribute.E_attributeValueText.text = config.isPrecent == 0?
                    UnitHelper.GetMyUnitNumericComponent(self.ZoneScene().CurrentScene()).GetAsLong(config.Id).ToString() :
                    $"{UnitHelper.GetMyUnitNumericComponent(self.ZoneScene().CurrentScene()).GetAsFloat(config.Id).ToString("0.00")}%";
        }
    }
}