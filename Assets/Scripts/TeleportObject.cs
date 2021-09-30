using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportObject : MonoBehaviour
{
    void FixedUpdate()
    {
        Vector3 leftBot = Camera.main.ViewportToWorldPoint(new Vector3(0, 0));
        Vector3 rightTop = Camera.main.ViewportToWorldPoint(new Vector3(1, 1));

        float x_left = leftBot.x;
        float x_right = rightTop.x;
        float y_top = rightTop.y;
        float y_bot = leftBot.y;

        Vector3 clampedPos = transform.position;
        if (Mathf.Clamp(clampedPos.x, x_left, x_right) == x_left)
            clampedPos.x = x_right;
        else if (Mathf.Clamp(clampedPos.x, x_left, x_right) == x_right)
            clampedPos.x = x_left;
        if (Mathf.Clamp(clampedPos.y, y_bot, y_top) == y_top)
            clampedPos.y = y_bot;
        else if (Mathf.Clamp(clampedPos.y, y_bot, y_top) == y_bot)
            clampedPos.y = y_top;
        transform.position = clampedPos;
    }
}
