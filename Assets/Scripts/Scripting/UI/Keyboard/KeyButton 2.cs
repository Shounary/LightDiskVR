using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class KeyButton : MonoBehaviour
{
    public Text InputTextField;
    //public Text keyButtonText;
    public TextMeshProUGUI keyButtonText;

    public void onTypeClick() {
        InputTextField.text = InputTextField.text + keyButtonText.text;
    }
}
