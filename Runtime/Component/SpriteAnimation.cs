using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.UI
{
    [RequireComponent(typeof(Image))]
    public class SpriteAnimation : MonoBehaviour
    {
        [SerializeField] private AnimationData[] m_Animations;
        public bool playOnWake = true;
        public bool ignoreTimeScale = true;

        [System.Serializable]
        public struct AnimationData
        {
            public string animationName;
            public bool loop;
            public AnimationSpriteData[] sprites;
        }

        [System.Serializable]
        public struct AnimationSpriteData
        {
            public Sprite sprite;
            public float time;
        }


        private Image _img;
        public Image img
        {
            get
            {
                if (_img == null) _img = GetComponent<Image>();
                return _img;
            }
        }

        private bool m_IsPlaying = false;
        public bool isPlaying => m_IsPlaying;

        private string m_AnimationName;
        public string animationName => m_AnimationName;

        private Dictionary<string, AnimationData> m_CachedAnimationDatas = null;
        private float m_Time = 0f;

        private void OnEnable()
        {
            if (playOnWake && m_Animations != null && m_Animations.Length > 0)
            {
                if (m_AnimationName == null)
                    m_AnimationName = m_Animations[0].animationName;

                Play();
            }
        }

        private void Update()
        {
            if (m_IsPlaying == false)
                return;

            m_Time += ignoreTimeScale ? Time.unscaledDeltaTime : Time.deltaTime;
            UpdateSprite();
        }

        public void Play()
        {
            m_Time = 0f;
            m_IsPlaying = true;
        }

        public void Play(string animName)
        {
            m_AnimationName = animName;
            m_Time = 0f;
            m_IsPlaying = true;
            UpdateSprite();
        }

        public void Pause()
        {
            m_IsPlaying = false;
            UpdateSprite();
        }

        public void Stop()
        {
            m_IsPlaying = false;
            m_Time = 0f;
            UpdateSprite();
        }

        public AnimationData GetAnimationData(string animName)
        {
            if (Application.isPlaying == false)
            {
                if (m_Animations != null)
                {
                    for (int i = 0; i < m_Animations.Length; i++)
                    {
                        if (m_Animations[i].animationName == animationName)
                            return m_Animations[i];
                    }
                }
                return default;
            }

            if (m_CachedAnimationDatas == null)
            {
                m_CachedAnimationDatas = new();
                for (int i = 0; i < m_Animations.Length; i++)
                {
                    m_CachedAnimationDatas[m_Animations[i].animationName] = m_Animations[i];
                }
            }

            return m_CachedAnimationDatas.ContainsKey(animName) ? m_CachedAnimationDatas[animationName] : default;
        }

        private void UpdateSprite()
        {
            var animData = GetAnimationData(m_AnimationName);
            if (animData.sprites == null)
                return;

            var time = m_Time;
            do
            {
                for (int i = 0; i < animData.sprites.Length; i++)
                {
                    var sprData = animData.sprites[i];
                    if (time < sprData.time)
                    {
                        img.sprite = sprData.sprite;
                        return;
                    }

                    time -= sprData.time;
                }
            }
            while (animData.loop);
        }
    }
}
