using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpawningPool: MonoBehaviour
{
    [SerializeField]
    int _monsterCount = 0;
    int _reserveCount = 0; //코루틴 예약 숫자
    [SerializeField]
    int _keepMonsterCount = 0;
    [SerializeField]
    Vector3 _spawnPos;
    [SerializeField]
    float _spawnRadius = 15.0f;
    [SerializeField]
    float _spawnTime = 5.0f;
    // Start is called before the first frame update

    public void AddMonsterCount(int value)
    {
        _monsterCount += value;
    }
    public void SetKeepMonsterCount(int count)
    {
        _keepMonsterCount = count;
    }
    void Start()
    {
        Managers.Game.OnSpawnEvent -= AddMonsterCount;
        Managers.Game.OnSpawnEvent += AddMonsterCount;
    }

    // Update is called once per frame
    void Update()
    {
        while(_reserveCount+_monsterCount< _keepMonsterCount)
        {
            StartCoroutine("ReserveSpawn");
        }
    }
    IEnumerator ReserveSpawn()//코루틴 시간기다렷다가 실행
    {
        _reserveCount++; //예약갯수 +++
        yield return new WaitForSeconds(Random.Range(0, _spawnTime));//min도 포함 max도포함
       GameObject obj= Managers.Game.Spawn(Define.WorldObject.Monster, "Knight");
        NavMeshAgent nma = obj.GetOrAddComponent<NavMeshAgent>();
        
        Vector3 randPos;//랜덤한위치 가지못하는대뺴고
        while (true)
        {
            Vector3 randDir = Random.insideUnitSphere * Random.Range(0,_spawnRadius);//구에서 랜덤좌표뽑아옴
            randDir.y = 0;
            randPos = _spawnPos + randDir;

            //갈수있는지
            NavMeshPath path= new NavMeshPath();
            if (nma.CalculatePath(randPos, path))
                break;
        }

        obj.transform.position = randPos;
        _reserveCount--; //예약끝

       
    }
}
