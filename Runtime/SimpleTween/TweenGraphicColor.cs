using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SimpleTween
{
    [ExecuteAlways]
    [RequireComponent(typeof(Graphic))]
    public class TweenGraphicColor : TweenBase
    {
        public Color from = Color.white;
        public Color to = Color.white;

        private Graphic m_Graphic;
        public Graphic graphic
        {
            get
            {
                if (Application.isPlaying == false)
                    return GetComponent<Graphic>();

                if (m_Graphic == null)
                    m_Graphic = GetComponent<Graphic>();
                return m_Graphic;
            }
        }

        protected override void SetValue(float t)
        {
            graphic.color = Color.Lerp(from, to, t);
        }

        protected override void Reset()
        {
            base.Reset();
            from = to = graphic.color;
        }
    }

}
