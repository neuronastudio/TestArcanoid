using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private Level level;
    public void Seek(Level _level)
    {
        level = _level;
    }
    void OnCollisionEnter2D(Collision2D myCollision)
    {
        level.Collision();
    }
}
