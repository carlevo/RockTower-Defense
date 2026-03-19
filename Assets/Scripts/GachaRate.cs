using UnityEngine;

[System.Serializable]

public class GachaRate
{
    public string rateName;

    [Range(1,100)]

    public int rate;

    public cardInfo[] reward;
}
