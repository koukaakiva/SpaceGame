extends Control;

func _input(event):
    if event is InputEventKey:
        if event.pressed:
            get_tree().change_scene("res://Scenes/Main.tscn");