﻿using UnityEngine;
using System.Collections;

public class Dialog : MonoBehaviour {

    public GameObject dialogRoot;

    public bool _isOpened = false;

    public void Close() {
        if(!_isOpened)
            return;
        Animator animator = gameObject.GetComponent<Animator>();
        animator.enabled = true;
        animator.CrossFade("dialog_hide",0);
        _isOpened = false;
    }
        
    public void CloseComplete() {
        if(dialogRoot)
            dialogRoot.SetActive(false);
        else
            gameObject.SetActive(false);
        Animator animator = gameObject.GetComponent<Animator>();
        animator.enabled = false;
        animator.StopPlayback();
    }
	virtual public void Open() {
        if(_isOpened)
            return;
        if (dialogRoot)
            dialogRoot.SetActive(true);
        else
            try
            {
                gameObject.SetActive(true);
            }
            catch
            {
                //doStuff
            }
        try
        {
            Animator animator = gameObject.GetComponent<Animator>();
            animator.enabled = true;
            animator.CrossFade("dialog_open", 0);
        }
        catch
        {
            //DoStuff
        }
        _isOpened = true;
    }

    virtual public void OpenComplete() {
        Animator animator = gameObject.GetComponent<Animator>();
        animator.StopPlayback();
        animator.enabled = false;
    }
}
