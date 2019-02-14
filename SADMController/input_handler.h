#pragma once
#include "Arduino.h"
#include <QueueArray.h>
#include <ArduinoJson.hpp>
#include <ArduinoJson.h>

class input_handler
{
public:
	input_handler(); // Constructor	
	int read_sensor(String sensor_name); // Takes the sensor name as a string, and returns the read value as an integer
	JsonObject* fetch_instruction();
	void listener(); // Listens on inputs from software layer
private:	
	void insert_json(String input_data); // Converts input to JSON and inserts it in the instruction queue
	String sensor_collection[10];
	QueueArray<JsonObject*> instruction_queue;
	StaticJsonBuffer<200>* jsonBuffer;
	JsonObject* root;
	String json;
};
