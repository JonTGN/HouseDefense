﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {
	
[SerializeField] private Rigidbody projectilePrefab;
[SerializeField] private Transform barrelEnd;

void Update()
{
	Rigidbody rocketInstance;
	rocketInstance = Instantiate(projectilePrefab, barrelEnd.position, barrelEnd.rotation) as Rigidbody;
	rocketInstance.AddForce(barrelEnd.forward * 5000);
	

}
}
