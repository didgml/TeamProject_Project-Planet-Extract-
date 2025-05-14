using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Test : MonoBehaviour
{
    public float health = 100f; // 초기 체력
    public float currentHealth = 100f; // 현재 체력
    public float attackPower = 4f; // 적의 공격력
    public float attackRange = 0.5f; // 공격 범위 (플레이어와의 거리)
    public float attackCooldown = 2f; // 공격 쿨다운 시간
    public AudioClip attackSound; // 공격 소리
    public bool isDead = false; // 사망 여부



    private float lastAttackTime; // 마지막 공격 시간
    private AudioSource audioSource; // AudioSource 컴포넌트
    private Vector3 initialPosition; // 초기 위치 저장
    private Quaternion initialRotation; // 초기 회전 저장
    public Vector3 respawnPosition; // 리스폰 위치 변수
    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();


        initialPosition = transform.position;
        initialRotation = transform.rotation;

        //if (anim != null)
        //    anim.SetBool("Move", true);
    }

    private void Update()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, attackRange);
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                if (Time.time - lastAttackTime >= attackCooldown)
                {
                    lastAttackTime = Time.time;
                    AttackPlayer(collider.gameObject);
                }
            }
        }
    }

    private void AttackPlayer(GameObject player)
    {
        Character playerCharacter = player.GetComponent<Character>();
        if (playerCharacter != null)
        {
            playerCharacter.TakeDamage(attackPower);
            Debug.Log($"{gameObject.name}가 {player.name}을(를) 공격하여 {attackPower}의 데미지를 주었습니다.");

            if (audioSource != null && attackSound != null)
            {
                anim.SetTrigger("Attack");
                audioSource.PlayOneShot(attackSound);
            }
        }
    }

    public void TakeDamage(float damage)
    {
        if (isDead || currentHealth <= 0) return; // 이미 죽었거나 체력이 0 이하일 경우 무시

        currentHealth -= damage;
        Debug.Log($"{gameObject.name}가 {damage}의 데미지를 받았습니다. 남은 체력: {currentHealth}");

        if (currentHealth <= 0)
        {
            isDead = true;
            anim.SetTrigger("Die");
            Die();
        }
    }

    private void Die()
    {
        anim.SetTrigger("Die");
        Debug.Log($"{gameObject.name}가 사망했습니다.");
        DisableEnemy();
        StartCoroutine(Respawn());
    }

    private void DisableEnemy()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
        enabled = false;
    }

    private IEnumerator Respawn()
    {
        Debug.Log("Respawn 코루틴 시작.");

        yield return new WaitForSeconds(90f);

        RespawnEnemy();
    }

    private void RespawnEnemy()
    {
        Debug.Log("리스폰 실행됨.");

        transform.position = respawnPosition;
        transform.rotation = initialRotation;

        currentHealth = health;
        isDead = false;

        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }

        enabled = true;
        lastAttackTime = Time.time;

        Debug.Log($"{gameObject.name} 리스폰 완료. 체력: {currentHealth}");
    }

    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}