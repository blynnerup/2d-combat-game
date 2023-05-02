using System;
using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;

public class GrapeProjectile : MonoBehaviour
{
    [SerializeField] private float duration = 1;
    [SerializeField] private AnimationCurve animationCurve;
    [SerializeField] private float heightY = 3;
    [SerializeField] private GameObject grapeProjectileShadow;
    [SerializeField] private GameObject grapeSplatter;
    
    private void Start()
    {
        GameObject grapeShadow = Instantiate(grapeProjectileShadow, transform.position + new Vector3(0, -.3f, 0), Quaternion.identity);
        
        Vector3 playerPos = PlayerController.Instance.transform.position;
        Vector3 grapeShadowStartPosition = grapeShadow.transform.position;
        
        StartCoroutine(ProjectileCurveRoutine(transform.position, playerPos));
        StartCoroutine(MoveGrapeShadowRoutine(grapeShadow, grapeShadowStartPosition, playerPos));
    }

    private IEnumerator ProjectileCurveRoutine(Vector3 startPosition, Vector3 endPosition)
    {
        float timePassed = 0;

        while (timePassed < duration)
        {
            timePassed += Time.deltaTime;
            float linearT = timePassed / duration;
            float heightT = animationCurve.Evaluate(linearT);
            float height = Mathf.Lerp(0, heightY, heightT);

            transform.position = Vector2.Lerp(startPosition, endPosition, linearT) + new Vector2(0, height);

            yield return null;
        }
        
        Destroy(gameObject);
    }
    
    private IEnumerator MoveGrapeShadowRoutine(GameObject grapeShadow, Vector3 startPosition, Vector3 endPosition)
    {
        float timePassed = 0;

        while (timePassed < duration)
        {
            if ((timePassed) < (duration / 2))
            {
                grapeShadow.transform.localScale -= new Vector3(.005f, .005f, 0);
            }
            else
            {
                grapeShadow.transform.localScale -= new Vector3(-.005f, -.005f, 0);
            }
            timePassed += Time.deltaTime;
            float linearT = timePassed / duration;

            grapeShadow.transform.position = Vector2.Lerp(startPosition, endPosition, linearT);

            yield return null;
        }

        Instantiate(grapeSplatter, transform.position, Quaternion.identity);
        Destroy(grapeShadow);
    }
}
