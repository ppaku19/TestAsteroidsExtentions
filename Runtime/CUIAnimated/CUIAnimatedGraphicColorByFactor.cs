using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(UnityEngine.UI.Graphic))]
public class CUIAnimatedGraphicColorByFactor : CUIAnimatedByFactor
{
    public List<Color> m_ColorList = new List<Color>();

    public override IList PointList => m_ColorList;

    public override int PointCnt => m_ColorList.Count;

    public override bool IsValid => graphic != null;


    private UnityEngine.UI.Graphic _graphic;
    public UnityEngine.UI.Graphic graphic
    {
        get
        {
            if (Application.isPlaying == false)
                return GetComponent<UnityEngine.UI.Graphic>();
            if (_graphic == null) _graphic = GetComponent<UnityEngine.UI.Graphic>();
            return _graphic;
        }
    }

    public override void OnUpdate(int _nStartIdx, float _fRemainderFactor, bool _bExistNextPoint)
    {
        var color = m_ColorList[_nStartIdx];
        if (_bExistNextPoint)
        {
            color = Color.Lerp(color, m_ColorList[_nStartIdx + 1], _fRemainderFactor);
        }

        graphic.color = color;
    }

    public override void Capture()
    {
        if (IsValid)
        {
            m_ColorList.Add(graphic.color);
        }
    }
}
