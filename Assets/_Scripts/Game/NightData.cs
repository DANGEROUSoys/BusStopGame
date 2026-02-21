using UnityEngine;

public class NightData
{
    public bool MenuIsActive;
    public static NightData Instance;

    public NightData()
    {
        Instance = this;
    }
}
