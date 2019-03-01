#pragma once
#include "./handlers/Input_handler.h"
#include "./handlers/Output_handler.h"

// Init input and output handler
Input_handler* input_handler;
Output_handler* output_handler;

// Timing constrains
unsigned long start_time = millis();
const int duration = 1000;

void setup()
{
	// Temp and hum sensor gnd and 5V
	pinMode(7, OUTPUT);
	digitalWrite(7, LOW);
	pinMode(5, OUTPUT);
	digitalWrite(5, HIGH);
	
	// Initialize the serial port
	Serial.begin(9600);
	
	// Instantiate input and output handler
	input_handler = new Input_handler();
	output_handler = new Output_handler();
	
	// Loop until handshake received
	bool connection_established = false;
	while (!connection_established)
	{		
		// Send ack if approved, nack if not
		if (input_handler->handshake_approved())
		{			
			output_handler->send_handshake_response();
			connection_established = true;
		}
		else
		{
			output_handler->send_nack();			
		}
	}	

	// Loop until connection request received
	connection_established = false;
	while (!connection_established)
	{		
		// Send nack if not approved
		if (input_handler->connection_request_approved())
		{
			connection_established = true;
		}
		else
		{
			output_handler->send_nack();
		}
	}	

	// Apply new config
	input_handler->serial_init();
	output_handler->set_newline_format(input_handler->get_newline_format());

	
	connection_established = false;	
	while (!connection_established)
	{
		output_handler->send_ack();

		if (input_handler->init_connection())
		{
			connection_established = true;
		}
	}
}

void loop()
{	
	input_handler->serial_listener();

	if (input_handler->instruction_available())
	{		
		output_handler->interpret_instruction(input_handler->get_instruction());
	}	

	if (output_handler->get_auto_rotate_en())
	{
		output_handler->auto_rotate_sadm();
	}
	// Runs with an interval equal to the duration
	if (!(millis() - start_time < duration))
	{	
		// Prints sensor data
		output_handler->print_to_serial(input_handler->read_sensors());

		// Update start time to current time
		start_time = millis();
	}
}