#pragma once
#include "input_handler.h"

// Constructor
input_handler::input_handler()
{
	// Instantiate JsonObject
	jsonBuffer = new DynamicJsonBuffer();
	root = &jsonBuffer->createObject();		

	// Insert sensors
	sensor_collection[0] = new Sensor("temp", 7);
}

// Listens on inputs from software layer
void input_handler::listener()
{
	// Testing insertion into the input queue
	for (int i = 0; i < 10; i++)
	{
		json = "{\"sensor\":\"gps" + String(i) + "\",\"time\":12345,\"data\":[48.756080,2.302038]}";
		insert_json(json);
	}

	Serial.println("Number of elements in queue: " + String(instruction_queue.count()));
}

int input_handler::instruction_count()
{
	return instruction_queue.count();
}

void input_handler::insert_json(String input_data)
{
	root = &jsonBuffer->parseObject(input_data);
	instruction_queue.enqueue(root);
}

// Takes the sensor name as a string, and returns the read value as an integer
int input_handler::read_sensor(String sensor_name)
{	
	for (int sensor_pin = 0; sensor_pin < sensor_collection->length(); sensor_pin++)
	{
		if (sensor_collection[sensor_pin] == sensor_name)
		{
			sensor_pin += 2;
			return digitalRead(sensor_pin);
		}		
	}

	return 0;	
}

JsonObject* input_handler::fetch_instruction()
{
	return instruction_queue.dequeue();
}
