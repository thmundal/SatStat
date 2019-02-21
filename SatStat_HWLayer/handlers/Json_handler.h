#pragma once
#include "Arduino.h"
#include "../libraries/ArduinoJson.h"
#include "../libraries/ArduinoJson.hpp"
#include "../libraries/QueueArray.h"
#include "../sensors/Result.h"

class Json_handler
{
public:
	Json_handler();
	~Json_handler();

	void insert_instruction(String input_data);
	JsonObject* fetch_instruction();	
	template<class T> JsonObject* to_json_object(const String& key, const T& value);
	JsonObject* to_json_object(const Result* data, const int& data_count);
	bool queue_is_empty();	
private:
	QueueArray<JsonObject*> instruction_queue;
	DynamicJsonBuffer* json_buffer;
	JsonObject* root;
};

template<class T>
JsonObject * Json_handler::to_json_object(const String& key, const T& value)
{
	root = &json_buffer->createObject();
	root->set(key, value);
	json_buffer->clear();

	return root;
}