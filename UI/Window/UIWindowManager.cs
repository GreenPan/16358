using UnityEngine;
using System.Collections.Generic;

namespace DuloGames.UI
{
	/// <summary>
	/// 窗口控制器,主要用于控制窗口响应escape按键
	/// </summary>
	public class UIWindowManager : MonoBehaviour {

        private static UIWindowManager m_Instance;

        /// <summary>
        /// Gets the current instance of the window manager.
        /// 获取当前WindowManager的实例
        /// </summary>
        public static UIWindowManager Instance
        {
            get { return m_Instance; }
        }
        
        [SerializeField] private string m_EscapeInputName = "Cancel";
        private bool m_EscapeUsed = false;

        /// <summary>
        /// Gets the escape input name.
        /// 获取当前escape按键的名字
        /// </summary>
        public string escapeInputName
        {
            get { return this.m_EscapeInputName; }
        }

        /// <summary>
        /// Gets a value indicating whether the escape input was used to hide a window in this frame.
        /// 标志位,用于标志当前是否执行过使用escape键隐藏窗口的指令
        /// </summary>
        public bool escapedUsed
        {
            get { return this.m_EscapeUsed; }
        }

        protected virtual void Awake()
        {
            if (m_Instance == null)
            {
                m_Instance = this;
            }
            else
            {
                Destroy(this.gameObject);
            }
        }

        protected virtual void OnDestroy()
        {
            if (m_Instance.Equals(this))
                m_Instance = null;
        }

        protected virtual void Update()
		{
            // Reset the escape used variable
			// 重置标志
            if (this.m_EscapeUsed)
                this.m_EscapeUsed = false;

			// Check for escape key press
			// 检测是否按下escape键
			if (Input.GetButtonDown(this.m_EscapeInputName))
			{
                // Check for currently opened modal and exit this method if one is found
				// 检测当前存在任何开启的消息框
                UIModalBox[] modalBoxes = FindObjectsOfType<UIModalBox>();

                if (modalBoxes.Length > 0)
                {
                    foreach (UIModalBox box in modalBoxes)
                    {
                        // If the box is active
	                    // 当消息框存在时不做任何处理返回函数
                        if (box.isActive && box.isActiveAndEnabled && box.gameObject.activeInHierarchy)
                            return;
                    }
                }

				// Get the windows list
				// 获取所有窗口组件
				List<UIWindow> windows = UIWindow.GetWindows();
				
				// Loop through the windows and hide if required
				// 遍历窗口,如果可以则隐藏
				foreach (UIWindow window in windows)
				{
					// Check if the window has escape key action
					// 检测窗口是否存在escape相应
					if (window.escapeKeyAction != UIWindow.EscapeKeyAction.None)
					{
						// Check if the window should be hidden on escape
						// 检测窗口是否应当在escape按下时隐藏
						if (window.IsOpen && (window.escapeKeyAction == UIWindow.EscapeKeyAction.Hide || window.escapeKeyAction == UIWindow.EscapeKeyAction.Toggle || (window.escapeKeyAction == UIWindow.EscapeKeyAction.HideIfFocused && window.IsFocused)))
						{
							// Hide the window
							// 隐藏窗口
							window.Hide();

                            // Mark the escape input as used
							// 将标志位设置为true
                            this.m_EscapeUsed = true;
                        }
					}
				}

                // Exit the method if the escape was used for hiding windows
				// 当当前执行过隐藏活动时跳出
                if (this.m_EscapeUsed)
                    return;
                
				// Loop through the windows again and show any if required
				// 当面板上原本没有任何窗口,并按下escape键时遍历所有窗口,判断是否执行show操作
				foreach (UIWindow window in windows)
				{
					// Check if the window has escape key action toggle and is not shown
					// 检测是否村存在Toggle标志但并没有显示的窗口组件
					if (!window.IsOpen && window.escapeKeyAction == UIWindow.EscapeKeyAction.Toggle)
					{
						// Show the window
						// 显示窗口
						window.Show();
					}
				}
			}
		}
	}
}
