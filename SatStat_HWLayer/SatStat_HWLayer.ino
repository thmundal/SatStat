#pragma once
#include "./handlers/Input_handler.h"
#include "./handlers/Output_handler.h"
#include "./libraries/DHT.h"

Input_handler* input_handler;
Output_handler* output_handler;

unsigned long start_time = millis();
const int duration = 1000;

void setup()
{
	// initialize the serial port:
	Serial.begin(9600);

	input_handler = new Input_handler();
	output_handler = new Output_handler();
	Serial.println(input_handler->get_sensor_list());
}

void loop()
{
	if (output_handler->auto_rotate_on())
	{
		output_handler->auto_rotate_sadm();
	}

	input_handler->serial_listener();

	if (!(millis() - start_time < duration))
	{
		output_handler->print_to_serial(input_handler->read_sensors());
		//output_handler->print_to_serial(input_handler->read_sensor("humidity"));
		start_time = millis();
	}
}
