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
        /* ui를 움직익에하게한다 ㅎㅎ
        GameObject go=GetButton((int)Buttons.PointButton).gameObject;//버튼
        UI_EventHandler evt = go.GetComponent<UI_EventHandler>();//이벤트
        evt.OnDragHandler += ((PointerEventData data) => { evt.gameObject.transform.position = data.position; });
        */
        GameObject go = GetButton((int)Buttons.PointButton).gameObject;//버튼
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
           
            GetText((int)Texts.ScoreText).text = ((int)scene2time).ToString(); //int 형으로바꾸면 순서로받아서 1
           
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
    void Bind<T>(Type type) where T:UnityEngine.Object      //Util에 조건이 걸렸으니 여기도 걸어야됨 ㅎㅎ
    {
        string[] names = Enum.GetNames(type);
        UnityEngine.Object[] objects = new UnityEngine.Object[names.Length];
        _objects.Add(typeof(T), objects);

        for(int i = 0; i < names.Length; i++)
        {
           if(typeof(T) == typeof(GameObject))  //t에 타입이 게임오브젝트랑 같다면
                objects[i] = Util.FindChild(gameObject, names[i], true);
           else                   //일반 컴퍼넌트 게임오브젝트는 컴퍼넌트가아님
                objects[i] = Util.FindChild<T>(gameObject, names[i], true);
            if (objects[i] == null)
                Debug.Log($"Failed to bind{names[i]}");
        }
    }
    T Get<T>(int idx) where T:UnityEngine.Object        //꺼내쓸꼬양 제너릭형식
    {
        UnityEngine.Object[] objects = null;
        if (_objects.TryGetValue(typeof(T), out objects) == false)  //trygetvalue 키값으로 출력 typeofT가 키 ojbects를 가져온다 
            return null;    //없다면 null

        return objects[idx] as T;   //t로 캐스팅 unityengine.object가 아닌 t타입으로
    }
    Text GetText(int idx)   
    {
        return Get<Text>(idx);//get 함수를 이용해 딕셔너리에 있는 idx키값을이용해 value를 찾아오세요 
    }
    Button GetButton(int idx)
    {
        return Get<Button>(idx);//get 함수를 이용해 딕셔너리에 있는 idx키값을이용해 value를 찾아오세요 
    }
    Image GetImage(int idx)
    {
        return Get<Image>(idx);//get 함수를 이용해 딕셔너리에 있는 idx키값을이용해 value를 찾아오세요 
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
        Time.timeScale = 0;//일시정지
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
