using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckCollider : MonoBehaviour
{
    [SerializeField] private LevelController _levelController;

    // Start is called before the first frame update
    void Start()
    {
        _levelController = FindObjectOfType<LevelController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out CollectableSignal collectable) && collectable.Letter == _levelController.CurrentLetter)
        {
            _levelController.WrongLetter();
            collectable.Disable();
        }
    }
}
