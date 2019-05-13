﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {
    [SerializeField] private float stunDuration;
    [SerializeField] private AudioClip impactSFX;
    private AudioSource audioSource;
    private TrapBase trapBase;
    //private CameraTwoRotator cam;

    private bool hit = false;
	private GameObject player = null;
    private Renderer[] child;
    private Animator anim;

	// Use this for initialization
	void Start () {
		trapBase = GetComponent<TrapBase>();
		Destroy(gameObject, 5.0f);
        child = this.GetComponentsInChildren<Renderer>();
        hit = false;
        audioSource = GetComponent<AudioSource>();
    }

	void FixedUpdate(){
		if (player != null)
		{
			if (hit)
			{
				trapBase.Stun(player, stunDuration, this.gameObject);
                anim.SetBool("Stunned", hit);
                StartCoroutine(Wait());
            }
		}
	}

	// Update is called once per frame
	void OnTriggerEnter(Collider col)
	{
		if(col.gameObject.tag == "Player"){
			player = col.gameObject;
            if(!hit) audioSource.PlayOneShot(impactSFX);
			hit = true;
            anim = player.GetComponent<PlayerOneMovement>().GetAnim();
            if (player.GetComponent<PlayerOneMovement>().IsCrouched() == false)
            {
                anim.Play("Stunned", 0);
            }
            Unrender();

        }
		else if(col.gameObject.tag == "Boundary" || col.gameObject.tag == "Platform"){
            if (hit == true)
            {
                Unrender();
                StartCoroutine(Death(stunDuration));
            }
            else if(hit == false)
            {
                Destroy(this.gameObject);
            }
        }
	}

    private void Unrender()
    {
        foreach(Renderer r in child)
        {
            r.enabled = false;
        }
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(stunDuration);
        anim.SetBool("Stunned", false);
    }

    private IEnumerator Death(float stunDuration)
    {
        yield return new WaitForSeconds(stunDuration + 2f);

        Destroy(this.gameObject);
    }
}
