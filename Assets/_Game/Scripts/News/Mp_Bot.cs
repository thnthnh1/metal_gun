using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Mp_Bot : MonoBehaviour
{
	[Header("Selected Targer, debug only")]
	public Transform target;
	private GameObject [] allPlayers;
	public List <Transform> allPlayersTransform;

	[Header("Move Target")]
	public Transform moveTarget;
	public float maxmoveDistance;
	public float moveTargetDistance;

	[Header("Waypoints system")]
	public bool waypointReached;
	public GameObject [] waypoints;

	[Header("Jetpack System")]
	public float distanceToActivateJetpack;
	public bool enableJetpack;

	[Header("Attack System")]
	public float distanceToAttack;
	public float targetDistance;
	public bool enemyNear;

	[Header("Sight System")]
	public bool targetInSight;
	public LayerMask sightdetectionLayer;
	public Ray sightDetection;

	[Header("Detection System")]
	public LayerMask detectionLayer;
	public Ray groundDetection;
	public RaycastHit2D groundHit;
	public float groundDistance;

	public Ray rightDetection;
	public Ray leftDetection;
	public RaycastHit2D rightHit;
	public RaycastHit2D leftHit;
	public float rightDistance;
	public float leftDistance;

	public Ray upDetection;
	public RaycastHit2D upHit;
	public float upDistance;

	[Header("Position Verification")]
	public Vector3 lastPos;
	public Vector3 newPos;

	public bool debug;

	[HideInInspector]
	public PhotonView pv;

	[HideInInspector]
	public MP_Player playerScript;

	// Start is called before the first frame update
	void Start()
    {
		pv = GetComponent<PhotonView>();

		//Get all waypoints
		waypoints = GameObject.FindGameObjectsWithTag("Waypoint");

		if (true) // PhotonNetwork.IsMasterClient
		{
			playerScript = GetComponent<MP_Player>();
			StartCoroutine(GetTargets());

			GetRandomWaypoint();
		}
		
		playerScript.ActiveAim(true);

		StartCoroutine(LastPosVerification());
	}

	void GetRandomWaypoint()
	{
		//Get Random waypoint
		moveTarget = waypoints[Random.Range(0, waypoints.Length)].transform;

		waypointReached = false;
	}

    // Update is called once per frame
    void Update()
    {
		if (true) // PhotonNetwork.IsMasterClient
		{
			MovementDetection();

			JetpackSystem();

			AttackDetection();

			ComprobateReachWaypoint();
		}
    }

	void MovementDetection()
	{
		if (moveTarget)
		{
			moveTargetDistance = Vector3.Distance(transform.position, moveTarget.position);

			if (moveTargetDistance > maxmoveDistance)
			{
				if (moveTarget.transform.position.x < transform.position.x)
				{
					if (leftDistance > 1f)
					{
						playerScript.xDirection = -1;
					}
					else
					{
						playerScript.xDirection = 0;
					}
					
				}
				else if (moveTarget.transform.position.x > transform.position.x)
				{
					if (rightDistance > 1f)
					{
						playerScript.xDirection = 1;
					}
					else
					{
						playerScript.xDirection = 0;
					}
				}
			}
			else
			{
				playerScript.xDirection = 0;
			}
		}
	}

	void JetpackSystem()
	{
		if (enableJetpack)
		{
			playerScript.UseJetpack();
		}
		else
		{
			playerScript.StopJetpack();
		}
	}

	void ComprobateReachWaypoint()
	{
		if (Mathf.Abs(moveTarget.position.x - transform.position.x) < maxmoveDistance && !waypointReached)
		{
			waypointReached = true;

			Invoke("GetRandomWaypoint", 3);
		}
	}

	void AttackDetection()
	{
		if (target)
		{
			targetDistance = Vector3.Distance(transform.position, target.position);

			if (targetDistance <= distanceToAttack)
			{
				playerScript.aimBone.transform.position = target.position;

				if (transform.localScale.x > 0 && transform.position.x > target.position.x)
				{
					pv.RPC("SetDirection", RpcTarget.All, false);
				}
				else if (transform.localScale.x < 0 && transform.position.x < target.position.x)
				{
					pv.RPC("SetDirection", RpcTarget.All, true);
				}

				SightDetection();

				enemyNear = true;
			}
			else
			{
				enemyNear = false;
			}

		}
	}

	void SightDetection()
	{
		targetInSight = Physics2D.Raycast(playerScript.currentWeapon.cannonPos.position, playerScript.currentWeapon.cannonPos.right, 100, sightdetectionLayer);

		if (targetInSight || targetDistance <= 1)
		{
			if (playerScript.currentWeapon.ammo > 0)
			{
				if (PhotonNetwork.IsMasterClient)
				{
					playerScript.Attack();
				}
				
			}
			else if (playerScript.currentWeapon.ammo <= 0 && !playerScript.currentWeapon.reloading)
			{
				StartCoroutine(playerScript.currentWeapon.Reload());
			}
			
		}
	}
	void FixedUpdate()
	{
		if (true) //PhotonNetwork.IsMasterClient
		{
			// Ground
			groundHit = Physics2D.Raycast(transform.position, -Vector2.up, 100, detectionLayer);

			if (groundHit.collider)
			{
				groundDistance = groundHit.distance;

				if (groundDistance < 0.04f)
				{
					playerScript.inGround = true;
				}
				else
				{
					playerScript.inGround = false;
				}
			}

			// Right
			rightHit = Physics2D.Raycast(transform.position, Vector2.right, 100, detectionLayer);

			if (rightHit.collider)
			{
				rightDistance = rightHit.distance;
			}

			// Left
			leftHit = Physics2D.Raycast(transform.position, Vector2.left, 100, detectionLayer);

			if (leftHit.collider)
			{
				leftDistance = leftHit.distance;
			}

			// Up
			upHit = Physics2D.Raycast(transform.position, Vector2.up, 100, detectionLayer);

			if (upHit.collider)
			{
				upDistance = upHit.distance;
			}
		}
	}

	IEnumerator GetTargets()
	{
		yield return new WaitForSeconds(1);

		StartCoroutine(GetTargets());

		//Get all players in the scene
		allPlayers = GameObject.FindGameObjectsWithTag("Player");

		for (int i = 0; i < allPlayers.Length; i++)
		{
			if (gameObject.name != allPlayers[i].name)
			{
				allPlayersTransform.Add(allPlayers[i].GetComponent<Transform>());
			}
		}

		target = GetClosestEnemy(allPlayersTransform);

		allPlayersTransform.Clear();
	}

	Transform GetClosestEnemy(List <Transform> enemies) //Find nearest enemy
	{
		Transform bestTarget = null;
		float closestDistanceSqr = Mathf.Infinity;
		Vector3 currentPosition = transform.position;

		foreach (Transform potentialTarget in enemies)
		{
			Vector3 directionToTarget = potentialTarget.position - currentPosition;
			float dSqrToTarget = directionToTarget.sqrMagnitude;
			if (dSqrToTarget < closestDistanceSqr)
			{
				closestDistanceSqr = dSqrToTarget;
				bestTarget = potentialTarget;
			}
		}

		return bestTarget;
	}

	public IEnumerator LastPosVerification() 
	{
		lastPos = transform.position;

		yield return new WaitForSeconds(1f);

		newPos = transform.position;

		if (lastPos == newPos)
		{
			if (debug)
			{
				Debug.Log("Using Jet");
			}
			enableJetpack = true;

			Invoke("GetRandomWaypoint", 3);
		}
		else
		{
			if (debug)
			{
				Debug.Log("No Using Jet");
			}
			enableJetpack = false;
		}

		StartCoroutine(LastPosVerification());
	}
}
