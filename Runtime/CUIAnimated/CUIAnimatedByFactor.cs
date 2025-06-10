using UnityEngine;
using System.Collections;

public abstract class CUIAnimatedByFactor : MonoBehaviour
{
    public float m_fFactor = 0f;

    public abstract IList PointList { get; }
    public abstract int PointCnt { get; }
    public abstract bool IsValid { get; }

    protected float _prevFactor = float.MinValue;
    protected bool _needUpdate = false;


    protected virtual void OnEnable()
    {
        _needUpdate = true;
    }

    protected virtual void OnDisable()
    {
    }

    protected virtual void OnValidate()
    {
        _needUpdate = true;
    }

    protected virtual void LateUpdate()
    {
        if (IsValid == false)
            return;

        if (PointCnt <= 0)
            return;

        if (Application.isPlaying == true && _needUpdate == false && _prevFactor == m_fFactor)
            return;

        m_fFactor = (PointCnt > 1) ? Mathf.Clamp(m_fFactor, 0f, (float)(PointCnt - 1)) : 0;
        int nStartIdx = Mathf.FloorToInt(m_fFactor);
        float fRemainderFactor = m_fFactor - (float)nStartIdx;

        OnUpdate(nStartIdx, fRemainderFactor, nStartIdx + 1 < PointCnt);
        _needUpdate = false;
        _prevFactor = m_fFactor;
    }

    public abstract void OnUpdate(int _nStartIdx, float _fRemainderFactor, bool _bExistNextPoint);

    public abstract void Capture();
}
