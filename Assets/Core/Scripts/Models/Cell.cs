using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(Image))]
[RequireComponent(typeof(Button))]
public class Cell : MonoBehaviour
{
    public TypeCellEnum _typeCell;
    [HideInInspector] public Image _image;
    [HideInInspector] public Button _button;

    public void Init(InputController inputController)
    {
        _image = GetComponent<Image>();
        _button = GetComponent<Button>();
        _button.targetGraphic = _image;
        _button.onClick.AddListener(() => { inputController.SelectButton(this); });
    }
}