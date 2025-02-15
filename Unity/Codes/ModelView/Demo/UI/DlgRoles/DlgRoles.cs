namespace ET
{
	
	public enum UIRoleInfoType
	{
		Create,
		Enter,
	}

	 [ComponentOf(typeof(UIBaseWindow))]
	public  class DlgRoles :Entity,IAwake,IUILogic
	{

		public DlgRolesViewComponent View { get => this.Parent.GetComponent<DlgRolesViewComponent>();} 

		 
		public string roleName;
        
		/// <summary>
		/// UI的角色信息展示类型
		/// </summary>
		public UIRoleInfoType roleInfoType;

	}
}
