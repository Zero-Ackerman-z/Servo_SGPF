﻿using UnityEngine;
using System.Collections;

public class RCCWheelSkidmarks : MonoBehaviour {
	
	
	public RCCCarControllerV2 vehicle;
	private Rigidbody vehicleRigid;
	private float startSlipValue = .25f;
	private RCCSkidmarks skidmarks = null;
	private int lastSkidmark = -1;
	private WheelCollider wheel_col;
	
	private float wheelSlipAmountSideways;
	private float wheelSlipAmountForward;
	
	void  Start (){

		wheel_col = GetComponent<WheelCollider>();
		vehicleRigid = vehicle.GetComponent<Rigidbody>();

		if(FindObjectOfType(typeof(RCCSkidmarks)))
			skidmarks = FindObjectOfType(typeof(RCCSkidmarks)) as RCCSkidmarks;
		else
			Debug.Log("No skidmarks object found. Skidmarks will not be drawn. Drag ''SkidmarksManager'' from Prefabs folder, and drop on to your existing scene...");

	}
	
	void  FixedUpdate (){

		if(skidmarks){
			WheelHit GroundHit;
			wheel_col.GetGroundHit(out GroundHit);
			
			wheelSlipAmountSideways = Mathf.Abs(GroundHit.sidewaysSlip);
			wheelSlipAmountForward = Mathf.Abs(GroundHit.forwardSlip);
			
			if ( wheelSlipAmountSideways > startSlipValue || wheelSlipAmountForward > .5f){
				
				Vector3 skidPoint = GroundHit.point + 2f * (vehicleRigid.velocity) * Time.deltaTime;

				if(vehicleRigid.velocity.magnitude > 1f)
					lastSkidmark = skidmarks.AddSkidMark(skidPoint, GroundHit.normal, (wheelSlipAmountSideways / 2f) + (wheelSlipAmountForward / 2.5f), lastSkidmark);
				else
					lastSkidmark = -1;

			}

			else{

				lastSkidmark = -1;

			}
			
		}

	}
}