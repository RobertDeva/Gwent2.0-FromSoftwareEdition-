using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShowLureStats : MonoBehaviour
{
    public Lure lure;

    public TMP_Text Title;
    public TMP_Text Power;
    public TMP_Text Description;
    public TMP_Text Ranges;

    private void Start()
    {
        Title.text = lure.card.Name;
        Power.text = lure.card.Power.ToString();
        Description.text = lure.card.Description;
        string range = null;
        foreach (var item in lure.card.Range)
        {
            range += "" + item;
        }
        Ranges.text = range;
    }
}
