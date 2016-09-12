using UnityEngine;
using UnityEditor;
using System.Collections;

// 메인 클래스. 여기서 grid에 데이터를 추가시켜준다.
[CustomEditor(typeof(GUITestScrollView))]
public class GUITestScrollViewEditor : Editor
{
    public int Idx = 0;
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        var component = target as GUITestScrollView;

        GUILayout.Space(10f);

        GUILayout.Label("[셀렉트 인덱스 선택기능 테스트]");
        if (!EditorApplication.isPlaying)
        {
            GUILayout.Label("플레이 상태일 때만 선택가능합니다.");
            return;
        }

        GUILayout.BeginHorizontal();
        {
            Idx = EditorGUILayout.IntField("idx", Idx);
            if (GUILayout.Button("SetPostion"))
            {
                component.Grid.SetPostion(Idx);
            }
        }
        GUILayout.EndHorizontal();
    }
}