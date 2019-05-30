#pragma once
#include "../sensors/Temp_hum_sensor.h"
#include "../sensors/Load_cell.h"
#include "../sensors/Pos_sensor.h"
#include "Json_container.h"

/**
*	The Sensor_container class holds every available sensor in a list, and has methods for retreiving a list of available sensors, as well as reading the different sensors.
*/
class Sensor_container
{
public:
	// Constructor and destructor
	Sensor_container();
	~Sensor_container();

	// Read methods	
	void read_sensor(const String& name);
	void read_all_sensors();

	// Get methods
	void append_data(Json_container<JsonObject>& dest, sstl::Subscribable* src);
	Json_container<JsonObject> get_data(const String& name);
	Json_container<JsonObject> get_all_data();
	Json_container<JsonObject> get_sub_data();
	void append_available_data(Json_container<JsonObject>& dest);

	LinkedList<String, Sensor*>& get_available_sensors();

private:	
	LinkedList<String, Sensor*> sensor_collection;
};