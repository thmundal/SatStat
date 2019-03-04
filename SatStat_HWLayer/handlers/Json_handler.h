#pragma once
#include "Arduino.h"
#include "../other/Json_object_container.h"
#include "../other/Json_array_container.h"

class Json_handler
{
public:
	// Create object and array methods
	Json_container<JsonObject>* create_object();
	Json_container<JsonArray>* create_array();
	template<class T> Json_container<JsonObject>* create_object(const String& key, const T& value);
	template<class T> Json_container<JsonArray>* create_array(const T* value, const int& data_count);

	// Append to object and append to array methods
	template<class T> void append_to(Json_container<JsonObject>* obj, const String& key, const T& value);
	template<class T> void append_to(Json_container<JsonArray>* arr, const String& key, const T& value);	
};

// Creates a pointer to a Json_object_container with predefined key and value
template<class T>
inline Json_container<JsonObject>* Json_handler::create_object(const String& key, const T& value)
{
	Json_container<JsonObject>* obj = new Json_object_container();
	obj->get()->set(key, value);

	return obj;
}

// Creates a pointer to a Json_array_container with predefined key and value
template<class T>
inline Json_container<JsonArray>* Json_handler::create_array(const T* value, const int& data_count)
{
	Json_container<JsonArray>* arr = new Json_array_container();	
	
	for (int i = 0; i < data_count; i++)
	{
		arr->get()->add(value[i]);
	}	

	return arr;
}

// Appends a key-value pair to an existing object
template<class T>
inline void Json_handler::append_to(Json_container<JsonObject>* obj, const String& key, const T& value)
{
	obj->get()->set(key, value);
}

// Appends a key-value pair to an existing array
template<class T>
inline void Json_handler::append_to(Json_container<JsonArray>* arr, const String& key, const T& value)
{
	arr->get()->set(key, value);
}