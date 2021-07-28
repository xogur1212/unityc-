using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseController : MonoBehaviour
{
	[SerializeField]
	protected Define.State _state = Define.State.Idle;

	[SerializeField]
	protected Vector3 _destPos;

	[SerializeField]
	protected GameObject _lockTarget;

	public Define.WorldObject WorldObjectType { get; protected set; } = Define.WorldObject.Unknown;	//타입을 머라고 정의함
	public  virtual Define.State State
	{
		get
		{
			return _state;

		}
		set
		{
			_state = value;

			Animator anim = GetComponent<Animator>();
			switch (_state)
			{
				case Define.State.Die:
					//	anim.SetBool("attack", false);
					break;
				case Define.State.Idle:
					anim.CrossFade("WAIT", 0.1f);   //어느정도시간주고넘어감
													//anim.Play("WAIT");
													//	anim.SetFloat("speed", 0);
													//	anim.SetBool("attack", false);
					break;
				case Define.State.Moving:
					anim.CrossFade("RUN", 0.1f);
					//anim.Play("RUN");
					//anim.SetFloat("speed", _stat.MoveSpeed);
					//	anim.SetBool("attack", false);
					break;
				case Define.State.Skill:
					anim.CrossFade("ATTACK", 0.1f, -1, 0); //-1, 0 하면 0초부터반복
														   //anim.Play("ATTACK");
														   //	anim.SetBool("attack", true);
					break;

			}
		}
	}
	
	
    private void Start()
    {
		Init();
    }
    void Update()
	{


		switch (State)      //상태받아와서 돌림
		{
			case Define.State.Die:
				UpdateDie();
				break;
			case Define.State.Moving:
				UpdateMoving();
				break;
			case Define.State.Idle:
				UpdateIdle(); 
				break;
			case Define.State.Skill:
				UpdateSkill();
				break;
		}
	}

	//protected virtual void Init() 밑에게아니라 왜이걸로쓰는지아직모름
	//{

	//}
	public abstract void Init() ;		//왠지모름
	protected virtual void UpdateDie() { }
	protected virtual void UpdateMoving() { }
	protected virtual void UpdateIdle() { }
	protected virtual void UpdateSkill() { }


}
