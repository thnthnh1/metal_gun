using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Spine;
using Spine.Unity;
using UnityEngine.UI;

public class MP_Player : MonoBehaviour, IPunObservable
{
	[Header("Movement values")]
	public float speed;
	public float maxSpeed;
	public float jumpForce;
	public float JetPackForce;

	[Header("Ground Checking")]
	public Transform groundCheckPos;
	public LayerMask groundHit;
	public float raylenght;
	public bool inGround;

	[Header("Jetpack Settings")]
	public float jetpackCurrentValue;
	public float jetpackConsumption;
	public float jetpackRegeneration;
	private float jetpackValue = 100;
	public bool usingJetpack;
	public ParticleSystem jectpackParticles;
	public ParticleSystem impactParticles;

	[Header("Granade Settings")]
	public Transform granadeDirection;
	public float trowForce;
	public GameObject granadePrefab;

	public float granades = 3;
	private float savedGranades;

	[Header("Input values")]
	public float xDirection;
	public float yDirection;

	[Header("Aim values")]
	public float xAttackDirection;
	public float yAttackDirection;
	public Transform aimParent;
	public Transform aimReference;
	public bool inReverse;

	[Header("Attack values")]
	public float attackRate;
	private float rate;

	[Header("Weapons values")]
	public GameObject [] weaponContainer;
	public Mp_Weapon currentWeapon;
	private int oldWeapon;

	[Header("Health values")]
	public float health;
	public float maxHealth;
	public Slider healthUI;
	public float timeToRevive = 3;
	public ParticleSystem deathFx;

	[Header("Invencible Time")]
	public bool invencible = false;
	public float invencibleTime;
	public GameObject invencibleEffect;

	[HideInInspector]
	public bool isBot = false;

	[Header("States")]
	public bool death;

	[Header("SPINE")]
	public SkeletonAnimation skeletonAnimation;

	public SkeletonRenderer skeletonRenderer;

	public SkeletonUtilityBone aimBone;

	[SpineAnimation("", "", true, false)]
	public string crouch;

	[SpineAnimation("", "", true, false)]
	public string move;

	[SpineAnimation("", "", true, false)]
	public string jump;

	[SpineAnimation("", "", true, false)]
	public string lookDown;

	[SpineAnimation("", "", true, false)]
	public string throwGrenade;

	[SpineAnimation("", "", true, false)]
	public string victory;

	[SpineAnimation("", "", true, false)]
	public string parachute;

	[SpineAnimation("", "", true, false)]
	public string fallBackward;

	[SpineAnimation("", "", true, false)]
	public string idle;

	[SpineAnimation("", "", true, false)]
	public string idleRifle;

	[SpineAnimation("", "", true, false)]
	public string idlePistol;

	[SpineAnimation("", "", true, false)]
	public string idleInJet;

	[SpineAnimation("", "", true, false)]
	public string shoot;

	[SpineAnimation("", "", true, false)]
	public string shootRifle;

	[SpineAnimation("", "", true, false)]
	public string shootPistol;

	[SpineAnimation("", "", true, false)]
	public string aim;

	[SpineAnimation("", "", true, false)]
	public string aimRifle;

	[SpineAnimation("", "", true, false)]
	public string aimPistol;

	[SpineAnimation("", "", true, false)]
	public string meleeAttack;

	[SpineAnimation("", "", true, false)]
	public string knife;

	[SpineAnimation("", "", true, false)]
	public string pan;

	[SpineAnimation("", "", true, false)]
	public string guitar;

	[SpineAnimation("", "", true, false)]
	public List<string> dieAnimationNames;

	[SpineBone("", "", true, false)]
	public string equipGunBoneName;

	[SpineBone("", "", true, false)]
	public string equipMeleeWeaponBoneName;

	[SpineBone("", "", true, false)]
	public string effectWindBoneName;

	[SpineEvent("", "", true, false)]
	public string eventFootstep;

	[SpineEvent("", "", true, false)]
	public string eventMeleeAttack;

