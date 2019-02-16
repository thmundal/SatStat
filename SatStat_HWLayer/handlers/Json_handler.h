#pragma once
#include "Arduino.h"
#include "../libraries/ArduinoJson.h"
#include "../libraries/ArduinoJson.hpp"
#include "../libraries/QueueArray.h"

class Json_handler
{
public:
	Json_handler();
	~Json_handler();
	void insert_instruction(String input_data);
	JsonObject* fetch_instruction();
	JsonObject* convert_to_json(String key, String value);
	JsonObject* convert_to_json(String formatted_string);
	bool queue_is_empty();	
private:
	QueueArray<JsonObject*> instruction_queue;
	DynamicJsonBuffer* jsonBuffer;
	JsonObject* root;
	String json;
};