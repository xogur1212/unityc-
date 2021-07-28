using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_HPBar : UI_Base
{
    enum GameObjects
    {
        HPBar
    }
    Stat _stat;
    public override void Init()
    {
        Bind<GameObject>(typeof(GameObjects));
        _stat = transform.parent.GetComponent<Stat>();      //스텟넣엉
    }

    private void Update()
    {
        Transform parent = transform.parent; //부모님찾아와!!
        transform.position = parent.position + Vector3.up * (parent.GetComponent<Collider>().bounds.size.y); //콜라이더만큼위에찍어서 위에찍어줌
        transform.rotation = Camera.main.transform.rotation;

        float ratio = _stat.Hp / (float)_stat.MaxHp; //인트형 둘중에하나  float해야 결과 float으로 
        SetHpRatio(ratio);
    }
    public void SetHpRatio(float ratio)
    {
        GetObject((int)GameObjects.HPBar).GetComponent<Slider>().value = ratio;
    }
}
