using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace SimpleTween
{
    [ExecuteAlways]
    public class TweenEventFloat : TweenBase
    {
        public float from = 1f;
        public float to = 1f;
        public ValueChangedEvent onValueChanged;

        [System.Serializable]
        public class ValueChangedEvent : UnityEvent<float> { }

        protected override void SetValue(float t)
        {
            onValueChanged.Invoke(Mathf.Lerp(from, to, t));
        }
    }

}
