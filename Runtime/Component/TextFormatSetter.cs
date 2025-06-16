using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.UI
{
    public class TextFormatSetter : MonoBehaviour
    {
        [SerializeField] private string m_Format = "{0}";
        public string format
        {
            get => m_Format;
            set
            {
                if (m_Format != value)
                {
                    m_Format = value;
                    return;
                }
            }
        }

        [SerializeField] private bool m_SafeFormat = false;
        public bool safeFormat { get => m_SafeFormat; set => m_SafeFormat = value; }

#if UNITY_EDITOR
        [SerializeField] private string m_TestParam1;
        [SerializeField] private string m_TestParam2;
        [SerializeField] private string m_TestParam3;
#endif


        private Text m_Text;
        public Text text
        {
            get
            {
                if (Application.isPlaying == false)
                    return GetComponent<Text>();
                return m_Text;
            }
        }

        private TMPro.TMP_Text m_TMPText;
        public TMPro.TMP_Text tmpText
        {
            get
            {
                if (Application.isPlaying == false)
                    return GetComponent<TMPro.TMP_Text>();
                return m_TMPText;
            }
        }


        private object[] _params;


        private void Awake()
        {
            m_Text = GetComponent<Text>();
            m_TMPText = GetComponent<TMPro.TMP_Text>();
        }

        public void SetParameter(params object[] p)
        {
            _params = p;
            UpdateText();
        }

        private void UpdateText()
        {
            string str = null;

#if UNITY_EDITOR
            if (Application.isPlaying == false)
            {
                if (string.IsNullOrWhiteSpace(m_TestParam3) == false)
                    str = string.Format(format, m_TestParam1, m_TestParam2, m_TestParam3);
                else if (string.IsNullOrWhiteSpace(m_TestParam2) == false)
                    str = string.Format(format, m_TestParam1, m_TestParam2);
                else if (string.IsNullOrWhiteSpace(m_TestParam1) == false)
                    str = string.Format(format, m_TestParam1);
                else
                    str = format;
            }
            else
#endif
            {
                if (_params != null)
                {
                    if (_params.Length >= 3)
                        str = string.Format(format, _params[0], _params[1], _params[2]);
                    else if (_params.Length >= 2)
                        str = string.Format(format, _params[0], _params[1]);
                    else if (_params.Length >= 1)
                        str = string.Format(format, _params[0]);
                    else
                        str = format;
                }
                else
                {
                    str = format;
                }
            }

            if (m_SafeFormat)
            {
                try
                {
                    if (text != null) text.text = str;
                    if (tmpText != null) tmpText.text = str;
                }
                catch (System.Exception e)
                {
                    Debug.LogException(e);
                }
            }
            else
            {
                if (text != null) text.text = str;
                if (tmpText != null) tmpText.text = str;
            }
        }

        private void OnValidate()
        {
            UpdateText();
        }
    }

}
