using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndingPanel : MonoBehaviour
{
    public float placarTotal;
    public Text scoreText;
    public InputField nameInputField;
    public void AddScore(Item itemComScore)
    {
        placarTotal += itemComScore.scoreValue;
    }
    void OnEnable()
    {
        scoreText.text = placarTotal.ToString();
    }

    void Submit(string Name)
    {
        PlayerPrefs.SetFloat("Score", placarTotal);
        PlayerPrefs.SetString("Nome", Name);

        GetComponent<HighscoreAzure>().Insert();
    }

    public void OnSubmitClick()
    {
        Submit(nameInputField.text);
    }
}
