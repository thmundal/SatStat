#pragma once
#include "SADM_functions.h"

int SADM_functions::m_steps = 0;
bool SADM_functions::m_dir = false;
bool SADM_functions::auto_rotate_en = false;
const int SADM_functions::stepsPerRev = 32;
float SADM_functions::ratio = 1;
float SADM_functions::factor = ratio;
int SADM_functions::step_limit = 1024 * factor;
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
*	Sets the gear_ratio. Used when converting from degrees to number of steps before rotating.
*/
void SADM_functions::set_ratio(Json_container<JsonObject>& ins)
{
	if (ins->containsKey("ratio"))
	{
		if (ins->is<float>("ratio"))
		{
			ratio = ins->get<float>("ratio");		
			Function_control::reserve("set_ratio", set_ratio);
		}
		else
		{
			Json_container<JsonObject> tmp;
			tmp->set("error", "Invalid value.");
			tmp->printTo(Serial);
			Serial.print("\r\n");
		}
	}
	else
	{
		Json_container<JsonObject> tmp;
		tmp->set("error", "Invalid argument.");
		tmp->printTo(Serial);
		Serial.print("\r\n");
	}
}

/**
*	The other set_ratio method will pass this one into Function_control.
*	This means that when calling Function_control::run() in the loop function,
*	This method will be called until it releases itself from Function_control.
*	This set_ratio method is the one actually setting the ratio, and it releases itself the first time it's called
*	as repeated calls is not required.
*/
void SADM_functions::set_ratio()
{
	factor = ratio;
	step_limit = 1024 * factor;
	Function_control::release();
}

/**
*	Set's the auto_rotate_en member true or false depending on the instruction parameter.
*/
void SADM_functions::set_auto_rotate(Json_container<JsonObject>& ins)
{
	if (ins->containsKey("enable"))
	{
		if (ins->is<bool>("enable"))
		{
			auto_rotate_en = ins->get<bool>("enable");
		}
		else
		{
			Json_container<JsonObject> tmp;
			tmp->set("error", "Invalid value.");
			tmp->printTo(Serial);
			Serial.print("\r\n");
		}
	}
	else
	{
		Json_container<JsonObject> tmp;
		tmp->set("error", "Invalid argument.");
		tmp->printTo(Serial);
		Serial.print("\r\n");
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
	if (ins->containsKey("degrees"))
	{
		if (ins->is<float>("degrees"))
		{
			float deg = ins->get<float>("degrees");
			rotate(deg);
		}
		else
		{
			Json_container<JsonObject> tmp;
			tmp->set("error", "Invalid value.");
			tmp->printTo(Serial);
			Serial.print("\r\n");
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
			tmp->set("error", "Invalid value.");
			tmp->printTo(Serial);
			Serial.print("\r\n");
		}
	}
	else
	{
		Json_container<JsonObject> tmp;
		tmp->set("error", "Invalid argument.");
		tmp->printTo(Serial);
		Serial.print("\r\n");
	}
}

/**
*	Rotates the SADM the given number of steps.
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
		Function_control::reserve("rotate_steps", rotate);
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
		m_steps = (int)((degrees * (2048.0f / 360.0f)) * ratio);
		Function_control::reserve("rotate_degrees", rotate);
	}
}

/**
*	The other rotate methods will pass this one into Function_control.
*	This means that when calling Function_control::run() in the loop function,
*	This method will be called until it releases itself from Function_control.
*	This rotate method is the one actually doning the rotation.
*/
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
