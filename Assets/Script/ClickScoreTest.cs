using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickScoreTest : MonoBehaviour
{
    public GameManager scoreManager;

    private void OnMouseDown()
    {
        scoreManager.Score();

    }
}
