using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class SpineAnimation : MonoBehaviour
{
    public SkeletonAnimation skeletonAnimation;
    public AnimationReferenceAsset[] animClip;
    public float speed;

    [SpineSlot]
    public string Slot;
    [SpineSkin]
    public string Skin;


    private void Awake() => skeletonAnimation = GetComponent<SkeletonAnimation>();

    

    public void PlayAnimation(string animName, bool loop, float timeScale) // �ִϸ��̼� ���
    {


        skeletonAnimation.AnimationState.SetAnimation(0, animName, loop);
        skeletonAnimation.state.TimeScale = timeScale;


    }

    public void AddAnimation(string animName, bool loop, float timeScale, float startDelay) // �ִϸ��̼� �߰�
    {
        skeletonAnimation.AnimationState.AddAnimation(0, animName, loop, startDelay);
        skeletonAnimation.state.TimeScale = timeScale;

    }

    public void SetSpineSkin(string skinName) // ��ü ��Ų ��ü
    {
        skeletonAnimation.skeleton.SetSkin(skinName);
        skeletonAnimation.skeleton.SetSlotsToSetupPose();

    }

    public void SetSpineAttachment(string _slot, string _attachment)
    {
        skeletonAnimation.skeleton.SetAttachment(_slot, _attachment);
    }


}
