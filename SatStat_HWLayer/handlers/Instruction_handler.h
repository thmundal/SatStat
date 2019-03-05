#pragma once
#include "Arduino.h"
#include "Json_handler.h"
#include "../libraries/QueueArray.h"
#include "../libraries/LinkedList.h"
#include "../other/SADM_functions.h"

class Instruction_handler
{
public:
	Instruction_handler();
	~Instruction_handler();
	
	void insert_instruction(const String& input_data);
	Json_container<JsonObject>* fetch_instruction();
	bool queue_is_empty() const;

	bool sadm_auto_rotate_en();
	void sadm_auto_rotate();

	void interpret_instruction();

private:	
	static QueueArray<Json_container<JsonObject>*> instruction_queue;
	LinkedList<String, void(*)(Json_container<JsonObject>*)> instruction_interpreter;
};