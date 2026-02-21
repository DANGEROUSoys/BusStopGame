using System.Collections.Generic;
using UnityEngine;


public class Bootstrap : MonoBehaviour
{
    [SerializeField] private ProjectContext _projectContext;
    [SerializeField] private NightHandler _night; //Delete

    private void Awake()
    {
        _projectContext.Initialize();
        _night.Initialize(); //Delete

    }
}
