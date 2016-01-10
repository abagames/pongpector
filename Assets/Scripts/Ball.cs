using UnityEngine;
using ProtoTurtle.BitmapDrawing;

public class Ball
{
    public Vector2 pos = new Vector2();
    public Vector2 vel = new Vector2();
    public float speed;
    Pongpector pongpector;
    Texture2D texture;
    int radius = 5;

    public Ball(Pongpector pongpector)
    {
        this.pongpector = pongpector;
        this.texture = pongpector.texture;
    }

    public bool Update()
    {
        pos += vel * speed;
        if (speed < 1)
        {
            speed += 0.01f;
        }
        if ((pos.x < pongpector.wallWidth + radius && vel.x < 0) ||
            (pos.x > pongpector.width - pongpector.wallWidth - radius && vel.x > 0))
        {
            vel.x *= -1;
        }
        if (pongpector.TestRacketCollision(pos, vel, radius))
        {
            vel.y *= -1;
            pongpector.AddScore();
        }
        var offset = 0;
        if (pos.y > pongpector.height + radius)
        {
            offset = 1;
            this.pos.y = -radius;
        }
        else if (pos.y < -radius)
        {
            offset = -1;
            this.pos.y = pongpector.height + radius;
        }
        if (offset != 0)
        {
            var np = pongpector.GetAnother(offset);
            if (np != null)
            {
                np.AddBall(this.pos, this.vel);
            }
            else
            {
                pongpector.Miss();
            }
            return false;
        }
        texture.DrawFilledCircle((int)pos.x, (int)pos.y, radius, Color.blue);
        return true;
    }
}
