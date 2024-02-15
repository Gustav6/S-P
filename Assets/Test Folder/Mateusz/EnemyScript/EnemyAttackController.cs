using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackController : MonoBehaviour
{
    Animator _anim;
    Enemy _enemy;

    GameObject go;

    SpriteRenderer _sr;
    Color originalColor;

    private void Awake()
    {
        _anim = GetComponent<Animator>();
        _enemy = GetComponent<Enemy>();
        _sr = GetComponentInChildren<SpriteRenderer>();

        originalColor = _sr.color;
    }

    public void EnterMovement()
    {
        _enemy._enemyAI.CanMove = true;
        _enemy._enemyAI.IsNotGettingHit = true;
        _anim.SetBool("IsMoving", true);
        _anim.SetBool("IsHit", false);
    }

    public void LeaveMovement(bool isNotGettingHit)
    {
        _enemy._enemyAI.CanMove = false;
        _enemy._enemyAI.IsNotGettingHit = isNotGettingHit;
        _anim.SetBool("IsMoving", false);
    }

    public void EnterAttackState()
    {
        _anim.SetBool("IsAttacking", true);
        _anim.SetFloat("AnimSpeed", 1.5f);
    }

    public void LeaveAttack()
    {
        _anim.SetBool("IsAttacking", false);
    }

    public void EnemyAttack()
    {
        go = Instantiate(_enemy._enemyAttack.hitbox, _enemy._enemyAttack.hitboxSpawn.transform);
        go.transform.localPosition *= new Vector2(Mathf.Sign(transform.localScale.x), 1);
    }

    public void EnemyHit()
    {
        Debug.Log("Is Hit");
        _anim.SetBool("IsHit", true);
        _anim.SetBool("IsAttacking", false);
    }

    public void GroundEnemyHit()
    {
        StartCoroutine(SwitchColor());
    }

    public void DespawnHitbox()
    {
        if (go != null)
            Destroy(go);
    }

    IEnumerator SwitchColor()
    {
        _sr.color =  new Color(1, 0.00028f, 0.0028f);
        yield return new WaitForSeconds(0.2f);
        _sr.color = originalColor;
    }
}
