#pragma once
#include "Json_handler.h"
#include "../../libraries/QueueArray.h"
#include "../../libraries/LinkedList/LinkedList.h"
#include "../containers/SADM_functions.h"
#include "../containers/Subscriber_functions.h"

/**
*	The Instruction_handler class holds the instructions received through the serial port in the instruction_queue member.
*	It also has a linked list with supported instruction and a pointer to the function corresponding to the specific instruction.
*	This list works as a lookup table where you look for a specific instruction (key),
*	and receive a pointer to a function (value) responsible for handling the execution of that specific instruction.
*	The main responsibility of this class is to insert, fetch and execute instructions.
*/
class Instruction_handler
{
public:
	Instruction_handler();
	~Instruction_handler();
	
	bool insert_instruction(const String& input_data);
	Json_container<JsonObject>* fetch_instruction();
	bool queue_is_empty() const;

	bool sadm_auto_rotate_en();
	void sadm_auto_rotate();

	void interpret_instruction();

private:	
	static QueueArray<Json_container<JsonObject>*> instruction_queue;
	LinkedList<String, void(*)(Json_container<JsonObject>*)> instruction_interpreter;
};