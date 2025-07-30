using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinAndShopManager : MonoBehaviour
{
    private const string COIN_KEY = "coin";
    private const string VEHICLE_UNLOCK_KEY = "vehicle_{0}_unlocked";
    private const string SELECTED_VEHICLE_KEY = "selected_vehicle";
    public int vehicleCount = 8; // Số lượng xe, chỉnh theo số xe thực tế

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Cộng tiền
    public void AddCoin(int amount)
    {
        int coin = GetCoin() + amount;
        PlayerPrefs.SetInt(COIN_KEY, coin);
        PlayerPrefs.Save();
    }

    // Tiêu tiền, trả về true nếu đủ tiền và trừ thành công
    public bool SpendCoin(int amount)
    {
        int coin = GetCoin();
        if (coin >= amount)
        {
            coin -= amount;
            PlayerPrefs.SetInt(COIN_KEY, coin);
            PlayerPrefs.Save();
            return true;
        }
        return false;
    }

    // Lấy số coin hiện tại
    public int GetCoin()
    {
        return PlayerPrefs.GetInt(COIN_KEY, 0);
    }

    // Mở khóa xe, trả về true nếu mở thành công
    public bool UnlockVehicle(int vehicleIndex, int price)
    {
        if (!IsVehicleUnlocked(vehicleIndex) && SpendCoin(price))
        {
            PlayerPrefs.SetInt(string.Format(VEHICLE_UNLOCK_KEY, vehicleIndex), 1);
            PlayerPrefs.Save();
            return true;
        }
        return false;
    }

    // Kiểm tra xe đã mở khóa chưa
    public bool IsVehicleUnlocked(int vehicleIndex)
    {
        // Mặc định xe đầu tiên luôn mở khóa
        if (vehicleIndex == 0) return true;
        return PlayerPrefs.GetInt(string.Format(VEHICLE_UNLOCK_KEY, vehicleIndex), 0) == 1;
    }

    // Chọn xe (chỉ chọn được nếu đã mở khóa)
    public bool SelectVehicle(int vehicleIndex)
    {
        if (IsVehicleUnlocked(vehicleIndex))
        {
            PlayerPrefs.SetInt(SELECTED_VEHICLE_KEY, vehicleIndex);
            PlayerPrefs.Save();
            return true;
        }
        return false;
    }

    // Lấy index xe đang chọn
    public int GetSelectedVehicle()
    {
        return PlayerPrefs.GetInt(SELECTED_VEHICLE_KEY, 0);
    }

    // Xe có thể chọn được không (chỉ khi đã mở khóa)
    public bool CanSelectVehicle(int vehicleIndex)
    {
        return IsVehicleUnlocked(vehicleIndex);
    }
}
