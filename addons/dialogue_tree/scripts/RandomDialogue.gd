tool
extends "res://addons/dialogue_tree/scripts/Dialogue.gd";

const type = "Random";

signal removed_branch(id);

func save_data():
	var data = {
		"type" : type,
		"name" : name,
		"rect_x" : rect_position.x,
		"rect_y" : rect_position.y,
		"rect_size_x" : rect_size.x,
		"rect_size_y" : rect_size.y,
		"RefName" : $ReferenceNameEdit.text,
		"branches" : []
	};
	var isBranch = false;
	for child in get_children():
		if(child is HSeparator):
			isBranch = true;
		elif(isBranch):
			data.branches.append({});
	return data;

func load_data(data):
	$ReferenceNameEdit.text = data.RefName;
	for branch in data.branches:
		addBranch();

func _on_MinusButton_pressed():
	var child = get_child(get_child_count() - 1);
	if child is Container:
		emit_signal("removed_branch", get_child_count() - 4);
		clear_slot(get_child_count() - 1);
		child.queue_free();

func _on_PlusButton_pressed():
	addBranch();

func addBranch():
	var newBranch = Container.new();
	newBranch.rect_min_size = Vector2(0, 20);
	add_child(newBranch);
	set_slot(get_child_count() - 1, false, 0, Color(1, 1, 1), true, 0, Color(1, 1, 1));
