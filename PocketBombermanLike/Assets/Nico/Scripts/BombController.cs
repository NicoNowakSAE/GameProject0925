using System.Collections.Generic;
using UnityEngine;

public class BombController : MonoBehaviour
{
    [SerializeField]
    private GameObject _bombPrefab;

    private int _curBombCount = 0;

    private int _bombPoolSize;

    private List<Bomb> _bombPool = new List<Bomb>();

    PlayerStatsSystem.Stats _stats;

    public void Init(PlayerStatsSystem.Stats stats, int maxbombs)
    {
        _stats = stats;
        _bombPoolSize = maxbombs;
        InitialiseBombPool();
    }

    private void InitialiseBombPool()
    {
        GameObject bombcontainer = new GameObject("BombContainer");

        for (int i = 0; i < _bombPoolSize; i++)
        {
            Bomb b = Instantiate(_bombPrefab, this.transform.position, Quaternion.identity).GetComponent<Bomb>();
            if (b == null)
            {
                Destroy(b);
                continue;
            }

            b.transform.parent = bombcontainer.transform;
            b.Init(DespawnBomb);
            b.gameObject.SetActive(false);
            _bombPool.Add(b);
        }
    }

    public void SpawnBomb()
    {
        for (int i = 0; i < _bombPool.Count; i++)
        {
            if (_bombPool[i].gameObject.activeSelf == false)
            {
                _bombPool[i].gameObject.SetActive(true);
                _bombPool[i].Spawn(this.transform.position, _stats.BombRange);
                _curBombCount++;
                return;
            }
        }
    }

    public void DespawnBomb(Bomb b)
    {
        b.gameObject.SetActive(false);
        _curBombCount--;
    }

    void Update()
    {
        // TODO: Change Input to PlayerInput
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (_curBombCount >= _stats.BombCount)
                return;

            SpawnBomb();
        }
    }

}
