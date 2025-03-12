using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
{

    PursueTargetState pursueTargetState;

    [Header("Detection Layer")]
    [SerializeField] LayerMask detectionLayer;

    [Header("Line of Sight Detection")]
    [SerializeField] float characterEyeLevel = 1.8f;
    [SerializeField] LayerMask ignoreForLineOfSightDetection;

    [Header("Detection Radius")]
    [SerializeField] float detectionRadius = 5;

    [Header("Detection Angle Radius")]
    [SerializeField] float minimumDetectionRadiusAngle = -50f;
    [SerializeField] float maximumDetectionRadiusAngle = 50f;



    // Hacemos que el personaje permanezca en idle hasta que encuentre un objetivo
    // Si encuentra un objetivo, cambia al estado "PursueTargetState"
    // Si no hay objetivo, permanece en idle

    private void Awake()
    {
        pursueTargetState = GetComponent<PursueTargetState>();
    }

    public override State Tick(ZombieManager zombieManager)
    {
        if (zombieManager.currentTarget != null)
        {
            return pursueTargetState;
        }
        else
        {
            FindATargetViaLineOfSight(zombieManager);
            return this;
        }
    }

    private void FindATargetViaLineOfSight(ZombieManager zombieManager)
    {
        // We are searching ALL colliders on the layer of the PLAYER within a certain radius
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius, detectionLayer);

        // For every collider that we find, that is on the same layer of the player, we try and search it for a PlayerManager script
        for (int i = 0; i < colliders.Length; i++)
        {
            PlayerManager player = colliders[i].transform.GetComponent<PlayerManager>();

            // If the playerManager is detected, we then check for line of sight
            if (player != null)
            {
                // The target must be in front of us
                Vector3 targetDirection = transform.position - player.transform.position;
                float viewableAngle = Vector3.Angle(targetDirection, transform.forward);
                Debug.Log("ViewAngle = " +viewableAngle);

                if (viewableAngle > minimumDetectionRadiusAngle && viewableAngle < maximumDetectionRadiusAngle)
                {
                    RaycastHit hit;

                    Vector3 playerStartPoint = new Vector3(player.transform.position.x, characterEyeLevel, player.transform.position.z);
                    Vector3 zombieStartPoint = new Vector3(transform.position.x, characterEyeLevel, transform.position.z);

                    Debug.DrawLine(playerStartPoint, zombieStartPoint, Color.yellow);

                    // CHECK ONE LAST TIME FOR OBJECT BLOCKING VIEW
                    if (Physics.Linecast(playerStartPoint, zombieStartPoint, out hit, ignoreForLineOfSightDetection))
                    {
                        Debug.Log("There is something in the way");
                    }
                    else
                    {
                        Debug.Log("We have a target, switching states");
                        zombieManager.currentTarget = player;

                    }
                }
            }
        }
    }
}

