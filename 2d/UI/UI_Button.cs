using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_Button : MonoBehaviour
{

    Dictionary<Type, UnityEngine.Object[]> _objects = new Dictionary<Type, UnityEngine.Object[]>();
    private float startTime;
    private float scene1time;
    private float scene2time;
    GameObject t1;
    TransferMap t11;

    enum Buttons
    {
        PointButton
    }
    enum Texts
    {
        PointText,
        ScoreText
    }
    enum GameObjects
    {
        TestObject,
    }
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);
        Bind<Button>(typeof(Buttons));
        Bind<Text>(typeof(Texts));
        Bind<GameObject>(typeof(GameObjects));
        startTime=Time.time;
        /* ui�� �����Ϳ��ϰ��Ѵ� ����
        GameObject go=GetButton((int)Buttons.PointButton).gameObject;//��ư
        UI_EventHandler evt = go.GetComponent<UI_EventHandler>();//�̺�Ʈ
        evt.OnDragHandler += ((PointerEventData data) => { evt.gameObject.transform.position = data.position; });
        */
        GameObject go = GetButton((int)Buttons.PointButton).gameObject;//��ư
        AddUIEvent(go, (PointerEventData data) => {
            getTime(); }, Define.UIEvent.Click);
    }




    // Update is called once per frame
    void Update()
    {
        scene2time = (Time.time - startTime)*6;
        t1 = GameObject.Find("TransferPoint");
        t11 = Util.GetOrAddComponent<TransferMap>(t1);

        if (SceneManager.GetActiveScene().name == "scene2")
        {
           
            GetText((int)Texts.ScoreText).text = ((int)scene2time).ToString(); //int �����ιٲٸ� �����ι޾Ƽ� 1
           
        }
        else if(SceneManager.GetActiveScene().name=="scene1")
        {
            

            if (t11 != null)
            {
                scene1time = Time.time - t11.getscene1starttime() + t11.getloadscene2time();
                GetText((int)Texts.ScoreText).text = ((int)scene1time).ToString(); ;
               // Debug.Log("i find transferpoint");
            }
            
        }
    }
    void Bind<T>(Type type) where T:UnityEngine.Object      //Util�� ������ �ɷ����� ���⵵ �ɾ�ߵ� ����
    {
        string[] names = Enum.GetNames(type);
        UnityEngine.Object[] objects = new UnityEngine.Object[names.Length];
        _objects.Add(typeof(T), objects);

        for(int i = 0; i < names.Length; i++)
        {
           if(typeof(T) == typeof(GameObject))  //t�� Ÿ���� ���ӿ�����Ʈ�� ���ٸ�
                objects[i] = Util.FindChild(gameObject, names[i], true);
           else                   //�Ϲ� ���۳�Ʈ ���ӿ�����Ʈ�� ���۳�Ʈ���ƴ�
                objects[i] = Util.FindChild<T>(gameObject, names[i], true);
            if (objects[i] == null)
                Debug.Log($"Failed to bind{names[i]}");
        }
    }
    T Get<T>(int idx) where T:UnityEngine.Object        //���������� ���ʸ�����
    {
        UnityEngine.Object[] objects = null;
        if (_objects.TryGetValue(typeof(T), out objects) == false)  //trygetvalue Ű������ ��� typeofT�� Ű ojbects�� �����´� 
            return null;    //���ٸ� null

        return objects[idx] as T;   //t�� ĳ���� unityengine.object�� �ƴ� tŸ������
    }
    Text GetText(int idx)   
    {
        return Get<Text>(idx);//get �Լ��� �̿��� ��ųʸ��� �ִ� idxŰ�����̿��� value�� ã�ƿ����� 
    }
    Button GetButton(int idx)
    {
        return Get<Button>(idx);//get �Լ��� �̿��� ��ųʸ��� �ִ� idxŰ�����̿��� value�� ã�ƿ����� 
    }
    Image GetImage(int idx)
    {
        return Get<Image>(idx);//get �Լ��� �̿��� ��ųʸ��� �ִ� idxŰ�����̿��� value�� ã�ƿ����� 
    }
    int _score = 0;

    public void OnButtonClicked()
    {
        _score++;
    }

    public string GetCurrentDate()
    {
        return DateTime.Now.ToString();
    }

    public static void AddUIEvent(GameObject go,Action<PointerEventData> action, Define.UIEvent type = Define.UIEvent.Click)
    {
        UI_EventHandler evt= Util.GetOrAddComponent<UI_EventHandler>(go);

        switch (type)
        {
            case Define.UIEvent.Click:
                evt.OnClickHandler -= action;
                evt.OnClickHandler += action; 
                break;
            case Define.UIEvent.Drag:
                evt.OnDragHandler -= action;
                evt.OnDragHandler += action;
                break;
        }
        
    }

    public void getTime()
    {
        Time.timeScale = 0;//�Ͻ�����
    }

    public float Getscene1time()
    {
        return scene1time;
    }
    public float Getscene2time()
    {
        return scene2time;
    }
}
