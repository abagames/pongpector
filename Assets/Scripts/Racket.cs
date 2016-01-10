using UnityEngine;
using ProtoTurtle.BitmapDrawing;

public class Racket
{
    public bool isValid = false;
    public float x = 0.5f;
    float y;
    Pongpector pongpector;
    Texture2D texture;
    float width = 50;
    float height = 5;
    bool isTop;

    public Racket(Pongpector pongpector, bool isTop)
    {
        this.pongpector = pongpector;
        this.texture = pongpector.texture;
        this.isTop = isTop;
        y = (isTop ? pongpector.height * 0.05f : pongpector.height * 0.95f);
    }

    public void Update()
    {
        if (isTop)
        {
            isValid = (pongpector.GetAnother(-1) == null);
        }
        else
        {
            isValid = (pongpector.GetAnother(1) == null);
        }
        if (!isValid)
        {
            return;
        }
        var rx = Mathf.Clamp(x * pongpector.width, width / 2 + 1, pongpector.width - width / 2 - 1);
        texture.DrawFilledRectangle(new Rect
            (rx - width / 2, y - height / 2, width, height), Color.blue);
    }

    public bool TestCollision(Vector2 p, float radius)
    {
        if (!isValid)
        {
            return false;
        }
        var rx = Mathf.Clamp(x * pongpector.width, width / 2 + 1, pongpector.width - width / 2 - 1);
        var w = width + radius * 2;
        var h = height + radius * 2;
        var rect = new Rect(rx - (w / 2), y - (h / 2), w, h);
        return rect.Contains(p);
    }
}
