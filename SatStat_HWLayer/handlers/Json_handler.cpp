#pragma once
#include "Json_handler.h"

Json_handler::Json_handler()
{
	json_buffer = new DynamicJsonBuffer();
	root = &json_buffer->createObject();
}

Json_handler::~Json_handler()
{
	delete json_buffer;
	delete &root;

	// Dequeue and delete every JsonObject in the instruction queue
	JsonObject* tmp;
	while (!instruction_queue.isEmpty())
	{
		tmp = instruction_queue.dequeue();
		delete &tmp;
	}
}

void Json_handler::insert_instruction(String input_data)
{
	root = &json_buffer->parseObject(input_data);
	instruction_queue.enqueue(root);
	json_buffer->clear();
}

JsonObject* Json_handler::fetch_instruction()
{
	return instruction_queue.dequeue();
}

JsonObject* Json_handler::to_json_object(const Result* data, const int& data_count)
{
	for (int i = 0; i < data_count; i++)
	{
		root->set(data[i].name, data[i].data);
	}	
	json_buffer->clear();
	return root;
}

bool Json_handler::queue_is_empty()
{
	return instruction_queue.isEmpty();
}
