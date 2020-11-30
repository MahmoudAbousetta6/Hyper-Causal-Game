using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    [SerializeField] private Transform _player;

    void Update()
    {
        FollowPlayer();
    }

    private void FollowPlayer()
    {
        if (_player.position.y > transform.position.y)
            transform.position = new Vector3(transform.position.x, _player.position.y, transform.position.z);
    }
}
