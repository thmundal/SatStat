#pragma once
#include "./Json_container.h"
#include "../libraries/Stepper.h"


class SADM_functions
{
public:
	// Pure virtual destructor to make the class abstract
	virtual ~SADM_functions() = 0;

	static void init_stepper();
	static void set_auto_rotate(Json_container<JsonObject>* instruction);
	static void auto_rotate();
	static void rotate(int steps);
	static void rotate(float degrees);
	static void rotate(Json_container<JsonObject>* instruction);
	static bool get_auto_rotate_en();

private:
	static int steps;
	static bool dir;
	static bool auto_rotate_en;
	static const int stepsPerRev;
	static const float factor;
	static const int step_limit;
	static Stepper* stepper;
};
