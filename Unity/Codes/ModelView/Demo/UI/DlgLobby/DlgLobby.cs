using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ET
{
	[ComponentOf(typeof(UIBaseWindow))]
	public  class DlgLobby :Entity,IAwake,IDestroy,IUILogic
	{

		public DlgLobbyViewComponent View { get => this.Parent.GetComponent<DlgLobbyViewComponent>();}

	
		public List<GameObject> listGOServerList = new List<GameObject>();
		public Image imgCurChoose;
		public long serverId;
	}
}
