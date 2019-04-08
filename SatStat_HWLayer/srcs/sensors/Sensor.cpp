#pragma once
#include "Sensor.h"

/**
*	Constructor setting name, pin and data_count as well as initializing the Result pointer as an array of size provided by the data_count parameter.
*/
Sensor::Sensor(const String& name, const int& pin)
{
	this->name = name;
	this->pin = pin;
}

/**
*	Returns a constant string reference to the name member.
*/
const String& Sensor::get_name() const
{
	return this->name;
}
