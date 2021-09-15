using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAreaController : MonoBehaviour
{
    public BossController boss;
    public GameObject stage;
    public GameObject preStage;
    public GameObject Player { get; set; }
    public float with;
    public float bossAndPlayerReferentDistance;

    

    // Update is called once per frame
    void Update()
    {
        MoveBoss();
        ChangePlataform();
    }

    private void MoveBoss()
    {
        if (Player != null)
        {
            float distancePlayerBoss = boss.transform.position.x - Player.transform.position.x;
            boss.IsPlayerClose = distancePlayerBoss < bossAndPlayerReferentDistance;
            if (boss.IsPlayerClose)
            {
                float diff = bossAndPlayerReferentDistance - distancePlayerBoss;
                Vector3 currentBossPosition = boss.transform.position;
                currentBossPosition.x += diff;
                boss.transform.position = currentBossPosition;
            }
        }
    }

    private void ChangePlataform()
    {
        if (Player != null && Player.transform.position.x > stage.transform.position.x)
        {
            Vector2 position = preStage.transform.position;
            position.x += with * 2;
            preStage.transform.position = position;
            Swap(ref preStage, ref stage);
        }
    }

    private void Swap<T>(ref T var1, ref T var2)
    {
        var temp = var1;
        var1 = var2;
        var2 = temp;
    }
}
