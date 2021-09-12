using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerEx
{
    GameObject _player;
    //Dictionary<int, GameObject> _players = new Dictionary<int, GameObject>();
    HashSet<GameObject> _monsters = new HashSet<GameObject>();

    public Action<int> OnSpawnEvent;

    public GameObject GetPlayer() { return _player; }


    public GameObject Spawn(Define.WorldObject type, int Id, Transform parent = null)
    {
        Data.MonsterData monsterData = null;
        Data.NpcData npcData = null;
        GameObject go = null;

        switch (type)
        {
            case Define.WorldObject.Monster:
                Managers.Data.MonsterDict.TryGetValue(Id, out monsterData);
                go = Managers.Resource.Instantiate(monsterData.monsterName, parent);
                go.GetComponent<BaseController>().Init(Id);

                _monsters.Add(go);
                if (OnSpawnEvent != null)
                    OnSpawnEvent.Invoke(1);
                break;
            case Define.WorldObject.Player:
                go = Managers.Resource.Instantiate("Arthur", parent);
                if(!go.GetComponent<PlayerController>())
                    go.GetOrAddComponent<PlayerController>();
                _player = go;
                break;
            case Define.WorldObject.Npc:
                Managers.Data.NpcDict.TryGetValue(Id, out npcData);
                go = Managers.Resource.Instantiate(npcData.npcName, parent);
                go.GetOrAddComponent<NpcController>().Init(Id);
                break;
        }

        return go;
    }

    public Define.WorldObject GetWorldObjectType(GameObject go)
    {
        BaseController bc = go.GetComponent<BaseController>();
        if (bc == null)
            return Define.WorldObject.Unknown;

        return bc.WorldObjectType;
    }

    public void Despawn(GameObject go)
    {
        Define.WorldObject type = GetWorldObjectType(go);

        switch (type)
        {
            case Define.WorldObject.Monster:
                {
                    if (_monsters.Contains(go))
                    {
                        _monsters.Remove(go);
                        if (OnSpawnEvent != null)
							OnSpawnEvent.Invoke(-1);
					}   
                }
                break;
            case Define.WorldObject.Player:
                {
					if (_player == go)
						_player = null;
				}
                break;
        }

        Managers.Resource.Destroy(go);
    }
}
