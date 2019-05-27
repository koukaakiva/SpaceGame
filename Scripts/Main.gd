extends Node2D

onready var button = load("res://Scenes/TextBox.tscn");
onready var node = get_node("MainUI/MarginContainer/ScrollContainer/VBoxContainer");

func _ready():
	for i in range(10):
		var b = button.instance(10);
		b.set_text("PLOP" + str(i));
		node.add_child(b);