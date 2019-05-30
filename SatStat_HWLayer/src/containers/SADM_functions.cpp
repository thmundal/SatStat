#include "SADM_functions.h"

// Definition of static members
float SADM_functions::m_step_size = 1.8;
int SADM_functions::m_stepping_mode = 8;
float SADM_functions::m_ratio = 1;
unsigned long SADM_functions::m_period = 1250;
int SADM_functions::m_steps = 0;
bool SADM_functions::m_rising = true;
unsigned long SADM_functions::m_last_pulse = micros();

const int SADM_functions::step_pin = 3;
const int SADM_functions::dir_pin = 4;

void SADM_functions::init()
{
	pinMode(step_pin, OUTPUT);
	pinMode(dir_pin, OUTPUT);
}

/**
*	Interprets the set_step_size instruction received from SWL.
*	Directly sets m_step_size.
*/
void SADM_functions::set_step_size(Json_container<JsonObject>& ins)
{
	if (ins->containsKey("step_size"))
	{
		if (ins->is<float>("step_size"))
		{
			m_step_size = ins->get<float>("step_size");
			Function_control::reserve("step_size", instant_release);
		}
	}
}

/**
*	Interprets the set_stepping_mode instruction received from SWL.
*	Directly sets m_stepping_mode.
*/
void SADM_functions::set_stepping_mode(Json_container<JsonObject>& ins)
{
	if (ins->containsKey("divisor"))
	{
		if (ins->is<int>("divisor"))
		{
			m_stepping_mode = ins->get<int>("divisor");
			Function_control::reserve("set_stepping_mode", instant_release);
		}
	}
}

/**
*	Interprets the set_ratio instruction received from SWL.
*	Directly sets m_ratio.
*/
void SADM_functions::set_ratio(Json_container<JsonObject>& ins)
{
	if (ins->containsKey("ratio"))
	{
		if (ins->is<float>("ratio"))
		{
			m_ratio = ins->get<float>("ratio");
			Function_control::reserve("set_ratio", instant_release);
		}
	}
}

/**
*	Interprets the set_speed instruction received from SWL.
*	Translates from RPM to period and updates m_period.
*/
void SADM_functions::set_speed(Json_container<JsonObject>& ins)
{
	if (ins->containsKey("rpm"))
	{
		if (ins->is<float>("rpm"))
		{
			float rpm = ins->get<float>("rpm");
			int freq = deg_to_steps(rpm * 360 / 60);
			m_period = pow(10, 6) / freq;
			Function_control::reserve("set_speed", instant_release);
		}
	}
}

/**
*	Interprets the set_direction instruction received from SWL.
*	Directly sets the voltage level on the direction pin to either high or low depending on the direction parameter.
*/
void SADM_functions::set_dir(Json_container<JsonObject>& ins)
{
	if (ins->containsKey("direction"))
	{
		if (ins->is<String>("direction"))
		{
			String dir = ins->get<String>("direction");
			if (dir == "forward")
			{
				digitalWrite(dir_pin, LOW);
			}
			else if (dir == "reverse")
			{
				digitalWrite(dir_pin, HIGH);
			}

			Function_control::reserve("set_direction", instant_release);
		}
	}
}

/**
*	Interprets the rotate instruction received from SWL.
*	Sets the number of steps to rotate, and reserves Function_control with the other rotate function.
*/
void SADM_functions::rotate(Json_container<JsonObject>& ins)
{
	if (ins->containsKey("steps"))
	{
		if (ins->is<int>("steps"))
		{
			m_steps = abs(ins->get<int>("steps"));
			Function_control::reserve("rotate_steps", rotate);
		}
	}
	else if (ins->containsKey("degrees"))
	{
		if (ins->is<float>("degrees"))
		{
			float degrees = abs(ins->get<float>("degrees"));
			m_steps = deg_to_steps(degrees);
			Function_control::reserve("rotate_degrees", rotate);
		}
	}
}

/**
*	Square wave generator responsible for rotating the SADM the given number of steps.
*/
void SADM_functions::rotate()
{
	if (m_steps == 0)
	{
		digitalWrite(step_pin, LOW);
		Function_control::release();
	}
	else if (micros() - m_last_pulse > m_period / 2)
	{
		if (m_rising)
		{
			digitalWrite(step_pin, HIGH);
			m_steps--;
		}
		else
		{
			digitalWrite(step_pin, LOW);
		}
		
		m_rising = !m_rising;
		m_last_pulse = micros();
	}
}

/**
*	Releases itself from Function_control the first time it's called.
*	When functions only needs to be called once, this function reserves Function_control.
*/
void SADM_functions::instant_release()
{
	Function_control::release();
}

/**
*	Converts from degrees to number of steps.
*/
int SADM_functions::deg_to_steps(const float& deg)
{
	return (int)(deg * m_stepping_mode / m_step_size * m_ratio);
}
