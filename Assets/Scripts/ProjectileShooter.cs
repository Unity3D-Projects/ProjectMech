﻿using UnityEngine;
using System.Collections;

public class ProjectileShooter : MonoBehaviour {
	public GameObject tankGunA;
	public GameObject tankGunB;
	private GameObject gun;

	GameObject prefab;
	public float projectileSpeed;
	private float rayDebugTime = 5F;
	private float maxRayDistance = 1000000F;

	public float timeBetweenShots = 1.0F;
	private bool fromGunA = true;
	private bool canShoot = true;

	public GameObject muzzleFlashPrefab;
	public Renderer muzzleFlash;
	public Light muzzleLight;
	private float muzzleFlashTime = 0.02F;

	private AudioSource shotSound;

	void Start () {
        Screen.lockCursor = true;
        Cursor.visible = true;
        prefab = Resources.Load("projectileWithSmoke") as GameObject;

		muzzleFlash.enabled = false;
		muzzleLight.enabled = false;

		shotSound = GetComponent<AudioSource> ();
    }

	void Update () {
		if (canShoot) {
			if (Input.GetMouseButton (0)) {
				Ray ray = new Ray ();
				ray.origin = transform.position;
				ray.direction = transform.forward;
				RaycastHit hit;


				if (Physics.Raycast (ray, out hit, maxRayDistance)) {
					Debug.DrawRay (ray.origin, ray.direction * maxRayDistance, Color.green, rayDebugTime);


					gun = (fromGunA ? tankGunA : tankGunB);

					GameObject projectile = Instantiate (prefab) as GameObject;
					projectile.transform.position = gun.transform.position + gun.transform.forward;
					projectile.transform.LookAt (hit.point);

					Rigidbody rb = projectile.GetComponent<Rigidbody> ();
					rb.velocity = projectile.transform.forward * projectileSpeed;
					Debug.DrawRay (gun.transform.position, gun.transform.forward * maxRayDistance, Color.red, rayDebugTime);

					fromGunA = !fromGunA;
					StartCoroutine (chamberShot ());
				}
			}
		}
	}
	IEnumerator chamberShot() {
		canShoot = false;
		StartCoroutine (MuzzleFlash ());
		ShotSound ();
		yield return new WaitForSeconds (timeBetweenShots);
		canShoot = true;
	}
	IEnumerator MuzzleFlash() {
		muzzleFlashPrefab.transform.position = gun.transform.position;
		muzzleFlashPrefab.transform.Rotate (new Vector3(0, 0, 10));
		muzzleFlash.enabled = true;
		muzzleLight.enabled = true;
		yield return new WaitForSeconds (muzzleFlashTime);
		muzzleFlash.enabled = false;
		muzzleLight.enabled = false;
	}
	void ShotSound() {
		shotSound.Play ();
	}
}
