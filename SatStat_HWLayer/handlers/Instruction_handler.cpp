#pragma once
#include "Instruction_handler.h"

QueueArray<Json_container<JsonObject>*> Instruction_handler::instruction_queue;

// Constructor
Instruction_handler::Instruction_handler()
{	
	sadm_functions.init_stepper();
	instruction_interpreter.append("auto_rotate", SADM_functions::set_auto_rotate);
	instruction_interpreter.append("rotate", SADM_functions::rotate);
}

// Delete every JsonObject in the instruction queue
Instruction_handler::~Instruction_handler()
{
	Json_container<JsonObject>* tmp;
	while (!instruction_queue.isEmpty())
	{
		tmp = instruction_queue.dequeue();
		delete tmp;
	}
}

// Insert instruction into instruction queue
void Instruction_handler::insert_instruction(const String& input_data)
{
	Json_container<JsonObject>* obj = new Json_object_container();
	obj->parse(input_data);		
	instruction_queue.enqueue(obj);	
}

// Fetches an instruction from the instruction queue
Json_container<JsonObject>* Instruction_handler::fetch_instruction()
{
	if (!instruction_queue.isEmpty())
	{
		return instruction_queue.dequeue();
	}

	return nullptr;
}

// Returns true if instruction queue is empty
bool Instruction_handler::queue_is_empty() const
{	
	return instruction_queue.isEmpty();
}

bool Instruction_handler::sadm_auto_rotate_en()
{
	return sadm_functions.get_auto_rotate_en();
}

void Instruction_handler::sadm_auto_rotate()
{
	sadm_functions.auto_rotate();
}

void Instruction_handler::interpret_instruction()
{	
	Json_container<JsonObject>* obj = instruction_queue.dequeue();
	String instruction = obj->get()->get<String>("instruction");	
	void(*ptr)(Json_container<JsonObject>*);
	ptr = instruction_interpreter.get(instruction);
	ptr(obj);
	delete obj;
}