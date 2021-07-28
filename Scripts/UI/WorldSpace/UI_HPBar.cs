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
        _stat = transform.parent.GetComponent<Stat>();      //���ݳ־�
    }

    private void Update()
    {
        Transform parent = transform.parent; //�θ��ã�ƿ�!!
        transform.position = parent.position + Vector3.up * (parent.GetComponent<Collider>().bounds.size.y); //�ݶ��̴���ŭ������ ���������
        transform.rotation = Camera.main.transform.rotation;

        float ratio = _stat.Hp / (float)_stat.MaxHp; //��Ʈ�� ���߿��ϳ�  float�ؾ� ��� float���� 
        SetHpRatio(ratio);
    }
    public void SetHpRatio(float ratio)
    {
        GetObject((int)GameObjects.HPBar).GetComponent<Slider>().value = ratio;
    }
}
