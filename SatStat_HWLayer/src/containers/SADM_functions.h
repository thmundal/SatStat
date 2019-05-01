#pragma once
#include "../../lib/Stepper/Stepper.h"
#include "../handlers/Function_control.h"
/**
*	Abstract class with all static members for the functions to be compatible with insertion into the instruction_interpreter list in the Instruction_handler.
*	The instruction_interpreter list takes a general function pointer as value for it to be able to contain functions for every device to be controlled.
*	For a class' member functions to be treated as functions rather than methods of a class, they have to be static, hence the structure of this class.
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
	static const float factor;
	static const int step_limit;
	static Stepper* stepper;
};
