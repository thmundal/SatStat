#pragma once
#include "Arduino.h"
#include "Json_handler.h"
#include "../libraries/Stepper.h"
#include "../libraries/DS1302.h"

class Output_handler
{
public:
	Output_handler();
	void print_to_serial(JsonObject *json);
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
};

