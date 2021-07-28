using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class SpineAnimationBehavior : StateMachineBehaviour
{

    public AnimationClip motion;
    string animationClip;
    bool loop;

    [Header("������ ��� ���̾�")]
    public int layer = 0;
    public float timeScale = 1.0f;

    private SkeletonAnimation skeletonAnimation;
    private Spine.AnimationState spineAnimationState;
    private Spine.TrackEntry trackEntry;
    // Start is called before the first frame update
    void Awake()
    {
        if(motion!=null)
        animationClip = motion.name;
        Debug.Log(animationClip);
    }
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (skeletonAnimation == null)
        {
            skeletonAnimation = animator.GetComponentInChildren<SkeletonAnimation>();   //�ڽ��߿� ã�Ƽ��ٿ���
            spineAnimationState = skeletonAnimation.state;


        }
        if (animationClip != null)
        {
            loop = stateInfo.loop;
            trackEntry = spineAnimationState.SetAnimation(layer,animationClip,loop);
            trackEntry.TimeScale = timeScale;
        }
    }
    // Update is called once per frame

}
