#pragma once
#include "Input_handler.h"

// Instantiate sensors
Input_handler::Input_handler()
{	
	sensor_collection.append("temp_hum", new Temp_hum_sensor("temp_hum", 7));
}

// Delete every sensor in the sensor collection
Input_handler::~Input_handler()
{
	for (int i = 0; i < sensor_collection.count(); i++)
	{
		delete sensor_collection[i];
	}	
}

// Listens on inputs from software layer
void Input_handler::serial_listener()
{
	if (Serial.available() > 0) 
	{
		String input = Serial.readStringUntil('\n');

		if (input)
		{
			json_handler.insert_instruction(input);
		}
		else
		{
			//Serial.println("No serial input received");
		}
	}
}

const JsonObject* Input_handler::read_sensor(const String& name)
{
	Sensor* sensor = sensor_collection.get(name);
	return json_handler.to_json_object(sensor->read_sensor(), sensor->get_data_count());
}

// Reads all sensors
const JsonObject* Input_handler::read_sensors()
{	
	const Result* result;

	for (int i = sensor_collection.count() - 1; i >= 0; i--)
	{
		result = sensor_collection[i]->read_sensor();

		for (int j = 0; j < sensor_collection[i]->get_data_count(); j++)
		{
			json_handler.to_json_object(result[i].name, result[i].data);
		}

		// delete result;
	}

	return json_handler.to_json_object("test", 1);
}

JsonObject* Input_handler::get_instruction()
{
	return json_handler.fetch_instruction();
}

String Input_handler::get_sensor_list()
{
	String tmp = "{\"available_sensors\":[";
	for (int i = sensor_collection.count() - 1; i >= 0; i--)
	{
		 tmp += "\"" + sensor_collection[i]->get_name() + "\"";
		 
		 if (i != 0)
		 {
			 tmp += ",";
		 }
		 else
		 {
			 tmp += "]}";
		 }
	}
	return tmp;	
}
