using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public float rotationSpeed = 90f;
    public float rotationTarget;
    public Transform model;

    private Rigidbody2D _rigidbody;
    private LevelController _levelController;
    private Collider2D _collider;

    private bool _alive = true;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
        _levelController = FindObjectOfType<LevelController>();
    }

    public void EnablePlayer()
    {
        gameObject.SetActive(true);

        _alive = true;
        _rigidbody.gravityScale = 0;
        _collider.enabled = true;
        model.localRotation = Quaternion.identity;
    }

    public void DisablePlayer()
    {
        gameObject.SetActive(false);
    }

    private void Update()
    {
        // rotação de morte
        if (!_alive)
        {
            _rigidbody.gravityScale = 1;
            model.Rotate(new Vector3(0, 0, rotationSpeed * Time.deltaTime));

            if (transform.position.y < -15)
                DisablePlayer();

            return;
        }

        _rigidbody.gravityScale = 0;

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector2 direction = new Vector2(horizontal, vertical);

        if (direction.magnitude > 1)
            direction.Normalize();

        transform.rotation = Quaternion.Euler(new Vector3(0, 0, -rotationTarget * horizontal));

        _rigidbody.velocity = direction * speed;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out CollectableSignal collectable))
        {
            _levelController.CollectLetter(collectable);
            collectable.Disable();
        }
    }

    public void Die()
    {
        _alive = false;
        _collider.enabled = false;
        _rigidbody.velocity = new Vector2(0, 10f);
    }
}