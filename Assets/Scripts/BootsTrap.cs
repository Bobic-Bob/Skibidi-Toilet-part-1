using UnityEngine;
using YG;

public class BootsTrap : MonoBehaviour
{
    [SerializeField] private Shop[] _shops;
    [SerializeField] private Spawner _spawner;
    [SerializeField] private Player _player;
    [SerializeField] private Comic _startComic;

    private void OnEnable() => YandexGame.GetDataEvent += Initialize;

    private void OnDisable() => YandexGame.GetDataEvent -= Initialize;

    private void Initialize()
    {
        _player.Initialize();

        foreach (var shop in _shops)
        {
            shop.Initialize();
        }

        _spawner.Initialize();

        _startComic.Initialize();

        YandexGame.GameReadyAPI();
    }
}
