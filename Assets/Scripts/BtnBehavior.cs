using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class BtnBehavior : MonoBehaviour
{
    private void Start()
    {
        Config.CreateScoreFile();
    }
    public void LoadScene(string scene_name)
    {
        SceneManager.LoadScene(scene_name);
    }
    public void ResetGameSettings()
    {
        GameSetting.Instance.ResetGameSettings();
    }
}
