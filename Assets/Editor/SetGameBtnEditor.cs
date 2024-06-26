using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SetGameBtn))]
[CanEditMultipleObjects]
[System.Serializable]
public class SetGameBtnEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        SetGameBtn myScript = target as SetGameBtn;
        switch (myScript.btntype)
        {
            case SetGameBtn.EBtnType.PairNumberBtn:
                myScript.PairNumber = (GameSetting.EPairNumber)EditorGUILayout.EnumPopup("Pair Numbers", myScript.PairNumber);
                break;
            case SetGameBtn.EBtnType.PuzzleCategoriesBtn:
                myScript.PuzzleCategories = (GameSetting.EPuzzleCategories)EditorGUILayout.EnumPopup("Puzzle Categories", myScript.PuzzleCategories);
                break;
        }
        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }
}
