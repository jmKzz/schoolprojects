using UnityEngine;
using System.Collections;

public class SpriteScriptNew : MonoBehaviour {

	//public Sprite kuva;
	public Texture2D source;
	public static SpriteRenderer sr;
	private int luku = 1;
	private string LevelPicName = "";

	void Start ()
	{
		//Palapeliin kuvan leikkaus ja kuvan palojen asettelu Pieceille
		SpriteRenderer sr = gameObject.AddComponent<SpriteRenderer> ();

		LevelPicName = "kuva" + Levelselect.whichlevel;
		source = Resources.Load (LevelPicName, typeof(Texture2D)) as Texture2D;

		GetComponent<Renderer> ().material.mainTexture = source;
		GetComponent<SpriteRenderer> ().sortingOrder = 2;


		float SliceHeight = source.height/3;
		float SliceWidth = source.width/4;


		for (float i = 0f; i < source.width; i += SliceWidth)
		{

			for (float j = source.height; j > 0f; j -= SliceHeight) 
			{
				

				Sprite uusiSprite = Sprite.Create (source, new Rect (i, j - SliceHeight, SliceWidth, SliceHeight), new Vector2 (0.5f, 0.5f), 256f);
				uusiSprite.name = ("A" + luku);
				luku++;

				if (uusiSprite.name == gameObject.name) 
				{
					sr.sprite = uusiSprite;
				}
	
				transform.localScale = new Vector3 (1.25f, 1.04f, 0f); 
			}
		}
	}
}
