
using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public Transform player;
 
    protected NavMeshAgent enemyMesh;


    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        enemyMesh = GetComponent<NavMeshAgent>();
        StartCoroutine(UpdatePath());   
        
    }



    void Update()
    {
        
    }


    IEnumerator UpdatePath()
    {
        float refreshRate = 1f;

        while(player != null)
        {
            Vector3 playerPosition = new Vector3(player.position.x, 0, player.position.z);
            enemyMesh.SetDestination(playerPosition); 
            yield return new WaitForSeconds(refreshRate);

        }
    }



}


