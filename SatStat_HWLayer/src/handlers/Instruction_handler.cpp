#pragma once
#include "Instruction_handler.h"

// Declaration of static member.
QueueArray<Json_container<JsonObject>> Instruction_handler::instruction_queue;

/**
*	Inserts passed instruction into the instruction_queue.
*	Returns true if success, false if not.
*/
bool Instruction_handler::insert_instruction(const String& input_data)
{
	Json_container<JsonObject> obj;
	if (obj.parse(input_data))
	{	
		instruction_queue.enqueue(obj);
		return true;
	}	

	return false;
}

/**
*	Fetches an instruction from the instruction queue. Returned as Json_container<JsonObject> pointer.
*/
Json_container<JsonObject> Instruction_handler::fetch_instruction()
{
	if (!instruction_queue.isEmpty())
	{
		return instruction_queue.dequeue();
	}

	Json_container<JsonObject> empty_obj;
	return empty_obj;
}

/**
*	Returns true if instruction queue is empty, false if not.
*/
bool Instruction_handler::queue_is_empty() const
{	
	return instruction_queue.isEmpty();
}

/**
*	Fetches the first instruction in the instruction_queue, getting the corresponding function from the instruction_interpreter
*	and calling the retreived function passing the instruction as argument.
*	Returns true if success, false if not.
*/
void Instruction_handler::interpret_instruction()
{	
	Json_container<JsonObject> obj = instruction_queue.dequeue();

	if (obj->containsKey("request"))
	{
		String request = obj->get<String>("request");
		Instruction* ins = container.get(request);

		if (ins != nullptr)
		{
			ins->run(obj);
		}
		else
		{
			Json_container<JsonObject> tmp;
			tmp->set("error", request + " is not a valid request!");
			tmp->printTo(Serial);
		}
	}
	else
	{
		Json_container<JsonObject> tmp;		
		tmp->set("error", "Invalid argument!");
		tmp->printTo(Serial);
	}	
}