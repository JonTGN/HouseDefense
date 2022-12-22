﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellEject : MonoBehaviour {
	
[SerializeField] private Rigidbody bulletCasing;
public int ejectSpeed = 20;
[SerializeField] private float fireRate = 0.1f;
private float nextFire = 0.0f;
public WeaponClass weapon;


// Update is called once per frame
void Update () {
 
if (weapon.shooting && Time.time > nextFire) {
nextFire =  Time.time + fireRate;

Rigidbody clone;
 
clone = Instantiate(bulletCasing, transform.position, transform.rotation);
clone.velocity = transform.TransformDirection(-2,Random.Range(2,3),Random.Range(0,-1));
}
}
}
