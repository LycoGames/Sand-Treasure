using System;
using System.Collections;
using System.Collections.Generic;
using _Game.Scripts.Player;
using UnityEngine;

public class Money : MonoBehaviour
{
    public int value;
    public Action<Vector3, int> OnCollect;
    [SerializeField] private BoxCollider myBoxCollider;
    [SerializeField] private Effects effect;
    [SerializeField] private MeshRenderer myRenderer;
    
    public void EnableCollider() => myBoxCollider.enabled = true;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        OnCollect?.Invoke(this.transform.position, value);
        myRenderer.enabled = false;
        effect.effect.Play();
        myBoxCollider.enabled = false;
        Invoke(nameof(Return),1f);
    }

    private void Return()
    {
        myRenderer.enabled = true;
        MoneyPool.Instance.ReturnToPool(this);
    }
}