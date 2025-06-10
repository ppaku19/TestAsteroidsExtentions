using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(RectTransform))]
public class CUIAnimatedAnchoredPositionByFactor : CUIAnimatedByFactor
{
    public List<Vector2> m_PosList = new List<Vector2>();

    public override IList PointList => m_PosList;

    public override int PointCnt => m_PosList.Count;

    public override bool IsValid => rectTransform != null;


    private RectTransform _rectTransform;
    public RectTransform rectTransform
    {
        get
        {
            if (Application.isPlaying == false)
                return transform as RectTransform;
            if (_rectTransform == null) _rectTransform = transform as RectTransform;
            return _rectTransform;
        }
    }

    public override void OnUpdate(int _nStartIdx, float _fRemainderFactor, bool _bExistNextPoint)
    {
        var Pos = m_PosList[_nStartIdx];
        if (_bExistNextPoint)
        {
            Pos = Vector2.Lerp(Pos, m_PosList[_nStartIdx + 1], _fRemainderFactor);
        }

        rectTransform.anchoredPosition = Pos;
    }

    public override void Capture()
    {
        if (IsValid)
        {
            m_PosList.Add(rectTransform.anchoredPosition);
        }
    }
}
