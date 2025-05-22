using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleTween
{
    [ExecuteAlways]
    [RequireComponent(typeof(RectTransform))]
    public class TweenAnchoredPosition : TweenBase
    {
        public Vector2 from = Vector3.zero;
        public Vector2 to = Vector3.zero;

        private RectTransform m_RectTransform;
        public RectTransform rectTransform
        {
            get
            {
                if (Application.isPlaying == false)
                    return GetComponent<RectTransform>();

                if (m_RectTransform == null)
                    m_RectTransform = GetComponent<RectTransform>();
                return m_RectTransform;
            }
        }

        protected override void SetValue(float t)
        {
            rectTransform.anchoredPosition = Vector3.Lerp(from, to, t);
        }

        protected override void Reset()
        {
            base.Reset();
            from = to = rectTransform.anchoredPosition;
        }
    }

}
