using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleTween
{
    [ExecuteAlways]
    [RequireComponent(typeof(CanvasGroup))]
    public class TweenCanvasGroupAlpha : TweenBase
    {
        public float from = 1f;
        public float to = 1f;

        private CanvasGroup m_CanvasGroup;
        public CanvasGroup canvasGroup
        {
            get
            {
                if (Application.isPlaying == false)
                    return GetComponent<CanvasGroup>();

                if (m_CanvasGroup == null)
                    m_CanvasGroup = GetComponent<CanvasGroup>();
                return m_CanvasGroup;
            }
        }

        protected override void SetValue(float t)
        {
            canvasGroup.alpha = Mathf.Lerp(from, to, t);
        }

        protected override void Reset()
        {
            base.Reset();
            from = to = canvasGroup.alpha;
        }
    }

}
