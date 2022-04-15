using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    [Header("Internal References")]
    public Transform moveArea;
    //public Transform target;
    public NavMeshAgent moveAgent;

    [Header ("Parameters")]
    public float replanFrequency = 0.5f;

    private float replanCoutner;

    // Start is called before the first frame update
    void Start()
    {
        replanCoutner = 1 / replanFrequency;
    }

    // Update is called once per frame
    void Update()
    {
        replanCoutner -= Time.deltaTime;
        if (replanCoutner < 0) {
            RandomMove();
            replanCoutner = 1 / replanFrequency;
        }
    }

    public void RandomMove() {
        float randXCoord = Random.Range(-0.5f * moveArea.localScale.x + moveArea.position.x, moveArea.position.x + 0.5f * moveArea.localScale.x);
        float randZCoord = Random.Range(-0.5f * moveArea.localScale.z + moveArea.position.z, moveArea.position.z + 0.5f * moveArea.localScale.z);
        Vector3 moveTargetPoint = new Vector3(randXCoord, transform.position.y, randZCoord);
        moveAgent.SetDestination(moveTargetPoint);
    }
}
