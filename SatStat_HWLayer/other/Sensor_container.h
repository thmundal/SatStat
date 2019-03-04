#pragma once
#include "Arduino.h"
#include "../sensors/Temp_hum_sensor.h"
#include "../libraries/LinkedList.h"
#include "../handlers/Json_handler.h"

class Sensor_container
{
public:
	// Constructor and destructor
	Sensor_container();
	~Sensor_container();

	// Read methods
	Json_container<JsonObject>* read_sensor(const String& name);
	Json_container<JsonObject>* read_sensors();

	LinkedList<String, Sensor*>& get_available_sensors();

private:
	LinkedList<String, Sensor*> sensor_collection;
	Json_handler json_handler;
};