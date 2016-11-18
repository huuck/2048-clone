using UnityEngine;
using System.Collections;

public class SwipeDetector : MonoBehaviour {
	private float 	fingerStartTime  = 0.0f;
	private Vector2 fingerStartPos = Vector2.zero;
	
	private bool 	isSwipe = false;
	private float 	minSwipeDist  = 50.0f;
	private float 	maxSwipeTime = 0.5f;

	private int 	latestAction;

	private bool 	debug = true;

	void Update () {
		if (Input.touchCount > 0 && latestAction == (int)SwipeDirection.NONE){
			foreach (Touch touch in Input.touches) {
				switch (touch.phase) {
					case TouchPhase.Began :
						/* this is a new touch */
						isSwipe = true;
						fingerStartTime = Time.time;
						fingerStartPos = touch.position;
						break;
						
					case TouchPhase.Canceled :
						/* The touch is being canceled */
						isSwipe = false;
						break;
						
					case TouchPhase.Ended :
						float gestureTime = Time.time - fingerStartTime;
						float gestureDist = (touch.position - fingerStartPos).magnitude;
						
						if (isSwipe && gestureTime < maxSwipeTime && gestureDist > minSwipeDist){
							Vector2 direction = touch.position - fingerStartPos;
							Vector2 swipeType = Vector2.zero;
							
							if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y)){
								// the swipe is horizontal:
								swipeType = Vector2.right * Mathf.Sign(direction.x);
							}else{
								// the swipe is vertical:
								swipeType = Vector2.up * Mathf.Sign(direction.y);
							}
							
							if(swipeType.x != 0.0f){
								if(swipeType.x > 0.0f){
									latestAction = (int)SwipeDirection.RIGHT;
								}else{
									latestAction = (int)SwipeDirection.LEFT;
								}
							}
							
							if(swipeType.y != 0.0f ){
								if(swipeType.y > 0.0f){
									latestAction = (int)SwipeDirection.UP;
								}else{
									latestAction = (int)SwipeDirection.DOWN;
								}
							}
						}
						
						break;
				}
			}
		}

		if(debug == true && latestAction == (int)SwipeDirection.NONE) {
			if(Input.GetKeyDown(KeyCode.LeftArrow) 	== true) latestAction = (int)SwipeDirection.LEFT;
			if(Input.GetKeyDown(KeyCode.RightArrow) == true) latestAction = (int)SwipeDirection.RIGHT;
			if(Input.GetKeyDown(KeyCode.UpArrow) 	== true) latestAction = (int)SwipeDirection.UP;
			if(Input.GetKeyDown(KeyCode.DownArrow) 	== true) latestAction = (int)SwipeDirection.DOWN;
		}
	}

	public int GetLatestAction() {
		return latestAction;
	}

	public void FlushLatestAction() {
		latestAction = (int)SwipeDirection.NONE;
	}
}