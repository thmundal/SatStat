#include "SADM_functions.h"

int SADM_functions::steps = 0;
bool SADM_functions::dir = false;
bool SADM_functions::auto_rotate_en = false;
const int SADM_functions::stepsPerRev = 32;
const float SADM_functions::factor = 3.25;
const int SADM_functions::step_limit = (int)(1024 * factor);
Stepper* SADM_functions::stepper = new Stepper(stepsPerRev, 8, 10, 9, 11);

void SADM_functions::init_stepper()
{
	stepper->setSpeed(700);
}

void SADM_functions::set_auto_rotate(Json_container<JsonObject>* instruction)
{
	auto_rotate_en = instruction->get()->get<bool>("enable");
}

/*
	Automatically rotates the SADM.
	Must be continuously called.
*/
void SADM_functions::auto_rotate()
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

// Rotates the SADM the passed number of steps
void SADM_functions::rotate(int steps)
{
	stepper->step(steps);
}

// Converts from degrees to steps, and rotates the SADM
void SADM_functions::rotate(float degrees)
{
	// 1 deg = 2048steps/360deg = 5.69 step/deg
	stepper->step((int)(degrees * ((float)2048 / (float)360)));
}

void SADM_functions::rotate(Json_container<JsonObject>* instruction)
{
	if (instruction->get()->containsKey("deg"))
	{
		float deg = instruction->get()->get<float>("deg");
		rotate(deg);
	}
	else if (instruction->get()->containsKey("steps"))
	{
		int steps = instruction->get()->get<int>("steps");
		rotate(steps);
	}
}

bool SADM_functions::get_auto_rotate_en()
{
	return auto_rotate_en;
}
