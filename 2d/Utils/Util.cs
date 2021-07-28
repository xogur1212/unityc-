using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Util 
{
    public static T GetOrAddComponent<T>(GameObject go)where T:UnityEngine.Component    //������Ʈ ã�� ������ �߰�
    {
        T component = go.GetComponent<T>();
        if (component == null)
            component = go.AddComponent<T>();
        return component;
    }

    public static GameObject FindChild(GameObject go, string name = null, bool recursive = false) 
    {
        Transform transform =FindChild<Transform>(go, name, recursive);   //transform�� ã�Ƽ� ������ 
        if (transform == null)
            return null;
        return transform.gameObject;        //�̷��� ���ӿ�����Ʈ ã�Ƽ� ������
    }
    //gameobject�� ���۳�Ʈ���ƴϴ�
    public static T FindChild<T>(GameObject go,string name=null, bool recursive=false) where T: UnityEngine.Object
    {
        if (go == null)     //���ӿ�����Ʈ�� ������
            return null;

        if (recursive == false)
        {
            for(int i = 0; i < go.transform.childCount; i++)    //���ϵ嵹�鼭 ã�Ƣa
            {
               Transform transform =go.transform.GetChild(i);
               if(string.IsNullOrEmpty(name) || transform.name == name) //���࿡ name�� �Է°��̾��ų� transform�� �̸��� �������
                {
                    T component =transform.GetComponent<T>();        // T���� ���۳�Ʈ�� ���۳�Ʈ������
                    if (component != null)      //���̾ƴϸ� ������~
                        return component;
                }
            }
         
        }
        else
        {
            foreach (T component in go.GetComponentsInChildren<T>()) //���� ã����� Ÿ���� �ڽ��� ���۳�Ʈ�� ��ĵ��
            {
                if (string.IsNullOrEmpty(name) || component.name == name)    //�̸��� �Է¾�������   ||����ã������� ã������
                    return component;
            }
        }
        return null;
    }
}
