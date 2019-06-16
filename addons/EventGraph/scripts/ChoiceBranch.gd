tool
extends "res://addons/EventGraph/scripts/Node.gd";

signal removed_branch(id);
const type = "Choice";

func save_data():
	var data = {
		"type" : type,
		"name" : name,
		"rect_x" : rect_position.x,
		"rect_y" : rect_position.y,
		"rect_size_x" : rect_size.x,
		"rect_size_y" : rect_size.y,
		"refName" : $ReferenceNameEdit.text,
		"branches" : []
	};
	var isBranch = false;
	for child in get_children():
		if(child is HSeparator):
			isBranch = true;
		elif(isBranch):
			data.branches.append(child.text);
	return data;

func load_data(data):
	$ReferenceNameEdit.text = data.refName;
	for i in range(data.branches.size()):
		addBranch(data.branches[i]);

func _on_PlusButton_pressed():
	addBranch("");
	
func addBranch(text):
	var newBranch = LineEdit.new();
	newBranch.set_placeholder("Choice");
	newBranch.text = text;
	add_child(newBranch);
	set_slot(get_child_count() - 1, false, 0, Color(1, 1, 1), true, 0, Color(1, 1, 1));

func _on_MinusButton_pressed():
	var child = get_child(get_child_count() - 1);
	if child is LineEdit:
		emit_signal("removed_branch", get_child_count() - 4);
		clear_slot(get_child_count() - 1);
		child.queue_free();
