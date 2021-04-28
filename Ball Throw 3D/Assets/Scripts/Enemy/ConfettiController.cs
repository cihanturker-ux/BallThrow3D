using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfettiController : MonoBehaviour
{
    public static ConfettiController instance;

    [SerializeField] private GameObject confettis;

    private List<Transform> confettiList;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        confettiList = new List<Transform>();

        foreach (var child in confettis.transform)
        {
            confettiList.Add((Transform)child);
        }
    }

    public void ActivateConfetti()
    {
        confettiList[0].gameObject.active = true;
        confettiList.Remove(confettiList[0]);
    }
}
