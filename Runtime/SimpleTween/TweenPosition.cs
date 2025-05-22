using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleTween
{
    [ExecuteAlways]
    public class TweenPosition : TweenBase
    {
        public Vector3 from = Vector3.zero;
        public Vector3 to = Vector3.zero;
        public bool worldPosition = false;

        protected override void SetValue(float t)
        {
            if (worldPosition)
                transform.position = Vector3.Lerp(from, to, t);
            else
                transform.localPosition = Vector3.Lerp(from, to, t);
        }

        protected override void Reset()
        {
            base.Reset();
            from = to = transform.localPosition;
        }
    }

}
