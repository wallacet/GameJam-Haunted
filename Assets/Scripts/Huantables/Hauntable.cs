using UnityEngine;
using System.Collections;
//Removed the abstract, we want to reference it directly.

public class Hauntable : MonoBehaviour {

	public string movetype = "cube";
	public bool isHauntable = true;
	public int health = 100;
	public bool explodes = false;
	/*
	 * cube = Can not move when on the ground. Can hop. Has great air control.
	 * ball = Can roll and move in air.
	 * vehicle = can drive. No air control.
	 * */

	public GameObject Haunt()
	{
		//Since we only have one player haunting, took out the "haunter" param.
		//haunted = true;
		return this.gameObject;
	}
}
