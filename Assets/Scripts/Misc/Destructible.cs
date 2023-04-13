using System;
using Unity.Mathematics;
using UnityEngine;

namespace Misc
{
    public class Destructible : MonoBehaviour
    {
        [SerializeField] private GameObject destroyVfx;

        private void OnTriggerEnter2D(Collider2D col)
        {
            Debug.Log("hit");
            Instantiate(destroyVfx, transform.position, quaternion.identity);
            Destroy(gameObject);
        }
    }
}
