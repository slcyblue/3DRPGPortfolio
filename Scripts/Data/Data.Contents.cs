using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{ 
	#region Stat
	[Serializable]
	public class Stat
	{
		public int level;
		public int maxHp;
		public int attack;
		public int defense;
		public float speed;
		public int totalExp;
	}


	[Serializable]
	public class StatData : ILoader<int, Stat>
	{
		public List<Stat> stats = new List<Stat>();

		public Dictionary<int, Stat> MakeDict()
		{
			Dictionary<int, Stat> dict = new Dictionary<int, Stat>();
			foreach (Stat stat in stats)
				dict.Add(stat.level, stat);
			return dict;
		}
	}
	#endregion

	#region Skill
	[Serializable]
	public class Skill{
		public int skillId;
		public string skillName;
		public string skillType;
		public int skillDmg;
		public int skillMp;
		public int skillCool;
		public int skillSlot;
		public float skillRange;
		public float skillDuration;
		public string skillPath;
		public string skillInput;
	}

	[Serializable]
	public class SkillData : ILoader<int, Skill>
	{
		public List<Skill> skills = new List<Skill>();

		public Dictionary<int, Skill> MakeDict()
		{
			Dictionary<int, Skill> dict = new Dictionary<int, Skill>();
			foreach (Skill skill in skills)
				dict.Add(skill.skillId, skill);
			return dict;
		}
	}
	#endregion
	
	#region Item
	[Serializable]
	public class Item{
		public int itemId;
		public string itemPath;
		public string itemName;
		public string itemTemplate;
		public string itemType;
		public string itemOption;
		public int itemCount;
		public int itemDmg;
		public int itemMp;
		public int itemDefense;
		public int itemHp;
		public int itemSlot;
		public int itemEndurance;
		public int itemEnhance;
		public int itemMaxEnhance;
		public bool itemTransaction;
		public int itemPrice;
	}

	[Serializable]
	public class ItemData : ILoader<int, Item>
	{
		public List<Item> items = new List<Item>();

		public Dictionary<int, Item> MakeDict()
		{
			Dictionary<int, Item> dict = new Dictionary<int, Item>();
			foreach (Item item in items)
				dict.Add(item.itemId, item);
			return dict;
		}
	}
	#endregion

	#region Monster
	[Serializable]
	public class RewardData{
		public int probability;
		public int itemId;
		public int count;
		public int gold;
	}
	
	[Serializable]
	public class MonsterData
	{
		public int monsterId;
		public string monsterName;
		public Stat stat;
		public List<RewardData> rewards;
		public string prefabPath;
	}


	[Serializable]
	public class MonsterLoader : ILoader<int, MonsterData>
	{
		public List<MonsterData> monsters = new List<MonsterData>();

		public Dictionary<int, MonsterData> MakeDict()
		{
			Dictionary<int, MonsterData> dict = new Dictionary<int, MonsterData>();
			foreach (MonsterData monster in monsters)
				dict.Add(monster.monsterId, monster);
			return dict;
		}
	}
	#endregion

	#region Npc
	[Serializable]
	public class NpcData
	{
		public int npcId;
		public string npcType;
		public string npcName;
		public List<Item> products;
	}


	[Serializable]
	public class NpcLoader : ILoader<int, NpcData>
	{
		public List<NpcData> npcs = new List<NpcData>();

		public Dictionary<int, NpcData> MakeDict()
		{
			Dictionary<int, NpcData> dict = new Dictionary<int, NpcData>();
			foreach (NpcData npc in npcs)
				dict.Add(npc.npcId, npc);
			return dict;
		}
	}
	#endregion
	#region Enhance
	[Serializable]
	public class Enhance{
		public int itemEnhance;
		public int nextEnhance;
		public int probability;
		public int increasedDmg;
		public int increasedDef;
		public int increasedHp;
		public int increasedMp;
	}

	[Serializable]
	public class EnhanceData : ILoader<int, Enhance>
	{
		public List<Enhance> enhances = new List<Enhance>();

		public Dictionary<int, Enhance> MakeDict()
		{
			Dictionary<int, Enhance> dict = new Dictionary<int, Enhance>();
			foreach (Enhance enhance in enhances)
				dict.Add(enhance.itemEnhance, enhance);
			return dict;
		}
	}
	#endregion
}