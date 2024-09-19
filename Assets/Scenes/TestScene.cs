using UnityEngine;
using UnityEngine.UI;

public class TestScene : MonoBehaviour
{
    [SerializeField]
    private Button[] button = null;

    private void Awake()
    {
        for (int i = 0; i < button.Length; i++)
        {
            button[i].onClick.AddListener(() =>
            {
                Debug.Log("押した");
            });
        }
    }
}
