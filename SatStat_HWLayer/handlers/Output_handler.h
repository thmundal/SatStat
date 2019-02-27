#pragma once
#include "Arduino.h"
#include "Json_handler.h"
#include "../libraries/Stepper.h"
#include "../libraries/DS1302.h"
#include "../libraries/LinkedList.h"
#include "../sensors/Sensor.h"

class Output_handler
{
public:
	Output_handler();
	void send_ack(LinkedList<String, Sensor*>& sensor_collection);
	void send_ack();
	void send_nack();

	void set_newline_format(const String& newline_format);

	void print_to_serial(Json_container<JsonObject>* json);
	void auto_rotate_sadm();
	bool auto_rotate_on();
	void rotate_sadm(int steps);
	void rotate_sadm(float degrees);
private:
	Json_handler json_handler;
	Stepper* stepper;
	int steps;
	bool dir;
	bool auto_rotate_en;
	const int stepsPerRev = 32;
	const float factor = 3.25;
	const int step_limit = (int)(1024 * factor);	
	
	String newline_format;

	LinkedList<JsonObject*, void(*)(void)> instruction_interpreter;
};

