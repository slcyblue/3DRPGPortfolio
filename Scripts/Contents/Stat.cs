using System.Collections;
using System.Collections.Generic;
using Data;
using UnityEngine;

public class Stat : MonoBehaviour
{
    [SerializeField]
    protected int _level;
    [SerializeField]
    protected int _hp;
    protected int _maxHp;
    protected int _attack;
    protected int _defense;
    [SerializeField]
    protected float _moveSpeed;

    public int Level { get { return _level; } set { _level = value; } }
    public int Hp { get { return _hp; } set { _hp = value; } }
    public int MaxHp { get { return _maxHp; } set { _maxHp = value; } }
    public int Attack { get { return _attack; } set { _attack = value; } }
    public int Defense { get { return _defense; } set { _defense = value; } }
    public float MoveSpeed { get { return _moveSpeed; } set { _moveSpeed = value; } }

    private void Start() {
        Init();
    }

    public virtual void Init(){ }
    
    public virtual void OnAttacked(GameObject attacker)
    {
        Stat attackerStat = attacker.GetComponent<Stat>();
		int damage = Mathf.Max(0, attackerStat.Attack - Defense);
		Hp -= damage;
        if (Hp <= 0)
        {
            Hp = 0;
            OnDead(attacker);
        }
    }

    protected virtual void OnDead(GameObject attacker)
    {
        PlayerStat playerStat = attacker.GetComponent<PlayerStat>();
		if (playerStat != null)
		{
            playerStat.Exp += 15;
		}
        Managers.Game.Despawn(gameObject);
    }

    public virtual void OnSkilled(GameObject attacker, Skill skill)
    {
		int damage = Mathf.Max(0, skill.skillDmg - Defense);
		Hp -= damage;
        if (Hp <= 0)
        {
            Hp = 0;
            OnDead(attacker);
        }
    }
}
