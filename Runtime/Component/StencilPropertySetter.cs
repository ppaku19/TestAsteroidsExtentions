using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;

namespace UnityEngine.UI
{
    [ExecuteAlways]
    [RequireComponent(typeof(Graphic))]
    public class StencilPropertySetter : MonoBehaviour
    {
        [Header("Stencil Settings")]
        [SerializeField, Range(0, 255)] private int _stencilRef = 1;
        [SerializeField] private CompareFunction _stencilComparison = CompareFunction.Always;
        [SerializeField] private StencilOp _stencilOperation = StencilOp.Keep;
        [SerializeField, Range(0, 255)] private int _stencilWriteMask = 255;
        [SerializeField, Range(0, 255)] private int _stencilReadMask = 255;

        [Header("Rendering Settings")]
        [SerializeField] private ColorWriteMask _colorMask = ColorWriteMask.All;
        [SerializeField] private bool _useAlphaClip = true;

        private Graphic _graphic = null;
        private Material _materialInstance = null;

        private void OnEnable()
        {
            CacheGraphic();
            DestroyMaterialInstance();
            CreateMaterialInstance();
            UpdateStencilSettings();
        }

        void CacheGraphic()
        {
            _graphic = GetComponent<Graphic>();
        }

        private void OnDisable()
        {
            if (_graphic != null)
            {
                _graphic.material = null;
            }

            DestroyMaterialInstance();
        }

        private void OnDestroy()
        {
            DestroyMaterialInstance();
        }

        void DestroyMaterialInstance()
        {
            if (_materialInstance != null)
            {
                if (Application.isPlaying)
                {
                    Destroy(_materialInstance);
                }
                else
                {
                    DestroyImmediate(_materialInstance);
                }
                _materialInstance = null;
            }
        }

        void CreateMaterialInstance()
        {
            if (_graphic == null)
            {
                CacheGraphic();
            }
            Material sourceMaterial = _graphic.material;
            if (sourceMaterial == null)
            {
                _materialInstance = new Material(Shader.Find("UI/Default"));
            }
            else
            {
                _materialInstance = new Material(sourceMaterial);
            }

            _graphic.material = _materialInstance;
        }

        void UpdateStencilSettings()
        {
            if (enabled == false) 
                return;

            if (_materialInstance == null)
            {
                CreateMaterialInstance();
            }

            _materialInstance.SetInt("_Stencil", _stencilRef);
            _materialInstance.SetInt("_StencilComp", (int)_stencilComparison);
            _materialInstance.SetInt("_StencilOp", (int)_stencilOperation);
            _materialInstance.SetInt("_StencilWriteMask", _stencilWriteMask);
            _materialInstance.SetInt("_StencilReadMask", _stencilReadMask);

            var colorMaskInt = (int)_colorMask;
            if (colorMaskInt < 0)
            {
                colorMaskInt = (int)ColorWriteMask.All;
            }
            _materialInstance.SetInt("_ColorMask", colorMaskInt);
            _materialInstance.SetInt("_UseUIAlphaClip", _useAlphaClip ? 1 : 0);
            if (_useAlphaClip)
            {
                _materialInstance.EnableKeyword("UNITY_UI_ALPHACLIP");
            }
            else
            {
                _materialInstance.DisableKeyword("UNITY_UI_ALPHACLIP");
            }

        }

        public void SetStencilRef(int value)
        {
            _stencilRef = Mathf.Clamp(value, 0, 255);
            UpdateStencilSettings();
        }

        public void SetStencilComparison(CompareFunction comparison)
        {
            _stencilComparison = comparison;
            UpdateStencilSettings();
        }

        public void SetStencilOperation(StencilOp op)
        {
            _stencilOperation = op;
            UpdateStencilSettings();
        }

        public void SetStencilWriteMask(int mask)
        {
            _stencilWriteMask = Mathf.Clamp(mask, 0, 255);
            UpdateStencilSettings();
        }

        public void SetStencilReadMask(int mask)
        {
            _stencilReadMask = Mathf.Clamp(mask, 0, 255);
            UpdateStencilSettings();
        }

#if UNITY_EDITOR
        void OnValidate()
        {
            UpdateStencilSettings();
        }
#endif
    }
}