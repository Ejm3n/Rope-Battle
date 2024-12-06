using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CharacterBossEmoji : MonoBehaviour
{
    private Character character;
    [SerializeField] private Transform target;
    [SerializeField] private Transform label;
    [SerializeField] private ParticleSystem emoji;
    private void OnEnable()
    {
        label.gameObject.SetActive(true);

    }
    private void Start()
    {
        character = GetComponent<Character>();
        character.Animation.OnPull += OnPull;
        character.Animation.OnFall += OnFall;

    }
    private void OnDestroy()
    {
        if (enabled)
        {
            character.Animation.OnPull -= OnPull;
            character.Animation.OnFall -= OnFall;
        }
    }
    private void OnPull()
    {
        float d = emoji.main.duration;
        emoji.Play();
        Sequence sequence = DOTween.Sequence();
        sequence.Append(label.DOScale(0f, d*0.2f));
        sequence.Append(DOTween.To(() => 0f, (v) => { }, 1f, d* 0.6f));
        sequence.Append(label.DOScale(1f, d * 0.2f));
    }
    private void OnFall()
    {
        label.gameObject.SetActive(false);
    }
    private void LateUpdate()
    {
        Transform cam = CinemachineBrain.Default.transform;
        target.LookAt(cam, cam.up);
    }
}
