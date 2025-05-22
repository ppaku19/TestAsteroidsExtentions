using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleTween
{
    public class TweenReset : MonoBehaviour
    {
        private void OnEnable()
        {
            var tweens = GetComponentsInChildren<TweenBase>();
            foreach (var tween in tweens)
                tween.Play();
        }
    }

}
