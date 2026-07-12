using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Celeste.Mod.Core;
using Monocle;
using Microsoft.Xna.Framework;
using Celeste.Mod.OverworldWithSprinkles.CustomOverworld.UI.Elements;
using Celeste.Pico8;

namespace Celeste.Mod.OverworldWithSprinkles.CustomOverworld.UI.Menus;

public class OwsMainMenu : OwsUi
{
	private List<OwsMenuButton> buttons;

	private bool startOnOptions;
	private bool mountainStartFront;
	private float regularButtonOffset;
	private bool needsRebuild;

	public OwsMainMenu()
	{
		buttons = new List<OwsMenuButton>();
	}
	
	public override void Added(Scene scene)
	{
		base.Added(scene);
		CreateButtons();
	}
	
	public override IEnumerator Enter(OwsUi from)
	{
		if (from is OwsTitle || from is OwsFileSelect)
		{
			Audio.Play("event:/ui/main/whoosh_list_in");
			yield return 0.1f;
		}
		if (from is OwsTitle)
		{
			OwsMenuButton.ClearSelection(Scene);
		}
		Visible = true;
		if (mountainStartFront)
			MyOverworld.Mountain.SnapCamera(-1, new MountainCamera(new Vector3(0f, 6f, 12f), MountainRenderer.RotateLookAt));
		MyOverworld.Mountain.GotoRotationMode();
//		MyOverworld.Maddy.Hide();
		foreach (OwsMenuButton button in buttons)
			button.TweenIn(0.2f);
		yield return 0.2f;
		Focused = true;
		mountainStartFront = false;
		yield return null;
	}

	public override IEnumerator Leave(OwsUi next)
	{
		yield return null;
		Focused = false;
		Visible = false;
	}

	public void CreateButtons()
	{
		buttons.ForEach(btn => btn.RemoveSelf());
		buttons.Clear();
		// todo add buttons
		// vanilla sets DirButtons for control
		UpdateLayout();
	}

	public override void Removed(Scene scene)
	{
		buttons.ForEach(scene.Remove);
		base.Removed(scene);
	}

	public override bool IsStart(OwsOverworld overworld, Overworld.StartMode start)
	{
		switch (start)
		{
		case Overworld.StartMode.ReturnFromOptions:
			startOnOptions = true;
			Add(new Coroutine(Enter(null)));
			return true;
		case Overworld.StartMode.MainMenu:
			mountainStartFront = true;
			Add(new Coroutine(Enter(null)));
			return true;
		default:
			return start == Overworld.StartMode.ReturnFromPico8;
		}
	}
	
	public override void Update()
	{
		if (needsRebuild)
		{
			RebuildMainAndTitle();
			needsRebuild = false;
		}
		if (CoreModule.Settings.MainMenuMode == "")
		{
			ScrollButtons();
		}
		if (Selected && Focused && Input.MenuCancel.Pressed)
		{
			Focused = false;
			Audio.Play("event:/ui/main/whoosh_list_out");
			Audio.Play("event:/ui/main/button_back");
			MyOverworld.Goto<OwsTitle>();
		}
		base.Update();
	}

	public override void Render()
	{
		foreach (OwsMenuButton button in buttons)
			if (button.Scene == this.Scene) button.Render();
	}
	
	public void NeedsRebuild() => needsRebuild = true;

	private void ScrollButtons()
	{
		int num = 0;
		for (int i = 0; i < buttons.Count; i++)
		{
			if (buttons[i].Selected)
			{
				num = i;
				break;
			}
		}
		float num2 = (float)Math.Max(0, num - 6) * regularButtonOffset;
		float num3 = 160f;
		foreach (OwsMenuButton button in buttons)
		{
			float num4 = num3 - num2;
			num3 += button.ButtonHeight;
			button.Position.Y += (num4 - button.Position.Y) * (1f - (float)Math.Pow(0.01, Engine.DeltaTime));
		}
	}

	private void RebuildMainAndTitle()
	{
		MyOverworld.UIs.Remove(MyOverworld.GetUI<OwsTitle>());
		OwsUi owsTitle = new OwsTitle { Visible = false };
		owsTitle.IsStart(MyOverworld, Overworld.StartMode.MainMenu);
		MyOverworld.Add(owsTitle);
		MyOverworld.UIs.Add(owsTitle);
		CreateButtons();
	}

	public void UpdateLayout() => UpdateLayout(CoreModule.Settings.MainMenuMode);

	public void UpdateLayout(string mode)
	{
	}
}
