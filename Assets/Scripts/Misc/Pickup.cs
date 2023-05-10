using System;
using System.Collections;
using Misc;
using Player;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class Pickup : MonoBehaviour
{
    private enum PickUpType
    {
        GoldCoin,
        StaminaGlobe,
        HealthGlobe,
    }

    [SerializeField] private PickUpType pickUpType;
    [SerializeField] private float pickUpDistance = 5;
    [SerializeField] private float moveSpeed = 3;
    [SerializeField] private float accelerationRate = 3;
    [SerializeField] private AnimationCurve animationCurve;
    [SerializeField] private float heightY = 1.5f;
    [SerializeField] private float popDuration = 1;
    [SerializeField] private float minAngle = 10;
    [SerializeField] private float angle = 40;

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
            moveSpeed = 0;
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
            DetectPickUpType();
            Destroy(gameObject);
        }
    }

    private IEnumerator AnimationCurveSpawnRoutine()
    {
        Vector2 startPoint = transform.position;
        float randomX = startPoint.x + Random.Range(-2, 2);
        float randomY = startPoint.y + Random.Range(-1, 1);
        
        Vector2 endPoint = new Vector2(randomX, randomY);

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

    private void DetectPickUpType()
    {
        switch (pickUpType)
        {
            case PickUpType.GoldCoin:
                // do gold stuff
                EconomyManager.Instance.UpdateCurrentGold();
                break;
            case PickUpType.HealthGlobe:
                // heal player
                PlayerHealth.Instance.HealDamage();
                break;
            case PickUpType.StaminaGlobe:
                // regen stamina
                Stamina.Instance.RefreshStamina();
                break;
            default:
                Debug.Log("Unknown pickup");
                break;
        }
    }
}
