using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace SimpleTween
{
    [ExecuteAlways]
    public abstract class TweenBase : MonoBehaviour
    {
        public AnimationCurve curve = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(1f, 1f));
        public float startDelay = 0f;
        public float duration = 1f;
        public int loop = 1; //루프횟수. 0이면 무한루프
        public bool ignoreTimeScale = true;
        public UnityEvent onFinished = new UnityEvent();


        protected bool m_IsReverse = false;
        protected float m_Time = 0f;
        protected int m_PlayFrameCount = 0;



        public void Play()
        {
            Stop();
            enabled = true;
        }

        public void PlayReverse()
        {
            StopReverse();
            enabled = true;
        }

        public void Stop()
        {
            m_IsReverse = false;
            m_Time = 0f;
            m_PlayFrameCount = 0;
            enabled = false;
            SetValue(0f);
        }

        public void StopReverse()
        {
            m_IsReverse = true;
            m_Time = 0f;
            m_PlayFrameCount = 0;
            enabled = false;
            SetValue(1f);
        }


        private void OnEnable()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.update += EditorUpdate;
#endif
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

            UpdateTween();
        }

#if UNITY_EDITOR
        private void EditorUpdate()
        {
            if (Application.isPlaying == true)
                return;

            UpdateTween();
        }
#endif



        private void UpdateTween()
        {
            if (m_Time < startDelay)
            {
                UpdateTime();
                return;
            }

            var t = m_Time - startDelay;
            var isEnded = false;
            if (loop > 0)
            {
                if ((t / duration) < loop)
                {
                    t = t % duration;
                }
                else
                {
                    t = duration;
                    isEnded = true;
                }
            }
            else
            {
                t = t % duration;
            }

            SetValue(curve.Evaluate(m_IsReverse ? 1f - (t / duration) : (t / duration)));
            if (isEnded == true)
            {
                enabled = false;
                onFinished.Invoke();
            }

            UpdateTime();
        }

        private void UpdateTime()
        {
            if (Application.isPlaying == false && (ignoreTimeScale ? Time.unscaledDeltaTime : Time.deltaTime) > 0.1f) //deltatime이 0.1을 넘어간다면 이상한거다.
            {
                m_Time = 0f;
            }
            else
            {
                m_Time += ignoreTimeScale ? Time.unscaledDeltaTime : Time.deltaTime;
            }

            m_PlayFrameCount++;
        }



        protected abstract void SetValue(float t);

        protected virtual void Reset()
        {
            enabled = false;
        }
    }
}
