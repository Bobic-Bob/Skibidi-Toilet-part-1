using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Comic : MonoBehaviour
{

    [SerializeField] private Button _nextSlideButton;

    [Space]

    [SerializeField] private float _slideTime;

    [Space]

    [SerializeField] private List<Image> _images;

    private Coroutine _coroutine;

    public void Initialize()
    {
        if (gameObject.activeSelf && gameObject.activeInHierarchy)
        {
            if (_images.Count > 1)
            {
                for (int i = 1; i < _images.Count; i++)
                {
                    SwitchSlide(i, false);
                }
            }

            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
            }

            if (_coroutine == null)
            {
                _coroutine = StartCoroutine(AutoShowNextSlide());
            }
        }
    }

    private void SwitchSlide(int slideId, bool enabled)
    {
        _images[slideId].enabled = enabled;
    }

    private IEnumerator AutoShowNextSlide()
    {
        for (int i = 1; i < _images.Count; i++)
        {
            if (!_images[i].IsActive())
            {
                yield return new WaitForSeconds(_slideTime);
                ShowNextSlide();
            }
        }

        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
        }
    }

    public void ShowNextSlide()
    {
        for (int i = 0; i < _images.Count; i++)
        {
            if (!_images[i].IsActive())
            {
                if (i >= _images.Count - 1 && _nextSlideButton)
                {
                    _nextSlideButton.gameObject.SetActive(false);
                }
                SwitchSlide(i, true);
                break;
            }
        }
    }
}
