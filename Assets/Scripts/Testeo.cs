using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Test : MonoBehaviour
{
    public GameObject text;
    public void Testeo()
    { 
        string code = text.GetComponent<TMP_InputField>().text;
        if (code == "") return;

        Programa.Main(code);
    }
}

