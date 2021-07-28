using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager
{    
    //gameobject a=managers.resource.inistatite("path")하면 알아서 불러왕
    //MANAGERS.resource.destory(a)넣으면 날라강
    //GameObject prefab =Resources.Load<GameObject>("Prefabs/Tank");
    //요런식으로 사용하니  밑처럼 사용  
    public T Load<T>(string path) where T : Object      
    {
        

        return Resources.Load<T>(path);
    }


    //생성하는부분도 필요함
    public GameObject Instantiate(string path, Transform parent = null)
    {
        GameObject original = Load<GameObject>($"Prefabs/{path}");//로드
        if (original == null)
        {
            Debug.Log($"Failed to load prefab : {path}");
            return null;        //못찾앗엉 눈물 ㅠ
        }

        

        GameObject go = Object.Instantiate(original, parent);       //object를 안붙이면 이걸참조하니 object에 instantite를사용하세요
       
        return go;
    }

    public void Destroy(GameObject go)
    {
        if (go == null)
            return;     //널이야 눈물 ㅜ

     

        Object.Destroy(go);
    }
}
