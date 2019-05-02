#pragma once
#include "../containers/Instruction_container.h"
#include "../containers/Request_container.h"
#include "../../lib/QueueArray.h"

/**
*	The Message_handler class holds the messages received through the serial port.
*	The main responsibility of this class is to insert, fetch and execute instructions and requests.
*/
class Message_handler
{
public:
	// Insert and fetch
	bool insert_message(const String& input_data);
	Json_container<JsonObject> fetch_message();

	// Has content checks
	bool has_requests() const;
	bool has_instructions() const;

	// Interpreters
	void interpret_request();
	void interpret_instruction();

	// Append available instrucitons to JSON object
	void append_available_instructions(Json_container<JsonObject>& dest);

private:	
	Request_container request_container;
	Instruction_container instruction_container;
	QueueArray<Json_container<JsonObject>> message_queue;
	QueueArray<Json_container<JsonObject>> instruction_queue;
};