#pragma once
#include "output_handler.h"

void output_handler::print_to_serial(JsonObject *json)
{
	json->printTo(Serial);
}

void output_handler::auto_rotate(bool state)
{
}

void output_handler::rotate_sadm(int steps)
{
}

void output_handler::rotate_sadm(float degrees)
{
}
