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
        [SerializeField] private Texture2D m_EmptyTexture;
        public Texture2D emptyTexture
        {
            get => m_EmptyTexture;
            set
            {
                if (m_EmptyTexture == value)
                    return;

                m_EmptyTexture = value;
                if (Application.isPlaying == true && isActiveAndEnabled == true && m_IsEmpty == true)
                    rawImage.texture = m_EmptyTexture;
            }
        }


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
        public CanvasRenderer canvasRenderer
        {
            get
            {
                if (m_CanvasRenderer == null) m_CanvasRenderer = GetComponent<CanvasRenderer>();
                return m_CanvasRenderer;
            }
        }

        private bool m_IsEmpty = true;
        private Coroutine m_LoadRoutine;

        private void Awake()
        {
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
                canvasRenderer.SetAlpha(1f);
                m_OnLoad.Invoke();
            }
        }

        private IEnumerator LoadRoutine(bool forceReload, string url, float fadeDuration)
        {
            canvasRenderer.SetAlpha(0f);
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
                        s_textureCache[url] = loadedTexture;
                        m_OnLoad.Invoke();
                    }
                }
            }

            m_IsEmpty = loadedTexture == null;
            rawImage.texture = m_IsEmpty ? m_EmptyTexture : loadedTexture;

            if (fadeDuration > 0f)
            {
                canvasRenderer.SetAlpha(0f);
                m_OnLoad.Invoke();

                var fadeStartTime = Time.realtimeSinceStartup;
                while ((Time.realtimeSinceStartup - fadeStartTime) < fadeDuration)
                {
                    canvasRenderer.SetAlpha((Time.realtimeSinceStartup - fadeStartTime) / fadeDuration);
                    yield return null;
                }
            }
            else
            {
                canvasRenderer.SetAlpha(1f);
                m_OnLoad.Invoke();
            }
        }



        private static Dictionary<string, Texture2D> s_textureCache = new Dictionary<string, Texture2D>();

        public static void ClearTextureCaches() => s_textureCache.Clear();
    }
}
