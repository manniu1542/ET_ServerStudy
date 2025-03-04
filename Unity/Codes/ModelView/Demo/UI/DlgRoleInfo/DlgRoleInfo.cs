using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

namespace ET
{

    [ComponentOf(typeof (UIBaseWindow))]
    public class DlgRoleInfo: Entity, IAwake, IUILogic
    {
        public DlgRoleInfoViewComponent View
        {
            get => this.Parent.GetComponent<DlgRoleInfoViewComponent>();
        }
        public Dictionary<int, Scroll_Item_attribute> ScrollItemAttributes;
        public SpriteAtlas saIcon;
        public Dictionary<RoleEuipPosType, ES_EquipItem> dicEquipUI =new();
    }
}