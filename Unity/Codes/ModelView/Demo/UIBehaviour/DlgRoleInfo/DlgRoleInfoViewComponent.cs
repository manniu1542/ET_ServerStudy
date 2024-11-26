
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

		public UnityEngine.RectTransform EGORoleRectTransform
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_EGORoleRectTransform == null )
     			{
		    		this.m_EGORoleRectTransform = UIFindHelper.FindDeepChild<UnityEngine.RectTransform>(this.uiTransform.gameObject,"EGBackGround/layout/EGORole");
     			}
     			return this.m_EGORoleRectTransform;
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

		public UnityEngine.UI.Button EEnterGameButton
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_EEnterGameButton == null )
     			{
		    		this.m_EEnterGameButton = UIFindHelper.FindDeepChild<UnityEngine.UI.Button>(this.uiTransform.gameObject,"EGBackGround/EEnterGame");
     			}
     			return this.m_EEnterGameButton;
     		}
     	}

		public UnityEngine.UI.Image EEnterGameImage
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_EEnterGameImage == null )
     			{
		    		this.m_EEnterGameImage = UIFindHelper.FindDeepChild<UnityEngine.UI.Image>(this.uiTransform.gameObject,"EGBackGround/EEnterGame");
     			}
     			return this.m_EEnterGameImage;
     		}
     	}

		public void DestroyWidget()
		{
			this.m_EGBackGroundRectTransform = null;
			this.m_ECreateRoleButton = null;
			this.m_ECreateRoleImage = null;
			this.m_EGORoleRectTransform = null;
			this.m_EInputFieldNameInputField = null;
			this.m_EInputFieldNameImage = null;
			this.m_EBackButton = null;
			this.m_EBackImage = null;
			this.m_EEnterGameButton = null;
			this.m_EEnterGameImage = null;
			this.uiTransform = null;
		}

		private UnityEngine.RectTransform m_EGBackGroundRectTransform = null;
		private UnityEngine.UI.Button m_ECreateRoleButton = null;
		private UnityEngine.UI.Image m_ECreateRoleImage = null;
		private UnityEngine.RectTransform m_EGORoleRectTransform = null;
		private UnityEngine.UI.InputField m_EInputFieldNameInputField = null;
		private UnityEngine.UI.Image m_EInputFieldNameImage = null;
		private UnityEngine.UI.Button m_EBackButton = null;
		private UnityEngine.UI.Image m_EBackImage = null;
		private UnityEngine.UI.Button m_EEnterGameButton = null;
		private UnityEngine.UI.Image m_EEnterGameImage = null;
		public Transform uiTransform = null;
	}
}
