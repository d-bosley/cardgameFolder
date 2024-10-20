using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Attack : MonoBehaviour
{
    string[] stringArray = {"ATTACK", "POWER"};
    [HideInInspector] public string cardType;
    [HideInInspector] public string cardInfo;
    [HideInInspector] public int damage;
    [HideInInspector] public string ability;
    public TextMeshProUGUI cardInformation;

    // Start is called before the first frame update
    void Start()
    {
        cardType = stringArray[Random.Range(0, 2)];
        if(cardType == "ATTACK"){cardInfo = AttackCard();}
        else if(cardType == "POWER"){cardInfo = PowerCard();}
        cardInformation.text = cardInfo;
    }

    // Update is called once per frame
    void Update()
    {

    }

    string AttackCard()
    {
        int[] attackPower = {1, 5, 10};
        string[] attackType = {"Strength", "Speed", "Intelligence", "Energy"};
        string[] attackWeakness = {"Strength", "Speed", "Intelligence", "Energy"};
        int cost = Random.Range(1, 4);
        int power = attackPower[Random.Range(0, 3)];
        string type = attackType[Random.Range(0, 4)];
        string weakness = attackWeakness[Random.Range(0, 4)];
        string info = "Card Cost: " + cost + "\nAttack Power: " + power + "\nAdvantage: " + type + "\nWeakness: " + weakness;
        damage = power;
        return info;
    }

    string PowerCard()
    {
        string[] powerType = {"Healing", "Buffing", "Protecting", "Effecting"};
        int cost = Random.Range(0, 3);
        string type = powerType[Random.Range(0, 4)];
        string info = "Card Cost: " + cost + "\nAbility: " + type;
        ability = type;
        return info;
    }
}
