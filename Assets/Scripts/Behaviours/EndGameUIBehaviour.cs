using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class EndGameUIBehaviour : MonoBehaviour {

	private Text score;

	// Use this for initialization
	void Start () {
		EventBus.Instance.AddEventListener((int)GameEvents.GAME_OVER, gameOverHandler);
		gameObject.SetActive(false);
		score = transform.FindChild("ScoreDynamic").GetComponent<Text>();
	}

	private void gameOverHandler() {
		gameObject.SetActive(true);

		score.text = BoardManager.Instance.CalculateScore() + " points";
	}

	public void restartGame() {
		EventBus.Instance.DispatchEvent((int)GameEvents.NEW_GAME);
	}

	// Update is called once per frame
	void Update () {
	}
}
