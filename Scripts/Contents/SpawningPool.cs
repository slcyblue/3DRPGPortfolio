using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpawningPool : MonoBehaviour
{
    [SerializeField]
    int _monsterCount = 0;
    [SerializeField]
    int _reserveCount = 0;
    [SerializeField]
    int _MaxMonsterCount = 10;
    [SerializeField]
    int level = 1;

    [SerializeField]
    int _keepMonsterCount = 0;

    [SerializeField]
    Vector3 _spawnPos;
    [SerializeField]
    float _spawnRadius = 15.0f;
    [SerializeField]
    float _spawnTime = 5.0f;
    Animator animator;

    public void AddMonsterCount(int value) { _monsterCount += value; }
    public void SetKeepMonsterCount(int count) { _keepMonsterCount = count; }

    void Start()
    {
        Managers.Game._monsters.Clear();
        Managers.Game.OnSpawnEvent -= AddMonsterCount;
        Managers.Game.OnSpawnEvent += AddMonsterCount;
    }

    void Update()
    {
        while (_reserveCount + _monsterCount < _keepMonsterCount)
        {
            if(_MaxMonsterCount>0){
                StartCoroutine(ReserveSpawn(level));
            }else{
                if(Managers.Game._monsters.Count>0)
                    break;
                else{
                    level++;
                    if(level > 3){
                        if(Managers.Game._monsters.Count>0)
                            break;
                        
                        _keepMonsterCount = 0;
                        GameObject bossDoor = GameObject.FindGameObjectWithTag("Door");
                        bossDoor.transform.GetChild(2).gameObject.SetActive(false);
                        bossDoor.GetComponent<Animator>().Play("DoorOpen");
                        break;
                    }        
                    _MaxMonsterCount = 10;
                    StartCoroutine(ReserveSpawn(level));
                }
            }
        }
    }

    IEnumerator ReserveSpawn(int _level)
    {
        _reserveCount++;
        yield return new WaitForSeconds(Random.Range(0, _spawnTime));
        
        GameObject obj = null;
        
        switch(_level){
            case 1:
                obj = Managers.Game.Spawn(Define.WorldObject.Monster, 400);    
                break;
            case 2:
                obj = Managers.Game.Spawn(Define.WorldObject.Monster, 401);    
                break;
            case 3:
                obj = Managers.Game.Spawn(Define.WorldObject.Monster, 402);    
                break;
        }
        
        NavMeshAgent nma = obj.GetOrAddComponent<NavMeshAgent>();

        Vector3 randPos;
        while (true)
        {
            Vector3 randDir = Random.insideUnitSphere * Random.Range(0, _spawnRadius);
			//randDir.y = 0;
			randPos = _spawnPos + randDir;

            // 갈 수 있나
            NavMeshPath path = new NavMeshPath();
            if (nma.CalculatePath(randPos, path))
                break;
		}

        obj.transform.position = randPos;
        _reserveCount--;
        _MaxMonsterCount--;
    }
}
