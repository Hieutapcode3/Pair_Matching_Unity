using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SetGameBtn : MonoBehaviour
{
    public enum EBtnType
    {
        NotSet,
        PairNumberBtn,
        PuzzleCategoriesBtn,
    };
    [SerializeField] public EBtnType btntype = EBtnType.NotSet;
    [HideInInspector] public GameSetting.EPairNumber PairNumber = GameSetting.EPairNumber.NotSet;
    [HideInInspector] public GameSetting.EPuzzleCategories PuzzleCategories = GameSetting.EPuzzleCategories.NotSet;
    void Start()
    {
        
    }

    public void SetGameOption(string GameSceneName)
    {
        var comp = gameObject.GetComponent<SetGameBtn>();
        switch(comp.btntype)
        {
            case SetGameBtn.EBtnType.PairNumberBtn:
                GameSetting.Instance.SetPairNumber(comp.PairNumber);
                break;
            case EBtnType.PuzzleCategoriesBtn:
                GameSetting.Instance.SetPuzzleCategories(comp.PuzzleCategories); 
                break;

        }
        if (GameSetting.Instance.AllSettingsReady())
        {
            SceneManager.LoadScene(GameSceneName);
        }
    }
}
