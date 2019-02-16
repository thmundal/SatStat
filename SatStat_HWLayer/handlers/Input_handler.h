#pragma once
#include "Arduino.h"
#include "Json_handler.h"
#include "../libraries/LinkedList.h"
#include "../sensors/Temperature_sensor.h"
#include "../sensors/Humidity_sensor.h"

class Input_handler
{
public:
	Input_handler(); // Constructor	
	void serial_listener(); // Listens on inputs from software layer
	JsonObject* read_sensor(String name);
	JsonObject* read_sensors(); // Takes the sensor name as a string, and returns the read value as an integer	
	JsonObject* get_instruction();
	String get_sensor_list();
private:
	Json_handler json_handler;
	LinkedList<String, Sensor*> sensor_collection;
};
