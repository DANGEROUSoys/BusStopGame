using UnityEngine;

public struct DogWasInteracted : IEvent
{
    public short BonesCount;
    public DogWasInteracted(short bones)
    {
        BonesCount = bones;
    }
}
