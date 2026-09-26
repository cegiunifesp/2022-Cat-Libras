using UnityEngine;

public class CollectableSignal : MonoBehaviour
{
    [SerializeField] private char letter;
    [Space]
    public float speed = 10;
    [SerializeField] private float xBound = -15;

    public char Letter { get { return letter; } }

    private void Update()
    {
        transform.position += Vector3.left * speed * Time.deltaTime;

        if (transform.position.x < xBound)
            Disable();
    }

    public void Disable()
    {
        Destroy(gameObject);
    }

}
