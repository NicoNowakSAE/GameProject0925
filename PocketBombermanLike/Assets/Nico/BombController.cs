using System.Collections.Generic;
using UnityEngine;

public class BombController : MonoBehaviour
{
    [SerializeField]
    private GameObject _bombPrefab;

    [SerializeField] private int _maxBombCount;
    private int _curBombCount = 0;

    private List<Bomb> _bombPool = new List<Bomb>();
    private void Awake()
    {
        GameObject bombcontainer = new GameObject("BombContainer");

        for (int i = 0; i < 5; i++)
        {
            Bomb b = Instantiate(_bombPrefab, this.transform.position, Quaternion.identity).GetComponent<Bomb>();
            if (b == null)
                return;

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
                _bombPool[i].Spawn(this.transform.position);
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
            if (_curBombCount >= _maxBombCount)
                return;

            SpawnBomb();
        }
    }

}
