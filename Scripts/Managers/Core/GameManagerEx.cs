using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerEx 
{
    GameObject _player;
    HashSet<GameObject> _monsters = new HashSet<GameObject>();
    //hashSet 그냥집합 키가없는 딕셔너리

    public Action<int> OnSpawnEvent;
    public GameObject GetPlayer() { return _player; }
    public GameObject Spawn(Define.WorldObject type,string path=null,Transform parent =null)
    {
        GameObject go= Managers.Resource.Instantiate(path, parent);

        switch (type)
        {
            case Define.WorldObject.Monster:
                _monsters.Add(go);
                if (OnSpawnEvent != null)
                    OnSpawnEvent.Invoke(1);
                break;
            case Define.WorldObject.Player:
                _player = go;
                break;
        }

        return go;
        
    }
    public Define.WorldObject GetWorldObjectType(GameObject go)
    {
        BaseController bc = go.GetComponent<BaseController>(); //정보받아옴 플레이언지 몬스턴지

        //bc is PlayerController; //플레이어 컨트롤런지
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
                    if(_monsters.Contains(go))  //몬스터가 고를포함하고잇으면
                    {
                        _monsters.Remove(go);   //배열에서 제거
                        if (OnSpawnEvent != null)
                            OnSpawnEvent.Invoke(0);
                    }    
                }
                break;

            case Define.WorldObject.Player:
                if(_player == go)
                {
                    _player = null;
                }
                break;
        }
        Managers.Resource.Destroy(go);
    }



}
