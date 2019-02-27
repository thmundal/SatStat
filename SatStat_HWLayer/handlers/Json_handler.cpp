#pragma once
#include "Json_handler.h"

// Delete every JsonObject in the instruction queue
Json_handler::~Json_handler()
{	
	Json_container<JsonObject>* tmp;
	while (!instruction_queue.isEmpty())
	{
		tmp = instruction_queue.dequeue();
		delete tmp;
	}
}

// Insert instruction into instruction queue
void Json_handler::insert_instruction(const String& input_data)
{
	Json_container<JsonObject>* obj = new Json_object_container();
	obj->parse(input_data);
	instruction_queue.enqueue(obj);
}

// Fetches an instruction from the instruction queue
Json_container<JsonObject>* Json_handler::fetch_instruction()
{
	if (!instruction_queue.isEmpty())
	{
		return instruction_queue.dequeue();
	}

	return nullptr;
}

// Returns true if instruction queue is empty
bool Json_handler::queue_is_empty()
{
	return instruction_queue.isEmpty();
}

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