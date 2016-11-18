using UnityEngine;
using System.Collections;

public class TileBehaviour : MonoBehaviour {

	public static float DISTANCE_BETWEEN_TILES_X = 4.0f;
	public static float DISTANCE_BETWEEN_TILES_Y = 4.0f;

	public 	VOTile 			Data;

	private GoTween 		runningAnimation;

	private TextMesh 		label;
	private	SpriteRenderer	background;

	private bool 			tileHasExpired;

	// Use this for initialization
	void Start () {
		label = transform.Find("Label").GetComponent<TextMesh>();
		background = transform.Find("Background").GetComponent<SpriteRenderer>();

		tileHasExpired = false;
	}

	// Update is called once per frame
	void Update () {
		//states (set by the board manager only) are used to start animations and different visual effects.
		if(Data.state == VOTile.STATE_MOVING) {
			Data.state = VOTile.STATE_ANIMATING;
			
			runningAnimation = Go.to(transform, 0.5f, new GoTweenConfig().localPosition(new Vector3(Data.column * DISTANCE_BETWEEN_TILES_X, Data.row * DISTANCE_BETWEEN_TILES_Y)).setEaseType(GoEaseType.SineInOut));
		}

		if(Data.state == VOTile.STATE_SPAWN) {
			transform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
			
			runningAnimation = Go.to(transform, 0.5f, new GoTweenConfig().scale(1.0f).setEaseType(GoEaseType.BounceOut));
			Data.state = VOTile.STATE_ANIMATING;

			transform.localPosition = new Vector3(Data.column * DISTANCE_BETWEEN_TILES_X, Data.row * DISTANCE_BETWEEN_TILES_Y);
		}

		if(Data.state == VOTile.STATE_ANIMATING) {
			if(runningAnimation.state == GoTweenState.Destroyed) {
				if(tileHasExpired == false) {
					Data.state = VOTile.STATE_IDLE;
				} else {
					Destroy(gameObject);
				}
			}
		}

		if(Data.state == VOTile.STATE_DIE) {
			tileHasExpired = true;
			Data.state = VOTile.STATE_ANIMATING;
			runningAnimation = Go.to(transform, 0.5f, new GoTweenConfig().scale(0.0f));
		}

		if(Data.state == VOTile.STATE_IDLE) {
			transform.localPosition = new Vector3(Data.column * DISTANCE_BETWEEN_TILES_X, Data.row * DISTANCE_BETWEEN_TILES_Y);
		}

		if(Data.value > 0) {
			label.text = Data.value.ToString();

			if(Data.value == 1) {
				background.color = Color.red;
			} else if(Data.value == 2) {
				background.color = Color.cyan;
			} else {
				background.color = Color.white;
			}
		} else {
			label.text = "J";
			background.color = Color.magenta;
		}
	}
}
