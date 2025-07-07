using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TMPro
{
    [RequireComponent(typeof(TMP_Text))]
    [ExecuteAlways]
    public class TextMaterialChanger : MonoBehaviour
    {
        [SerializeField] private Material m_ChangeMaterial;
        public Material changeMaterial
        {
            get => m_ChangeMaterial;
            set
            {
                if (m_ChangeMaterial == value)
                    return;

                m_ChangeMaterial = value;
                if (enabled == true)
                    text.fontSharedMaterial = value;
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


        private void OnEnable()
        {
            if (changeMaterial != null)
                text.fontSharedMaterial = changeMaterial;

            text.OnPreRenderText += OnPreRenderText;
        }

        private void OnDisable()
        {
            text.OnPreRenderText -= OnPreRenderText;
        }

        private void OnValidate()
        {
            if (enabled == true)
                text.fontSharedMaterial = changeMaterial;
        }

        private void OnPreRenderText(TMP_TextInfo textInfo)
        {
            if (changeMaterial == null)
                return;

            if (text.fontSharedMaterial != changeMaterial)
                text.fontSharedMaterial = changeMaterial;
        }
    }
}
