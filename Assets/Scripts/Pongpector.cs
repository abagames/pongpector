using System;
using System.Collections.Generic;
using UnityEngine;
using ProtoTurtle.BitmapDrawing;

public class Pongpector : MonoBehaviour
{
    public Texture2D texture;
    public float topRacketX = 0.5f;
    public bool isTopRacketValid;
    public float bottomRacketX = 0.5f;
    public bool isBottomRacketValid;
    public int width = 240;
    public int height = 160;
    public int wallWidth = 10;
    List<Ball> balls = new List<Ball>();
    Racket topRacket;
    Racket bottomRacket;
    int missCount = 0;

    void Start()
    {
        texture = new Texture2D(width, height);
        balls.Clear();
        topRacket = new Racket(this, true);
        bottomRacket = new Racket(this, false);
        AddBall();
    }

    public void AddBall(float rankSpeed = 1)
    {
        var a = (90.0f).Rand(45);
        if ((1.0f).Rand() < 0.5f)
        {
            a += 180;
        }
        var vel = new Vector2().MoveAngle(a, 1) * rankSpeed;
        AddBall(new Vector2(width / 2, height / 2), vel, 0);
    }

    public void AddBall(Vector2 pos, Vector2 vel, float speed = 1)
    {
        var b = new Ball(this);
        b.pos = pos;
        b.vel = vel;
        b.speed = speed;
        balls.Add(b);
    }

    public void UpdateFrame()
    {
        if (texture == null || topRacket == null || bottomRacket == null)
        {
            Start();
        }
        texture.DrawFilledRectangle(new Rect(0, 0, width, height), Color.clear);
        var wallColor = Color.blue;
        if (missCount > 0)
        {
            wallColor = Color.red;
            missCount--;
        }
        texture.DrawFilledRectangle(new Rect(0, 0, wallWidth, height), wallColor);
        texture.DrawFilledRectangle(new Rect(width - wallWidth, 0, wallWidth, height), wallColor);
        balls = balls.FindAll((b) => b.Update());
        topRacket.x = topRacketX;
        topRacket.Update();
        isTopRacketValid = topRacket.isValid;
        bottomRacket.x = bottomRacketX;
        bottomRacket.Update();
        isBottomRacketValid = bottomRacket.isValid;
        texture.Apply();
        Status status = null;
        try
        {
            status = gameObject.GetComponent<Status>() as Status;
        }
        catch (MissingReferenceException) { }
        if (status != null)
        {
            if (status.ticks % 500 == 0 && !status.isGameover)
            {
                AddBall(Mathf.Sqrt(1 + status.ticks * 0.0003f));
            }
            if (status.isRestarting || status.isGameover)
            {
                balls.Clear();
                if (status.isRestarting)
                {
                    AddBall();
                }
            }
            var bc = balls.Count;
            if (isTopRacketValid)
            {
                status.tmpBallCount = bc;
            }
            else
            {
                status.tmpBallCount += bc;
            }
            if (isBottomRacketValid)
            {
                status.ballCount = status.tmpBallCount;
                status.ticks++;
                if (status.isRestarting)
                {
                    status.isRestarting = false;
                }
            }
        }
    }

    public bool TestRacketCollision(Vector2 pos, Vector2 vel, float radius)
    {
        if (vel.y > 0)
        {
            return (bottomRacket.TestCollision(pos, radius));
        }
        else
        {
            return (topRacket.TestCollision(pos, radius));
        }
    }

    public Pongpector GetAnother(int offset = 1)
    {
        try
        {
            var others = gameObject.GetComponents<Pongpector>();
            var idx = Array.IndexOf(others, this) + offset;
            if (idx < 0 || idx >= others.Length)
            {
                return null;
            }
            return others[idx];
        }
        catch (MissingReferenceException)
        {
            return null;
        }
    }

    public void AddScore()
    {
        var status = gameObject.GetComponent<Status>() as Status;
        if (status != null)
        {
            status.AddScore();
        }
    }

    public void Miss()
    {
        missCount = 60;
        var status = gameObject.GetComponent<Status>() as Status;
        if (status != null)
        {
            status.Miss();
        }
    }
}
