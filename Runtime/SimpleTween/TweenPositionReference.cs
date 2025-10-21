using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleTween
{
    [ExecuteAlways]
    public class TweenPositionReference : TweenBase
    {
        public Transform from = null;
        public Transform to = null;

        protected override void SetValue(float t)
        {
            if (from == null || to == null)
                return;

            transform.position = Vector3.Lerp(from.position, to.position, t);
        }
    }
}
