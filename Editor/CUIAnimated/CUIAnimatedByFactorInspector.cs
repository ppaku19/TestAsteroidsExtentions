using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CUIAnimatedByFactor), true)]
public class CUIAnimatedByFactorInspector : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        var AnimatedAnchor = target as CUIAnimatedByFactor;

        if (GUILayout.Button("Capture"))
        {
            AnimatedAnchor.Capture();
        }
    }
}