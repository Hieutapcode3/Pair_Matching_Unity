using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameSetting : MonoBehaviour
{
    private readonly Directory<EPuzzleCategories,string> _puzzleCatDirection = new Dictionary<EPuzzleCategories,string>();
    private int                     _settings;
    private const int               _SettingNumber = 2;

    public enum EPairNumber
    {
        NotSet =0,
        E10Pairs = 10,
        E15Pairs = 15,
        E20Pairs = 20,
    }
    public enum EPuzzleCategories
    {
        NotSet,
        Fruits,
        Vegetables
    }
    public struct Settings
    {
        public EPairNumber PairsNumber;
        public EPuzzleCategories PuzzleCategories;
    };
    private Settings                _gameSettings;
    public static GameSetting       Instance;
    private void Awake()
    {
        if(Instance == null)
        {
            DontDestroyOnLoad(this);
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    void Start()
    {
        _gameSettings = new Settings();
        ResetGameSettings();
    }

    public void SetPairNumber(EPairNumber Nb)
    {
        if(_gameSettings.PairsNumber == EPairNumber.NotSet)
        {
            _settings++;
        }
        _gameSettings.PairsNumber = Nb;
    }
    public void SetPuzzleCategories(EPuzzleCategories categories)
    {
        if(_gameSettings.PuzzleCategories == EPuzzleCategories.NotSet)
        {
            _settings++;
        }
        _gameSettings.PuzzleCategories = categories;
    }
    public EPairNumber GetPuzzleNumber()
    {
        return _gameSettings.PairsNumber;
    }
    public EPuzzleCategories GetPuzzleCategories()
    {
        return _gameSettings.PuzzleCategories;
    }
    public void ResetGameSettings()
    {
        _settings = 0;
        _gameSettings.PuzzleCategories = EPuzzleCategories.NotSet;
        _gameSettings.PairsNumber = EPairNumber.NotSet;
    }
    public bool AllSettingsReady()
    {
        return _settings == _SettingNumber;
    }
    public string GetMaterialDirectionName()
    {
        return "Materials/";
    }
    public string GetPuzzleCattegoryTextureDirectionName()
    {

    }
}
