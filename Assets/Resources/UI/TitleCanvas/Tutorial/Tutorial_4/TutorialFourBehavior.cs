using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialFourBehavior : MonoBehaviour
{
    [SerializeField] GameObject[] texts;
    void Start()
    {
        Instantiate(texts[(int)Managers.instance.nowLanguage], transform);
    }

    void OnEnable()
    {
        ChangeDisplayButtons();
    }

    void Update()
    {
        if (InputManager.isChangedController) { ChangeDisplayButtons(); }
    }

    void ChangeDisplayButtons()
    {
        Sprite[] applySprites = InputManager.nowButtonSpriteData.GetAllSprites();
        transform.GetChild(0).GetComponent<Image>().sprite = applySprites[4];
        transform.GetChild(1).GetComponent<Image>().sprite = applySprites[6];
        transform.GetChild(2).GetComponent<Image>().sprite = applySprites[7];
        transform.GetChild(3).GetComponent<Image>().sprite = applySprites[6];

        Color color;
        if (InputManager.currentController == ControllerType.Keyboard) { color = Color.clear; }
        else { color = Color.white; }

        transform.GetChild(0).GetComponent<Image>().color = color;
        transform.GetChild(2).GetComponent<Image>().color = color;
    }
}
