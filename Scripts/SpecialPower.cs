using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SpecialPower : MonoBehaviour
{
    public static SpecialPower instance;

    [SerializeField] private Slider feverSlider;
    [SerializeField] private Animator feverAnimator;

    private static int enemyCounter = 0;

    [HideInInspector] public bool isPowerEnabled = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            feverSlider.value = enemyCounter * 0.2f;
        } 
    }

    public void UpdateFever()
    {
        if (!isPowerEnabled)
        {
            enemyCounter++;
            feverSlider.value = enemyCounter * 0.2f;

            if (enemyCounter >= 5)
            {
                // ACTIVATE SPECIAL POWER
                StartCoroutine(ActivatePower());
                // ACTIVATE SPECIAL POWER
            }
        }
    }

    private IEnumerator ActivatePower()
    {
        isPowerEnabled = true;

        feverAnimator.SetTrigger("activated");

        yield return new WaitForSecondsRealtime(3f);

        enemyCounter = 0;
        feverSlider.value = 0;

        isPowerEnabled = false;
    }
}
