#pragma once
#include "Arduino.h"
#include <QueueArray.h>
#include <ArduinoJson.hpp>
#include <ArduinoJson.h>
#include "Sensor.h"

class input_handler
{
public:
	input_handler(); // Constructor	
	int read_sensor(String sensor_name); // Takes the sensor name as a string, and returns the read value as an integer
	JsonObject* fetch_instruction();
	void listener(); // Listens on inputs from software layer
	int instruction_count();
private:	
	void insert_json(String input_data); // Converts input to JSON and inserts it in the instruction queue
	Sensor* sensor_collection[10];	
	QueueArray<JsonObject*> instruction_queue;	
	DynamicJsonBuffer* jsonBuffer;
	JsonObject* root;
	String json;
};
