#pragma once
#include "Arduino.h"
#include "../other/Json_object_container.h"
#include "../other/Json_array_container.h"
#include "../libraries/QueueArray.h"
#include "../sensors/Result.h"

class Json_handler
{
public:
	/*Json_handler();
	~Json_handler();*/

	void insert_instruction(const String& input_data);
	Json_container<JsonObject>* fetch_instruction();	
	template<class T> Json_container<JsonObject>* to_json_object(const String& key, const T& value);
	template<class T> Json_container<JsonArray>* to_json_array(const T* value, const int& data_count);
	Json_container<JsonObject>* to_json_object(const Result* data, const int& data_count);
	bool queue_is_empty();	
private:
	QueueArray<Json_container<JsonObject>*> instruction_queue;
};

template<class T>
Json_container<JsonObject>* Json_handler::to_json_object(const String& key, const T& value)
{
	Json_container<JsonObject>* obj = new Json_object_container;
	obj->get()->set(key, value);

	return obj;
}

template<class T>
Json_container<JsonArray>* Json_handler::to_json_array(const T* value, const int& data_count)
{
	Json_container<JsonArray>* arr = new Json_array_container;	
	
	for (int i = 0; i < data_count; i++)
	{
		arr->get()->add(value[i]);
	}	

	return arr;
}