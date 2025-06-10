using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteAlways]
public class CUIAnimatedScale2DByFactor : CUIAnimatedByFactor
{
    public List<Vector2> m_ScaleList = new List<Vector2>();

    public override IList PointList => m_ScaleList;

    public override int PointCnt
    {
        get { return m_ScaleList.Count; }
    }

    public override bool IsValid => transform != null;

    public override void OnUpdate(int _nStartIdx, float _fRemainderFactor, bool _bExistNextPoint)
    {
        var Scale = m_ScaleList[_nStartIdx];
        if (_bExistNextPoint)
        {
            Scale = Vector2.Lerp(Scale, m_ScaleList[_nStartIdx + 1], _fRemainderFactor);
        }

        transform.localScale = new Vector3(Scale.x, Scale.y, 1f);
    }

    public override void Capture()
    {
        if (IsValid)
        {
            m_ScaleList.Add(transform.localScale);
        }
    }
}
