using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace SimpleTween
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(TweenBase), true)]
    public class TweenBaseEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                if (GUILayout.Button("Play"))
                    for (int i = 0; i < targets.Length; i++)
                        ((TweenBase)targets[i]).Play();
                if (GUILayout.Button("Play Reverse"))
                    for (int i = 0; i < targets.Length; i++)
                        ((TweenBase)targets[i]).PlayReverse();
                if (GUILayout.Button("Stop"))
                    for (int i = 0; i < targets.Length; i++)
                        ((TweenBase)targets[i]).Stop();
                if (GUILayout.Button("Stop Reverse"))
                    for (int i = 0; i < targets.Length; i++)
                        ((TweenBase)targets[i]).StopReverse();
            }

            base.OnInspectorGUI();
        }
    }
}
