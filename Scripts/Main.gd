extends Node2D;

const events = [
	preload("res://Events/TestEvent.tres"),
	preload("res://Events/Grue.tres")
	];
var i = 1;

func _ready():
	$EventManager.start(events[0]);


func _on_EventManager_Event_Ended():
	if(i < events.size()):
		$EventManager.start(events[i]);
		i += 1;
