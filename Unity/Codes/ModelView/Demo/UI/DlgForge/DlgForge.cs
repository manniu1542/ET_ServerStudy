using System.Collections.Generic;
using UnityEngine.U2D;
using UnityEngine.UIElements;

namespace ET
{
	 [ComponentOf(typeof(UIBaseWindow))]
	public  class DlgForge :Entity,IAwake,IUILogic
	{

		public DlgForgeViewComponent View { get => this.Parent.GetComponent<DlgForgeViewComponent>();}


		public List<ES_MakeQueue> listMakeQueue =new();

		public SpriteAtlas saIcon;
		public Dictionary<int, Scroll_Item_production> ScrollItems = new();

		public long SpawnForgeRemainTimeID;
	}
}
