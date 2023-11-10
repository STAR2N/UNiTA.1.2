using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Game.Player;

public class AnimCharacterController : NetworkBehaviour
{
    [SerializeField]
    private WalkingCharacter walkingCharacter;

    [SerializeField]
    private Transform targetTransform;

    public Animator animator;

    private Transform cameraArm; 
    public float rotateSpeed = 10.0f;

    private bool isWalk = false;
    private bool isRun = false;

    void Start()
    {
        Invoke("SetCameraArm", 0.3f);
    }

    void Update()
    {
        if(IsCanNotMove()) return;

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 moveVector = new Vector3(h, 0f, v);
        bool isMove = moveVector.magnitude > 0;
        if(isMove) {
            if(cameraArm == null) return;

            Vector3 lookForward = new Vector3(cameraArm.forward.x, 0f, cameraArm.forward.z).normalized;
            Vector3 lookRight = new Vector3(cameraArm.right.x, 0f, cameraArm.right.z).normalized;
            Vector3 moveDir = lookForward * v + lookRight * h;

            targetTransform.forward = moveDir;
        }

        if(animator == null) return;
        if(isMove) {
            if(!isWalk) {
                CmdSetAnimatorBool("IsWalk", true);
                animator.SetBool("IsWalk", true);
                isWalk = true;
            }

            if(isWalk) {
                if(Input.GetKey(KeyCode.LeftShift)) {
                    if(!isRun) {
                        CmdSetAnimatorBool("IsRun", true);
                        animator.SetBool("IsRun", true);
                        isRun = true;
                    }
                } else {
                    if(isRun) {
                        CmdSetAnimatorBool("IsRun", false);
                        animator.SetBool("IsRun", false);
                        isRun = false;
                    }
                }
            }
        } else {
            if(isWalk) {
                CmdSetAnimatorBool("IsWalk", false);
                animator.SetBool("IsWalk", false);
                isWalk = false;
            }

            if(isRun) {
                CmdSetAnimatorBool("IsRun", false);
                animator.SetBool("IsRun", false);
                isRun = false;
            }
        }
    }

    void SetCameraArm() 
    {
        cameraArm = walkingCharacter.transform.GetChild(4).transform;
    }

    [Command]
    void CmdSetAnimatorBool(string parameterName, bool isOn)
    {
        RpcOnSetAnimatorBool(parameterName, isOn);
    }

    [ClientRpc]
    void RpcOnSetAnimatorBool(string parameterName, bool isOn)
    {
        if(isLocalPlayer) return;
        if(animator) {
            animator.SetBool(parameterName, isOn);
        }
    }

    void ResetAnim() {
        isWalk = false;
        isRun = false;
        animator.SetBool("IsRun", false);
        animator.SetBool("IsWalk", false);
        CmdSetAnimatorBool("IsRun", false);
        CmdSetAnimatorBool("IsWalk", false);
    }

    bool IsCanNotMove() {
        if(!isLocalPlayer) return true;

        if (InGameUI_ChatWindow.Instance)
        {
            if (InGameUI_ChatWindow.Instance.isSelected)
            {
                if (isWalk || isRun) ResetAnim();
                return true;
            }
        }

        if(TutorialManager.Instance) {
            if(!TutorialManager.Instance.isDone) {
                if(isWalk || isRun) ResetAnim();
                return true;
            }
        }

        if(LetterMaster.Instance) {
            if(LetterMaster.Instance.IsWriting) {
                if(isWalk || isRun) ResetAnim();
                return true;
            }
        }

        return false;
    }
}
