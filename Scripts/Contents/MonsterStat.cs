using System.Collections;
using System.Collections.Generic;
using Data;
using UnityEngine;

public class MonsterStat : Stat
{
    [SerializeField]
	protected int _exp;
    [SerializeField]
	protected int _gold;

	Define.State _state;
	Data.Stat _monsterStat;
	Data.MonsterData _monsterData;
	Data.Item _itemData;
	MonsterController mc = null;
	int _monsterId;


	public override void Init(){
		mc = gameObject.GetComponent<MonsterController>();
		_monsterId = mc._Id;
		_defense = 5;

		SetStat(_monsterId);
	}

	public void SetStat(int monsterId)
	{
		Managers.Data.MonsterDict.TryGetValue(monsterId, out _monsterData);
		_monsterStat = _monsterData.stat;
		_hp = _monsterStat.maxHp;
		_maxHp = _monsterStat.maxHp;
		_attack = _monsterStat.attack;
		_exp = _monsterStat.totalExp;
		_moveSpeed = _monsterStat.speed;
	}

	protected override void OnDead(GameObject attacker)
	{
		PlayerStat playerStat = attacker.GetComponent<PlayerStat>();
		if (playerStat != null)
		{
            playerStat.Exp += _exp;
		}

		Data.RewardData reward = GetRandomReward();
		
		if(reward != null){
			Item rewardItem = Item.MakeItem(reward.itemId);
			int rewardGold = reward.gold + (int)(reward.gold * UnityEngine.Random.Range((float)-0.1,(float)0.1));
			playerStat.Gold += rewardGold;

			if(rewardItem.itemTemplate == Define.ItemTemplate.Equipment.ToString())
			{
				Managers.Inven.Add(rewardItem);
				Util.FindGameSceneChild("UI_Inven",true).GetComponent<UI_Inven>().RefreshUI();
			}
			else if(rewardItem.itemTemplate == Define.ItemTemplate.Consumable.ToString())
			{
				Item potion = Managers.Inven.Find(item => (item.itemId==rewardItem.itemId));
				
				if(potion != null){
					//지우고 다시 생성해야되는지 아님 이걸로 수정이 되는지 확인 필요.
					potion.itemCount ++;
					Util.FindGameSceneChild("UI_Inven",true).GetComponent<UI_Inven>().RefreshUI();
				}else{
					Managers.Inven.Add(rewardItem);
					Util.FindGameSceneChild("UI_Inven",true).GetComponent<UI_Inven>().RefreshUI();
				}
			}
		}else{
			Debug.Log("꽝");
		}

		Managers.Game.Despawn(gameObject);
	}

	RewardData GetRandomReward()
    {
        MonsterData monsterData = null;
        Managers.Data.MonsterDict.TryGetValue(_monsterId, out monsterData);
        int rand = UnityEngine.Random.Range(0, 101);

        int sum = 0;
        foreach (RewardData rewardData in monsterData.rewards)
        {
            sum += rewardData.probability;

            if (rand <= sum)
            {
                return rewardData;
            }
        }

        return null;
    }
}
