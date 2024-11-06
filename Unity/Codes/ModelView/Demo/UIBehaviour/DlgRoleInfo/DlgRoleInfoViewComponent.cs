
using UnityEngine;
using UnityEngine.UI;
namespace ET
{
	[ComponentOf(typeof(UIBaseWindow))]
	[EnableMethod]
	public  class DlgRoleInfoViewComponent : Entity,IAwake,IDestroy 
	{
		public UnityEngine.RectTransform EGBackGroundRectTransform
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_EGBackGroundRectTransform == null )
     			{
		    		this.m_EGBackGroundRectTransform = UIFindHelper.FindDeepChild<UnityEngine.RectTransform>(this.uiTransform.gameObject,"EGBackGround");
     			}
     			return this.m_EGBackGroundRectTransform;
     		}
     	}

		public UnityEngine.UI.Button ECreateRoleButton
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_ECreateRoleButton == null )
     			{
		    		this.m_ECreateRoleButton = UIFindHelper.FindDeepChild<UnityEngine.UI.Button>(this.uiTransform.gameObject,"EGBackGround/ECreateRole");
     			}
     			return this.m_ECreateRoleButton;
     		}
     	}

		public UnityEngine.UI.Image ECreateRoleImage
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_ECreateRoleImage == null )
     			{
		    		this.m_ECreateRoleImage = UIFindHelper.FindDeepChild<UnityEngine.UI.Image>(this.uiTransform.gameObject,"EGBackGround/ECreateRole");
     			}
     			return this.m_ECreateRoleImage;
     		}
     	}

		public UnityEngine.RectTransform EGoRoleTmpRectTransform
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_EGoRoleTmpRectTransform == null )
     			{
		    		this.m_EGoRoleTmpRectTransform = UIFindHelper.FindDeepChild<UnityEngine.RectTransform>(this.uiTransform.gameObject,"EGBackGround/layout/EGoRoleTmp");
     			}
     			return this.m_EGoRoleTmpRectTransform;
     		}
     	}

		public UnityEngine.UI.InputField EInputFieldNameInputField
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_EInputFieldNameInputField == null )
     			{
		    		this.m_EInputFieldNameInputField = UIFindHelper.FindDeepChild<UnityEngine.UI.InputField>(this.uiTransform.gameObject,"EGBackGround/EInputFieldName");
     			}
     			return this.m_EInputFieldNameInputField;
     		}
     	}

		public UnityEngine.UI.Image EInputFieldNameImage
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_EInputFieldNameImage == null )
     			{
		    		this.m_EInputFieldNameImage = UIFindHelper.FindDeepChild<UnityEngine.UI.Image>(this.uiTransform.gameObject,"EGBackGround/EInputFieldName");
     			}
     			return this.m_EInputFieldNameImage;
     		}
     	}

		public UnityEngine.UI.Button EBackButton
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_EBackButton == null )
     			{
		    		this.m_EBackButton = UIFindHelper.FindDeepChild<UnityEngine.UI.Button>(this.uiTransform.gameObject,"EGBackGround/EBack");
     			}
     			return this.m_EBackButton;
     		}
     	}

		public UnityEngine.UI.Image EBackImage
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_EBackImage == null )
     			{
		    		this.m_EBackImage = UIFindHelper.FindDeepChild<UnityEngine.UI.Image>(this.uiTransform.gameObject,"EGBackGround/EBack");
     			}
     			return this.m_EBackImage;
     		}
     	}

		public void DestroyWidget()
		{
			this.m_EGBackGroundRectTransform = null;
			this.m_ECreateRoleButton = null;
			this.m_ECreateRoleImage = null;
			this.m_EGoRoleTmpRectTransform = null;
			this.m_EInputFieldNameInputField = null;
			this.m_EInputFieldNameImage = null;
			this.m_EBackButton = null;
			this.m_EBackImage = null;
			this.uiTransform = null;
		}

		private UnityEngine.RectTransform m_EGBackGroundRectTransform = null;
		private UnityEngine.UI.Button m_ECreateRoleButton = null;
		private UnityEngine.UI.Image m_ECreateRoleImage = null;
		private UnityEngine.RectTransform m_EGoRoleTmpRectTransform = null;
		private UnityEngine.UI.InputField m_EInputFieldNameInputField = null;
		private UnityEngine.UI.Image m_EInputFieldNameImage = null;
		private UnityEngine.UI.Button m_EBackButton = null;
		private UnityEngine.UI.Image m_EBackImage = null;
		public Transform uiTransform = null;
	}
}
