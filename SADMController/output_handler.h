#pragma once
#include "Arduino.h"
#include "Json_handler.h"
#include "Stepper.h"
#include "DS1302.h"

class output_handler
{
public:
	void print_to_serial(JsonObject *json);
	void auto_rotate(bool state);
	void rotate_sadm(int steps);
	void rotate_sadm(float degrees);
private:
	Json_handler json_handler;
};

