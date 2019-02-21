#pragma once
#include "Arduino.h"
#include "Json_handler.h"
#include "../libraries/LinkedList.h"
#include "../sensors/Temp_hum_sensor.h"

class Input_handler
{
public:
	Input_handler(); // Constructor	
	~Input_handler(); // Destructor	

	void serial_listener(); // Listens on inputs from software layer
	const JsonObject* read_sensor(const String& name);
	const JsonObject* read_sensors(); // Takes the sensor name as a string, and returns the read value as an integer	
	JsonObject* get_instruction();
	String get_sensor_list();
private:
	Json_handler json_handler;
	LinkedList<String, Sensor*> sensor_collection;
};
