extends Node2D;

func _ready():
	#$EventManager.start(load("res://Events/GroundhogDay.tres"));
	for child in $Sol.get_children():
		print(child.name);
		for grandchild in child.get_children():
			if(grandchild is AstronomicalObject):
				print("|-" + grandchild.name + ", mass:" + str(grandchild.mass));
			else:
				print("|-" + grandchild.name);
