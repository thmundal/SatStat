#pragma once
#include "Arduino.h"
#include "Json_handler.h"
#include "Temp_sensor.h"

class input_handler
{
public:
	input_handler(); // Constructor	
	void listener(); // Listens on inputs from software layer	
	int read_sensor(String sensor_name); // Takes the sensor name as a string, and returns the read value as an integer	
	JsonObject* get_instruction();
private:
	Json_handler json_handler;
	Sensor* sensor_collection[10];
};
