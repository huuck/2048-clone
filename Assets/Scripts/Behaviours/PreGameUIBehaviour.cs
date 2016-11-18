using UnityEngine.UI;
using UnityEngine;
using System.Collections;

public class PreGameUIBehaviour : MonoBehaviour {

	private InputField 	numColumns;
	private InputField 	numRows;
	private Slider 		jokerChanceToSpawn;
	private Text		jokerSliderValue;

	// Use this for initialization
	void Start () {
		EventBus.Instance.AddEventListener((int)GameEvents.NEW_GAME, newGameHandler);

		numColumns = transform.Find("NumColumnsSelector").Find("InputNumColumns").GetComponent<InputField>();
		numRows = transform.Find("NumRowsSelector").Find("InputNumRows").GetComponent<InputField>();
		jokerSliderValue = transform.Find("JokerSelector").Find("SliderValue").GetComponent<Text>();
		jokerChanceToSpawn = transform.Find("JokerSelector").Find("Slider").GetComponent<Slider>();

		jokerChanceToSpawn.onValueChanged.AddListener(updateJokerSliderValue);
	}

	public void SetGameParameters() {
		BoardManager.Instance.NumRows = int.Parse(numRows.text);
		BoardManager.Instance.NumColumns = int.Parse(numColumns.text);
		BoardManager.Instance.ChanceToSpawnJoker = jokerChanceToSpawn.value;
	}

	private void updateJokerSliderValue(float value) {
		jokerSliderValue.text = (value * 100).ToString("F1") + "%";
	}

	private void newGameHandler() {
		gameObject.SetActive(true);
	}

	// Update is called once per frame
	void Update () {
	
	}
}
