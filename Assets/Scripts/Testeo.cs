using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Test : MonoBehaviour
{
    public void Testeo()
    {
        GameObject text = GameObject.Find("Text");
        string code = text.GetComponent<TextMeshProUGUI>().text;
        if (code == "") return;

        Programa.Main(code);
    }
}

