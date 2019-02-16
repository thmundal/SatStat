#pragma once
#include "input_handler.h"

// Constructor
input_handler::input_handler()
{
	// Insert sensors
	sensor_collection[0] = new Temp_sensor("temp", 7);
}

// Listens on inputs from software layer
void input_handler::listener()
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

// Takes the sensor name as a string, and returns the read value as an integer
int input_handler::read_sensor(String sensor_name)
{	
	for (int i = 0; i < 10; i++)
	{
		if (sensor_collection[i]->get_name() == sensor_name)
		{			
			return sensor_collection[i]->read_sensor();
		}		
	}

	return 0;	
}

JsonObject * input_handler::get_instruction()
{
	return json_handler.fetch_instruction();
}
