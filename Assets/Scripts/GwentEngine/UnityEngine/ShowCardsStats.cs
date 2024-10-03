using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShowCardsStats : MonoBehaviour
{
    public Card card;

    public TMP_Text Title;
    public TMP_Text Power;
    public TMP_Text Description;
    public TMP_Text Ranges;

    private void Start()
    {
        Title.text = card.card.Name;
        Power.text = card.card.Power.ToString();
        Description.text = card.card.Description;
        string range = null;
        foreach(var item in card.card.Range)
        {
            range += "" + item;
        }
        Ranges.text = range;
    }
}
