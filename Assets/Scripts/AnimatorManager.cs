using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorManager : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private BodyPartManager leftUpLeg;
    [SerializeField] private BodyPartManager leftLeg;
    [SerializeField] private BodyPartManager rightUpLeg;
    [SerializeField] private BodyPartManager rightLeg;
    [SerializeField] private List<Transform> animationPointList;
    int leftLegTotalCount, leftUpLegTotalCount, rightLegTotalCount, rightUpLegTotalCount;
    private void Start()
    {
        leftLegTotalCount = leftLeg.GetCount();
        rightLegTotalCount = rightLeg.GetCount();
        leftUpLegTotalCount = leftUpLeg.GetCount();
        rightUpLegTotalCount = rightUpLeg.GetCount();
    }
    public void Break(Transform playerTransform)
    {
        var bool1 = (leftLeg.GetCount() == rightLeg.GetCount());
        var bool2 = (leftUpLeg.GetCount() == rightUpLeg.GetCount());

        if (!bool1 || !bool2)
        {
            animator.SetBool("Stumble", true);
        }

        else if(!leftUpLeg.BodyPartIsAllEmpty() && !rightUpLeg.BodyPartIsAllEmpty())
        {
            animator.SetBool("Stumble", false);
        }

        if (leftUpLeg.BodyPartIsAllEmpty() && rightUpLeg.BodyPartIsAllEmpty())
        {
            animator.SetBool("Crawl", true);
            playerTransform.position = playerTransform.position = new Vector3(playerTransform.position.x, animationPointList[0].position.y, playerTransform.position.z);
        }
    }

    public void Heal(Transform playerTransform)
    {
        var bool1 = (leftLeg.GetCount() == rightLeg.GetCount());
        var bool2 = (leftUpLeg.GetCount() == rightUpLeg.GetCount());

        if (!leftUpLeg.BodyPartIsAllEmpty() || !rightUpLeg.BodyPartIsAllEmpty())
        {
            animator.SetBool("Crawl", false);

            if (bool1 && bool2)
            {
                animator.SetBool("Stumble", false);
            }
        }
        ChangeTransform(playerTransform);
    }

    public void FinishAnimation()
    {
        animator.SetBool("Finish", true);
    }

    public void StartRunning()
    {
        animator.SetBool("Finish", false);
    }

    public void ChangeTransform(Transform playerTransform)
    {
        int leftLegCount = leftLeg.GetCount();
        int rightLegCount = rightLeg.GetCount();
        int rightUpLegCount = rightUpLeg.GetCount();
        int leftUpLegCount = leftUpLeg.GetCount();

        if (leftLegCount < leftLegTotalCount / 2 && rightLegCount < rightLegTotalCount / 2)
        {
            playerTransform.position = new Vector3(playerTransform.position.x, animationPointList[1].position.y, playerTransform.position.z);
            if (leftLegCount == 0 && rightLegCount == 0)
            {
                playerTransform.position = new Vector3(playerTransform.position.x, animationPointList[2].position.y, playerTransform.position.z);
                if (leftUpLegCount < leftUpLegTotalCount && rightUpLegCount < rightUpLegTotalCount)
                {
                    playerTransform.position = new Vector3(playerTransform.position.x, animationPointList[3].position.y, playerTransform.position.z);
                }
            }
        }
        else
        {
            playerTransform.position = new Vector3(playerTransform.position.x, animationPointList[0].position.y, playerTransform.position.z);
        }
    }
}
