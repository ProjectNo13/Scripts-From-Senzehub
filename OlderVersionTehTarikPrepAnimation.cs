using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TehTarikPrepAnimation : MonoBehaviour, IPrepAnimation
{
    [Header("Animation References")]
    public Transform handWithKettle;
    public Transform kettlePosition;
    public Transform mugPosition;
    public Transform endingPosition;
    public GameObject kettleInHand;
    public GameObject kettleOnCounter;
    public GameObject pouringTeh;

    [Header("Animation Settings")]
    public float delay = 0.1f;
    public Vector3 targetRotation;

    private Quaternion originalRotation;

    /* PlayAnimation(): 
    Controls the animation sequence for the prep scene. 
    Moves the hand to the appropriate position, tilts the kettle to pour the Teh and resets the hand position.
    */
    public IEnumerator PlayAnimation()
    {
        originalRotation = handWithKettle.rotation;

        yield return MoveHandToPosition(handWithKettle, kettlePosition.position, 0.5f);

        yield return new WaitForSeconds(delay);
        kettleInHand.SetActive(true);
        kettleOnCounter.SetActive(false);

        yield return MoveHandToPosition(handWithKettle, mugPosition.position, 1f);

        yield return new WaitForSeconds(delay);
        
        yield return RotateHandToPour(handWithKettle, targetRotation, 0.5f);

        yield return new WaitForSeconds(delay);

        yield return PourTehTarik();

        yield return new WaitForSeconds(delay);

        yield return RotateHandToPour(handWithKettle, originalRotation.eulerAngles, 0.5f);

        yield return new WaitForSeconds(delay);

        yield return MoveHandToPosition(handWithKettle, kettlePosition.position, 0.5f);

        yield return new WaitForSeconds(delay);
        kettleInHand.SetActive(false);
        kettleOnCounter.SetActive(true);

        yield return MoveHandToPosition(handWithKettle, endingPosition.position, 0.5f);
    }

    /* MoveHandToPosition(): 
    Moves the hand to a target position using Vector3.Lerp to interpolate the hand’s movement 
    over a set duration to the target position.
    */
    private IEnumerator MoveHandToPosition(Transform hand, Vector3 targetPosition, float duration)
    {
        Vector3 startPosition = hand.position;
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            hand.position = Vector3.Lerp(startPosition, targetPosition, t);
            yield return null;
        }

        hand.position = targetPosition;
    }

    /* RotateHandToPour(): 
    Rotates the hand to the target angle using Quaternion.Lerp to gradually rotate the hand to simulate pouring.
    */
    private IEnumerator RotateHandToPour(Transform hand, Vector3 targetRotation, float duration)
    {
        Quaternion startRotation = hand.rotation;
        Quaternion targetQuaternion = Quaternion.Euler(targetRotation);
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            hand.rotation = Quaternion.Lerp(startRotation, targetQuaternion, t); 
            yield return null;
        }

        hand.rotation = targetQuaternion;
    }

    /* RotateHandToPour(): 
    Simulates pouring the drink by activating the Teh pouring sprite for a short period of time before deactivating it.
    */
    private IEnumerator PourTehTarik()
    {
        pouringTeh.SetActive(true);

        AudioManager.instance.Play("Teh Tarik Drip");
        yield return new WaitForSeconds(1f);

        pouringTeh.SetActive(false);
    }
}
