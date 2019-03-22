#pragma once
#include "Instruction_handler.h"

// Declaration of static member.
QueueArray<Json_container<JsonObject>*> Instruction_handler::instruction_queue;

/**
*	Constructor initializing the stepper, as well as appending instructions and the corresponding functions to the instruction_interpreter.
*/
Instruction_handler::Instruction_handler()
{	
	SADM_functions::init_stepper();
	instruction_interpreter.append("auto_rotate", SADM_functions::set_auto_rotate);
	instruction_interpreter.append("rotate", SADM_functions::rotate);
}

/**
*	Delete every JsonObject in the instruction_queue.
*/
Instruction_handler::~Instruction_handler()
{
	Json_container<JsonObject>* tmp;
	while (!instruction_queue.isEmpty())
	{
		tmp = instruction_queue.dequeue();
		delete tmp;
	}
}

/**
*	Inserts passed instruction into the instruction_queue.
*	Returns true if success, false if not.
*/
bool Instruction_handler::insert_instruction(const String& input_data)
{
	Json_container<JsonObject>* obj = new Json_object_container();
	
	if (obj->parse(input_data))
	{
		instruction_queue.enqueue(obj);
		return true;
	}

	return false;
}

/**
*	Fetches an instruction from the instruction queue. Returned as Json_container<JsonObject> pointer.
*/
Json_container<JsonObject>* Instruction_handler::fetch_instruction()
{
	if (!instruction_queue.isEmpty())
	{
		return instruction_queue.dequeue();
	}

	return nullptr;
}

/**
*	Returns true if instruction queue is empty, false if not.
*/
bool Instruction_handler::queue_is_empty() const
{	
	return instruction_queue.isEmpty();
}

/**
*	Returns true if auto rotate is enabled, false if not.
*/
bool Instruction_handler::sadm_auto_rotate_en()
{
	return SADM_functions::get_auto_rotate_en();
}

/**
*	Calls the static auto_rotate function from the SADM_functions container.
*/
void Instruction_handler::sadm_auto_rotate()
{
	SADM_functions::auto_rotate();
}

/**
*	Fetches the first instruction in the instruction_queue, getting the corresponding function from the instruction_interpreter
*	and calling the retreived function passing the instruction as argument.
*	Returns true if success, false if not.
*/
void Instruction_handler::interpret_instruction()
{	
	Json_container<JsonObject>* obj = instruction_queue.dequeue();
	String instruction = obj->get().get<String>("instruction");	

	if (instruction != NULL)
	{
		void(*ptr)(Json_container<JsonObject>*);	
		ptr = instruction_interpreter.get(instruction);

		if (ptr != NULL)
		{
			ptr(obj);
		}
		else
		{
			delete obj;

			obj = new Json_object_container();
			JsonObject& tmp = obj->get();
			tmp.set("error", instruction + " is not a valid instruction!");
			tmp.printTo(Serial);
		}
	}
	else
	{
		delete obj;

		obj = new Json_object_container();
		JsonObject& tmp = obj->get();
		tmp.set("error", "Invalid argument!");
		tmp.printTo(Serial);
	}
	
	delete obj;
}