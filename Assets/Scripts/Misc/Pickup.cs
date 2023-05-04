using System;
using System.Collections;
using Player;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class Pickup : MonoBehaviour
{
    [SerializeField] private float pickUpDistance = 5;
    [SerializeField] private float moveSpeed = 3;
    [SerializeField] private float accelerationRate = 3;
    [SerializeField] private AnimationCurve animationCurve;
    [SerializeField] private float heightY = 1.5f;
    [SerializeField] private float popDuration = 1;
    [SerializeField] private float minAngle = 90;
    [SerializeField] private float angle = 180;

    private Vector3 _moveDir;
    private Rigidbody2D _rigidbody2D;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        StartCoroutine(AnimationCurveSpawnRoutine());
    }

    private void Update()
    {
        Vector3 playerPos = PlayerController.Instance.transform.position;

        if (Vector3.Distance(transform.position, playerPos) < pickUpDistance)
        {
            _moveDir = (playerPos - transform.position).normalized;
            moveSpeed += accelerationRate;
        }
        else
        {
            _moveDir = Vector3.zero;
        }
    }

    private void FixedUpdate()
    {
        _rigidbody2D.velocity = _moveDir * (moveSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.GetComponent<PlayerController>())
        {
            Destroy(gameObject);
        }
    }

    private IEnumerator AnimationCurveSpawnRoutine()
    {
        Vector2 startPoint = transform.position;
        Vector2 endPoint = RandomVector2(angle, minAngle);

        float timePassed = 0;

        while (timePassed < popDuration)
        {
            timePassed += Time.deltaTime;
            float linearT = timePassed / popDuration;
            float heightT = animationCurve.Evaluate(linearT);
            float height = Mathf.Lerp(0, heightY, heightT);
            
            transform.position = Vector2.Lerp(startPoint, endPoint, linearT) + new Vector2(0, height);
            
            yield return null;
        }
    }
    
    public Vector2 RandomVector2(float startAngle, float angleMin){
        float random = Random.value * angle + angleMin;
        return new Vector2(Mathf.Cos(random), Mathf.Sin(random));
    }
}
