using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Tham chiếu đến GameObject motor
    public Transform motor;
    // Offset giữa camera và motor (có thể chỉnh trong Inspector)
    public Vector3 offset = new Vector3(0, 5, -10);
    
    void Start()
    {
        // Nếu chưa gán offset, lấy offset hiện tại giữa camera và motor
        if (motor != null && offset == Vector3.zero)
        {
            offset = transform.position - motor.position;
        }
    }

    void Update()
    {
        // Nếu đã gán motor, cập nhật vị trí camera
        if (motor != null)
        {
            transform.position = motor.position + offset;
        }
    }
}
