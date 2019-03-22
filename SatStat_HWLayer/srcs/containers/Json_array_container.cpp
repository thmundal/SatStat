#pragma once
#include "Json_array_container.h"

/**
*	Instantiate parent json member as JsonArray.
*/
Json_array_container::Json_array_container() : Json_container()
{
	Json_container::json = &Json_container::buffer->createArray();
}

/**
*	Deletes whatever currently stored in the JsonBuffer, and instantiates parent json member as a new JsonArray.
*/
void Json_array_container::create()
{
	delete Json_container::buffer;
	Json_container::buffer = new DynamicJsonBuffer();

	Json_container::json = &Json_container::buffer->createArray();
}

/**
*	Deletes whatever currently stored in the JsonBuffer, and instantiates parent json member as a new JsonArray parsed from the string passed as argument.
*	Returns true if success, false if not.
*/
bool Json_array_container::parse(const String& json)
{
	delete Json_container::buffer;
	Json_container::buffer = new DynamicJsonBuffer();

	Json_container::json = &Json_container::buffer->parseArray(json);
	
	if (Json_container::json->success())
	{
		return true;
	}

	return false;
}
