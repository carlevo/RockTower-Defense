using System;
using UnityEngine;

public class GachaManager : MonoBehaviour
{
[SerializeField] private GachaRate [] gacha;
[SerializeField] private Transform parent, pos;
[SerializeField] private GameObject characterCardGo;

GameObject characterCard;
Cards card;


public void Gacha()
    {
        if(characterCard == null)
        {
            characterCard = Instantiate(characterCardGo, pos.position, Quaternion.identity) as GameObject;
            characterCard.transform.SetParent(parent);
            characterCard.transform.localScale = new Vector3(1, 1, 1);
            card = characterCard.GetComponent<Cards>();

            int rnd = UnityEngine.Random.Range(1, 101);
            for(int i=0; i<gacha.Length; i++)
            {
                if(gacha[i].rate <= rnd)
                {
                    card.card = Reward(gacha[i].rarity);
                    return;
                }
            }
        }
    }
    cardInfo Reward(string rarity)
    {
        GachaRate gr = Array.Find(gacha, rt => rt.rarity == rarity);
        cardInfo[] reward = gr.reward;

        int rnd = UnityEngine.Random.Range(0, reward.Length);
        return reward[rnd];
    }
}
