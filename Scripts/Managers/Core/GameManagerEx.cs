using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerEx 
{
    GameObject _player;
    HashSet<GameObject> _monsters = new HashSet<GameObject>();
    //hashSet �׳����� Ű������ ��ųʸ�

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
        BaseController bc = go.GetComponent<BaseController>(); //�����޾ƿ� �÷��̾��� ������

        //bc is PlayerController; //�÷��̾� ��Ʈ�ѷ���
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
                    if(_monsters.Contains(go))  //���Ͱ� �������ϰ�������
                    {
                        _monsters.Remove(go);   //�迭���� ����
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
