using System;
using Microsoft.Xna.Framework;
using Monocle;

namespace Celeste.Mod.VanillaWithSprinkles.VWSWorld.UI.Elements;

public class VWSMainMenuTextButton : VWSMenuButton
{
	private const float IconWidth = 64f;

	private const float IconSpacing = 20f;

	private const float MaxLabelWidth = 400f;

	private string label;

	private float labelScale;

	private Wiggler wiggler;

	private float ease;

	/// <summary>
	///       The original label name dialog key.<br />
	///       Useful when inserting your own button between others.
	///       </summary>
	public string LabelName;

	public override float ButtonHeight => ActiveFont.LineHeight * 1.25f;

	public float Ease => ease;

	public Wiggler Wiggler => wiggler;

	public VWSMainMenuTextButton(string labelName, VWSUi owsui, Vector2 targetPosition, Vector2 tweenFrom, Action onConfirm)
		: base(owsui, targetPosition, tweenFrom, onConfirm)
	{
		LabelName = labelName;
		label = Dialog.Clean(labelName);
		labelScale = 1f;
		float x = ActiveFont.Measure(label).X;
		if (x > 400f) labelScale = 400f / x;
		Add(wiggler = Wiggler.Create(0.25f, 4f));
	}

	public override void Update()
	{
		base.Update();
		ease = Calc.Approach(ease, base.Selected ? 1 : 0, 6f * Engine.DeltaTime);
	}

	public override void Render()
	{
		base.Render();
//		Vector2 vector = new Vector2(Monocle.Ease.CubeInOut(ease) * 32f, ActiveFont.LineHeight / 2f + wiggler.Value * 8f);
		ActiveFont.DrawOutline(label, Position, new Vector2(0.5f, 0.5f), Vector2.One * labelScale, SelectionColor, 2f, Color.Black);
	}

	public override void OnSelect()
	{
		wiggler.Start();
	}
}