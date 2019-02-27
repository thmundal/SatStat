#pragma once
#include "Arduino.h"
#include "../other/Json_object_container.h"
#include "../other/Json_array_container.h"
#include "../libraries/QueueArray.h"
#include "../sensors/Result.h"

class Json_handler
{
public:
	~Json_handler();

	void insert_instruction(const String& input_data);
	Json_container<JsonObject>* fetch_instruction();
	template<class T> Json_container<JsonObject>* create_object();
	template<class T> Json_container<JsonArray>* create_array();
	template<class T> Json_container<JsonObject>* create_object(const String& key, const T& value);
	template<class T> Json_container<JsonArray>* create_array(const T* value, const int& data_count);
	template<class T> void append_to(Json_container<JsonObject>* obj, const String& key, const T& value);
	template<class T> void append_to(Json_container<JsonArray>* arr, const String& key, const T& value);
	bool queue_is_empty();	
private:
	QueueArray<Json_container<JsonObject>*> instruction_queue;
};

template<class T>
Json_container<JsonObject>* Json_handler::create_object()
{
	Json_container<JsonObject>* obj = new Json_object_container();

	return obj;
}

template<class T>
Json_container<JsonArray>* Json_handler::create_array()
{
	Json_container<JsonArray>* arr = new Json_array_container();

	return arr;
}

template<class T>
Json_container<JsonObject>* Json_handler::create_object(const String& key, const T& value)
{
	Json_container<JsonObject>* obj = new Json_object_container();
	obj->get()->set(key, value);

	return obj;
}

template<class T>
Json_container<JsonArray>* Json_handler::create_array(const T* value, const int& data_count)
{
	Json_container<JsonArray>* arr = new Json_array_container();	
	
	for (int i = 0; i < data_count; i++)
	{
		arr->get()->add(value[i]);
	}	

	return arr;
}

template<class T>
void Json_handler::append_to(Json_container<JsonObject>* obj, const String& key, const T& value)
{
	obj->get()->set(key, value);
}

template<class T>
void Json_handler::append_to(Json_container<JsonArray>* arr, const String& key, const T& value)
{
	arr->get()->set(key, value);
}