	[SpineEvent("", "", true, false)]
	public string eventThrowGrenade;

	private Vector2 idleAimPointPosition;

	private Vector2 crouchAimPointPosition;

	//References//
	[HideInInspector]
	public Rigidbody2D rb;
	public PhotonView pv;

	[Header("Character Selection")]
	public GameObject [] characters;
	public GameObject selectedChar;

	[Header("Arrow indicator system")]
	public Transform [] arrows;
	public float minDistance;
	public List <GameObject> playersObjs;
	// Start is called before the first frame update
	void Start()
    {
		rb = GetComponent<Rigidbody2D>();
		pv = GetComponent<PhotonView>();

		maxHealth = health;

		jetpackCurrentValue = jetpackValue;

		savedGranades = granades;

		//Determine if is a Bot
		if (GetComponent<Mp_Bot>())
		{
			isBot = true;
		}
		else
		{
			isBot = false;
		}

		if (pv.IsMine && !isBot)
		{
			//Consume medal
			GameData.playerResources.ConsumeTournamentTicket(1);

			HudManager.instance.localPlayerReference = this;

			Camera.main.GetComponent<Mp_Camera>().target = transform;

			healthUI.gameObject.SetActive(false);

			HudManager.instance.RefreshGranades(granades);

			HudManager.instance.weaponImage.sprite = currentWeapon.weaponImage;

			HudManager.instance.RefreshAmmo(currentWeapon.ammo, currentWeapon.maxAmmo);

			oldWeapon = 0;

			StartCoroutine(SendLeaderboardRegister(pv.Owner.NickName));


			SoundManager.Instance.gameObject.GetComponent<AudioListener>().enabled = false;

			Invoke("LoadStartValues", 1);
		}
		else
		{
			if (isBot)
			{
				Mp_GameController.instance.bots.Add(gameObject);

				if (PhotonNetwork.IsMasterClient)
				{
					pv.RPC("RegisterBot", RpcTarget.AllBuffered, Mp_playerSettings.instance.botsNames[Launcher.launcher.botSpawned]);

					pv.RPC("SelectRandomCharacter", RpcTarget.AllBuffered, Random.Range(0, characters.Length));
				}
			}
			else
			{
				healthUI.GetComponentInChildren<Text>().text = pv.Owner.NickName;
			}
		}
		if (pv.IsMine)
		{
			rb.isKinematic = false;
		}
		else
		{
			rb.isKinematic = true;
		}

		RefreshHealth();

		//Register in Leaderboard
	}

	[PunRPC]
	public void RegisterBot(string botName) 
	{
		gameObject.name = botName;
		healthUI.GetComponentInChildren<Text>().text = botName;

		StartCoroutine(SendLeaderboardRegister(botName));
	}

	public void LoadStartValues()
	{
		pv.RPC("ChangeWeapon", RpcTarget.AllBuffered, Mp_playerSettings.instance.weaponSelected, "");
		pv.RPC("SelectCharacter", RpcTarget.AllBuffered, Mp_playerSettings.instance.characterSelected);

		InvokeRepeating("ArrowIndicator", 1f, 0.05f);
	}

	[PunRPC]
	public void SelectCharacter(string charactername)
	{
		int characterNumber = 0;

		for (int i = 0; i < characters.Length; i++)
		{
			if (characters[i].name == charactername)
			{
				characterNumber = i;
				Debug.Log("Founded Character Selected!");
				break;
			}
		}

		skeletonAnimation = characters[characterNumber].GetComponent<SkeletonAnimation>();
		skeletonRenderer = characters[characterNumber].GetComponent<SkeletonRenderer>();
		aimBone = characters[characterNumber].transform.Find("SkeletonUtility-Root").transform.Find("root").transform.Find("aim [Override]").GetComponent<SkeletonUtilityBone>();

		for (int i = 0; i < weaponContainer.Length; i++)
		{
			weaponContainer[i].GetComponent<BoneFollower>().skeletonRenderer = skeletonRenderer;
		}

		characters[characterNumber].SetActive(true);

		selectedChar = characters[characterNumber];
	}

