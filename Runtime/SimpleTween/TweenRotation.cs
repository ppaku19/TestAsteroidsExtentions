using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleTween
{
    [ExecuteAlways]
    public class TweenRotation : TweenBase
    {
        public Vector3 from = Vector3.zero;
        public Vector3 to = Vector3.zero;
        public bool worldRotation = false;

        protected override void SetValue(float t)
        {
            if (worldRotation)
                transform.eulerAngles = Vector3.Lerp(from, to, t);
            else
                transform.localEulerAngles = Vector3.Lerp(from, to, t);
        }

        protected override void Reset()
        {
            base.Reset();
            from = to = transform.localEulerAngles;
        }
    }

}
