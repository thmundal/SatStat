#pragma once
#include "../containers/Instruction_container.h"
#include "../../lib/QueueArray.h"

/**
*	The Instruction_handler class holds the instructions received through the serial port in the instruction_queue member.
*	The main responsibility of this class is to insert, fetch and execute instructions.
*/
class Instruction_handler
{
public:
	bool insert_instruction(const String& input_data);
	Json_container<JsonObject> fetch_instruction();
	bool queue_is_empty() const;

	void interpret_instruction();

	void append_available_instructions(Json_container<JsonObject>& dest);

private:	
	Instruction_container container;
	static QueueArray<Json_container<JsonObject>> instruction_queue;
};