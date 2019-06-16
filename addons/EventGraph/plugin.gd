tool
extends EditorPlugin;

var dock;
var dockButton;

const event_script = preload("res://addons/EventGraph/scripts/EventManager.gd");
const event_resource_script = preload("res://addons/EventGraph/resource/Event.gd");

func _enter_tree():
	dock = preload("res://addons/EventGraph/Editor.tscn").instance();
	dockButton = add_control_to_bottom_panel(dock, "Dialogue Tree");
	dockButton.hide();
	add_custom_type("Event", "Node", event_script, preload("res://addons/EventGraph/assets/Icon.png"));
	add_custom_type("EventResource", "Resource", event_resource_script, preload("res://addons/EventGraph/assets/ResIcon.png"));

func _exit_tree():
	dock.hide();
	dockButton.hide();
	remove_control_from_bottom_panel(dock);
	dock.queue_free();
	remove_custom_type("Event");
	remove_custom_type("EventResource");

func make_visible(visible):
	dockButton.visible = visible;
	if !visible:
		dock.visible = false;
		dock.set_edit_node(null);

func save_external_data():
	dock.save_resource();

func edit(object):
	if dockButton.pressed:
		dock.visible = true;
	dock.set_edit_node(object);

func handles(object):
	return object is event_script;