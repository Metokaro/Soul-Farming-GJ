using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMoveAnimation
{
   public virtual void Move(bool isMoving, BaseAIBehaviour aiBehaviour)
    {

        float directionX = (aiBehaviour.transform.position.x - aiBehaviour.aiPathfinder.destination.x);
        bool flipX = aiBehaviour.initiallyFacingLeft ? directionX < 0: directionX > 0;
        aiBehaviour.GetComponent<SpriteRenderer>().flipX = flipX ;
        aiBehaviour.GetComponent<Animator>().SetBool("isWalking", isMoving);
        
    }
}

public interface IStaticAnimation
{

}
