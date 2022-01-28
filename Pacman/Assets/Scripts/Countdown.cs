using System.Collections;
using TMPro;
using UnityEngine;

public class Countdown : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;

    private int startValue = 3;

    private void Start()
    {
        StartCoroutine(CountdownCoroutine());
    }

    private IEnumerator CountdownCoroutine()
    {
        Time.timeScale = 0;

        for (int i=startValue; i>0; i--)
        {
            text.text = i.ToString();

            yield return new WaitForSecondsRealtime(1f);
        }

        text.text = "Go!";

        yield return new WaitForSecondsRealtime(1f);

        Time.timeScale = 1;

        gameObject.SetActive(false);
    }
}
