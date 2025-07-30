using UnityEngine;

public class EnemySetup : MonoBehaviour
{
    [Header("Enemy Settings")]
    public int enemyLayer = 8; // Layer cho enemy (có thể thay đổi)
    public string enemyTag = "Enemy";
    
    private void Start()
    {
        SetupEnemy();
    }
    
    private void SetupEnemy()
    {
        // Đặt tag
        gameObject.tag = enemyTag;
        
        // Đặt layer
        gameObject.layer = enemyLayer;
        
        // Đảm bảo có SimpleEnemy component
        if (GetComponent<SimpleEnemy>() == null)
        {
            gameObject.AddComponent<SimpleEnemy>();
        }
        
        // Đảm bảo có Collider2D
        if (GetComponent<Collider2D>() == null)
        {
            BoxCollider2D collider = gameObject.AddComponent<BoxCollider2D>();
            collider.size = Vector2.one; // Kích thước mặc định
        }
        
        // Đảm bảo có SpriteRenderer
        if (GetComponent<SpriteRenderer>() == null)
        {
            SpriteRenderer renderer = gameObject.AddComponent<SpriteRenderer>();
            renderer.color = Color.red; // Màu đỏ để dễ nhận biết
        }
        
        Debug.Log($"Enemy {gameObject.name} đã được setup:");
        Debug.Log($"- Tag: {gameObject.tag}");
        Debug.Log($"- Layer: {gameObject.layer}");
        Debug.Log($"- Có SimpleEnemy: {GetComponent<SimpleEnemy>() != null}");
        Debug.Log($"- Có Collider2D: {GetComponent<Collider2D>() != null}");
    }
    
    private void OnDrawGizmos()
    {
        // Vẽ gizmo để dễ nhận biết enemy
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, Vector3.one);
    }
} 