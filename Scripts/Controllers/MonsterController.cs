using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterController : BaseController
{
	Stat _stat;
	[SerializeField]
	float _scanRange = 10; //인식범위
	[SerializeField]
	float _attackRange = 2; 
    public override void Init()
    {
		WorldObjectType = Define.WorldObject.Monster;
		_stat = gameObject.GetComponent<Stat>();  //스텟가져왓

		if(gameObject.GetComponentInChildren<UI_HPBar>() ==null) //없으면
		Managers.UI.MakeWorldSpaceUI<UI_HPBar>(transform); //(나한테 붙여줭)
	}
	protected  override void UpdateIdle()
    {
		
		//Debug.Log("monster");
		GameObject player = Managers.Game.GetPlayer();
		//GameObject.FindGameObjectWithTag("Player");//태그로찾기
		if (player == null)
			return;

		
		float distance = (player.transform.position - transform.position).magnitude;
		if(distance <= _scanRange)
        {
			_lockTarget = player;
			State = Define.State.Moving;
			return;
        }

    }
	protected override void UpdateMoving()
	{
		//플레이어 사정거리보다 가까우면 공격
		if (_lockTarget != null)
		{
			_destPos = _lockTarget.transform.position;
			float distance = (_destPos - transform.position).magnitude;
			if (distance <= _attackRange)
			{
				NavMeshAgent nma = gameObject.GetOrAddComponent<NavMeshAgent>();
				nma.SetDestination(transform.position);//밀치는걸 해결
				State = Define.State.Skill;
				return;
			}
		}

		//이동
		Vector3 dir = _destPos - transform.position;    //목적지에서 플레이어의위치 뺴서 나온 방향백터
		if (dir.magnitude < 0.1f)       //dir의 크기가아주 작은값이면은	만약에 캐릭이 안멈출경우나 더움직일경우 0.1f를 수정할것
		{
			State = Define.State.Idle;      //상태변경
		}
		else
		{
			//navigation
			NavMeshAgent nma = gameObject.GetOrAddComponent<NavMeshAgent>();
			nma.SetDestination(_destPos);
			nma.speed = _stat.MoveSpeed;

			transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 20 * Time.deltaTime);
		}
	}
	protected override void UpdateSkill()
	{
		if (_lockTarget != null)
		{
			Vector3 dir = _lockTarget.transform.position - transform.position;
			Quaternion quat = Quaternion.LookRotation(dir);
			transform.rotation = Quaternion.Lerp(transform.rotation, quat, 20 * Time.deltaTime);//이동한다
		}
	}
	void OnHitEvent()
    {
		
        
			//체력깍기
			if (_lockTarget != null)
			{
				Stat targetStat = _lockTarget.GetComponent<Stat>();
				targetStat.OnAttacked(_stat);
				//Stat myStat = gameObject.GetComponent<Stat>();
				//int damage = Mathf.Max(0, myStat.Attack - targetStat.Defencse); //0보다작지않게
			//	Debug.Log(damage);
				//targetStat.Hp -= damage;
				//Debug.Log(targetStat.Hp);
			if(targetStat.Hp <= 0)
            {
				Managers.Game.Despawn(targetStat.gameObject);
            }
			if (targetStat.Hp > 0)
                {
					float distance = (_lockTarget.transform.position - transform.position).magnitude;
			
					if (distance <= _attackRange)
						State = Define.State.Skill;
					else
						State = Define.State.Moving;
                }
                else
                {
				Debug.Log(targetStat.Hp);
				State = Define.State.Idle;
                }

			}
			else 
			{
				State = Define.State.Idle;
			}
		}
      
	
}
