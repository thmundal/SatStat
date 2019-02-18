#pragma once
#include "Json_handler.h"

Json_handler::Json_handler()
{
	jsonBuffer = new DynamicJsonBuffer();
	root = &jsonBuffer->createObject();
}

Json_handler::~Json_handler()
{
	delete jsonBuffer;
	delete &root;
}

void Json_handler::insert_instruction(String input_data)
{
	root = &jsonBuffer->parseObject(input_data);
	instruction_queue.enqueue(root);
	jsonBuffer->clear();
}

JsonObject * Json_handler::fetch_instruction()
{
	return instruction_queue.dequeue();
}

JsonObject * Json_handler::convert_to_json(String key, String value)
{
	bool is_numeric = true;
	
	for (int i = 0; i < value.length(); i++)
	{
		if (!isdigit(value.charAt(i)) && value.charAt(i) != '.')
		{
			is_numeric = false;
		}
	}

	if (is_numeric)
	{
		json = "{\"" + key + "\":" + value + "}";
	}
	else
	{
		json = "{\"" + key + "\":\"" + value + "\"}";
	}

	root = &jsonBuffer->parseObject(json);	
	jsonBuffer->clear();

	return root;
}

JsonObject * Json_handler::convert_to_json(String formatted_string)
{	
	root = &jsonBuffer->parseObject(formatted_string);
	jsonBuffer->clear();
	return root;
}

bool Json_handler::queue_is_empty()
{
	return instruction_queue.isEmpty();
}
