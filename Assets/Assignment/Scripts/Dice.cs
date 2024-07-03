using UnityEngine;
using System.Collections;

public class Dice : MonoBehaviour
{
    [SerializeField] private Sprite[] diceSides;  // Ensure to assign these in the Inspector
    private SpriteRenderer rend;
    private bool coroutineAllowed = true;
    private int finalSide;

    private void Start ()
    {
        rend = GetComponent<SpriteRenderer>();
        rend.sprite = diceSides[5];  // Initialize with a default side (6)
    }

    public void RollDice ()
    {
        if (coroutineAllowed)
            StartCoroutine("RollTheDice");
    }

    private IEnumerator RollTheDice ()
    {
        coroutineAllowed = false;
        var randomDiceSide = 0;
        for (var i = 0; i <= 20; i++)
        {
            randomDiceSide = Random.Range(0, 6);
            rend.sprite = diceSides[randomDiceSide];
            yield return new WaitForSeconds(0.05f);
        }
        finalSide = randomDiceSide + 1;
        coroutineAllowed = true;
        Debug.Log("Rolled number: " + finalSide);
        GameManager.Instance.OnDiceRolled(finalSide);
    }

    public void SetSide (int side)
    {
        rend.sprite = diceSides[side - 1];
        finalSide = side;
    }

    public int GetFinalSide ()
    {
        return finalSide;
    }
}
