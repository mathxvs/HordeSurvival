using Godot;
using System;

public partial class TitleButton : Button
{
    public void OnButtonPressed() {
        GetTree().ChangeSceneToFile("res://scenes/Game.tscn");
    }
}
