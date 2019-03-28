using UnityEngine;
using System.Collections;

public class SpriteScriptMuistiPeli : MonoBehaviour {
	
	public static Sprite spriteVar;
	public static GameObject selectedObject;
	public static GameObject selectedObject2;

	private int numero = 0;
	private int luku = 0;
	public static float xDiff;
	public static bool valinta = false;
	public static bool odota = true;
	public static bool tarkistus = false;
	public static bool joku = false;

	private RaycastHit2D hit;													//	TOUCH INPUT VAR
	public static bool isTouching = false;									//	TOUCH INPUT VAR
	public static Vector3 touchPoint;												//	TOUCH INPUT VAR

	void Awake()
	{
		gameMaster3.kuvaLista.Add (gameObject);
		spriteVar = null;
		selectedObject = null;
		selectedObject2 = null;
		xDiff = -4.5f;
	}

	void Update()
	{
	}

	IEnumerator Odotus()
	{
		
		joku = true;
		yield return new WaitForSecondsRealtime (1);
		tarkistus = true;

		if ((valinta) && (odota) && (joku) && (tarkistus))
		{

			if ((selectedObject.name != selectedObject2.name)) {
				VaaraPari ();
			}

			else if ((selectedObject.name == selectedObject2.name)) {
				OikeaPari ();																				//CHECKKAA VALINNAT JA ODOTA BOOL
			}
		}
	}

	void FixedUpdate()
	{
		if (!isTouching) {
			if (!odota) {
				
				if (Input.GetMouseButtonDown (0)) {
					isTouching = true;

					hit = Physics2D.Raycast (Camera.main.ScreenToWorldPoint (Input.mousePosition), Vector2.zero);		// Raycast osoittimen koordinaateista
					touchPoint = Camera.main.ScreenToWorldPoint (Input.mousePosition);

					if (hit != null && hit.collider != null && !valinta && !odota && !tarkistus) {
						Valinta ();
					}
				}
			}
		}

		if (isTouching) {
			if (!odota) {

				if (Input.GetMouseButtonUp (0)) {
					isTouching = false;

					if (valinta && selectedObject2 != null && selectedObject != null && !tarkistus && !odota) {
						Tarkista();
					}
				}
			}
		}
	}

	void Valinta()
	{
		foreach (Sprite spritE in gameMaster3.spriteLista) 
		{
			if (spritE.name == hit.collider.gameObject.name)
			{
				if (!valinta) {
					numero++;
				}

				valinta = false;

				if (numero == 1) 
				{
					selectedObject = hit.collider.gameObject;
					spriteVar = spritE;
					selectedObject.GetComponent<SpriteRenderer> ().sprite = spritE;
					selectedObject.GetComponent<SpriteRenderer> ().sortingOrder = 10;
					selectedObject.GetComponent<BoxCollider2D> ().enabled = false;
				} 

				else if (numero == 2) 
				{
					selectedObject2 = hit.collider.gameObject;

					spriteVar = spritE;
					selectedObject2.GetComponent<SpriteRenderer> ().sprite = spritE;
					selectedObject2.GetComponent<SpriteRenderer> ().sortingOrder = 10;
					selectedObject2.GetComponent<BoxCollider2D> ().enabled = false;
					valinta = true;
				}
			}
		}
	}



	void Tarkista()
	{
		odota = true;
		StartCoroutine (Odotus ());
	}

	void OikeaPari()
	{
		gameMaster3.kuvaLista.Remove (selectedObject);
		gameMaster3.kuvaLista.Remove (selectedObject2);
		selectedObject.GetComponent<SpriteRenderer> ().sortingOrder = luku;
		luku += 1;
		selectedObject2.GetComponent<SpriteRenderer> ().sortingOrder = luku;
		selectedObject.transform.position = new Vector2 (xDiff, -4.85f);
		xDiff += 0.3f;
		selectedObject2.transform.position = new Vector2 (xDiff, -4.85f);
		xDiff += 0.75f;
		luku++;
		gameMaster3.spriteLista.Remove (spriteVar);
		numero = 0;

		selectedObject = null;
		selectedObject2 = null;
		spriteVar = null;
		tarkistus = false;
		valinta = false;
		joku = false;

		if (gameMaster3.kuvaLista.Count > 0) {
			odota = false;
		}
	}

	void VaaraPari()
	{
		selectedObject.GetComponent<SpriteRenderer> ().sprite = gameMaster3.kortti;
		selectedObject2.GetComponent<SpriteRenderer> ().sprite = gameMaster3.kortti;
		selectedObject.GetComponent<SpriteRenderer> ().sortingOrder = 0;
		selectedObject2.GetComponent<SpriteRenderer> ().sortingOrder = 0;
		selectedObject.GetComponent<BoxCollider2D> ().enabled = true;
		selectedObject2.GetComponent<BoxCollider2D> ().enabled = true;
		numero = 0;

		selectedObject = null;
		selectedObject2 = null;
		spriteVar = null;
		tarkistus = false;
		valinta = false;
		joku = false;
		odota = false;
	}
}
