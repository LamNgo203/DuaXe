using UnityEngine;
using DG.Tweening;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Stats")]
    public float moveDistance = 2f;
    public float moveSpeed = 2f;

    [Header("Visual Effects")]
    public ParticleSystem deathEffectPrefab;
    public SpriteRenderer spriteRenderer;
    public Color damageColor = Color.red;
    public float damageFlashDuration = 0.1f;

    private float flashTimer = 0f;
    private Vector3 startPos;
    private Tween moveTween;

    private void Start()
    {
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();

        startPos = transform.position;
        StartMoveLoop();
    }

    private void StartMoveLoop()
    {
        Vector3 upPos = startPos + Vector3.up * moveDistance;
        Vector3 downPos = startPos - Vector3.up * moveDistance;

        moveTween = transform.DOMove(upPos, moveSpeed)
            .SetEase(Ease.InOutSine)
            .OnComplete(() =>
            {
                moveTween = transform.DOMove(downPos, moveSpeed)
                    .SetEase(Ease.InOutSine)
                    .OnComplete(StartMoveLoop);
            });
    }

    private void Update()
    {
        // Hiệu ứng flash khi bị đánh
        if (flashTimer > 0)
        {
            flashTimer -= Time.deltaTime;
            if (flashTimer <= 0 && spriteRenderer != null)
            {
                spriteRenderer.color = Color.white;
            }
        }
    }

    public void TakeDamage()
    {
        // Hiệu ứng flash khi bị đánh
        if (spriteRenderer != null)
        {
            spriteRenderer.color = damageColor;
            flashTimer = damageFlashDuration;
        }

        // Phát âm thanh bị đánh
        if (SoundManager.Instance != null)
        {
            // SoundManager.Instance.PlayGameSound(1);
        }

        Die();
    }

    private void Die()
    {
        if (moveTween != null) moveTween.Kill();

        // Tạo hiệu ứng chết
        if (deathEffectPrefab != null)
        {
            var a = Instantiate(deathEffectPrefab, transform.position, Quaternion.identity);
            a.Play();
        }

        // Cộng điểm
        if (GameManager.Instance != null)
        {
            GameManager.Instance.AddScore(1);
        }

        // Phát âm thanh chết
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlayGameSound(1);
        }

        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position + Vector3.up * moveDistance, 0.2f);
        Gizmos.DrawWireSphere(transform.position - Vector3.up * moveDistance, 0.2f);
        Gizmos.DrawLine(transform.position + Vector3.up * moveDistance, transform.position - Vector3.up * moveDistance);
    }
}