	[PunRPC]
	public void SelectRandomCharacter(int value) 
	{
		skeletonAnimation = characters[value].GetComponent<SkeletonAnimation>();
		skeletonRenderer = characters[value].GetComponent<SkeletonRenderer>();
		aimBone = characters[value].transform.Find("SkeletonUtility-Root").transform.Find("root").transform.Find("aim [Override]").GetComponent<SkeletonUtilityBone>();

		for (int i = 0; i < weaponContainer.Length; i++)
		{
			weaponContainer[i].GetComponent<BoneFollower>().skeletonRenderer = skeletonRenderer;
		}

		characters[value].SetActive(true);

		selectedChar = characters[value];
	}


	// Update is called once per frame
	void Update()
	{
		if (death || !Mp_GameController.instance.gameStarted || Mp_GameController.instance.gameOver)
		{
			return;
		}
		if (pv)
		{
			if (pv.IsMine && !isBot)
			{
				xDirection = CnControls.CnInputManager.GetAxis("Horizontal");
				yDirection = CnControls.CnInputManager.GetAxis("Vertical");

				xAttackDirection = CnControls.CnInputManager.GetAxis("AttackHorizontal");
				yAttackDirection = CnControls.CnInputManager.GetAxis("AttackVertical");

				//Attack Input
				if (xAttackDirection != 0 || yAttackDirection != 0)
				{
					if (pv.IsMine)
					{
						if (xAttackDirection > 0.7f || xAttackDirection < -0.7f || yAttackDirection > 0.7f || yAttackDirection < -0.7f)
						{
							if (HudManager.instance.firing)
							{
								Attack();
							}
						}
					}

					
				}
				//else
				//{
				//	ActiveAim(false);
				//}

				JetPackValues();
				jumpAndJetpackJump();
				
			}
		}

		AttackDirection();
		ActiveAim(true);

		if (transform.position.y < -20 && !death)
		{
			ApplyDamage(9999, "");
		}

		rate += Time.deltaTime;
	}

	void FixedUpdate()
	{
		if (death || !Mp_GameController.instance.gameStarted || Mp_GameController.instance.gameOver)
		{
			return;
		}
		if (pv)
		{
			if (!pv.IsMine)
			{
				return;
			}
		}
		if (!isBot)
		{
			GroundChecking();
		}
		
		Movement();
	}

	void GroundChecking()
	{
		if (Physics2D.Raycast(groundCheckPos.position, -Vector2.up, raylenght, groundHit))
		{
			if (!Input.GetButton("Jump") && usingJetpack)
			{
				//usingJetpack = false; // Only for use jetpack forever
				
				//if (!impactParticles.isPlaying)
				//{
				//	pv.RPC("GroundImpactSystemEffect", RpcTarget.All);
				//}
			}

			//if (HudManager.instance.jetpackButton.activeSelf) // Only for use jetpack forever
			//{
			//	HudManager.instance.ResetJumpButtons();
			//}

			inGround = true;
		}
		else
		{
			inGround = false;

			if (HudManager.instance.jumpButton.activeSelf)
			{
				HudManager.instance.jumpButton.SetActive(false);
				HudManager.instance.jetpackButton.SetActive(true);
			}
		}
	}

