using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.UI
{
    public class TextFormatSetter : MonoBehaviour
    {
        [SerializeField] [TextArea] private string m_Format = "{0}";
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


        protected virtual void Awake()
        {
            m_Text = GetComponent<Text>();
            m_TMPText = GetComponent<TMPro.TMP_Text>();
        }

        public virtual void SetParameter(params object[] p)
        {
            _params = p;
            UpdateText();
        }

        protected virtual string GetFormatString() => format;

        protected virtual void UpdateText()
        {
            string str = null;
            string formatStr = GetFormatString();

#if UNITY_EDITOR
            if (Application.isPlaying == false)
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(m_TestParam3) == false)
                        str = string.Format(formatStr, m_TestParam1, m_TestParam2, m_TestParam3);
                    else if (string.IsNullOrWhiteSpace(m_TestParam2) == false)
                        str = string.Format(formatStr, m_TestParam1, m_TestParam2);
                    else if (string.IsNullOrWhiteSpace(m_TestParam1) == false)
                        str = string.Format(formatStr, m_TestParam1);
                    else
                        str = formatStr;
                }
                catch (System.Exception e)
                {
                    Debug.LogException(e);
                    str = formatStr;
                }
            }
            else
#endif
            {
                if (_params != null)
                {
                    if (m_SafeFormat)
                    {
                        try
                        {
                            if (_params.Length >= 3)
                                str = string.Format(formatStr, _params[0], _params[1], _params[2]);
                            else if (_params.Length >= 2)
                                str = string.Format(formatStr, _params[0], _params[1]);
                            else if (_params.Length >= 1)
                                str = string.Format(formatStr, _params[0]);
                            else
                                str = formatStr;
                        }
                        catch (System.Exception e)
                        {
                            Debug.LogException(e);
                            str = formatStr;
                        }
                    }
                    else
                    {
                        if (_params.Length >= 3)
                            str = string.Format(formatStr, _params[0], _params[1], _params[2]);
                        else if (_params.Length >= 2)
                            str = string.Format(formatStr, _params[0], _params[1]);
                        else if (_params.Length >= 1)
                            str = string.Format(formatStr, _params[0]);
                        else
                            str = formatStr;
                    }
                }
                else
                {
                    str = formatStr;
                }
            }

            if (text != null) text.text = str;
            if (tmpText != null) tmpText.text = str;
        }

        protected virtual void OnValidate()
        {
            UpdateText();
        }

        protected virtual void Reset()
        {
            if (text != null) m_Format = text.text;
            else if (tmpText != null) m_Format = tmpText.text;
        }
    }

}
