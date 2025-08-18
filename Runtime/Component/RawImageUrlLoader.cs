using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

namespace UnityEngine.UI
{
    [RequireComponent(typeof(RawImage))]
    public class RawImageUrlLoader : MonoBehaviour
    {
        [SerializeField] private string m_Url;
        public string url
        {
            get => m_Url;
            set
            {
                if (m_Url == value)
                    return;

                m_Url = value;
                if (Application.isPlaying == true && isActiveAndEnabled == true)
                    Load();
            }
        }

        [SerializeField] private bool m_AutoLoad = true;
        public bool autoLoad
        {
            get => m_AutoLoad;
            set
            {
                if (m_AutoLoad == value)
                    return;

                if (Application.isPlaying == true)
                {
                    if (m_AutoLoad == true && rawImage.texture == null)
                        Load();
                }
            }
        }

        [SerializeField] private float m_FadeDuration = 0.2f;
        public float faceDuration { get => m_FadeDuration; set => m_FadeDuration = value; }

        [SerializeField] private UnityEvent m_OnLoad = new UnityEvent();
        public UnityEvent onLoad => m_OnLoad;

        private RawImage m_RawImage;
        public RawImage rawImage
        {
            get
            {
                if (m_RawImage == null) m_RawImage = GetComponent<RawImage>();
                return m_RawImage;
            }
        }

        private CanvasRenderer m_CanvasRenderer;
        private Coroutine m_LoadRoutine;

        private void Awake()
        {
            m_CanvasRenderer = GetComponent<CanvasRenderer>();
        }

        private void OnEnable()
        {
            Load();
        }

        private void OnDisable()
        {
            m_LoadRoutine = null;
        }


        public void Load(bool forceReload = false)
        {
            if (m_LoadRoutine != null)
            {
                StopCoroutine(m_LoadRoutine);
                m_LoadRoutine = null;
            }

            if (string.IsNullOrEmpty(m_Url) == false)
            {
                m_LoadRoutine = StartCoroutine(LoadRoutine(forceReload, m_Url, m_FadeDuration));
            }
            else
            {
                m_CanvasRenderer.SetAlpha(1f);
                m_OnLoad.Invoke();
            }
        }

        private IEnumerator LoadRoutine(bool forceReload, string url, float fadeDuration)
        {
            m_CanvasRenderer.SetAlpha(0f);
            rawImage.texture = null;

            Texture2D loadedTexture = null;
            if (forceReload == false)
                s_textureCache.TryGetValue(url, out loadedTexture);

            if (loadedTexture == null)
            {
                using (var request = UnityWebRequestTexture.GetTexture(url))
                {
                    yield return request.SendWebRequest();

                    if (request.result != UnityWebRequest.Result.Success)
                    {
                        Debug.LogError($"[RawImageUrlLoader] Image Load Failed: {request.error}");
                    }
                    else
                    {
                        loadedTexture = DownloadHandlerTexture.GetContent(request);
                        rawImage.texture = loadedTexture;
                        s_textureCache[url] = loadedTexture;
                        m_OnLoad.Invoke();
                    }
                }
            }

            rawImage.texture = loadedTexture;

            if (fadeDuration > 0f)
            {
                m_CanvasRenderer.SetAlpha(0f);
                m_OnLoad.Invoke();

                var fadeStartTime = Time.realtimeSinceStartup;
                while ((Time.realtimeSinceStartup - fadeStartTime) < fadeDuration)
                {
                    m_CanvasRenderer.SetAlpha((Time.realtimeSinceStartup - fadeStartTime) / fadeDuration);
                    yield return null;
                }
            }
            else
            {
                m_CanvasRenderer.SetAlpha(1f);
                m_OnLoad.Invoke();
            }
        }



        private static Dictionary<string, Texture2D> s_textureCache = new Dictionary<string, Texture2D>();

        public static void ClearTextureCaches() => s_textureCache.Clear();
    }
}
