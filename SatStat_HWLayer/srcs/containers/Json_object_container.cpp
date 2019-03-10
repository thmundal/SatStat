#pragma once
#include "Json_object_container.h"

/**
*	Instantiate parent json member as JsonObject.
*/
Json_object_container::Json_object_container() : Json_container()
{	
	Json_container::json = &Json_container::buffer->createObject();
}

/**
*	Deletes whatever currently stored in the JsonBuffer, and instantiates parent json member as a new JsonObject.
*/
void Json_object_container::create()
{	
	delete Json_container::buffer;
	Json_container::buffer = new DynamicJsonBuffer();

	Json_container::json = &Json_container::buffer->createObject();
}

/**
*	Deletes whatever currently stored in the JsonBuffer, and instantiates parent json member as a new JsonObject parsed from the string passed as argument.
*/
void Json_object_container::parse(const String& json)
{
	delete Json_container::buffer;
	Json_container::buffer = new DynamicJsonBuffer();

	Json_container::json = &Json_container::buffer->parseObject(json);
}
