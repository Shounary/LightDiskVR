using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyButton : MonoBehaviour
{
    public Text InputTextField;
    public Text keyButtonText;

    public void onTypeClick() {
        InputTextField.text = InputTextField.text + keyButtonText.text;
    }
}
