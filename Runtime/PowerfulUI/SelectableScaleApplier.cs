using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PowerfulUI
{
    [ExecuteAlways]
    public class SelectableScaleApplier : MonoBehaviour, ISelectableTransitionApplier
    {
        [SerializeField] private Vector3 m_NormalScale = Vector3.one;
        public Vector3 normalScale { get { return m_NormalScale; } set { if (UIUtil.SetStruct(ref m_NormalScale, value)) OnSetProperty(); } }
        [SerializeField] private Vector3 m_HighlightedScale = Vector3.one;
        public Vector3 highlightedScale { get { return m_HighlightedScale; } set { if (UIUtil.SetStruct(ref m_HighlightedScale, value)) OnSetProperty(); } }
        [SerializeField] private Vector3 m_PressedScale = Vector3.one * 0.9f;
        public Vector3 pressedScale { get { return m_PressedScale; } set { if (UIUtil.SetStruct(ref m_PressedScale, value)) OnSetProperty(); } }
        [SerializeField] private Vector3 m_SelectedScale = Vector3.one;
        public Vector3 selectedScale { get { return m_SelectedScale; } set { if (UIUtil.SetStruct(ref m_SelectedScale, value)) OnSetProperty(); } }
        [SerializeField] private Vector3 m_DisabledScale = Vector3.one;
        public Vector3 disabledScale { get { return m_DisabledScale; } set { if (UIUtil.SetStruct(ref m_DisabledScale, value)) OnSetProperty(); } }
        [SerializeField] private float m_FadeDuration = 0.1f;
        public float fadeDuration { get { return m_FadeDuration; } set { if (UIUtil.SetStruct(ref m_FadeDuration, value)) OnSetProperty(); } }


        private Selectable m_Selectable;
        private int m_LastState = 0;

        private void OnEnable()
        {
            m_Selectable = GetComponentInParent<Selectable>();
            if (m_Selectable != null) m_Selectable.RegistTransitionApplier(this);
        }

        private void OnDisable()
        {
            if (m_Selectable != null) m_Selectable.UnregistTransitionApplier(this);
        }

        private void OnSetProperty()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
                DoStateTransition(m_LastState, true);
            else
#endif
                DoStateTransition(m_LastState, false);
        }

        public void DoStateTransition(int state, bool instant)
        {
            m_LastState = state;
            if (!gameObject.activeInHierarchy)
                return;

            Vector3 scale = m_NormalScale;
            switch ((SelectionState)state)
            {
                case SelectionState.Normal:
                    scale = m_NormalScale;
                    break;
                case SelectionState.Highlighted:
                    scale = m_HighlightedScale;
                    break;
                case SelectionState.Pressed:
                    scale = m_PressedScale;
                    break;
                case SelectionState.Selected:
                    scale = m_SelectedScale;
                    break;
                case SelectionState.Disabled:
                    scale = m_DisabledScale;
                    break;
            }

            if (instant == true || Application.isPlaying == false)
            {
                transform.localScale = scale;
            }
            else
            {
                StopCoroutine("ScaleAnimationRoutine");
                StartCoroutine(ScaleAnimationRoutine(scale));
            }
        }

        /// <summary>
        /// An enumeration of selected states of objects
        /// </summary>
        protected enum SelectionState
        {
            /// <summary>
            /// The UI object can be selected.
            /// </summary>
            Normal,

            /// <summary>
            /// The UI object is highlighted.
            /// </summary>
            Highlighted,

            /// <summary>
            /// The UI object is pressed.
            /// </summary>
            Pressed,

            /// <summary>
            /// The UI object is selected
            /// </summary>
            Selected,

            /// <summary>
            /// The UI object cannot be selected.
            /// </summary>
            Disabled,
        }

        private IEnumerator ScaleAnimationRoutine(Vector3 scale)
        {
            var startScale = transform.localScale;
            var startTime = Time.unscaledTime;
            while (Time.unscaledTime < (startTime + m_FadeDuration))
            {
                transform.localScale = Vector3.Lerp(startScale, scale, (Time.unscaledTime - startTime) / m_FadeDuration);
                yield return null;
            }
            transform.localScale = scale;
        }

        private void OnValidate()
        {
            DoStateTransition(m_LastState, true);
        }
    }
}
