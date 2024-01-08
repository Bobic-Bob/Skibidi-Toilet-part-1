using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using YG;

public class Spawner : MonoBehaviour
{

    [Header("UI buttons")]
    [SerializeField] private GameObject _goNext;
    [SerializeField] private Toggle _dontSwitch;
    [SerializeField] private GameObject _goPrevious;

    [Space, Header("Comics after last enemy")]
    [SerializeField] private GameObject _comics;

    [Space, Header("Enemies to spawn")]
    [SerializeField] private GameObject[] _enemies;
    [SerializeField] private List<bool> _isKilledEnemies;

    private int _currentEnemy = 0;

    private void OnEnable()
    {
        Enemy.Die += SetEnemyKilled;
        Enemy.Die += SpawnNext;
    }

    private void OnDisable()
    {
        Enemy.Die -= SetEnemyKilled;
        Enemy.Die -= SpawnNext;
    }

    public void Initialize()
    {
        LoadData();

        if (!_goNext)
        {
            throw new NullReferenceException(nameof(_goNext));
        }

        if (!_dontSwitch)
        {
            throw new NullReferenceException(nameof(_dontSwitch));
        }

        if (!_goPrevious)
        {
            throw new NullReferenceException(nameof(_goPrevious));
        }

        if (!_comics)
        {
            throw new NullReferenceException(nameof(_comics));
        }

        SpawnPrevious();
        SwitchButtonsActivity();
    }

    private void LoadData()
    {
        if (_enemies.Length != YandexGame.savesData.KilledEnemies.Count)
        {
            YandexGame.savesData.KilledEnemies.Clear();

            for (int i = 0; i < _enemies.Length; i++)
            {
                _isKilledEnemies.Add(false);
                YandexGame.savesData.KilledEnemies.Add(false);
            }

            YandexGame.SaveProgress();
        }
        _isKilledEnemies = YandexGame.savesData.KilledEnemies;
    }

    public void SpawnNext()
    {
        if (_currentEnemy < _enemies.Length - 1)
        {
            if (_isKilledEnemies[_currentEnemy])
            {
                ClearChilds();
                _currentEnemy++;
                Instantiate(_enemies[_currentEnemy], gameObject.transform);
                SwitchButtonsActivity();
            }
            else
            {
                Debug.Log("Не убит прошлый чел");
            }
        }
        else
        {
            if (!_dontSwitch.isOn)
            {
                _comics.SetActive(true);
                if (_comics.GetComponentInChildren<Comic>())
                {
                    _comics.GetComponentInChildren<Comic>().Initialize();
                }
            }

            ClearChilds();
            Instantiate(_enemies[_enemies.Length - 1], gameObject.transform);
            _currentEnemy = _enemies.Length - 1;
            SwitchButtonsActivity();
        }
    }

    private void RespawnCurrent()
    {
        ClearChilds();
        Instantiate(_enemies[_currentEnemy], gameObject.transform);
        SwitchButtonsActivity();
    }

    public void SpawnPrevious()
    {
        ClearChilds();
        if (_currentEnemy > 0)
        {
            _currentEnemy--;
            Instantiate(_enemies[_currentEnemy], gameObject.transform);
            SwitchButtonsActivity();
        }
        else
        {
            Instantiate(_enemies[0], gameObject.transform);
            _currentEnemy = 0;
            SwitchButtonsActivity();
        }
    }

    public void DontSwitchEnemy()
    {
        if (_dontSwitch.isOn)
        {
            Enemy.Die -= SpawnNext;
            Enemy.Die += RespawnCurrent;
        }
        else
        {
            Enemy.Die += SpawnNext;
            Enemy.Die -= RespawnCurrent;
        }
    }

    private void SetEnemyKilled()
    {
        if (!_isKilledEnemies[_currentEnemy])
        {
            _isKilledEnemies[_currentEnemy] = true;

            YandexGame.savesData.KilledEnemies[_currentEnemy] = true;
            YandexGame.SaveProgress();
        }
    }

    private void ClearChilds()
    {
        if (gameObject.transform.childCount > 0)
        {
            for (int i = 0; i < gameObject.transform.childCount; i++)
            {
                gameObject.transform.GetChild(i).GetComponent<Enemy>().Dead();
            }
        }
    }

    private void SwitchButtonsActivity()
    {
        if (_goNext)
        {
            if (_currentEnemy == _enemies.Length - 1)
            {
                _goNext.SetActive(false);
            }
            else
            {
                if (_isKilledEnemies[_currentEnemy])
                {
                    _goNext.SetActive(true);
                }
                else
                {
                    _goNext.SetActive(false);
                }
            }
        }

        if (_goPrevious)
        {
            if (_currentEnemy == 0)
            {
                _goPrevious.SetActive(false);
            }
            else
            {
                _goPrevious.SetActive(true);
            }
        }
    }
}