	void Movement()
	{
		if (xDirection != 0)
		{
			if (rb.velocity.magnitude < maxSpeed)
			{
				rb.AddForce(transform.right * xDirection * speed);
			}
			else if (inGround)
			{
				rb.velocity = rb.velocity*0.8f;
			}
			else if (usingJetpack || HudManager.instance.jetpackPushed)
			{
				if (rb.velocity.magnitude > maxSpeed*2f)
				{
					rb.velocity = rb.velocity * 0.8f;
				}
				rb.AddForce(transform.right * xDirection * speed);
			}


			TrackEntry current = this.skeletonAnimation.AnimationState.GetCurrent(0);
			if (current == null || string.Compare(current.animation.name, this.move) != 0 && inGround)
			{
				pv.RPC("PlayAnimationRun", RpcTarget.All);
			}
			else if(!inGround)
			{
				TrackEntry current2 = this.skeletonAnimation.AnimationState.GetCurrent(0);
				if (current2 == null || string.Compare(current2.animation.name, this.idle) != 0)
				{
					pv.RPC("PlayAnimationIdle", RpcTarget.All);
				}
			}

			//Set scale
			if (xAttackDirection == 0 && yAttackDirection == 0)
			{
				if (isBot)
				{
					if (GetComponent<Mp_Bot>().enemyNear)
					{
						return;
					}
				}
				if (xDirection > 0)
				{
					if (transform.localScale.x < 0)
					{
						pv.RPC("SetDirection", RpcTarget.All, true);
					}

				}
				else if (xDirection < 0)
				{
					if (transform.localScale.x > 0)
					{
						pv.RPC("SetDirection", RpcTarget.All, false);
					}
				}
			}
			
		}
		else
		{
			TrackEntry current2 = this.skeletonAnimation.AnimationState.GetCurrent(0);
			if (current2 == null || string.Compare(current2.animation.name, this.idle) != 0)
			{
				pv.RPC("PlayAnimationIdle", RpcTarget.All);
			}
		}
	}
	void JetPackValues()
	{
		if (jetpackCurrentValue < jetpackValue)
		{
			jetpackCurrentValue += jetpackRegeneration * Time.deltaTime;

			if (jetpackCurrentValue >= jetpackValue)
			{
				jetpackCurrentValue = jetpackValue;
			}
		}

		HudManager.instance.RefreshJetpackBar(jetpackCurrentValue, jetpackValue);
	}
	void jumpAndJetpackJump()
	{
		if (yDirection>=0.25f)
		{
			HudManager.instance.JumpOn();

			HudManager.instance.JetpackPressed();
		}
		else
		{
			HudManager.instance.JumpOff();
			HudManager.instance.ReleaseJetpack();
		}
		if (Input.GetButtonDown("Jump") || HudManager.instance.jumpPushed)
		{
			if (inGround)
			{
				rb.AddForce(Vector3.up * jumpForce * 100);
				//usingJetpack = false;
				usingJetpack = true; // Only for use Jetpack forever
			}
			else
			{
				usingJetpack = true;
			}

			HudManager.instance.jumpPushed = false;
		}

		if (Input.GetButton("Jump") && usingJetpack || HudManager.instance.jetpackPushed)
		{
			if (jetpackCurrentValue >= 10)
			{
				//usinJetpack
				UseJetpack();
			}
		}
		if (Input.GetButtonUp("Jump") || !HudManager.instance.jetpackPushed)
		{
			StopJetpack();
		}
	}

	public void UseJetpack()
	{
		rb.AddForce(Vector3.up * JetPackForce * 100 * yDirection* Time.deltaTime);

		if (!jectpackParticles.isPlaying)
		{
			pv.RPC("PlayParticleSystemEffect", RpcTarget.All);
		}

		jetpackCurrentValue -= jetpackConsumption * Time.deltaTime;
	}

	public void StopJetpack()
	{
		pv.RPC("StopParticleSystemEffect", RpcTarget.All);
	}
	[PunRPC]
	public void PlayParticleSystemEffect()
	{
		jectpackParticles.Play();
	}
	[PunRPC]
	public void StopParticleSystemEffect()
	{
		jectpackParticles.Stop();
	}
	[PunRPC]
	public void GroundImpactSystemEffect()
	{
		impactParticles.Play();
	}

