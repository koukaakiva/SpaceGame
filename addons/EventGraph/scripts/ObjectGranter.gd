tool
extends "res://addons/EventGraph/scripts/Node.gd";

const type = "Object Granter";

func initialize(EventResource):
	for i in range(EventResource.Objects.size()):
		$OptionButton.add_item(EventResource.Objects[i].name, i);

func save_data():
	return {
		"type" : type,
		"name" : name,
		"rect_x" : rect_position.x,
		"rect_y" : rect_position.y,
		"rect_size_x" : rect_size.x,
		"rect_size_y" : rect_size.y,
		"refName" : $ReferenceNameEdit.text,
		"object" : $OptionButton.get_selected_id()
	};

func load_data(data):
	$ReferenceNameEdit.text = data.refName;
	$OptionButton.select(data.object);
