using System.Collections.Generic;
using UnityEngine;

namespace ET
{
    public enum UIRoleInfoType
    {
        Create,
        Enter,
    }

    [ComponentOf(typeof (UIBaseWindow))]
    public class DlgRoleInfo: Entity, IAwake, IUILogic
    {
        public DlgRoleInfoViewComponent View
        {
            get => this.Parent.GetComponent<DlgRoleInfoViewComponent>();
        }

        public string roleName;
        
        /// <summary>
        /// UI的角色信息展示类型
        /// </summary>
        public UIRoleInfoType roleInfoType;
    }
}