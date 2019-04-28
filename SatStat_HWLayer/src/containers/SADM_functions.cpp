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
void SADM_functions::set_auto_rotate(Json_container<JsonObject>& ins)
{
	//Json_container<JsonObject> ins = instruction;	

	if (ins->containsKey("enable"))
	{
		if (ins->is<bool>("enable"))
		{
			auto_rotate_en = ins->get<bool>("enable");
		}
		else
		{
			Json_container<JsonObject> tmp;
			tmp->set("error", "Invalid value!");
			tmp->printTo(Serial);
		}
	}
	else
	{
		Json_container<JsonObject> tmp;
		tmp->set("error", "Invalid argument!!");
		tmp->printTo(Serial);
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
void SADM_functions::rotate(Json_container<JsonObject>& ins)
{
	//Json_container<JsonObject> ins = instruction;
	if (ins->containsKey("deg"))
	{
		if (ins->is<float>("deg"))
		{
			float deg = ins->get<float>("deg");
			rotate(deg);
		}
		else
		{
			Json_container<JsonObject> tmp;
			tmp->set("error", "Invalid value!");
			tmp->printTo(Serial);
		}
	}
	else if (ins->containsKey("steps"))
	{
		if (ins->is<int>("steps"))
		{
			int steps = ins->get<int>("steps");
			rotate(steps);		
		}
		else
		{
			Json_container<JsonObject> tmp;
			tmp->set("error", "Invalid value!");
			tmp->printTo(Serial);
		}
	}
	else
	{
		Json_container<JsonObject> tmp;
		tmp->set("error", "Invalid argument!");
		tmp->printTo(Serial);
	}
}

/**
*	Rotates the SADM the passed number of steps.
*/
void SADM_functions::rotate(int steps)
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
void SADM_functions::rotate(float degrees)
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
