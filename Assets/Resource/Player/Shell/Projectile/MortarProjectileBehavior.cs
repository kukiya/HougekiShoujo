using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MortarProjectileBehavior : MonoBehaviour
{
    [SerializeField] GameObject explosion;
    [SerializeField] Animator imageAnimator;

    [SerializeField] RuntimeAnimatorController projectileUp;
    [SerializeField] RuntimeAnimatorController projectileDown;

    [SerializeField] float lifeTime = 3;

    float timer = 0;
    Vector3 defaultPosition;
    Vector3 targetPoint;

    public void ProjectileStart(Vector3 _point)
    {
        targetPoint = _point;
        defaultPosition = this.transform.position;
    }

    void Update()
    {
        imageAnimator.speed = 1 * TimeManager.TimeRate();

        timer += TimeManager.deltaTime;
        if (timer > lifeTime) { Destroy(gameObject); }

        float timeRate = timer / lifeTime;

        if (timeRate < 0.5f) { imageAnimator.runtimeAnimatorController = projectileUp; }
        else { imageAnimator.runtimeAnimatorController = projectileDown; }

        Vector3 currentHorizon = Vector3.Lerp(defaultPosition, targetPoint, timeRate);
        float currentVertical = Mathf.Sin(timeRate * Mathf.PI) * 15;

        transform.position = currentHorizon + Vector3.up * currentVertical;
    }

    private void OnDestroy()
    {
        Vector3 spawnPos = transform.position;
        spawnPos.y = 2;
        GameObject obj = Instantiate(explosion, spawnPos, Quaternion.identity);

        OwnerID id;
        id = obj.AddComponent<OwnerID>();
        id.SetID(this.transform.GetComponent<OwnerID>().GetID());
    }
}
