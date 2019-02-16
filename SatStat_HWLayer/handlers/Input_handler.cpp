#pragma once
#include "Input_handler.h"

// Constructor
Input_handler::Input_handler()
{	
	// Init sensors
	sensor_collection.append("temperature", new Temperature_sensor("temperature", 7));
	sensor_collection.append("humidity", new Humidity_sensor("humidity", 7));
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

JsonObject * Input_handler::read_sensor(String name)
{
	return json_handler.convert_to_json(name, (String)sensor_collection.get(name)->read_sensor());
}

// Reads all sensors
JsonObject* Input_handler::read_sensors()
{		
	String tmp = "{";
	for (int i = sensor_collection.count() - 1; i >= 0; i--)
	{
		tmp += "\"" + sensor_collection[i]->get_name() + "\":";
		tmp += sensor_collection[i]->read_sensor();
		if (i != 0)
		{
			tmp += ",";
		}
		else
		{
			tmp += "}";
		}
		delay(110);
	}

	return json_handler.convert_to_json(tmp);
}

JsonObject * Input_handler::get_instruction()
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
