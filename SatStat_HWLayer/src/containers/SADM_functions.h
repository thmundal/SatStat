#pragma once
#include "../handlers/Function_control.h"

/**
*	Abstract class with all static members for the methods to be compatible with list insertion.
*	This class is responsible for controlling the the SADM, and has member functions for rotating in different ways by providing different arguments.
*/
class SADM_functions : public Function_control
{
public:
	/**
	*	Pure virtual destructor to make the class abstract.
	*/
	virtual ~SADM_functions() = 0;

	static void set_step_size(Json_container<JsonObject>& ins);
	static void set_stepping_mode(Json_container<JsonObject>& ins);
	static void set_ratio(Json_container<JsonObject>& ins);
	static void set_speed(Json_container<JsonObject>& ins);
	static void set_dir(Json_container<JsonObject>& ins);
	static void rotate(Json_container<JsonObject>& ins);
	static void rotate();
	static void instant_release();


private:
	static int deg_to_steps(const float & deg);

	static float m_step_size;
	static int m_stepping_mode;
	static float m_ratio;
	static unsigned long m_period;
	static int m_steps;
	static bool m_rising;
	static unsigned long m_last_pulse;

	static const int step_pin;
	static const int dir_pin;
};