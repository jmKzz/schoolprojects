using UnityEngine;
using System.Collections;

public class TheDot : MonoBehaviour {
	public KeyCode MouseLeft;

	// Use this for initialization
	void Start (){
		GetComponent<SpriteRenderer> ().color = PaintGM.currentColor;
		GetComponent<Transform> ().localScale = new Vector2 (PaintGM.currentScale, PaintGM.currentScale);

	}
	
	// Update is called once per frame
	void Update () {
		// estää värivalikon ja backnuolen alle piirtämisen 
		if (gameObject.transform.position.x >= 5 ) {
			Destroy (gameObject);
		}
		if (gameObject.transform.position.x <= -4.5 && gameObject.transform.position.x >= -5.9 && gameObject.transform.position.y <= -3.5 && gameObject.transform.position.y >= -4.3) {
			Destroy (gameObject);
		}
	
	}
}
