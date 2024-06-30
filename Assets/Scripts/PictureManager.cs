using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;
using UnityEngine.UI;

public class PictureManager : MonoBehaviour
{
    public Picture PicturePrefab;
    public Transform PicSpawnPosition;
    public Vector2 StartPosition = new Vector2(-2.15f,3.62f);

    [Space]
    [Header("End Game Screen")]
    public GameObject EndgamePanel;
    public GameObject NewBestScoreTxt;
    public GameObject YourScoreTxt;
    public GameObject EndTimeTxt;
    public enum GameState
    {
        NoAction,MovingOnPosition,DeletingPuzzles,FlipBack,Checking,GameEnd
    };
    public enum PuzzleState
    {
        PuzzleRotating,
        CanRotate
    };
    public enum RevealedState
    {
        NoRevealed,
        OneRevealed,
        TwoRevealed
    };
    [HideInInspector] public GameState CurrentGameState;
    [HideInInspector] public PuzzleState CurrentPuzzleState;
    [HideInInspector] public RevealedState PuzzleRevealedNumber;
    [HideInInspector] public List<Picture> PictureList;

    private Vector2 _offset = new Vector2(1.5f,1.52f);
    private Vector2 _offsetFor15Pairs = new Vector2(1.08f,1.22f);
    private Vector2 _offsetFor20Pairs = new Vector2(1.08f,1f);
    private Vector3 _newScaleDown = new Vector3(0.9f,0.9f,.001f);

    private List<Material> _materialList = new List<Material>();
    private List<string> _texturePathList = new List<string>();
    private Material _firstMaterial;
    private string _firstTexturePath;

    private int _firstRevealedPic;
    private int _secondRevealedPic;
    private int _revealedPicNumber = 0;
    private int _picToDesTroy1;
    private int _picToDesTroy2;

    private bool _corutingStarted = false;

    private int _pairNumber;
    private int _removedPairs;
    private Timer _gameTimer;

    void Start()
    {
        CurrentGameState = GameState.NoAction;
        CurrentPuzzleState = PuzzleState.CanRotate;
        PuzzleRevealedNumber = RevealedState.NoRevealed;
        _revealedPicNumber = 0;
        _firstRevealedPic = -1;
        _secondRevealedPic = -1;

        _removedPairs = 0;
        _pairNumber = (int)GameSetting.Instance.GetPairNumber();

        _gameTimer = GameObject.Find("Main Camera").GetComponent<Timer>();

        LoadMaterials();
        if(GameSetting.Instance.GetPairNumber() == GameSetting.EPairNumber.E10Pairs)
        {
            CurrentGameState = GameState.MovingOnPosition;
            SpawnPictureMesh(4,5,StartPosition,_offset,false);
            MovePicture(4,5,StartPosition,_offset);
        }
        else if(GameSetting.Instance.GetPairNumber() == GameSetting.EPairNumber.E15Pairs)
        {
            CurrentGameState = GameState.MovingOnPosition;
            SpawnPictureMesh(5,6,StartPosition,_offset,false);
            MovePicture(5,6,StartPosition,_offsetFor15Pairs);
        }else if(GameSetting.Instance.GetPairNumber() == GameSetting.EPairNumber.E20Pairs)
        {
            CurrentGameState = GameState.MovingOnPosition;
            SpawnPictureMesh(5,8,StartPosition,_offset,true);
            MovePicture(5,8,StartPosition,_offsetFor20Pairs);
        }
    }
    public void CheckPicture()
    {
        CurrentGameState = GameState.Checking;
        _revealedPicNumber = 0;
        for (int id = 0;id < PictureList.Count;id++)
        {
            if (PictureList[id].Revealed && _revealedPicNumber < 2)
            {
                if(_revealedPicNumber == 0)
                {
                    _firstRevealedPic = id;
                    _revealedPicNumber++;
                }
                else if (_revealedPicNumber == 1)
                {
                    _secondRevealedPic = id;
                    _revealedPicNumber++;
                }
            }
        }
        if(_revealedPicNumber == 2)
        {
            if (PictureList[_firstRevealedPic].GetIndex() == PictureList[_secondRevealedPic].GetIndex() && _firstRevealedPic != _secondRevealedPic)
            {
                CurrentGameState = GameState.DeletingPuzzles;
                _picToDesTroy1 = _firstRevealedPic;
                _picToDesTroy2 = _secondRevealedPic;
            }
            else
            {
                CurrentGameState = GameState.FlipBack;
            }
        }
        CurrentPuzzleState = PictureManager.PuzzleState.CanRotate;
        if(CurrentGameState == GameState.Checking)
        {
            CurrentGameState = GameState.NoAction;
        }
    }

