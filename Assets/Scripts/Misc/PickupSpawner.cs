using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupSpawner : MonoBehaviour
{
    [SerializeField] private GameObject goldCoinPrefab, healthGlobePrefab, staminaGlobePrefab;

    public void DropItems()
    {
        var randomNum = Random.Range(1, 5);

        switch (randomNum)
        {
            case 1:
                Instantiate(healthGlobePrefab, transform.position, Quaternion.identity);
                break;
            case 2:
                Instantiate(staminaGlobePrefab, transform.position, Quaternion.identity);
                break;
            case 3:
            {
                var randomAmountOfGold = Random.Range(1, 4);

                var i = 0;
                for (; i < randomAmountOfGold; i++)
                {
                    Instantiate(goldCoinPrefab, transform.position, Quaternion.identity);
                }
                break;
            }
        }
    }
}
