using UnityEngine;

public class EnemySpriteAnimator : MonoBehaviour 
{
    [SerializeField] Sprite[] frames;
    [SerializeField] float fps = 8f;
    SpriteRenderer sr;
    float timer;
    int currentFrame;

    void Start() => sr = GetComponent<SpriteRenderer>();

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= 1f / fps)
        {
            currentFrame = (currentFrame + 1) % frames.Length;
            sr.sprite = frames[currentFrame];
            timer = 0f;
        }
    }
}   