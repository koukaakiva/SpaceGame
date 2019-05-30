extends Node2D

onready var button = preload("res://Scenes/Textbox.tscn");
onready var node = get_node("MainUI/MarginContainer/ScrollContainer/VBoxContainer");

func _ready():
	for i in range(20):
		var b = button.instance();
		for j in range(i):
			b.text += "Plop\n";
		node.add_child(b);