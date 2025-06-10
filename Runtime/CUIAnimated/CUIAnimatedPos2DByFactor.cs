using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteAlways]
public class CUIAnimatedPos2DByFactor : CUIAnimatedByFactor
{
    public List<Vector2> m_PosList = new List<Vector2>();

    public override IList PointList => m_PosList;

    public override int PointCnt => m_PosList.Count;

    public override bool IsValid => transform != null;

    public override void OnUpdate(int _nStartIdx, float _fRemainderFactor, bool _bExistNextPoint)
    {
        var Pos = m_PosList[_nStartIdx];
        if (_bExistNextPoint)
        {
            Pos = Vector2.Lerp(Pos, m_PosList[_nStartIdx + 1], _fRemainderFactor);
        }

        transform.localPosition = Pos;
    }

    public override void Capture()
    {
        if (IsValid)
        {
            m_PosList.Add(transform.localPosition);
        }
    }
}
