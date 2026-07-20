using System;
using System.Collections.Generic;
using System.Linq;
using Celeste.Mod.Core;
using Microsoft.Xna.Framework;
using Monocle;

namespace Celeste.Mod.VanillaWithSprinkles.VWSWorld.UI.Elements;

public abstract class VWSMenuButton : Entity
{	public Vector2 TargetPosition;

    public Vector2 TweenFrom;

    public VWSMenuButton LeftButton;

    public VWSMenuButton RightButton;

    public VWSMenuButton UpButton;

    public VWSMenuButton DownButton;

    public Action OnConfirm;

    private bool canAcceptInput;

    private VWSUi owsui;

    private bool selected;

    private Tween tween;

    public Color SelectionColor
    {
        get
        {
            if (selected)
            {
                if (CoreModule.Settings.AllowTextHighlight && !base.Scene.BetweenInterval(0.1f))
                {
                    return TextMenu.HighlightColorB;
                }
                return TextMenu.HighlightColorA;
            }
            return Color.White;
        }
    }
    public bool Selected
    {
        get => selected;
        set
        {
            if (base.Scene == null)
            {
                throw new Exception("Cannot set Selected while VWSMenuButton is not in a Scene.");
            }
            if (!selected && value)
            {
                VWSMenuButton selection = GetSelection(base.Scene);
                if (selection != null)
                {
                    selection.Selected = false;
                }
                selected = true;
                canAcceptInput = false;
                OnSelect();
            }
            else if (selected && !value)
            {
                selected = false;
                OnDeselect();
            }
        }
    }
    public abstract float ButtonHeight { get; }

    public bool _Selected
    {
        get => selected;
        set => selected = value;
    }

    public static VWSMenuButton GetSelection(Scene scene)
    {
        IEnumerable<VWSMenuButton> buttons = scene.Entities
            .Where(ent => ent.GetType().IsAssignableTo(typeof(VWSMenuButton)))
            .Select(ent => (VWSMenuButton)ent);
        foreach (Entity btn in buttons) {
            Logger.Info("vws",btn.ToString());
            if (((VWSMenuButton)btn).Selected) return (VWSMenuButton)btn;
        }
        Logger.Info("vws","nothing selected");
        return null;
    }

    public static void ClearSelection(Scene scene)
    {
        VWSMenuButton selection = GetSelection(scene);
        if (selection != null) selection.Selected = false;
    }

    public VWSMenuButton(VWSUi owsui, Vector2 targetPosition, Vector2 tweenFrom, Action onConfirm)
        : base(tweenFrom)
    {
        TargetPosition = targetPosition;
        TweenFrom = tweenFrom;
        OnConfirm = onConfirm;
        this.owsui = owsui;
    }

    public override void Update()
    {
        base.Update();
        if (!canAcceptInput)
        {
            canAcceptInput = true;
        }
        else if (owsui.Selected && owsui.Focused && selected)
        {
            if (Input.MenuConfirm.Pressed)
            {
                Confirm();
            }
            else if (Input.MenuLeft.Pressed && LeftButton != null)
            {
                Audio.Play("event:/ui/main/rollover_up");
                LeftButton.Selected = true;
            }
            else if (Input.MenuRight.Pressed && RightButton != null)
            {
                Audio.Play("event:/ui/main/rollover_down");
                RightButton.Selected = true;
            }
            else if (Input.MenuUp.Pressed && UpButton != null)
            {
                Audio.Play("event:/ui/main/rollover_up");
                UpButton.Selected = true;
            }
            else if (Input.MenuDown.Pressed && DownButton != null)
            {
                Audio.Play("event:/ui/main/rollover_down");
                DownButton.Selected = true;
            }
        }
    }
    
    public void TweenIn(float time)
    {
        orig_TweenIn(time);
        tween.OnComplete = delegate(Tween t)
        {
            t.RemoveSelf();
            tween = null;
        };
    }

    public void TweenOut(float time)
    {
        orig_TweenOut(time);
        tween.OnComplete = delegate(Tween t)
        {
            t.RemoveSelf();
            tween = null;
        };
    }

    public virtual void OnSelect()
    {
    }

    public virtual void OnDeselect()
    {
    }

    public virtual void Confirm()
    {
        OnConfirm();
    }

    public virtual void StartSelected()
    {
        selected = true;
    }

    public void orig_TweenIn(float time)
    {
        if (tween != null && tween.Entity == this)
        {
            tween.RemoveSelf();
        }
        Vector2 from = Position;
        Add(tween = Tween.Create(Tween.TweenMode.Oneshot, Ease.CubeOut, time, start: true));
        tween.OnUpdate = t => Position = Vector2.Lerp(from, TargetPosition, t.Eased);
    }

    public void orig_TweenOut(float time)
    {
        if (tween != null && tween.Entity == this)
        {
            tween.RemoveSelf();
        }
        Vector2 from = Position;
        Add(tween = Tween.Create(Tween.TweenMode.Oneshot, Ease.CubeIn, time, start: true));
        tween.OnUpdate = t => Position = Vector2.Lerp(from, TweenFrom, t.Eased);
    }
    public void SetSelected(bool value) => Selected = value;
}