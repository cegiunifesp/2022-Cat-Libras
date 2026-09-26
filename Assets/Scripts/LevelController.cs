using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Cinemachine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    [SerializeField] private int Score = 100;
    [SerializeField] private int scoreRate;
    [Space(15)]
    [SerializeField] private int collectSequence;
    public int minRangeTimeLetter;
    public int maxRangeTimeLetter;
    [Space(5)]
    public int maxTimesWithoutLetterTarget;
    [Space (5)]
    [SerializeField] private int Lives = 3;
    [SerializeField] private GameObject[] hearts;
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI scoreField;
    [SerializeField] private TextMeshProUGUI currentLetterField;
    [Header ("Objetos")]
    [SerializeField] private GameObject gameCanvas;
    [SerializeField] private GameObject loseCanvas;
    [SerializeField] private GameObject tipCanvas;
    [SerializeField] private GameObject tutorialCanvas;
    [SerializeField] private PlayerController cat;
    [SerializeField] private GameObject menuCanvas;
    [SerializeField] private GameObject spawner;
    [SerializeField] private TextMeshProUGUI loseScore;
    [SerializeField] private GameObject checkCollider;
    [Header("Animator")]
    [SerializeField] private Animator letterAnimator;
    [Header("Audio")]
    [SerializeField] private AudioClip correctCollect;
    [SerializeField] private AudioClip wrongCollect;
    [SerializeField] private AudioClip looseGame;

    private float _timer = 0.5f;
    private List<int> usedLetters = new List<int>();
    private CinemachineImpulseSource _impulse;
    private AudioSource _audioSource;

    private int _currentScore;
    private int _currentLives;

    private SinalsSpawner _sinalsSpawner;

    public char CurrentLetter { get; private set; } = 'A';

    // saber está ou não no menu
    private bool isPlaying = false;

    // posição inicial
    private Vector2 _startPosition;

    //contador de acertos
    [Space(15)]
    public int letterCount;

    private void Start()
    {
        // salvar posicao inicial
        _startPosition = cat.transform.position;

        letterCount = 0;

        SetMenuMode();

        _sinalsSpawner = spawner.GetComponent<SinalsSpawner>();
        _impulse = GetComponent<CinemachineImpulseSource>();
        _audioSource = GetComponent<AudioSource>();
    }

    private void InitiliazeVars()
    {
        cat.transform.position = _startPosition;


        _sinalsSpawner.ResetSpeed();
        _currentScore = Score;
        _currentLives = Lives;

        _timer = 1f;
    }

    private void DestroyAllCollectables()
    {
        List<CollectableSignal> collectables = new List<CollectableSignal>();
        collectables.AddRange(FindObjectsOfType<CollectableSignal>());
        collectables.ForEach(n => n.Disable());
    }

    public void SetMenuMode()
    {
        isPlaying = false;
        cat.DisablePlayer();
        spawner.SetActive(false);
        menuCanvas.SetActive(true);
        tutorialCanvas.SetActive(false);
        loseCanvas.SetActive(false);
        gameCanvas.SetActive(false);
        tipCanvas.SetActive(true);
    }

    public void SetPlayMode()
    {
        isPlaying = true;
        menuCanvas.SetActive(false);
        tipCanvas.SetActive(false);
        loseCanvas.SetActive(false);
        tutorialCanvas.SetActive(false);
        gameCanvas.SetActive(true);
        cat.EnablePlayer();
        spawner.SetActive(true);

        RandomNewLetter();
        UpdateHeartUI();
    }

    public void SetLooseMode()
    {
        isPlaying = false;
        checkCollider.SetActive(false);
        loseScore.text = $"SEUS PONTOS:\n{_currentScore}";

        gameCanvas.SetActive(false);
        menuCanvas.SetActive(false);
        tipCanvas.SetActive(true);
        loseCanvas.SetActive(true);

        spawner.GetComponent<SinalsSpawner>().DestroyAllChildren();

        cat.Die();
        spawner.SetActive(false);

        _audioSource.PlayOneShot(looseGame);
    }

    public void AddScore()
    {
        _currentScore += 100;
    }

    private void Update()
    {
        if(isPlaying)
            DecreaseScorePerSecond();

        scoreField.text = $"PONTOS: {_currentScore}";
    }


    private void DecreaseScorePerSecond()
    {
        _timer -= Time.deltaTime;

        if (_timer <= 0)
        {
            _currentScore+=scoreRate;
            _timer = 0.5f;
        }
    }

    private void RandomNewLetter()
    {
        int letter = Random.Range(65, 91);
        while (usedLetters.Contains(letter))
            letter = Random.Range(65, 91);

        usedLetters.Add(letter);
        if (usedLetters.Count >= 26)
            usedLetters.Clear();

        CurrentLetter = (char)letter;
        currentLetterField.text = CurrentLetter.ToString();
        maxTimesWithoutLetterTarget = Random.Range(minRangeTimeLetter, maxRangeTimeLetter);
    }

    public void CollectLetter(CollectableSignal collectable)
    {
        if(collectable.Letter == CurrentLetter)
        {
            AddScore();
            letterAnimator.SetTrigger("Change");
            RandomNewLetter();
            _audioSource.PlayOneShot(correctCollect);
            if(_currentLives != 5) 
                letterCount++;
            if(letterCount >= collectSequence)
            {
                letterCount = 0;
                _currentLives++;
                UpdateHeartUI();
            }
        }
        else
        {
            WrongLetter();
        }
    }

    public void WrongLetter()
    {
        _currentLives--;
        _audioSource.PlayOneShot(wrongCollect);
        if (_currentLives <= 0)
        {
            // perder
            SetLooseMode();
        }

        UpdateHeartUI();
        _impulse.GenerateImpulse();
    }

    private void UpdateHeartUI()
    {
        for(int i=0; i < hearts.Length; i++)
            hearts[i].SetActive(_currentLives > i);
    }

    public void ReloadGame()
    {
        StartGame();
    }

    // botao MENU
    public void GoToMenu()
    {
        SceneManager.LoadScene(0);
    }

    // botao INICIAR
    public void StartGame()
    {
        checkCollider.SetActive(true);
        DestroyAllCollectables();
        InitiliazeVars();
        SetPlayMode();
    }

    // botao SAIR
    public void QuitGame()
    {

        Application.Quit();
    }

    public void ShowTutorial()
    {
        menuCanvas.SetActive(false);
        tutorialCanvas.SetActive(true);
    }

    public void ChangeSpeed(float val)
    {
        cat.speed += val;
    }

    public void SetSpeed(float val)
    {
        cat.speed = val;
    }

    public void PausePlayGame(bool value)
    {
        if (value)
        {
            PauseGame();
        }
        else
        {
            UnpauseGame();
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
    }

    public void UnpauseGame()
    {
        Time.timeScale = 1;
    }
}
