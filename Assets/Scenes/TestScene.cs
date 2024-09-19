using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TestScene : MonoBehaviour
{
    [SerializeField]
    private Button[] button = null;

    [SerializeField]
    private TextMeshProUGUI _text = null;

    private void Awake()
    {
        for (int i = 0; i < button.Length; i++)
        {
            int no = i;
            button[i].onClick.AddListener(() =>
            {
                _text.text = $"number: {no} button click!";
            });
        }
    }
}
