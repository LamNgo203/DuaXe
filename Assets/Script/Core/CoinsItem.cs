using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Manager;

public class CoinsItem : MonoBehaviour
{
    public ParticleSystem fxPrefab; // Gán hiệu ứng fx khi ăn coin từ Inspector
    public int coinValue = 1; // Số coins nhận được khi ăn

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Giả sử xe có tag là "Player"
        if (other.CompareTag("Player"))
        {
            // Cộng coins
            var coinManager = FindObjectOfType<CoinAndShopManager>();
            if (coinManager != null)
            {
                SoundManager.Instance.PlayGameSound(2);
                coinManager.AddCoin(coinValue);
                GameManager.Instance.AddScore(coinValue);
            }
            // Tạo hiệu ứng fx nếu có
            if (fxPrefab != null)
            {
                var a = Instantiate(fxPrefab, transform.position, Quaternion.identity);
                a.Play();
            }
            // Hủy object coin
            Destroy(gameObject);
        }
    }
}
