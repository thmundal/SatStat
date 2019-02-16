#pragma once
#include "Output_handler.h"

// Constructor
Output_handler::Output_handler()
{
	stepper = new Stepper(stepsPerRev, 8, 10, 9, 11);
	steps = 0;
	dir = false;
	auto_rotate_en = false;
	stepper->setSpeed(700);
}

void Output_handler::print_to_serial(JsonObject *json)
{
	json->printTo(Serial);
}

/*
	Automatically rotates the SADM.
	Must be continuously called.
*/
void Output_handler::auto_rotate_sadm() 
{
	if (steps < step_limit)
	{
		steps++;
	}
	else
	{
		dir = !dir;
		steps = 0;
	}

	if (!dir)
	{
		stepper->step(1);
	}
	else
	{
		stepper->step(-1);
	}
}

bool Output_handler::auto_rotate_on()
{
	return auto_rotate_en;
}

// Rotates the SADM the passed number of steps
void Output_handler::rotate_sadm(int steps)
{
	stepper->step(steps);
}

// Converts from degrees to steps, and rotates the SADM
void Output_handler::rotate_sadm(float degrees)
{
	// 1 deg = 2048steps/360deg = 5.69 step/deg
	stepper->step((int)(degrees * ((float)2048 / (float)360)));
}
