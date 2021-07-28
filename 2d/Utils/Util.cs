using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Util 
{
    public static T GetOrAddComponent<T>(GameObject go)where T:UnityEngine.Component    //컴포넌트 찾고 없으면 추가
    {
        T component = go.GetComponent<T>();
        if (component == null)
            component = go.AddComponent<T>();
        return component;
    }

    public static GameObject FindChild(GameObject go, string name = null, bool recursive = false) 
    {
        Transform transform =FindChild<Transform>(go, name, recursive);   //transform을 찾아서 던져줌 
        if (transform == null)
            return null;
        return transform.gameObject;        //이렇게 게임오브젝트 찾아서 던져줌
    }
    //gameobject는 컴퍼넌트가아니다
    public static T FindChild<T>(GameObject go,string name=null, bool recursive=false) where T: UnityEngine.Object
    {
        if (go == null)     //게임오브젝트가 없을때
            return null;

        if (recursive == false)
        {
            for(int i = 0; i < go.transform.childCount; i++)    //차일드돌면서 찾아줭
            {
               Transform transform =go.transform.GetChild(i);
               if(string.IsNullOrEmpty(name) || transform.name == name) //만약에 name에 입력값이없거나 transform에 이름이 같을경우
                {
                    T component =transform.GetComponent<T>();        // T형의 컴퍼넌트에 컴퍼넌트가져옴
                    if (component != null)      //널이아니면 리턴행~
                        return component;
                }
            }
         
        }
        else
        {
            foreach (T component in go.GetComponentsInChildren<T>()) //내가 찾고싶은 타입의 자식의 컴퍼넌트를 스캔함
            {
                if (string.IsNullOrEmpty(name) || component.name == name)    //이름을 입력안햇을떄   ||내가찾고싶은걸 찾앗을떄
                    return component;
            }
        }
        return null;
    }
}
