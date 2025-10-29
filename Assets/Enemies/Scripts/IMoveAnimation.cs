using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMoveAnimation
{
   public virtual void Move(bool isMoving, BaseAIBehaviour aiBehaviour)
    {
        bool flipX = (aiBehaviour.transform.position.x - aiBehaviour.aiPathfinder.destination.x) < 0;
        aiBehaviour.GetComponent<SpriteRenderer>().flipX = flipX;
        aiBehaviour.GetComponent<Animator>().SetBool("isWalking", isMoving);
        
    }
}

public interface IStaticAnimation
{

}
