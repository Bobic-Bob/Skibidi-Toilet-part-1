using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class ShowCurrency : MonoBehaviour
{
    [SerializeField] private Currencies _currency;
    [SerializeField] private TextMeshProUGUI _text;

    private void OnValidate()
    {
        _text ??= GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        Wallet.ShowCurrency += ShowCurrentCurrency;
    }

    private void OnDisable()
    {
        Wallet.ShowCurrency -= ShowCurrentCurrency;
    }

    private void ShowCurrentCurrency(Currencies currencyName, int currency)
    {
        if (_currency == currencyName)
        {
            if (_text != null)
            {
                _text.text = $"{currency}";
            }
        }
    }
}
