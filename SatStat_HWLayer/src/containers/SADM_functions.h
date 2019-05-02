#pragma once
#include "../../lib/Stepper/Stepper.h"
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

	static void init_stepper();
	static void set_ratio(Json_container<JsonObject>& ins);
	static void set_ratio();
	static void set_auto_rotate(Json_container<JsonObject>& ins);
	static void auto_rotate();
	static void rotate(Json_container<JsonObject>& ins);
	static void rotate(int steps);
	static void rotate(float degrees);
	static void rotate();
	static bool get_auto_rotate_en();

private:
	static int m_steps;
	static bool m_dir;
	static bool auto_rotate_en;
	static const int stepsPerRev;
	static float ratio;
	static float factor;
	static int step_limit;
	static Stepper* stepper;
};
