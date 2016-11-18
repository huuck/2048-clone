using UnityEngine;
using System.Collections.Generic;

public class BoardBehaviour : MonoBehaviour {

	private static	int				STATE_PRE_GAME		= 	0;
	private static	int				STATE_INIT_BOARD	= 	1;
	private static	int				STATE_IDLE 			= 	2;
	private static	int				STATE_SPAWN_TILES	= 	3;
	private static	int				STATE_MOVE_TILES 	= 	4;

	public 			Transform 		tilePrefab;

	private			StateMachine	stateMachine;

	private			SwipeDetector	swipeDetector;

	// Use this for initialization
	void Start () {
		stateMachine = new StateMachine(false);
		BoardManager.Instance.Init();

		stateMachine.AddState(STATE_PRE_GAME, 		new List<int>() {STATE_IDLE}, 								preGameEnterHandler, 	preGameUpdateHandler, 			preGameExitHandler);
		stateMachine.AddState(STATE_INIT_BOARD, 	new List<int>() {STATE_PRE_GAME}, 							initBoardEnterHandler, 	initBoardUpdateHandler, 		initBoardExitHandler);
		stateMachine.AddState(STATE_IDLE, 			new List<int>() {STATE_INIT_BOARD, STATE_SPAWN_TILES}, 		idleEnterHandler, 		idleUpdateHandler, 				idleExitHandler);
		stateMachine.AddState(STATE_SPAWN_TILES, 	new List<int>() {STATE_IDLE, STATE_MOVE_TILES}, 			spawnTilesEnterHandler, spawnTilesUpdateHandler, 		spawnTilesExitHandler);
		stateMachine.AddState(STATE_MOVE_TILES, 	new List<int>() {STATE_IDLE}, 								moveTilesEnterHandler, 	moveTilesUpdateHandler, 		moveTilesExitHandler);

		stateMachine.SetInitialState(STATE_PRE_GAME);

		//deterministic (if needed) random number generator
		Random.seed = (int)System.DateTime.Now.Ticks;

		transform.position = new Vector3(-BoardManager.Instance.NumColumns * TileBehaviour.DISTANCE_BETWEEN_TILES_X / 2.0f + TileBehaviour.DISTANCE_BETWEEN_TILES_X * 0.5f, -BoardManager.Instance.NumRows * TileBehaviour.DISTANCE_BETWEEN_TILES_Y / 2.0f + TileBehaviour.DISTANCE_BETWEEN_TILES_Y * 0.5f);

		swipeDetector = GetComponent<SwipeDetector>();
	}

	public void StartGame() {
		stateMachine.ChangeState(STATE_INIT_BOARD);
	}

	//init board state handlers
	private void preGameEnterHandler() {
		BoardManager.Instance.Init();
		foreach (Transform child in transform) {
			Destroy(child.gameObject);
		}
	}
	
	private void preGameUpdateHandler() {
			
	}
	
	private void preGameExitHandler() {
	}

	//init board state handlers
	private void initBoardEnterHandler() {
		List<VOTile> spawnedTiles = BoardManager.Instance.SpawnTiles(Mathf.RoundToInt(Random.Range(5, 8)));
		int i;
		Transform newTile;
		for(i = 0; i < spawnedTiles.Count; i++) {
			newTile = Instantiate(tilePrefab);
			newTile.GetComponent<TileBehaviour>().Data = spawnedTiles[i];
			newTile.transform.SetParent(transform);
		}
	}
	
	private void initBoardUpdateHandler() {
		if(BoardManager.Instance.IsBoardAnimating() == false) {
			stateMachine.ChangeState(STATE_IDLE);
		}
	}
	
	public void initBoardExitHandler() {
	}

	//idle state handlers
	private void idleEnterHandler() {
		//checks if there's any more moves possible
		if(BoardManager.Instance.AreMovesAvailable() == false) {
			EventBus.Instance.DispatchEvent((int)GameEvents.GAME_OVER);
			stateMachine.ChangeState (STATE_PRE_GAME);
		}
	}

	private void idleUpdateHandler() {
		int action = swipeDetector.GetLatestAction();

		if(action != (int)SwipeDirection.NONE) {
			stateMachine.ChangeState(STATE_MOVE_TILES);
		}
	}

	private void idleExitHandler() {
	}

	//animating state handlers
	private void animatingEnterHandler() {
	}
	
	private void animatingUpdateHandler() {
		if(BoardManager.Instance.IsBoardAnimating() == false) {
			stateMachine.ChangeState(STATE_IDLE);
		}
	}
	
	private void animatingExitHandler() {
	}

	//spawn tiles handlers
	private void spawnTilesEnterHandler() {
		List<VOTile> spawnedTiles = BoardManager.Instance.SpawnTiles(1);
		int i;
		Transform newTile;
		if (spawnedTiles.Count > 0) {
			for (i = 0; i < spawnedTiles.Count; i++) {
				newTile = Instantiate (tilePrefab);
				newTile.GetComponent<TileBehaviour> ().Data = spawnedTiles [i];
				newTile.transform.SetParent (transform);
			}
		}
	}
	
	private void spawnTilesUpdateHandler() {
		if (BoardManager.Instance.IsBoardAnimating () == false) {
			stateMachine.ChangeState(STATE_IDLE);
		}
	}
	
	private void spawnTilesExitHandler() {
	}

	//move tiles handlers
	private void moveTilesEnterHandler() {
		BoardManager.Instance.MoveTiles(swipeDetector.GetLatestAction());
		swipeDetector.FlushLatestAction();
	}
	
	private void moveTilesUpdateHandler() {
		if (BoardManager.Instance.IsBoardAnimating () == false) {
			stateMachine.ChangeState(STATE_SPAWN_TILES);
		}
	}
	
	private void moveTilesExitHandler() {
	}

	void Update () {
		stateMachine.Update();
	}
}
