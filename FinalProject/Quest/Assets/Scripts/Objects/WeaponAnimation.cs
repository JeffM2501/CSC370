using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class WeaponAnimation : MonoBehaviour 
{
    bool Playing = false;

    public AnimationSequence Sequence = null;

    public void SetSequence(AnimationSequence sequence)
    {
        Sequence = sequence;
        sequence.SetGameObject(this.gameObject);

        sequence.AnimationComplete += AnimComplete;
    }

    protected void AnimComplete(object sender, EventArgs args)
    {
        Playing = false;
        Sequence.Clear();
    }

    public void Play()
    {
        if (Playing)
            return;

        Playing = true;
        gameObject.SetActive(true);
        Sequence.SetSequence("Attack");
    }

    public void Stop()
    {
        if (Sequence != null)
            AnimComplete(Sequence,EventArgs.Empty);
    }

    public void SetMaterial(Material mat)
    {
        renderer.materials[0] = mat;
    }

	void Start ()
	{
        gameObject.SetActive(false);
	}
	
	void Update ()
	{
        if (Sequence != null && Playing)
            Sequence.Update();

        if (!Playing && gameObject.activeSelf)
            gameObject.SetActive(false);
	}
}
