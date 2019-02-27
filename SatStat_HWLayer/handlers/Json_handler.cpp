#pragma once
#include "Json_handler.h"

Json_handler::~Json_handler()
{
	// Dequeue and delete every JsonObject in the instruction queue
	Json_container<JsonObject>* tmp;
	while (!instruction_queue.isEmpty())
	{
		tmp = instruction_queue.dequeue();
		delete tmp;
	}
}

void Json_handler::insert_instruction(const String& input_data)
{
	Json_container<JsonObject>* obj = new Json_object_container;
	obj->parse(input_data);
	instruction_queue.enqueue(obj);
}

Json_container<JsonObject>* Json_handler::fetch_instruction()
{
	return instruction_queue.dequeue();
}

bool Json_handler::queue_is_empty()
{
	return instruction_queue.isEmpty();
}
