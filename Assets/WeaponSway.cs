using UnityEngine;
using System.Collections;

public class WeaponSway : MonoBehaviour {

	private float timer = 0.0f;
	private float timerX = 0.0f;
	public float bobbingSpeed = 0.18f;
	public float bobbingSpeedX = 0.18f;
	public float bobbingAmount = 0.01f;
	public float bobbingAmountX = 0.01f;
	public float midpoint = -0.2f;
	public float midpointX = 0.1f;
	private float waveslice;
	private float wavesliceX;
	private float horizontal;
	private float vertical;
	private float totalAxes;
	private float totalAxesX;
	private float translateChange;
	private float translateChangeX;

	void Start(){

	}

	void FixedUpdate () {
		waveslice = 0.0f;
		wavesliceX = 0.0f;
		horizontal = Input.GetAxis("Horizontal");
		vertical = Input.GetAxis("Vertical");

		if (Mathf.Abs(horizontal) == 0 && Mathf.Abs(vertical) == 0) {
			timer = 0.0f;
			timerX = 0.0f;
		}
		else {
			waveslice = Mathf.Sin(timer);
			timer = timer + bobbingSpeed;
			if (timer > Mathf.PI * 2) {
				timer = timer - (Mathf.PI * 2);
			}

			wavesliceX = Mathf.Sin(timerX);
			timerX = timerX + bobbingSpeedX;
			if (timerX > Mathf.PI * 2){
				timerX = timerX - (Mathf.PI * 2);
			}
		}

		if (waveslice != 0) {
			translateChange = waveslice * bobbingAmount;
			translateChangeX = wavesliceX * bobbingAmountX;
			totalAxes = Mathf.Abs(horizontal) + Mathf.Abs(vertical);
			totalAxes = Mathf.Clamp (totalAxes, 0.0f, 1.0f);
			totalAxesX = Mathf.Abs(horizontal) + Mathf.Abs(vertical);
			totalAxesX = Mathf.Clamp (totalAxesX, 0.0f, 1.0f);
			translateChange = totalAxes * translateChange;
			translateChangeX = totalAxesX * translateChangeX;
			transform.localPosition = new Vector3(midpointX + translateChangeX,
			                                      midpoint + translateChange,
			                                      transform.localPosition.z);
		}
		else {
			transform.localPosition = new Vector3(midpointX,
			                                      midpoint,
			                                      transform.localPosition.z);
		}
	}
}
