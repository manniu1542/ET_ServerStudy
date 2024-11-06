using System.Collections.Generic;
using UnityEngine;

namespace ET
{
	 [ComponentOf(typeof(UIBaseWindow))]
	public  class DlgRoleInfo :Entity,IAwake,IUILogic
	{

		public DlgRoleInfoViewComponent View { get => this.Parent.GetComponent<DlgRoleInfoViewComponent>();}


        public List<GameObject> listGORoleInfo = new List<GameObject>();
		public string roleName;
    }
}
