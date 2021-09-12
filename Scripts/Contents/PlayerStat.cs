using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : Stat
{
    [SerializeField]
	protected int _exp;
    [SerializeField]
	protected int _gold;

	protected int _equipHp;
	protected int _equipAttack;
	protected int _equipDefense;
	protected int _statHp;
	protected int _statAttack;
	protected int _statDefense;
	
	public Data.Stat stat;

	public int Exp 
	{ 
		get { return _exp; } 
		set 
		{ 
			_exp = value;

			int level = 1;
			while (true)
			{
				if (Managers.Data.StatDict.TryGetValue(level + 1, out stat) == false)
					break;
				if (_exp < stat.totalExp)
					break;
				level++;
			}

			if (level != Level)
			{
				Debug.Log("Level Up!");
				Level = level;
				SetStat(Level);
			}
		}
	}

	public int Gold { get { return _gold; } 
		set { 
		_gold = value; 
		
		} 
	}

	private void Start()
	{
		_level = 1;
		_exp = 0;
		_moveSpeed = 5.0f;
		_gold = 0;

		SetStat(_level);
	}
	public void PlusEquipStat(Item item){
		_equipHp += item.itemHp;
		_equipAttack += item.itemDmg;
		_equipDefense += item.itemDefense;
		UpdateStat();
	}

	public void AbsEquipStat(Item item){
		_equipHp -= item.itemHp;
		_equipAttack -= item.itemDmg;
		_equipDefense -= item.itemDefense;
		UpdateStat();
	}

	public void SetStat(int level)
	{
		Dictionary<int, Data.Stat> dict = Managers.Data.StatDict;
		Data.Stat stat = dict[level];
		_hp = stat.maxHp;
		_statHp = stat.maxHp;
		_statAttack = stat.attack;
		_statDefense = stat.defense;
		UpdateStat();
	}

	private void UpdateStat() {
		_maxHp = _statHp + _equipHp;
		_attack = _statAttack + _equipAttack;
		_defense = +_statDefense + _equipDefense;
	}

	protected override void OnDead(GameObject attacker)
	{
		Managers.Game.GetPlayer().GetComponent<PlayerController>().State = Define.State.Die;
	}
}
