using UnityEngine;
using UnityEngine.UI;

namespace DuloGames.UI
{
    /// <summary>
    /// 使Input生成的光标框产生偏移
    /// </summary>
    [RequireComponent(typeof(InputField))]
    public class UIInputCaretOffset : MonoBehaviour
    {
        [SerializeField] private Vector2 m_Offset = Vector2.zero;
        private Vector2 m_InitialPosition = Vector2.zero;

        protected void OnTransformChildrenChanged()
        {
            Invoke("ApplyOffset", 0.5f);
        }

        public void ApplyOffset()
        {
            foreach (Transform trans in this.transform)
            {
                if (trans.gameObject.name.ToLower().Contains("caret"))
                {
                    RectTransform rect = (trans as RectTransform);

                    this.m_InitialPosition = rect.anchoredPosition;
                    rect.anchoredPosition = this.m_InitialPosition + this.m_Offset;
                    break;
                }
            }
        }
    }
}