	void AttackDirection()
	{
		float angle = Mathf.Atan2(xAttackDirection, yAttackDirection) * Mathf.Rad2Deg;
		aimParent.rotation = Quaternion.Euler(0, 0, -angle);

		if (aimBone)
		{
			aimBone.transform.position = aimReference.transform.position;
		}

		//Define new player direction
		if (xAttackDirection != 0 || yAttackDirection != 0)
		{
			if (xAttackDirection > 0)
			{
				if (transform.localScale.x < 0)
				{
					pv.RPC("SetDirection", RpcTarget.All, true);
				}

			}
			else if (xAttackDirection < 0)
			{
				if (transform.localScale.x > 0)
				{
					pv.RPC("SetDirection", RpcTarget.All, false);
				}
			}
		}
	}

	public void Attack()
	{
		if (rate >= attackRate && !currentWeapon.reloading)
		{
			//TrackEntry current = this.skeletonAnimation.AnimationState.GetCurrent(0);
			//if (current == null || string.Compare(current.animation.name, this.shootRifle) != 0)
			//{
			//	pv.RPC("PlayAnimationShoot", RpcTarget.All);
			//}

			if (!isBot)
			{
				FireRPC(pv.Owner.NickName);
				//pv.RPC("FireRPC", RpcTarget.All, pv.Owner.NickName);
			}
			else
			{
				FireRPC(gameObject.name);
				//pv.RPC("FireRPC", RpcTarget.All, gameObject.name);
			}
			

			rate = 0;
		}
	}
	public void ThrowGranade()
	{
		if (granades>0 && !death)
		{
			//pv.RPC("ThrowGranadeRPC", RpcTarget.All, pv.Owner.NickName);
			ThrowGranadeRPC(pv.Owner.NickName);
		}	
	}

	[PunRPC]
	public void ThrowGranadeRPC(string playerFired)
	{
		//GameObject granadeSpawned = Instantiate (granadePrefab, granadeDirection.position, Quaternion.identity) as GameObject;

		GameObject granadeSpawned = PhotonNetwork.Instantiate(granadePrefab.name, granadeDirection.position, Quaternion.identity);

		granadeSpawned.GetComponent<Mp_Granade>().playerFired = playerFired;
		granadeSpawned.GetComponent<Mp_Granade>().activated = true;

		if (!inReverse)
		{
			granadeSpawned.GetComponent<Rigidbody2D>().AddForce(currentWeapon.cannonPos.right * trowForce);
		}
		else
		{
			granadeSpawned.GetComponent<Rigidbody2D>().AddForce(-currentWeapon.cannonPos.right * trowForce);
		}

		granades--;
		HudManager.instance.RefreshGranades(granades);

		//this.skeletonAnimation.AnimationState.SetAnimation(0, this.throwGrenade, false);
	}

	[PunRPC]
	public void FireRPC(string playerFired)
	{
		currentWeapon.Attack(playerFired);
	}
	[PunRPC]
	public void PlayAnimationIdle()
	{
		this.skeletonAnimation.AnimationState.SetAnimation(0, this.idle, true);
	}
	[PunRPC]
	public void PlayAnimationRun()
	{
		this.skeletonAnimation.AnimationState.SetAnimation(0, this.move, true);
	}
	[PunRPC]
	public void PlayAnimationShoot()
	{
		this.skeletonAnimation.AnimationState.SetAnimation(0, this.shootRifle, false);
	}
	[PunRPC]
	public void SetDirection(bool right)
	{
		if (right)
		{
			transform.localScale = new Vector3(1, 1, 1);

			healthUI.transform.localScale = new Vector3(1, 1, 1);

			inReverse = false;
		}
		else
		{
			transform.localScale = new Vector3(-1, 1, 1);

			healthUI.transform.localScale = new Vector3(-1, 1, 1);

			inReverse = true;
		}
	}

	public void ActiveAim(bool isActive)
	{
		if (isActive)
		{
			if (this.skeletonAnimation.AnimationState.GetCurrent(2) != null && string.Compare(this.skeletonAnimation.AnimationState.GetCurrent(2).animation.name, this.aimRifle) == 0)
			{
				return;
			}
			this.skeletonAnimation.AnimationState.SetAnimation(2, this.aimRifle, false);
		}
		else
		{
			this.skeletonAnimation.AnimationState.SetEmptyAnimation(2, 0f);
		}
	}

