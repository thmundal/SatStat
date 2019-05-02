#pragma once
#include "Message_handler.h"

/**
*	Inserts passed message into the message or instruction queue.
*	Returns true if success, false if not.
*/
bool Message_handler::insert_message(const String& input_data)
{
	Json_container<JsonObject> obj;

	if (obj.parse(input_data))
	{	
		if (obj->containsKey("instruction"))
		{
			instruction_queue.enqueue(obj);			
		}		
		else
		{
			message_queue.enqueue(obj);
		}
		return true;
	}	
	return false;
}

/**
*	Fetches a message from the message queue. Returned as Json_container<JsonObject>.
*/
Json_container<JsonObject> Message_handler::fetch_message()
{
	if (!message_queue.isEmpty())
	{
		return message_queue.dequeue();
	}

	Json_container<JsonObject> empty_obj;
	return empty_obj;
}

/**
*	Returns true if message queue is empty, false if not.
*/
bool Message_handler::has_requests() const
{	
	return !message_queue.isEmpty();
}

/**
*	Returns true if instruction queue is empty, false if not.
*/
bool Message_handler::has_instructions() const
{
	return !instruction_queue.isEmpty();
}

/**
*	Fetches the first request in the message_queue, gets the corresponding function from the request_container
*	and calls the retreived function.
*/
void Message_handler::interpret_request()
{	
	Json_container<JsonObject> obj = message_queue.dequeue();

	if (obj->containsKey("request"))
	{		
		String request = obj->get<String>("request");
		bool found = false;

		if (obj->size() > 1)
		{
			auto& func = request_container.get_with_param(request);
			
			if (func.is_set())
			{
				func(obj);
				found = true;
			}
		}
		else
		{
			auto& func = request_container.get_no_param(request);

			if (func.is_set())
			{
				func();
				found = true;
			}			
		}

		if (!found)
		{
			Json_container<JsonObject> tmp;
			tmp->set("error", request + " is not a valid request!");
			tmp->printTo(Serial);
			Serial.println("\r\n");
		}
	}	
	else
	{
		Json_container<JsonObject> tmp;		
		tmp->set("error", "Invalid argument!");
		tmp->printTo(Serial);
		Serial.println("\r\n");
	}	
}

/**
*	Fetches the first instruction in the instruction_queue, gets the corresponding function from the instruction_container
*	and calls the retreived function.
*/
void Message_handler::interpret_instruction()
{
	Json_container<JsonObject> obj = instruction_queue.dequeue();

	String instruction = obj->get<String>("instruction");
	Instruction* ins = instruction_container.get(instruction);

	if (ins != nullptr)
	{
		ins->run(obj);
	}
	else
	{
		Json_container<JsonObject> tmp;
		tmp->set("error", instruction + " is not a valid instruction.");
		tmp->printTo(Serial);
		Serial.println("\r\n");
	}
}

/**
*	Appends all available instructions to the destination Json_container<JsonObject>.
*/
void Message_handler::append_available_instructions(Json_container<JsonObject>& dest)
{
	JsonObject& nested = dest->createNestedObject("available_instructions");

	auto& list = instruction_container.get_available_instructions();

	for (int i = 0; i < list.count(); i++)
	{
		String identifier = list[i]->get_identifier();		
				
		Json_container<JsonObject>& signature_container = list[i]->get_signature();
		JsonObject& signature = signature_container.get();

		nested.set(identifier, signature);
	}		
}
