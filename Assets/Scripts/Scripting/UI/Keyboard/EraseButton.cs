using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EraseButton : MonoBehaviour
{
    public Text inputField;

    public void OnEraseButtonClick() {
        inputField.text = inputField.text.Substring(0, inputField.text.Length - 1);
    }

    
}