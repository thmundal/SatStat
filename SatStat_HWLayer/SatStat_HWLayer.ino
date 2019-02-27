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

	// Loop until connection established
	bool connection_established = false;
	while (!connection_established)
	{
		// Send ack if connection is propperly established, nack if not
		if (input_handler->establish_connection())
		{
			output_handler->send_ack(input_handler->get_sensor_collection());
			connection_established = true;
		}
		else
		{
			output_handler->send_nack();
		}
	}	
	
	// Wait until serial is done sending ack
	while (!Serial.available());

	// Apply new config
	input_handler->serial_init();
	output_handler->set_newline_format(input_handler->get_newline_format());

	// Loop until confirmation is received with new settings
	connection_established = false;
	while (!connection_established)
	{
		if (input_handler->init_connection())
		{
			output_handler->send_ack();
			connection_established = true;
		}
		else
		{
			output_handler->send_nack();
		}
	}	
		
}

void loop()
{	
	// Runs with an interval equal to the duration
	if (!(millis() - start_time < duration))
	{	
		// Prints sensor data
		output_handler->print_to_serial(input_handler->read_sensors());

		// Update start time to current time
		start_time = millis();
	}
}