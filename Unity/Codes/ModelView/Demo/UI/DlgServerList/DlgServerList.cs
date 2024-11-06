using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ET
{
	 [ComponentOf(typeof(UIBaseWindow))]
	public  class DlgServerList :Entity,IAwake,IUILogic
	{

		public DlgServerListViewComponent View { get => this.Parent.GetComponent<DlgServerListViewComponent>();}



        public List<GameObject> listGOServerList = new List<GameObject>();
        public Image imgCurChoose;
    }
}
