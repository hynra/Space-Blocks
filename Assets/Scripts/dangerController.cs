using UnityEngine;
using System.Collections;

public class dangerController : MonoBehaviour
{
	private playerController player;
	private bool isCountingScore = true;

	void OnBecameInvisible () {
		if (isCountingScore) {
			player.addScore ();
		}
		Destroy (gameObject);
	}

	void Awake(){
		player = GameObject.Find ("Player").GetComponent<playerController>();

		Color newColor = ColorHSV.GetRandomColor(Random.Range(0.0f, 360f), 1, 1);
		gameObject.GetComponent<Renderer> ().material.color = newColor;
		Vector3 pos = gameObject.transform.localScale;
		pos.y *= Random.Range (1.2f, 8.0f);
		gameObject.transform.localScale = pos;
		Vector3 position = transform.position;
		//position.y = -2 + transform.localScale.y / 2;
		transform.position = position;
	}

	public void stopCountingScore(){
		isCountingScore = false;
	}
}
