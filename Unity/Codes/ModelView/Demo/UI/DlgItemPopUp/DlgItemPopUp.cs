using UnityEngine.U2D;

namespace ET
{
	 [ComponentOf(typeof(UIBaseWindow))]
	public  class DlgItemPopUp :Entity,IAwake,IUILogic
	{

		public DlgItemPopUpViewComponent View { get => this.Parent.GetComponent<DlgItemPopUpViewComponent>();}

		public Item item;
		public SpriteAtlas saIcon;
	}

}
