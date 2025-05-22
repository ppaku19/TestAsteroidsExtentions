using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleTween
{
    [ExecuteAlways]
    public class TweenScale : TweenBase
    {
        public Vector3 from = Vector3.one;
        public Vector3 to = Vector3.one;

        protected override void SetValue(float t)
        {
            transform.localScale = Vector3.Lerp(from, to, t);
        }

        protected override void Reset()
        {
            base.Reset();
            from = to = transform.localScale;
        }
    }

}
