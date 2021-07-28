using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : Stat
{
    [SerializeField]
    protected int _exp;
    [SerializeField]
    protected int _gold;

    public int Exp 
    {
        get { return _exp; } 
        set 
        { 
            _exp = value;

            int level = Level;
            while (true)
            {
                Data.Stat stat;
                if (Managers.Data.StatDict.TryGetValue(level + 1, out stat) == false)//flase면 다음레벨없음
                    break;
                if (_exp < stat.totalExp)//경험치부족
                    break;
                level++;
            }
            if(level != Level)
            {
                Debug.Log("levelup");
                Level = level;
                SetStat(Level);
            }
        }
    }
    public int Gold { get { return _gold; } set { _gold = value; } }

    private void Start()
    {
        _level = 1;

        _defense = 5;
        _moveSpeed = 5.0f;
        _exp = 0;
        _gold = 0;
        SetStat(_level);
    }

    public void SetStat(int level)
    {
        Dictionary<int, Data.Stat> dict = Managers.Data.StatDict;

        Data.Stat stat = dict[level];
        _hp = stat.maxHp;
        _maxHp = stat.maxHp;
        _attack = stat.attack;
        
        _defense = 5;
        _moveSpeed = 5.0f;
        
        _gold = 0;

    }
    protected override void OnDead(Stat attacker)
    {
       // Managers.Game.Despawn(gameObject);
    }
}
