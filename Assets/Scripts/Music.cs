using System.Collections;
using UnityEngine;

public class Music : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(this);
    }
}
