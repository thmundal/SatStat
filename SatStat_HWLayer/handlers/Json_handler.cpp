#pragma once
#include "Json_handler.h"

// Creates a pointer to a Json_object_continer with no initial key-value pair
Json_container<JsonObject>* Json_handler::create_object()
{
	Json_container<JsonObject>* obj = new Json_object_container();

	return obj;
}

// Creates a pointer to a Json_array_continer with no initial key-value pair
Json_container<JsonArray>* Json_handler::create_array()
{
	Json_container<JsonArray>* arr = new Json_array_container();

	return arr;
}