	//PhotonStream
	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.IsWriting)
		{
			// We own this player: send the others our data
			stream.SendNext(xAttackDirection);
			stream.SendNext(yAttackDirection);
		}
		else
		{
			// Network player, receive data
			this.xAttackDirection = (float)stream.ReceiveNext();
			this.yAttackDirection = (float)stream.ReceiveNext();
		}
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (PhotonNetwork.IsMasterClient)
		{
			if (collision.gameObject.tag == "Bullet Enemy")
			{
				//Get Bullet
				MP_Bullet bullet = collision.gameObject.GetComponent<MP_Bullet>();
				float randomDamage = bullet.damage;
				string playerFired = bullet.playerFired;

				pv.RPC("Takedamage", RpcTarget.All, randomDamage, playerFired);
			}
		}
	}

	public void ApplyDamage(float damage, string playerFired)
	{
		pv.RPC("Takedamage", RpcTarget.All, damage, playerFired);
	}

	[PunRPC]
	public void Takedamage(float damage, string playerFired)
	{
		if (death || invencible)
		{
			return;
		}
		health -= damage;

		if (health <= 0)
		{
			//this.skeletonAnimation.AnimationState.SetAnimation(0, this.fallBackward, false);

			selectedChar.SetActive(false);
			GetComponent<Rigidbody2D>().simulated = false;

			deathFx.Play();
			deathFx.GetComponent<AudioSource>().Play();

			health = 0;

			if (playerFired != "")
			{
				//Add point for player Fired
				if (isBot)
				{
					GameLeaderboard.instance.AddScrore(playerFired);

					Singleton<Popup>.Instance.ShowToastMessage(playerFired + " Killed " + gameObject.name, ToastLength.Normal);
				}
				else if (playerFired != pv.Owner.NickName)
				{
					GameLeaderboard.instance.AddScrore(playerFired);

					Singleton<Popup>.Instance.ShowToastMessage(playerFired + " Killed " + pv.Owner.NickName, ToastLength.Normal);
				}
				

				if (isBot)
				{
					GameLeaderboard.instance.AddDeath(gameObject.name);
				}
				else
				{
					GameLeaderboard.instance.AddDeath(pv.Owner.NickName);
				}
				
			}

			death = true;

			currentWeapon.gameObject.SetActive(false);
			if (PhotonNetwork.IsMasterClient)
			{
				Invoke("Respawn", timeToRevive);
			}
			
		}

		RefreshHealth();
	}

	[PunRPC] //HUD Manager Call this RPC
	public void ChangeWeapon(int weaponID, string containerName)
	{
		if (!pv)
		{
			pv = GetComponent<PhotonView>();
		}

		for (int i = 0; i < weaponContainer.Length; i++)
		{
			weaponContainer[i].SetActive(false);
		}

		weaponContainer[weaponID].SetActive(true);

		currentWeapon = weaponContainer[weaponID].GetComponent<Mp_Weapon>();
		if (pv.IsMine)
		{
			HudManager.instance.RefreshGranades(granades);

			HudManager.instance.weaponImage.sprite = currentWeapon.weaponImage;

			HudManager.instance.RefreshAmmo(currentWeapon.ammo, currentWeapon.maxAmmo);

			//Set cam zoom
			Camera.main.orthographicSize = currentWeapon.zoomValue;

			currentWeapon.line.SetActive(true);
		}
		else
		{
			currentWeapon.line.SetActive(false);
		}

		if (GameObject.Find(containerName) && containerName != "")
		{
			GameObject.Find(containerName).GetComponent<Mp_ChangeWeapon>().weaponID = oldWeapon;
			GameObject.Find(containerName).GetComponent<Mp_ChangeWeapon>().sprite.sprite = weaponContainer[oldWeapon].GetComponent<Mp_Weapon>().weaponImage;
		}

		attackRate = currentWeapon.attackRate;

		rate = attackRate;

		//Refill Weapon
		currentWeapon.RefillWeapon();

		oldWeapon = weaponID;
	}

	public void RefreshHealth()
	{
		if (pv.IsMine && !isBot)
		{
			HudManager.instance.RefreshHealthBar(health, maxHealth);
		}
		else
		{
			healthUI.maxValue = maxHealth;
			healthUI.value = health;
		}
	}

	public void Respawn()
	{
		pv.RPC("RepawnPlayer", RpcTarget.All);
	}
	[PunRPC]
	public void RepawnPlayer() 
	{
		invencible = true;
		invencibleEffect.SetActive(true);

		transform.position = Launcher.launcher.startPosition[Random.Range(0, Launcher.launcher.startPosition.Length)].position;
		health = maxHealth;
		RefreshHealth();

		jetpackCurrentValue = jetpackValue;

		granades = savedGranades;
		HudManager.instance.RefreshGranades(granades);

		currentWeapon.gameObject.SetActive(true);

		currentWeapon.RefillWeapon();

		currentWeapon.reloading = false;

		this.skeletonAnimation.AnimationState.SetAnimation(0, this.idle, true);

		death = false;

		selectedChar.SetActive(true);

		GetComponent<Rigidbody2D>().simulated = true;

		Invoke("DissableInvencible", invencibleTime);
	}

	void DissableInvencible()
	{
		if (PhotonNetwork.IsMasterClient)
		{
			pv.RPC("DissableInvencibleRPC", RpcTarget.All);
		}
	}
	[PunRPC]
	void DissableInvencibleRPC()
	{
		invencible = false;
		invencibleEffect.SetActive(false);
	}

	public IEnumerator SendLeaderboardRegister(string playerName)
	{
		yield return new WaitForSeconds(1);

		pv.RPC("RegisterInLeaderBoard", RpcTarget.AllBuffered, playerName);
	}
	[PunRPC]
	public void RegisterInLeaderBoard(string playerName)
	{
		if (!GameLeaderboard.instance.ComprobatePlayer(playerName))
		{
			GameLeaderboard.instance.RegisterPlayer(playerName);
		}
		
	}

	public void ArrowIndicator()
	{
		//Comprobate players list
		ComprobatePlayerList();

		if (playersObjs.Count > 1)
		{
			//Get players number
			int playersNumber = playersObjs.Count;

			//Set max player number
			if (playersNumber >= 5)
			{
				playersNumber = 5;
			}

			//Set Transparent all arrows
			for (int i = 0; i < arrows.Length; i++)
			{
				arrows[i].transform.Find("ArrowImage").GetComponent<SpriteRenderer>().color = new Color(0.5f, 0, 0, 0);
			}

			//Activate just player count arrows
			for (int i = 0; i < playersNumber; i++)
			{
				//Calculate distance
				float distance = Vector3.Distance(transform.position, playersObjs[i].transform.position);

				if (distance > minDistance)
				{
					//Enable arrow
					arrows[i].transform.Find("ArrowImage").GetComponent<SpriteRenderer>().color = new Color(0.5f, 0, 0, 0.3f);
				}
				else
				{
					arrows[i].transform.Find("ArrowImage").GetComponent<SpriteRenderer>().color = new Color(0.5f, 0, 0, 1f);
				}


				Vector3 dir = transform.InverseTransformDirection(transform.position - playersObjs[i].transform.position);
				float angle = Mathf.Atan2(-dir.x, dir.y) * Mathf.Rad2Deg;
				arrows[i].eulerAngles = new Vector3(0, 0, angle);
			}
		}
	}

	void ComprobatePlayerList() 
	{
		GameObject [] allPlayers = GameObject.FindGameObjectsWithTag("Player");
		playersObjs.Clear();

		for (int i = 0; i < allPlayers.Length; i++)
		{
			if (!allPlayers[i].GetComponent<PhotonView>().IsMine || allPlayers[i].GetComponent<MP_Player>().isBot)
			{
				playersObjs.Add(allPlayers[i]);
			}
		}
	}
}
