using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ET
{
	


	 [ComponentOf(typeof(UIBaseWindow))]
	public  class DlgRoles :Entity,IAwake,IUILogic
	{

		public DlgRolesViewComponent View { get => this.Parent.GetComponent<DlgRolesViewComponent>();} 

		 
		public string roleName;

		public Image imgClick;
		public List<GameObject> listGORoleInfo=new List<GameObject>();
	}
}
