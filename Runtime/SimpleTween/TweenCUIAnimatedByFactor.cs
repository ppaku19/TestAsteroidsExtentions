using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleTween
{
    [ExecuteAlways]
    [RequireComponent(typeof(CUIAnimatedByFactor))]
    public class TweenCUIAnimatedByFactor : TweenBase
    {
        public float from = 1f;
        public float to = 1f;

        private CUIAnimatedByFactor m_CUIAnimatedByFactor;
        public CUIAnimatedByFactor cUIAnimatedByFactor
        {
            get
            {
                if (Application.isPlaying == false)
                    return GetComponent<CUIAnimatedByFactor>();

                if (m_CUIAnimatedByFactor == null)
                    m_CUIAnimatedByFactor = GetComponent<CUIAnimatedByFactor>();
                return m_CUIAnimatedByFactor;
            }
        }

        protected override void SetValue(float t)
        {
            cUIAnimatedByFactor.m_fFactor = Mathf.Lerp(from, to, t);
        }

        protected override void Reset()
        {
            base.Reset();
            from = to = cUIAnimatedByFactor.m_fFactor;
        }
    }
}
