using System.Collections.Generic;
using UnityEngine;

public class Enemies : MonoBehaviour
{
    public static Enemies instance;
    
    public List<List<Transform>> enemies;
    public Animator animator;
    public string levelName;
    
    [SerializeField] GameObject roof;

    private List<Transform> roofs;
    
    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        roofs = new List<Transform>();
        enemies = new List<List<Transform>>();

        foreach (var child in roof.transform)
        {
            roofs.Add((Transform)child);
        }

        for (int i = 0; i < roofs.Count; i++)
        {
            List<Transform> newList = new List<Transform>();
            foreach (var enemy in roofs[i])
            {
                newList.Add((Transform)enemy);
            }
            enemies.Add(newList);
        }
    }

    public void JumpOverTheRoof()
    {
        if (enemies[0].Count == 0) // mevcut çatıda düşman bittiyse
        {
            enemies.Remove(enemies[0]); // mevcut çatının listesini baştan sil
            if (enemies.Count != 0) // son çatıda değilsen, çatı değiştir
            {
                Debug.Log("Çatıda düşman kalmadı");
                animator.SetTrigger(levelName);
            }
            else // son çatıdaysa oyunu bitir
            {
                TapToStart.instance.LevelFinished();
            }
            ConfettiController.instance.ActivateConfetti();
        }
    }
}
