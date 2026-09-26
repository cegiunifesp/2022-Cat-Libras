using UnityEngine;

public class SinalsSpawner : MonoBehaviour
{
    [SerializeField] private int maxTimesWithoutLetterTarget = 5;
    [SerializeField] private float xPosition = -10;
    [SerializeField] private float minY = -5;
    [SerializeField] private float maxY = 5;
    [SerializeField] private float minDelay = 0.5f;
    [SerializeField] private float maxDelay = 1.5f;
    [Header("Game Speed")]
    [SerializeField] private float time;
    [SerializeField] private float gameSpeed;
    [SerializeField] private float maxGameSpeed;
    [SerializeField] private float timeIncreaseRate;
    [SerializeField] private float speedIncreaseRate;
    [Header("Prefabs for Signals")]
    public GameObject A_signal;
    public GameObject B_signal;
    public GameObject C_signal;
    public GameObject D_signal;
    public GameObject E_signal;
    public GameObject F_signal;
    public GameObject G_signal;
    public GameObject H_signal;
    public GameObject I_signal;
    public GameObject J_signal;
    public GameObject K_signal;
    public GameObject L_signal;
    public GameObject M_signal;
    public GameObject N_signal;
    public GameObject O_signal;
    public GameObject P_signal;
    public GameObject Q_signal;
    public GameObject R_signal;
    public GameObject S_signal;
    public GameObject T_signal;
    public GameObject U_signal;
    public GameObject V_signal;
    public GameObject W_signal;
    public GameObject X_signal;
    public GameObject Y_signal;
    public GameObject Z_signal;

    private int lastLetter;
    private int notTargetTimes;
    private float lastTimespawned;
    private float currentDelay;

    private LevelController _levelController;

    private void Awake()
    {
        _levelController = FindObjectOfType<LevelController>();

    }

    private void Update()
    {
        if (gameSpeed < maxGameSpeed)
            time += Time.deltaTime;
        if(time >= timeIncreaseRate)
        {
            SetGameSpeed(gameSpeed += speedIncreaseRate);
            time = 0.0f;
        }
            
        if (Time.time - lastTimespawned > currentDelay)
        {
            RandomAndSpawnSignal();
            lastTimespawned = Time.time;
            currentDelay = Random.Range(minDelay, maxDelay);
        }
    }

    private void RandomAndSpawnSignal()
    {
        maxTimesWithoutLetterTarget = _levelController.maxTimesWithoutLetterTarget;

        int letter = Random.Range(65, 91);

        while (letter == lastLetter)
            letter = Random.Range(65, 91);

        if (letter != _levelController.CurrentLetter)
        {
            notTargetTimes++;
            if (notTargetTimes >= maxTimesWithoutLetterTarget)
            {
                letter = _levelController.CurrentLetter;
                notTargetTimes = 0;
            }
        }
        else
            notTargetTimes = 0;

        lastLetter = letter;

        SpawnSignal((char)letter);
    }

    public void SpawnSignal(char letter)
    {
        GameObject toSpawn = GetPrefab(letter);
        if (toSpawn == null) return; // não faça nada se não encontrou um prefab

        // spawn the letter
        float yPosition = Random.Range(minY, maxY);
        GameObject spawned = Instantiate(toSpawn);
        spawned.transform.parent = gameObject.transform;
        spawned.transform.position = new Vector2(xPosition, yPosition);
        spawned.GetComponent<CollectableSignal>().speed = gameSpeed;
    }

    public GameObject GetPrefab(char letter)
    {
        switch (letter)
        {
            case 'A':
                return A_signal;
            case 'B':
                return B_signal;
            case 'C':
                return C_signal;
            case 'D':
                return D_signal;
            case 'E':
                return E_signal;
            case 'F':
                return F_signal;
            case 'G':
                return G_signal;
            case 'H':
                return H_signal;
            case 'I':
                return I_signal;
            case 'J':
                return J_signal;
            case 'K':
                return K_signal;
            case 'L':
                return L_signal;
            case 'M':
                return M_signal;
            case 'N':
                return N_signal;
            case 'O':
                return O_signal;
            case 'P':
                return P_signal;
            case 'Q':
                return Q_signal;
            case 'R':
                return R_signal;
            case 'S':
                return S_signal;
            case 'T':
                return T_signal;
            case 'U':
                return U_signal;
            case 'V':
                return V_signal;
            case 'X':
                return X_signal;
            case 'W':
                return W_signal;
            case 'Y':
                return Y_signal;
            case 'Z':
                return Z_signal;
        }

        return null;
    }

    public void DestroyAllChildren()
    {
        foreach (Transform child in gameObject.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }

    public void SetGameSpeed(float val)
    {
        gameSpeed = val;
        minDelay -= 0.107f;
        maxDelay -= 0.214f;
        _levelController.ChangeSpeed(speedIncreaseRate);
    }

    public void ResetSpeed()
    {
       time = 0f;
       gameSpeed = 3f;
       minDelay = 1f;
       maxDelay = 2.5f;
       _levelController.SetSpeed(5f);
    }
}
