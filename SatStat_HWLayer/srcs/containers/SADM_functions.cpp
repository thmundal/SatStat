#pragma once
#include "SADM_functions.h"

int SADM_functions::m_steps = 0;
bool SADM_functions::m_dir = false;
bool SADM_functions::auto_rotate_en = false;
const int SADM_functions::stepsPerRev = 32;
const float SADM_functions::factor = 3.25;
const int SADM_functions::step_limit = (int)(1024 * factor);
Stepper* SADM_functions::stepper = new Stepper(stepsPerRev, 8, 10, 9, 11);

/**
*	Sets the speed of the stepper motor to a fixed speed (700 in this case).
*	The number used to set the speed is usualy defined as RPM,
*	but in this case the motor is set up in way so that it's no longer RPM.
*/
void SADM_functions::init_stepper()
{
	stepper->setSpeed(700);
}

/**
*	Set's the auto_rotate_en member either true or false depending on the instruction parameter.
*/
#define auto_rotate_params bool enable
void SADM_functions::set_auto_rotate(Json_container<JsonObject>* instruction)
{
	JsonObject& ins_obj = instruction->get();

	if (ins_obj.containsKey("enable"))
	{
		if (ins_obj.is<bool>("enable"))
		{
			auto_rotate_en = ins_obj.get<bool>("enable");
		}
		else
		{
			instruction->create();
			JsonObject& tmp_obj = instruction->get();
			tmp_obj.set("error", "Invalid value!");
			tmp_obj.printTo(Serial);
		}
	}
	else
	{
		instruction->create();
		JsonObject& tmp_obj = instruction->get();
		tmp_obj.set("error", "Invalid argument!");
		tmp_obj.printTo(Serial);
	}
}

/**
*	Rotates the SADM one step each time it's called. Automatically rotates the SADM when continuously called.
*/
void SADM_functions::auto_rotate()
{
	if (m_steps < step_limit)
	{
		m_steps++;
	}
	else
	{
		m_dir = !m_dir;
		m_steps = 0;
	}

	if (m_dir)
	{
		stepper->step(1);
	}
	else
	{
		stepper->step(-1);
	}
}

/**
*	Calls the rotate function matching the instruction parameter type.
*/
void SADM_functions::rotate(Json_container<JsonObject>* instruction)
{
	JsonObject& ins_obj = instruction->get();

	if (ins_obj.containsKey("deg"))
	{
		if (ins_obj.is<float>("deg"))
		{
			float deg = ins_obj.get<float>("deg");
			rotate(deg);
		}
		else
		{
			instruction->create();
			JsonObject& tmp_obj = instruction->get();
			tmp_obj.set("error", "Invalid value!");
			tmp_obj.printTo(Serial);
		}
	}
	else if (ins_obj.containsKey("steps"))
	{
		if (ins_obj.is<int>("steps"))
		{
			int steps = ins_obj.get<int>("steps");
			rotate(steps);
		}
		else
		{
			instruction->create();
			JsonObject& tmp_obj = instruction->get();
			tmp_obj.set("error", "Invalid value!");
			tmp_obj.printTo(Serial);
		}
	}
	else
	{
		instruction->create();
		JsonObject& tmp_obj = instruction->get();
		tmp_obj.set("error", "Invalid argument!");
		tmp_obj.printTo(Serial);
	}
}

/**
*	Rotates the SADM the passed number of steps.
*/
#define rotate_steps_params int steps
void SADM_functions::rotate(rotate_steps_params)
{
	if (Function_control::is_available())
	{
		if (steps > 0)
		{
			m_dir = true;
		}
		else if (steps < 0)
		{
			m_dir = false;
		}

		m_steps = steps;
		Function_control::reserve(rotate);
	}
}
/**
*	Converts from degrees to steps, and rotates the SADM.
*/
#define rotate_deg_params float degrees
void SADM_functions::rotate(rotate_deg_params)
{
	if (Function_control::is_available())
	{
		if (degrees > 0)
		{
			m_dir = true;
		}
		else if (degrees < 0)
		{
			m_dir = false;
		}

		// 1 deg = 2048steps/360deg = 5.69 step/deg
		m_steps = (int)(degrees * ((float)2048 / (float)360));
		Function_control::reserve(rotate);
	}
}

void SADM_functions::rotate()
{	
	if (m_steps == 0)
	{
		release();
	}
	else if (m_dir)
	{
		stepper->step(1);
		m_steps--;
	}
	else
	{
		stepper->step(-1);
		m_steps++;
	}	
}

/**
*	Returns true if auto rotate is enabled, false if not.
*/
bool SADM_functions::get_auto_rotate_en()
{
	return auto_rotate_en;
}
