using UnityEngine;
using UnityEngine.UI;
using YG;

public enum ButtonTypes
{
    Music,
    Sound
}


[RequireComponent(typeof(Button))]
public class ButtonSpriteSwitch : MonoBehaviour
{

    [Header("Button settings")]
    [SerializeField] private Button _button;
    [SerializeField] private ButtonTypes _buttonType;

    [Space, Header("Graphics")]

    [SerializeField] private Graphic _activeSprite;
    [SerializeField] private Graphic _inactiveSprite;

    private void OnValidate()
    {
        _button ??= GetComponent<Button>();
        _activeSprite ??= _button.targetGraphic;
    }

    private void Start()
    {
        LoadData();

        if (!_activeSprite || !_inactiveSprite)
        {
            throw new System.NullReferenceException();
        }
    }

    private void LoadData()
    {
        switch (_buttonType)
        {
            case ButtonTypes.Music:
                if (YandexGame.savesData.Music)
                {
                    _inactiveSprite.enabled = false;
                    _button.targetGraphic = _activeSprite;
                }
                else
                {
                    _inactiveSprite.enabled = true;
                    _button.targetGraphic = _inactiveSprite;
                }
                break;

            case ButtonTypes.Sound:
                if (YandexGame.savesData.Sound)
                {
                    _inactiveSprite.enabled = false;
                    _button.targetGraphic = _activeSprite;
                }
                else
                {
                    _inactiveSprite.enabled = true;
                    _button.targetGraphic = _inactiveSprite;
                }
                break;
        }
    }

    public void SpriteSwitch()
    {
        if (_button.targetGraphic == _activeSprite)
        {
            _inactiveSprite.enabled = true;
            _button.targetGraphic = _inactiveSprite;
        }
        else
        {
            _inactiveSprite.enabled = false;
            _button.targetGraphic = _activeSprite;
        }
    }
}
