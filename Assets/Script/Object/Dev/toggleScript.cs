using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class toggleScript : MonoBehaviour
{
    public CatJump cjump;
    public LeftRightController cMove;
    public Animator Animator;
    public CanvasGroup cg;
    public Button Button;

    public void Start()
    {
        if (cg == null)
        cg = GetComponent<CanvasGroup>();
        if (Button == null)
        Button = GetComponent<Button>();

    }

    public void toggle()
    {
        if (Button.enabled)
        {
            if (cjump != null)
                cjump.enabled = !cjump.enabled;
            if (cMove != null)
                cMove.enabled = !cMove.enabled;
            if (Animator != null)
                Animator.enabled = !Animator.enabled;

            if (cg != null)
            {
                if (cg.alpha == 0.4f)
                {
                    cg.alpha = 1.0f;
                }
                else
                {
                    cg.alpha = 0.4f;
                }
            }
        }
    }
}
