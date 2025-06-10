using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteAlways]
public class CUIAnimatedRotByFactor : CUIAnimatedByFactor
{
    public List<Vector3> m_RotList = new List<Vector3>();

    public override IList PointList => m_RotList;

    public override int PointCnt
    {
        get { return m_RotList.Count; }
    }

    public override bool IsValid => transform != null;


    public override void OnUpdate(int _nStartIdx, float _fRemainderFactor, bool _bExistNextPoint)
    {
        var Rot = m_RotList[_nStartIdx];
        if (_bExistNextPoint)
        {
            Rot = Vector3.Lerp(Rot, m_RotList[_nStartIdx + 1], _fRemainderFactor);
        }

        transform.localEulerAngles = Rot;
    }

    public override void Capture()
    {
        if (IsValid)
        {
            m_RotList.Add(transform.localEulerAngles);
        }
    }
}
