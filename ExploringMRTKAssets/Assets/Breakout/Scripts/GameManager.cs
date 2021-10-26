using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
  public GameObject GameContent;
  public GameObject BallPrefab;
  public GameObject PlayerPrefab;
  public GameObject GamePlayUi;
  public GameObject LevelCompleteDisplay;
  public GameObject GameOverDisplay;
  public GameObject StartButton;

  public GameObject[] Levels;

  public Text ScoreText;
  public Text BallsText;
  public Text LevelText;

  public static GameManager Instance { get; private set; }

  private Transform _contentParent;
  private GameObject _currentBall;
  private GameObject _currentLevel;

  private int _score;
  public int Score {
    get { return _score; }
    set { 
      _score = value;
      ScoreText.text = $"SCORE: {_score}";
    }
  }

  private int _level;
  public int Level {
    get { return _level; }
    set { 
      _level = value; 
      LevelText.text = $"LEVEL: {_level}";
    }
  }

  private int _balls;
  public int Balls {
    get { return _balls; }
    set { 
      _balls = value; 
      BallsText.text = $"BALLS: {_balls}";
    }
  }

  public enum State {
    Menu,
    Init,
    Play,
    LevelCompleted,
    LoadLevel,
    GameOver
  }

  private State _state;
  bool _isSwitchingState;

  private void Start() {
    _contentParent = GameContent.transform;
    Instance = this;
    SwitchState(State.Menu);
  }

  public void SwitchState(State newState, float delay = 0) {
    _isSwitchingState = true;
    StartCoroutine(SwitchDelay(newState, delay));
  }

  private IEnumerator SwitchDelay(State newState, float delay) {
    yield return new WaitForSeconds(delay);
    EndState();
    _state = newState;
    BeginState(newState);
    _isSwitchingState = false;
  }

  private void BeginState(State newState) {
    switch (newState) {
      case State.Menu:
        StartButton.SetActive(true);
        break;
      case State.Init:
        GamePlayUi.SetActive(true);
        Score = 0;
        Level = 0;
        Balls = 3;
        var player = Instantiate(PlayerPrefab);
        player.transform.SetParent(_contentParent, false);
        SwitchState(State.LoadLevel);
        break;
      case State.Play:
        break;
      case State.LevelCompleted:
        LevelCompleteDisplay.SetActive(true);
        break;
      case State.LoadLevel:
        if(Level >= Levels.Length) {
          SwitchState(State.GameOver);
        } else {
          _currentLevel = Instantiate(Levels[Level]);
          _currentLevel.transform.SetParent(_contentParent, false);
          SwitchState(State.Play);
        }
        break;
      case State.GameOver:
        GameOverDisplay.SetActive(true);
        break;
    }
  }

  private void Update() {
    switch (_state) {
      case State.Menu:
        break;
      case State.Init:
        break;
      case State.Play:
        if(_currentBall == null) {
          if(Balls > 0) {
            _currentBall = Instantiate(BallPrefab);
            _currentBall.transform.SetParent(_contentParent, false);
          } else {
            SwitchState(State.GameOver);
          }
        }
        break;
      case State.LevelCompleted:
        break;
      case State.LoadLevel:
        break;
      case State.GameOver:
        break;
    }
  }

  private void EndState() {
    switch (_state) {
      case State.Menu:
        StartButton.SetActive(false);
        break;
      case State.Init:
        break;
      case State.Play:
        break;
      case State.LevelCompleted:
        LevelCompleteDisplay.SetActive(false);
        break;
      case State.LoadLevel:
        break;
      case State.GameOver:
        StartButton.SetActive(false);
        GameOverDisplay.SetActive(false);
        break;
    }
  }

  public void OnStartPressed() {
    SwitchState(State.Init);
  }
}
