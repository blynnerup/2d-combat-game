using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAi : MonoBehaviour
{
    private State _state;
    private EnemyPathfinding _enemyPathfinding;

    private void Awake()
    {
        _state = State.Roaming;
        _enemyPathfinding = GetComponent<EnemyPathfinding>();
    }

    private IEnumerator RoaminRoutine()
    {
        while (true)
        {
            
        }
    }

    private enum State
    {
        Roaming,
    }
}
