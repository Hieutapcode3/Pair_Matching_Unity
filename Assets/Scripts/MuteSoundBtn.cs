using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MuteSoundBtn : MonoBehaviour
{
    public Sprite UnmutedFxSprite;
    public Sprite MutedFxSprite;

    private Button _btn;
    private SpriteState _state;

    void Start()
    {
        _btn = GetComponent<Button>();
        if(GameSetting.Instance.IsSoundEffectMuted())
        {
            _state.pressedSprite = MutedFxSprite;
            _state.highlightedSprite = MutedFxSprite;
            _btn.GetComponent<Image>().sprite = MutedFxSprite;
        }
        else
        {
            _state.pressedSprite = UnmutedFxSprite;
            _state.highlightedSprite = UnmutedFxSprite;
            _btn.GetComponent<Image>().sprite = UnmutedFxSprite;
        }
    }
    private void OnGUI()
    {
        if (GameSetting.Instance.IsSoundEffectMuted())
        {
            _btn.GetComponent<Image>().sprite = MutedFxSprite;
        }
        else
            _btn.GetComponent<Image>().sprite = UnmutedFxSprite;

    }
    public void ToggleFxIcon()
    {
        if (GameSetting.Instance.IsSoundEffectMuted())
        {
            _state.pressedSprite = UnmutedFxSprite;
            _state.highlightedSprite= UnmutedFxSprite;
            GameSetting.Instance.MuteSoundEffect(false);
        }
        else
        {
            _state.pressedSprite = MutedFxSprite;
            _state.highlightedSprite = MutedFxSprite;
            GameSetting.Instance.MuteSoundEffect(true);
        }
        _btn.spriteState = _state;
    }

  
}
