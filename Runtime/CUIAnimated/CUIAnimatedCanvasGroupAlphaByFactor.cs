using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(CanvasGroup))]
public class CUIAnimatedCanvasGroupAlphaByFactor : CUIAnimatedByFactor
{
    public List<float> m_AlphaList = new List<float>();

    public override IList PointList => m_AlphaList;

    public override int PointCnt => m_AlphaList.Count;

    public override bool IsValid => canvasGroup != null;


    private CanvasGroup _canvasGroup;
    public CanvasGroup canvasGroup
    {
        get
        {
            if (Application.isPlaying == false)
                return GetComponent<CanvasGroup>();
            if (_canvasGroup == null) _canvasGroup = GetComponent<CanvasGroup>();
            return _canvasGroup;
        }
    }

    public override void OnUpdate(int _nStartIdx, float _fRemainderFactor, bool _bExistNextPoint)
    {
        var alpha = m_AlphaList[_nStartIdx];
        if (_bExistNextPoint)
        {
            alpha = Mathf.Lerp(alpha, m_AlphaList[_nStartIdx + 1], _fRemainderFactor);
        }

        canvasGroup.alpha = alpha;
    }

    public override void Capture()
    {
        if (IsValid)
        {
            m_AlphaList.Add(canvasGroup.alpha);
        }
    }
}
