using System.Collections.Generic;

namespace ET
{


	 [ComponentOf(typeof(UIBaseWindow))]
	public  class DlgBag :Entity,IAwake,IUILogic
	{

		public DlgBagViewComponent View { get => this.Parent.GetComponent<DlgBagViewComponent>();}

		/// <summary>
		/// 当前滚动列表的页码
		/// </summary>
		public int curPageIdx;
		/// <summary>
		/// 当前滚动列表的页码
		/// </summary>
		public ItemType curType;
		
		
		public Dictionary<int, Scroll_Item_bagItem> ScrollItemBagItems;

		public int onePageItemCount = 30;
	}
}
