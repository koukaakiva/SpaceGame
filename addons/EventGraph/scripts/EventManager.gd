extends Node;

signal Dialogue_Next(ref, actor, text);
signal Choice_Next(ref, branches);
signal Object_Granted(ref, object);
signal Event_Ended;
export (Resource) var EventResource = null;
var in_event = false;
var current_node = {};
var savedKeys = {};

func start(event_resource = null, start_at = "StartNode"):
	EventResource = event_resource;
	if(EventResource == null):
		return;
	in_event = true;
	_process_next(EventResource.Nodes[find_next_node(start_at)]);

func next(branch = 0):
	if(!in_event):
		return;
	var connections = find_connections(current_node["name"]);
	if((connections.size() - 1) < branch):
		print("Error: Not enough branches.");
		end();
		return;
	var next_node;
	for connection in connections:
		if(connection["from_port"] == branch):
			next_node = find_next_node(connection["to"]);
	if(next_node == null):
		print("Error: Branch not found.");
		end();
		return;
	_process_next(EventResource.Nodes[next_node]);

func _process_next(nextNode):
	current_node = nextNode;
	match current_node["type"]:
		"Dialogue":
			emit_signal("Dialogue_Next", current_node["refName"], current_node["actor"], current_node["dialogue"]);
		"Choice":
			emit_signal("Choice_Next", current_node["refName"], current_node["branches"]);
		"Key Saver":
			savedKeys[current_node.key] = current_node.value;
			next();
		"Key Branch":
			if(savedKeys.has(current_node["key"])):
				var value = savedKeys[current_node["key"]];
				for i in range(current_node["branches"].size() - 1):
					if(current_node["branches"][i] == value):
						next(i);
						return;
			next(current_node["branches"].size() - 1);
		"Random":
			randomize();
			next(round(rand_range(0, current_node["branches"] - 1)));
		"Object Granter":
			emit_signal("Object_Granted", current_node["refName"], EventResource.Objects[current_node["object"]]);
		"Start":
			next();
		"End":
			end();
		_:
			print("Error: Unknown node type.");
			end();

func end():
	savedKeys = {};
	in_event = false;
	emit_signal("Event_Ended");

func find_connections(name):
	var results = [];
	for connection in EventResource.Connections:
		if(connection.from == name):
			results.append(connection);
	return results;

func find_next_node(name):
	for i in range(EventResource.Nodes.size()):
		if(EventResource.Nodes[i].name == name):
			return i;
	return -1;
