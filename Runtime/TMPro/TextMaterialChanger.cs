using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TMPro
{
    [RequireComponent(typeof(TMP_Text))]
    [ExecuteAlways]
    public class TextMaterialChanger : MonoBehaviour
    {
        [Header("Material")]
        [SerializeField] private Material m_OriginMaterial;
        public Material originMaterial
        {
            get => m_OriginMaterial;
            set
            {
                if (m_OriginMaterial == value)
                    return;

                m_OriginMaterial = value;
                ResetMaterial();
                UpdateMaterial();
            }
        }
        [SerializeField] private Material m_ChangeMaterial;
        public Material changeMaterial
        {
            get => m_ChangeMaterial;
            set
            {
                if (m_ChangeMaterial == value)
                    return;

                m_ChangeMaterial = value;
                ResetMaterial();
                UpdateMaterial();
            }
        }

        private TMP_Text m_Text;
        public TMP_Text text
        {
            get
            {
                if (Application.isPlaying == false)
                    return GetComponent<TMP_Text>();

                if (m_Text == null)
                    m_Text = GetComponent<TMP_Text>();
                return m_Text;
            }
        }

        private TMP_FontAsset m_PrevFontAsset;

        private void OnEnable()
        {
            if (m_PrevFontAsset != text.font || originMaterial == null || text.fontMaterial == null)
            {
                ResetMaterial();
                m_PrevFontAsset = text.font;
                originMaterial = text.fontSharedMaterial;
            }

            UpdateMaterial();
        }

        private void OnDisable()
        {
            ResetMaterial();
            m_PrevFontAsset = null;
        }

        private void OnValidate()
        {
            UpdateMaterial();
        }

        private void OnTransformParentChanged()
        {
            if (text == null)
                return;

            text.fontSharedMaterial = m_OriginMaterial;
            m_OriginMaterial = null;
            ResetMaterial();
            UpdateMaterial();
        }


        private void UpdateMaterial()
        {
            if (changeMaterial == null)
                return;

            text.fontMaterial = changeMaterial;
        }

        private void ResetMaterial()
        {
            if (originMaterial == null)
                return;

            text.fontMaterial = originMaterial;
        }

    }
}
