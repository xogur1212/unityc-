using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : BaseController
{
	//[SerializeField]
	// float _speed = 10.0f;
	int _mask = (1 << (int)Define.Layer.Ground) | (1 << (int)Define.Layer.Monster);

	PlayerStat _stat;
	bool _stopSkill = false;




	public override void Init()
    {

		WorldObjectType = Define.WorldObject.Player;
		_stat = gameObject.GetComponent<PlayerStat>();	//스텟가져왓
		
		//키입력받을거면 Managers.Input.KeyAction+=함수
		Managers.Input.MouseAction -= OnMouseEvent;		//실수 할수도 잇으니까 
		Managers.Input.MouseAction += OnMouseEvent;     //키가 눌리면 해주세욧~
														//인풋매니저를 작성하여 인풋매니저에서만 누가 키입력을 실행이했는지 찾기편함 ㅇㅇ
														//실험하는법 캐릭터에 플레이어컨트롤러를준다.
		if (gameObject.GetComponentInChildren<UI_HPBar>() == null)
			Managers.UI.MakeWorldSpaceUI<UI_HPBar>(transform); //(나한테 붙여줭)
	}

	

	

	protected override void UpdateMoving()
	{

		if (_lockTarget != null)
		{
			float distance = (_destPos - transform.position).magnitude;
			if (distance <= 1)
			{
				State = Define.State.Skill;
				return;
			}
		}
		Vector3 dir = _destPos - transform.position;//목적지에서 플레이어의위치 뺴서 나온 방향백터
		dir.y = 0; //높이변경x
		if (dir.magnitude < 0.1f)		//dir의 크기가아주 작은값이면은	만약에 캐릭이 안멈출경우나 더움직일경우 0.1f를 수정할것
		{
			State = Define.State.Idle;		//상태변경
		}
		else
		{
				//navigation
		//	NavMeshAgent nma= gameObject.GetOrAddComponent<NavMeshAgent>();
		//	float moveDist = Mathf.Clamp(_stat.MoveSpeed * Time.deltaTime, 0, dir.magnitude);
			//nma.CalculatePath

		//	nma.Move(dir.normalized * moveDist);

			//raycasting
			Debug.DrawRay(transform.position+Vector3.up *0.5f, dir.normalized, Color.green);

            if(Physics.Raycast(transform.position+ Vector3.up * 0.5f, dir, 1.0f, LayerMask.GetMask("Block")))
			{
				if(Input.GetMouseButton(0)==false)      //누른상태로 이동중일떄 벽에부딛혀도 움직인은 모션이나옴
					State = Define.State.Idle;
				return;
            }



			//transform.position += dir.normalized * moveDist;	//이걸로 움직였으나 navmeshagent로 이동하게 변경되었음
			//normalized 방향벡터를 크기를 1로 만들음 moveDist가아니라 _speed*time.deltaTime을 곱하게되면 
			//지나쳐버리면다시돌아가기때문에
			//해결하기위해 clamp 를쓴다. (vlaue,min,max) value가 min max 사이에 값을 보장하게됨
			//여기선 이동할거리와 0 사이를 보장해서 넘어갈일이없음
			//lookat으로 쳐다보게하면 확확돌아감
			float moveDist = Mathf.Clamp(_stat.MoveSpeed * Time.deltaTime, 0, dir.magnitude);
			transform.position += dir.normalized * moveDist;
			transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 20 * Time.deltaTime);
																//slerp(현재위치,마지막위치,적당한수치);
		}

		// 애니메이션
		Animator anim = GetComponent<Animator>();
		// 현재 게임 상태에 대한 정보를 넘겨준다
		
	}


	protected override void UpdateSkill()
    {
		if(_lockTarget !=null)
        {
			Vector3 dir = _lockTarget.transform.position - transform.position;
			Quaternion quat = Quaternion.LookRotation(dir);
			transform.rotation = Quaternion.Lerp(transform.rotation,quat,20*Time.deltaTime);//이동한다
        }

		//Animator anim = GetComponent<Animator>();
		
		//Debug.Log("skill");
    }
	void OnHitEvent()
    {

        //Animator anim = GetComponent<Animator>();
        //anim.SetBool("attack", false);
        //Debug.Log("skill");
		if(_lockTarget !=null)
        {
			Stat targetStat =_lockTarget.GetComponent<Stat>();
			targetStat.OnAttacked(_stat);
			//	PlayerStat myStat = gameObject.GetComponent<PlayerStat>();
			//	int damage = Mathf.Max(0,myStat.Attack - targetStat.Defencse); //0보다작지않게
			//	Debug.Log(damage);
			//	targetStat.Hp -= damage;
		}

        if (_stopSkill)
        {
			State = Define.State.Idle;
		}
        else
        {
			State = Define.State.Skill;
		}

		
	}
	


	void OnMouseEvent(Define.MouseEvent evt)		//마우스클릭이벤트
	{
        switch (State)
        {
			case Define.State.Idle:
				OnMouseEvent_IdleRun(evt);
				break;
			case Define.State.Moving:
				OnMouseEvent_IdleRun(evt);
				break;
			case Define.State.Skill:
                {
					if (evt == Define.MouseEvent.PointerUp)
						_stopSkill = true;
                }
				break;
        }
		

		//if (Physics.Raycast(ray, out hit, 100.0f, LayerMask.GetMask("Ground")))	//벽일경우클릭한경우가
	
	}

	void OnMouseEvent_IdleRun(Define.MouseEvent evt)
    {
		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		bool raycastHit = Physics.Raycast(ray, out hit, 100.0f, _mask);


		//어디클릭했는지 다시 알고싶으면 살령
		//Debug.DrawRay(Camera.main.transform.position, ray.direction * 100.0f, Color.red, 1.0f);



		switch (evt)
		{
			case Define.MouseEvent.PointerDown:
				if (raycastHit)
				{
					_destPos = hit.point;       //목적지를 내가 정한곳으로
					State = Define.State.Moving;
					_stopSkill = false;

					if (hit.collider.gameObject.layer == (int)Define.Layer.Monster)
					{
						_lockTarget = hit.collider.gameObject;

					}
					else
					{
						_lockTarget = null;

					}
				}
				break;
			case Define.MouseEvent.Press:
				{
					if (_lockTarget == null && raycastHit)
					{
						_destPos = hit.point;
					}

				}
				break;
			case Define.MouseEvent.PointerUp:
                {
					_stopSkill = true;
                }
				break;

		}
	}
}
