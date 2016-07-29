using UnityEngine;
using System.Collections;

public class FloorController : MonoBehaviour {

	void Awake(){
		Color newColor = ColorHSV.GetRandomColor(Random.Range(0.0f, 360f), 1, 1);
		gameObject.GetComponent<Renderer> ().material.color = newColor;
	}
}