    private void DestroyPicture()
    {
        PuzzleRevealedNumber = RevealedState.NoRevealed;
        PictureList[_picToDesTroy1].Deactivate();
        PictureList[_picToDesTroy2].Deactivate();
        _revealedPicNumber = 0;
        _removedPairs++;
        CurrentGameState = GameState.NoAction;
        CurrentPuzzleState = PuzzleState.CanRotate;
    }
    private IEnumerator FlipBack()
    {
        _corutingStarted = true;
        yield return new WaitForSeconds(0.5f);

        PictureList[_firstRevealedPic].FlipBack();
        PictureList[_secondRevealedPic].FlipBack();

        PictureList[_firstRevealedPic].Revealed = false;
        PictureList[_secondRevealedPic].Revealed = false;

        PuzzleRevealedNumber = RevealedState.NoRevealed;
        CurrentGameState = GameState.NoAction;
        _corutingStarted = false;
    }
    private void LoadMaterials()
    {
        var materialFilePath = GameSetting.Instance.GetMaterialDirectionName();
        var textureFilePath = GameSetting.Instance.GetPuzzleCattegoryTextureDirectionName();
        var pairNumber = (int)GameSetting.Instance.GetPairNumber();
        const string matbaseName = "Pic";
        var firstMateriaName = "Back";
        for(var index = 1;index <= pairNumber; index++)
        {
            var currentFilePath = materialFilePath + matbaseName + index;
            Material mat  = Resources.Load(currentFilePath,typeof(Material)) as Material;
            _materialList.Add(mat);
            var currentTextureFilePath = textureFilePath + matbaseName + index;
            _texturePathList.Add(currentTextureFilePath);
        }
        _firstTexturePath = textureFilePath + firstMateriaName;
        _firstMaterial = Resources.Load(materialFilePath + firstMateriaName,typeof(Material)) as Material;
    }

    void Update()
    {
        if(CurrentGameState == GameState.DeletingPuzzles)
        {
            if(CurrentPuzzleState == PuzzleState.CanRotate)
            {
                DestroyPicture();
                CheckGameEnd();
            }
        }
        if(CurrentGameState == GameState.FlipBack)
        {
            if(CurrentPuzzleState == PuzzleState.CanRotate && !_corutingStarted)
            {
               StartCoroutine(FlipBack());
            }
        }

        if(CurrentGameState == GameState.GameEnd)
        {
            if (PictureList[_firstRevealedPic].gameObject.activeSelf == false && 
                PictureList[_secondRevealedPic].gameObject.activeSelf == false &&
                EndgamePanel.activeSelf == false)
            {
                ShowEndGameInformation();
            }
        }
    }
    private void ShowEndGameInformation()
    {
        EndgamePanel.SetActive(true);
        if (Config.IsBestScore())
        {
            NewBestScoreTxt.SetActive(true);
            YourScoreTxt.SetActive(false);
        }
        else
        {
            NewBestScoreTxt.SetActive(false);
            YourScoreTxt.SetActive(true);
        }
        var timer = _gameTimer.GetCurrentTime();
        var minutes = Mathf.Floor(timer / 60);
        var seconds = Mathf.Floor(timer % 60);
        var newTxt = minutes.ToString("00") + ":" + seconds.ToString("00");
        EndTimeTxt.GetComponent<Text>().text = newTxt;
    }
    private bool CheckGameEnd()
    {
        if(_removedPairs == _pairNumber && CurrentGameState != GameState.GameEnd)
        {
            CurrentGameState = GameState.GameEnd;
            _gameTimer.StopTimer();
            Config.PlaceScoreOnBoard(_gameTimer.GetCurrentTime());
        }
        return (CurrentGameState == GameState.GameEnd);
    }
    private void SpawnPictureMesh(int rows,int colums,Vector2 Pos,Vector2 offset,bool scaleDown)
    {
        for(int col =0;col < colums; col++)
        {
            for(int row=0;row < rows;row++)
            {
                var tmpPicture = (Picture)Instantiate(PicturePrefab,PicSpawnPosition.position, PicturePrefab.transform.rotation);
                if (scaleDown)
                {
                    tmpPicture.transform.localScale = _newScaleDown;
                }
                tmpPicture.name = tmpPicture.name + 'c' + col + 'r' + row;
                PictureList.Add(tmpPicture);
            }
        }
        ApplyTextures();
    }
    public void ApplyTextures()
    {
        var randMatindex = Random.Range(0, _materialList.Count);
        var AppliedTimes = new int[_materialList.Count];

        for (int i = 0; i < _materialList.Count; i++)
        {
            AppliedTimes[i] = 0;
        }
        foreach (var o in PictureList)
        {
            var randPrevious = randMatindex;
            var counter = 0;
            var forceMat = false;
            while (AppliedTimes[randMatindex] >= 2 || ((randPrevious == randMatindex) && !forceMat))
            {
                randMatindex = Random.Range(0,_materialList.Count);
                counter++;
                if(counter > 100)
                {
                    for(var j =0; j< _materialList.Count; j++)
                    {
                        if (AppliedTimes[j] < 2)
                        {
                            randMatindex = j;
                            forceMat = true;
                        }
                    }
                    if (!forceMat)
                        return;
                }
            }
            o.SetFirstMaterial(_firstMaterial, _firstTexturePath);
            o.ApplyFirstMaterial();
            o.SetSecondMaterial(_materialList[randMatindex], _texturePathList[randMatindex]);
            o.SetIndex(randMatindex);
            o.Revealed = false;
            AppliedTimes[randMatindex] += 1;
            forceMat = false;
        }
    }
    private void MovePicture(int rows,int colums,Vector2 pos,Vector2 offset)
    {
        var index = 0;
        for(var col =0;col < colums; col++)
        {
            for(int row=0;row < rows; row++)
            {
                var targetPosition = new Vector3((pos.x + (offset.x * row)) ,(pos.y - (offset.y *col)),0.0f);
                StartCoroutine(MoveToPosition(targetPosition, PictureList[index]));
                index++;
            }
        }
    }
    private IEnumerator MoveToPosition(Vector3 target,Picture obj)
    {
        var randomDis = 50;
        while(obj.transform.position != target)
        {
            obj.transform.position = Vector3.MoveTowards(obj.transform.position,target,randomDis * Time.deltaTime);
            yield return 0;
        }
    }
}
