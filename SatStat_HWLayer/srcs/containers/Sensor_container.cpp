#pragma once
#include "Sensor_container.h"

/**
*	Instantiate sensors with name and the pin it's connected to.
*/
Sensor_container::Sensor_container()
{
	sensor_collection.append("temp_hum", new Temp_hum_sensor("temp_hum", 6));
}

/**
*	Delete every sensor in the sensor collection.
*/
Sensor_container::~Sensor_container()
{
	for (int i = 0; i < sensor_collection.count(); i++)
	{
		delete sensor_collection[i];
	}
}

/**
*	Read sensor corresponding to the name sent as argument, and return result as Json_container<JsonObject> pointer.
*/
Json_container<JsonObject>* Sensor_container::read_sensor(const String& name)
{
	Sensor* sensor = sensor_collection.get(name);
	const Result* result = sensor->read_sensor();
	const int count = sensor->get_data_count();
	Json_container<JsonObject>* obj = json_handler.create_object();

	for (int i = 0; i < count; i++)
	{
		json_handler.append_to(obj, result[i].name, result[i].data);
	}

	return obj;
}

/**
*	Reads all sensors in the sensor collection, and return result as a Json_container<JsonObject> pointer.
*/
Json_container<JsonObject>* Sensor_container::read_sensors()
{
	Sensor* sensor;
	int sensor_count = sensor_collection.count();
	const Result* result;
	int result_count;

	Json_container<JsonObject>* obj = json_handler.create_object();

	for (int i = 0; i < sensor_count; i++)
	{
		sensor = sensor_collection[i];
		result = sensor->read_sensor();
		result_count = sensor->get_data_count();

		for (int j = 0; j < result_count; j++)
		{
			json_handler.append_to(obj, result[j].name, result[j].data);
		}
	}

	return obj;
}

/**
*	Returns a reference to the linked list containing every available sensor.
*/
LinkedList<String, Sensor*>& Sensor_container::get_available_sensors()
{
	return sensor_collection;
}
