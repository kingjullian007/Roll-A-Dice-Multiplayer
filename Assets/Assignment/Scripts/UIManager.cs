using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Button rollButton;
    [SerializeField] private TextMeshProUGUI resultText;

    private void OnEnable ()
    {
        rollButton.onClick.AddListener(OnRollButtonClicked);
    }

    private void OnDisable ()
    {
        rollButton.onClick.RemoveListener(OnRollButtonClicked);
    }

    private void OnRollButtonClicked ()
    {
        GameManager.Instance.RollDice();
    }

    public void UpdateResultText (int result)
    {
        resultText.text = "Dice Result: " + result;
    }
}
