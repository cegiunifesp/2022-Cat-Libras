using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleImage : MonoBehaviour
{
    public Sprite buttonOn;
    public Sprite buttonOff;

    [SerializeField] private Image _image;
    [SerializeField] private Toggle _toggle;

    // Start is called before the first frame update
    void Start()
    {
        ChangeStatus();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ChangeStatus()
    {
        if (!_toggle.isOn)
        {
            _image.sprite = buttonOff;
        }
        else
        {
            _image.sprite = buttonOn;
        }
    }
}
