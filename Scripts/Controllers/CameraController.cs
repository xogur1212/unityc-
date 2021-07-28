using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour       //target texture  https://docs.unity3d.com/kr/530/Manual/class-RenderTexture.html
{

    //카메라에 스크립트 던져서 사용
    [SerializeField]            //private이면서 외부에서표시되게
    
    Define.CameraMode _mode = Define.CameraMode.QuarterView;        //초기값을 quertview로

    [SerializeField]
    Vector3 _delta = new Vector3(0.0f, 6.0f, -5.0f);        //카메라 위치

    [SerializeField]
    GameObject _player = null;      //누구쫓아댕길거야

    public void SetPlayer(GameObject player) { _player = player; }

    void Start()
    {
        
    }

    void LateUpdate()           //update로하면 화면이 덜덜거림 라이프사이클에서 게임로직에 lateupate가 맨마지막일아 이걸씀 
    { 
        if (_mode == Define.CameraMode.QuarterView)     //쿼터뷰이면
        {
           // _player.activeSelf; 만약에 오류가 생길경우 삭제시켜라
            if (_player.IsValid()==false)
            {
                return;
            }
            RaycastHit hit;
            if (Physics.Raycast(_player.transform.position, _delta, out hit, _delta.magnitude, LayerMask.GetMask("Block")))
            {       //raycast(플레이어위치,방향벡터,hit을 out으로 이거왜?? ,크기,레이어마스크)
                    //out -? 매개변수 한정자 ,매개변수 직접 바꿀수잇음 https://bluemeta.tistory.com/10
                float dist = (hit.point - _player.transform.position).magnitude * 0.8f; //hit.point 충돌한 점-플레이어위치*maginitude(제곱근)피타고라스 살짝조금앞에
                transform.position = _player.transform.position + _delta.normalized * dist;//플레이어 기준을 카메라 이동
            }
            else
            {
				transform.position = _player.transform.position + _delta; // 플레이어의 포지션에 방향벡터인 델타를 더해

                transform.LookAt(_player.transform);        //플레이어보는방향처다보게
			}
		}
    }

    public void SetQuarterView(Vector3 delta)       //쿼터뷰로 셋팅
    {
        _mode = Define.CameraMode.QuarterView;
        _delta = delta;
    }
}
