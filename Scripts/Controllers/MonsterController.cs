using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterController : BaseController
{
	Stat _stat;
	[SerializeField]
	float _scanRange = 10; //�νĹ���
	[SerializeField]
	float _attackRange = 2; 
    public override void Init()
    {
		WorldObjectType = Define.WorldObject.Monster;
		_stat = gameObject.GetComponent<Stat>();  //���ݰ�����

		if(gameObject.GetComponentInChildren<UI_HPBar>() ==null) //������
		Managers.UI.MakeWorldSpaceUI<UI_HPBar>(transform); //(������ �ٿ��a)
	}
	protected  override void UpdateIdle()
    {
		
		//Debug.Log("monster");
		GameObject player = Managers.Game.GetPlayer();
		//GameObject.FindGameObjectWithTag("Player");//�±׷�ã��
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
		//�÷��̾� �����Ÿ����� ������ ����
		if (_lockTarget != null)
		{
			_destPos = _lockTarget.transform.position;
			float distance = (_destPos - transform.position).magnitude;
			if (distance <= _attackRange)
			{
				NavMeshAgent nma = gameObject.GetOrAddComponent<NavMeshAgent>();
				nma.SetDestination(transform.position);//��ġ�°� �ذ�
				State = Define.State.Skill;
				return;
			}
		}

		//�̵�
		Vector3 dir = _destPos - transform.position;    //���������� �÷��̾�����ġ ���� ���� �������
		if (dir.magnitude < 0.1f)       //dir�� ũ�Ⱑ���� �������̸���	���࿡ ĳ���� �ȸ����쳪 �������ϰ�� 0.1f�� �����Ұ�
		{
			State = Define.State.Idle;      //���º���
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
			transform.rotation = Quaternion.Lerp(transform.rotation, quat, 20 * Time.deltaTime);//�̵��Ѵ�
		}
	}
	void OnHitEvent()
    {
		
        
			//ü�±��
			if (_lockTarget != null)
			{
				Stat targetStat = _lockTarget.GetComponent<Stat>();
				targetStat.OnAttacked(_stat);
				//Stat myStat = gameObject.GetComponent<Stat>();
				//int damage = Mathf.Max(0, myStat.Attack - targetStat.Defencse); //0���������ʰ�
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
