using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour           //모든 매니저를 총괄하는 친구  
                                                //monobehaviour https://skuld2000.tistory.com/25
{
    static Managers s_instance; // 유일성이 보장된다
    static Managers Instance { get { Init(); return s_instance; } } // 유일한 매니저를 갖고온다 싱글톤

    #region Contents

    GameManagerEx _game = new GameManagerEx();
    public static GameManagerEx Game { get { return Instance._game; } }
    #endregion

    #region Core  
    //https://nsstbg.tistory.com/5 Visual Basic 파일에서 코드 섹션을 축소하고 숨깁니다. https://docs.microsoft.com/ko-kr/dotnet/visual-basic/language-reference/directives/region-directive

    DataManager _data = new DataManager();  //데이터관련
    InputManager _input = new InputManager();   //인풋관련 1번 키입력받았을떄
    PoolManager _pool = new PoolManager();  //풀관련
    ResourceManager _resource = new ResourceManager(); //리소스관련  2번 프리팹있는걸 생성처럼 관리해주는곳 instantiate해주는곳
    SceneManagerEx _scene = new SceneManagerEx(); //장면관련
    SoundManager _sound = new SoundManager();   //사운드관련
    UIManager _ui = new UIManager(); //ui관련

    public static DataManager Data { get { return Instance._data; } }       //싱글톤 
    public static InputManager Input { get { return Instance._input; } }
    public static PoolManager Pool { get { return Instance._pool; } }
    public static ResourceManager Resource { get { return Instance._resource; } }
    public static SceneManagerEx Scene { get { return Instance._scene; } }
    public static SoundManager Sound { get { return Instance._sound; } }
    public static UIManager UI { get { return Instance._ui; } }

    #endregion

    void Start()        //시작할떄 
    {
        Init();            //init 호출
	}

    void Update()           //업데이트
    {
        _input.OnUpdate();          //키보드체크를 매니저에서 해줌 ㅎㅎ
    }

    static void Init()
    {
        if (s_instance == null)
        {
			GameObject go = GameObject.Find("@Managers");   //매니저찾아와
            if (go == null)
            {
                go = new GameObject { name = "@Managers" }; //없으면만들어줭
                go.AddComponent<Managers>();    //추가해줭
            }

            DontDestroyOnLoad(go);
            s_instance = go.GetComponent<Managers>();

            s_instance._data.Init();
            s_instance._pool.Init();
            s_instance._sound.Init();
        }		
	}

    public static void Clear()
    {
        Input.Clear();
        Sound.Clear();
        Scene.Clear();
        UI.Clear();
        Pool.Clear();
    }
}
