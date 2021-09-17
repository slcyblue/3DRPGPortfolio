using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpawningPool : MonoBehaviour
{
    [SerializeField]
    int _monsterCount = 0;
    int _reserveCount = 0;
    int _MaxMonsterCount = 20;
    int level = 1;

    [SerializeField]
    int _keepMonsterCount = 0;

    [SerializeField]
    Vector3 _spawnPos;
    [SerializeField]
    float _spawnRadius = 15.0f;
    [SerializeField]
    float _spawnTime = 10.0f;
    Animator animator;

    public void AddMonsterCount(int value) { _monsterCount += value; }
    public void SetKeepMonsterCount(int count) { _keepMonsterCount = count; }

    void Start()
    {
        Managers.Game.OnSpawnEvent -= AddMonsterCount;
        Managers.Game.OnSpawnEvent += AddMonsterCount;
    }

    void Update()
    {
        while (_reserveCount + _monsterCount < _keepMonsterCount)
        {
            if(_MaxMonsterCount>=0){
                StartCoroutine(ReserveSpawn(level));
            }else{
                level++;
                if(level > 3){
                    _keepMonsterCount = 0;
                    GameObject[] bossDoor = GameObject.FindGameObjectsWithTag("Door");
                    foreach(var door in bossDoor){
                        door.GetComponent<Animator>().Play("DoorOpen");
                    }
                    break;
                }        
                _MaxMonsterCount = 20;
                StartCoroutine(ReserveSpawn(level));
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
