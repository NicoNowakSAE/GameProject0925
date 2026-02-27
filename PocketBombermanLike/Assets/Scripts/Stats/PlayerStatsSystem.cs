using UnityEngine;

public class PlayerStatsSystem : MonoBehaviour
{
    [System.Serializable]
    public class Stats
    {
        public float Speed;
        public int JumpForce;
        public int BombCount;
        public int BombRange;
    }

    [SerializeField] private int _modifierLevels;

    [SerializeField] private Stats _playerBaseStats;
    [SerializeField] private Stats _playerMaxStats;

    public Stats GetMaxStats => _playerMaxStats;

    private Stats _playerStats;

    public Stats GetStats => _playerStats;

    private static PlayerStatsSystem _instance;
    public static PlayerStatsSystem Instance => _instance;

    public void Init()
    {
        _instance = this;
        _playerStats = new Stats();
        ClearModifiers();
    }

    public void AddModifier(StatModifier mod)
    {
        switch(mod.GetType)
        {
            case StatModifier.Type.MoveSpeed:
                AddSpeedStat(mod.GetAmount);
                break;
            case StatModifier.Type.BombCount:
                AddBombAmountStat(mod.GetAmount);
                break;
            case StatModifier.Type.BombRange:
                AddBombRangeStat(mod.GetAmount);
                break;
        }
    }

    private void AddSpeedStat(int _amount)
    {
        float upgrade = (_playerMaxStats.Speed - _playerBaseStats.Speed) / _modifierLevels;

        _playerStats.Speed += upgrade;

        if (_playerStats.Speed > _playerMaxStats.Speed)
            _playerStats.Speed = _playerMaxStats.Speed;
    }

    private void AddBombAmountStat(int _amount)
    {
        if(_playerStats.BombCount >= _playerMaxStats.BombCount)
        {
            _playerStats.BombCount = _playerMaxStats.BombCount;
            return;
        }

        _playerStats.BombCount++;
    }

    private void AddBombRangeStat(int _amount)
    {
        if (_playerStats.BombRange >= _playerMaxStats.BombRange)
        {
            _playerStats.BombRange = _playerMaxStats.BombRange;
            return;
        }

        _playerStats.BombRange++;
    }

    public void ClearModifiers()
    {
        _playerStats.Speed  = _playerBaseStats.Speed;
        _playerStats.JumpForce = _playerBaseStats.JumpForce;
        _playerStats.BombCount = _playerBaseStats.BombCount;
        _playerStats.BombRange = _playerBaseStats.BombRange;
    }
}
