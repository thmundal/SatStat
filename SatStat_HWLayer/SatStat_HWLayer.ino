#pragma once
#include "./handlers/Input_handler.h"
#include "./handlers/Output_handler.h"

Input_handler* input_handler;
Output_handler* output_handler;

unsigned long start_time = millis();
const int duration = 1000;

void setup()
{
	// For temp and hum sensor gnd and 5V
	pinMode(7, OUTPUT);
	digitalWrite(7, LOW);
	pinMode(5, OUTPUT);
	digitalWrite(5, HIGH);

	// initialize the serial port:
	Serial.begin(9600);	
	input_handler = new Input_handler();
	output_handler = new Output_handler();
}

void loop()
{
	/*if (output_handler->auto_rotate_on())
	{
		output_handler->auto_rotate_sadm();
	}

	input_handler->serial_listener();*/

	if (!(millis() - start_time < duration))
	{	
		output_handler->print_to_serial(input_handler->read_sensor("temp_hum"));
		start_time = millis();
	}
}
