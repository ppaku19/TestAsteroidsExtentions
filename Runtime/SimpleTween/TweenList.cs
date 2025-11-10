using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleTween
{
    public class TweenList : MonoBehaviour
    {
        public List<TweenBase> list = new List<TweenBase>();
        public bool playOnEnable = true;

        private void OnEnable()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.update += EditorUpdate;
#endif

            if (playOnEnable)
                Play();
        }

        private void OnDisable()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.update -= EditorUpdate;
#endif
        }

        private void Update()
        {
            if (Application.isPlaying == false)
                return;

            UpdateTweens();
        }

#if UNITY_EDITOR
        private void EditorUpdate()
        {
            if (Application.isPlaying == true)
                return;

            UpdateTweens();
        }
#endif


        public void Play()
        {
            Stop();
            m_TweenIndex = -1;
        }

        public void Stop()
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i] == null) continue;
                list[i].Stop();
            }
        }


        private int m_TweenIndex = -1;

        private void UpdateTweens()
        {
            if (m_TweenIndex >= list.Count)
                return;

            for (int i = m_TweenIndex; i < list.Count; i++)
            {
                if (i >= 0)
                {
                    if (list[i] == null)
                        continue;

                    if (list[i].enabled == true)
                        break;
                }

                m_TweenIndex = i + 1;
                if (m_TweenIndex >= list.Count)
                    break;

                list[m_TweenIndex].Play();
                break;
            }
        }
    }
}
