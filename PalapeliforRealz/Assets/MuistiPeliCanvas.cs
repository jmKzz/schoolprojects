using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MuistiPeliCanvas : MonoBehaviour {

	public CanvasGroup canvas;
	public static bool EndGame = false;

	void Awake()
	{
		canvas = GetComponent<CanvasGroup> ();
		canvas.alpha = 0;
	}

	// Use this for initialization
	void Start () {
		EndGame = false;
	
	}
	
	// Update is called once per frame
	void Update () {
		if (EndGame == false) {
			if (gameMaster3.kuvaLista.Count <= 0) {
				StartCoroutine (canvasFunktio ());	
			}
		}
	}

	IEnumerator canvasFunktio()
	{
		canvas.alpha = 1;
		yield return new WaitForSecondsRealtime (2);

		gameMaster3.spriteLista.Clear ();
		gameMaster3.kuvaLista.Clear ();
		canvas.alpha = 0;
		SpriteScriptMuistiPeli.odota = false;
		SceneManager.LoadScene ("GameMainMenu");
	}
}
