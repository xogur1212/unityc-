using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransferMap : MonoBehaviour
{
    private float scene1starttime;
    private float loadscene2time;
   
   
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);
       
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (SceneManager.GetActiveScene().name == "scene2")
        {
            if (collision.gameObject.name == "Player")
            {
                GameObject u1 = GameObject.Find("UI_Button");
                UI_Button u11 = Util.GetOrAddComponent<UI_Button>(u1);
                if (u1 != null)
                {


                    loadscene2time = u11.Getscene2time();
                    Debug.Log(loadscene2time);
                    scene1starttime = Time.time;
                }

                SceneManager.LoadScene("scene1");


            }
        }
        else if (SceneManager.GetActiveScene().name == "scene1")
        {
            if (collision.gameObject.name == "Player")
            {
                GameObject u1 = GameObject.Find("UI_Button");
                UI_Button u11 = Util.GetOrAddComponent<UI_Button>(u1);
                if (u1 != null)
                {


                    loadscene2time = u11.Getscene2time();
                    Debug.Log(loadscene2time);
                    scene1starttime = Time.time;
                }

                SceneManager.LoadScene("scene2");


            }
        }
    }

    public float getscene1starttime()
    {
        return scene1starttime;
    }
    public float getloadscene2time()
    {
        return loadscene2time;
    }
}
